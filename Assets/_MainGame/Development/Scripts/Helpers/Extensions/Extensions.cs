using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

namespace VertigoCase.Helpers.Extensions
{
    public static class GeneralExtensions
    {
        public static void DOTypeText(this TextMeshProUGUI text, int targetValue, float duration)
        {
            int startValue = int.TryParse(text.text, out int v) ? v : 0;

            DOTween.To(
               () => startValue,
               x =>
               {
                   startValue = x;
                   text.SetText(x.ToString());
               },
               targetValue,
               duration
           ).SetEase(Ease.OutCubic);
        }
        public static void DOTypeTextScalePulse(this TextMeshProUGUI text, int targetValue, float duration, float pulseScale = 1.1f)
        {
            int startValue = int.TryParse(text.text, out int v) ? v : 0;
            Transform tr = text.transform;
            tr.DOKill(); DOTween.Kill(text);

            Tween valueTween = DOTween.To(
                () => startValue,
                x =>
                {
                    startValue = x;
                    text.SetText(x.ToString());
                },
                targetValue,
                duration
            ).SetEase(Ease.OutCubic).SetTarget(text);

            Tween scaleTween = tr.DOScale(pulseScale, .2f).SetLoops(-1, LoopType.Yoyo).SetTarget(tr);

            valueTween.OnComplete(() =>
            {
                scaleTween.Kill();
                tr.DOScale(1, .2f).SetTarget(tr);
            });
        }

        public static void DoMaxSize(this Transform textTransform, int maxSize) =>
            textTransform.GetComponent<TextMeshProUGUI>().fontSizeMax = maxSize;
        public static void DoMaxSize(this GameObject textTransform, int maxSize) =>
            textTransform.GetComponent<TextMeshProUGUI>().fontSizeMax = maxSize;
        public static void SetChildrenSetActive(this Transform transform, bool isActive)
        {
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(isActive);
        }

    }
    public static class MathExtensions
    {
        public static int IntervalIndexByLevel(int interval, int level)
        {
            if (interval <= 0 || level <= 0)
                return 1;

            return (level - 1) / interval + 1;
        }

    }
    public static class ImageExtensions
    {
        public static Vector2 GetImageNativeSize(this Image image)
        {
            if (image == null || image.sprite == null || image.canvas == null)
                return Vector2.zero;

            Sprite sprite = image.sprite;
            float refPPU = image.canvas.referencePixelsPerUnit;
            float spritePPU = sprite.pixelsPerUnit;

            return sprite.rect.size * (refPPU / spritePPU);
        }

        /// <summary>
        /// HEdef Imageyi frame icine oranı bozmadan sigdirir.
        /// bazı gönderilen icon dosyalarında alpha alanlar çok fazla olduğu icin uzerine designer ayarlayabilsin diye bir scale multiplier de ekliyoruz
        /// </summary>
        public static Vector2 FitToFrame(this Image targetImage, RectTransform frameRect, float scaleMultiplier = 1f)
        {
            //calisan bnm kod TODO Test
            /* Vector2 nativeSize = targetImage.GetImageNativeSize();
             Vector2 frameSize = frameRect.sizeDelta;
             Vector2 ratios = new Vector2(nativeSize.x / frameSize.x, nativeSize.y / frameSize.y);
             Debug.Log(ratios + " - " + frameSize + " - " + nativeSize);
             float ratio = ratios.x >= ratios.y ? ratios.x : ratios.y;
             return new Vector2(nativeSize.x / ratio, nativeSize.y / ratio) * scaleMultiplier;*/
            //calismayan gpt kodu
            if (targetImage == null || frameRect == null)
                return Vector2.zero;

            Sprite sp = targetImage.sprite;
            Canvas canvas = targetImage.canvas;

            if (sp == null || canvas == null)
                return Vector2.zero;

            // Frame size: rect kullan (sizeDelta değil) -> daha stabil
            float frameW = frameRect.rect.width;
            float frameH = frameRect.rect.height;
            if (frameW <= 0f || frameH <= 0f)
                return Vector2.zero;

            // Native size (Image.SetNativeSize ile aynı)
            float refPPU = canvas.referencePixelsPerUnit;
            float spritePPU = sp.pixelsPerUnit <= 0f ? 100f : sp.pixelsPerUnit;
            Vector2 nativeSize = sp.rect.size * (refPPU / spritePPU);

            float targetW = nativeSize.x;
            float targetH = nativeSize.y;
            if (targetW <= 0f || targetH <= 0f)
                return Vector2.zero;

            float targetAspect = targetW / targetH;
            float frameAspect = frameW / frameH;

            float newW, newH;


            if (targetAspect > frameAspect)
            {
                newW = frameW;
                newH = newW / targetAspect;
            }
            else
            {
                newH = frameH;
                newW = newH * targetAspect;
            }


            return new Vector2(newW, newH) * scaleMultiplier;
        }


    }
}