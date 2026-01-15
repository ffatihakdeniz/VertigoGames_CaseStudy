using UnityEngine;
using UnityEngine.UI;

namespace VertigoCase.Helpers.Extensions
{
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
}
