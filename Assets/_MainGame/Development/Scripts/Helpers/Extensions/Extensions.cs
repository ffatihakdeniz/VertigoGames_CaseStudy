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
        public static void DoMaxSize(this Transform textTransform, int maxSize) =>
            textTransform.GetComponent<TextMeshProUGUI>().fontSizeMax = maxSize;
        public static void DoMaxSize(this GameObject textTransform, int maxSize) =>
            textTransform.GetComponent<TextMeshProUGUI>().fontSizeMax = maxSize;

    }
    public static class ImageExtensions
    {
        public static void FitInside(this Image targetImage, Image containerImage)
        {
            if (targetImage == null || containerImage == null)
                return;

            RectTransform targetRect = targetImage.rectTransform;
            RectTransform containerRect = containerImage.rectTransform;

            Vector2 containerSize = containerRect.rect.size;
            Vector2 targetSize = targetImage.sprite.rect.size;

            float widthRatio = containerSize.x / targetSize.x;
            float heightRatio = containerSize.y / targetSize.y;

            float scale = Mathf.Min(widthRatio, heightRatio);

            Vector2 finalSize = targetSize * scale;

            targetRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, finalSize.x);
            targetRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, finalSize.y);

            targetRect.anchoredPosition = Vector2.zero;
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
}
