using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

namespace AdaptableDialogAnalyzer
{
    public static class Capturer
    {
        public static Texture2D Capture(RectTransform rectTransform, RenderTexture renderTexture)
        {
            int width = (int)(rectTransform.rect.width);
            int height = (int)(rectTransform.rect.height);

            float x = rectTransform.position.x + rectTransform.rect.xMin;
            float y = rectTransform.position.y + rectTransform.rect.yMin;

            RenderTexture.active = renderTexture;
            Texture2D texture2D = new Texture2D((int)rectTransform.sizeDelta.x, (int)rectTransform.sizeDelta.y);
            texture2D.ReadPixels(new Rect(x, y, width, height), 0, 0);
            texture2D.Apply();

            return texture2D;
        }

        public static Texture2D Capture(RectTransform rectTransform, Canvas targetCanvas)
        {
            int width = (int)(rectTransform.rect.width * targetCanvas.scaleFactor);
            int height = (int)(rectTransform.rect.height * targetCanvas.scaleFactor);

            float x = rectTransform.position.x + rectTransform.rect.xMin * targetCanvas.scaleFactor;
            float y = rectTransform.position.y + rectTransform.rect.yMin * targetCanvas.scaleFactor;

            Texture2D texture2D = new Texture2D((int)rectTransform.sizeDelta.x, (int)rectTransform.sizeDelta.y);
            texture2D.ReadPixels(new Rect(x, y, width, height), 0, 0);
            texture2D.Apply();
            return texture2D;
        }
    }
}