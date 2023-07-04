using DG.Tweening;

namespace AdaptableDialogAnalyzer.UIElements
{
    public static class DoTweenHelper
    {
        public static void DoFade(this GraphicsAlphaController graphicsAlphaController, float endValue, float duration)
        {
            DOTween.To(() => graphicsAlphaController.Alpha, v => graphicsAlphaController.Alpha = v, endValue, duration);
        }

        public static void DoScale(this RuntimeScaleController runtimeScaleController, float endValue, float duration)
        {
            DOTween.To(() => runtimeScaleController.ScaleRatio, v => runtimeScaleController.ScaleRatio = v, endValue, duration);
        }
    }
}