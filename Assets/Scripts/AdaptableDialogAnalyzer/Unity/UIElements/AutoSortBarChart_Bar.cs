using DG.Tweening;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity.UIElements
{
    public class AutoSortBarChart_Bar : MonoBehaviour
    {
        [Header("Components")]
        public CanvasGroup canvasGroup;
        public RectTransform barTransform;
        [Header("Settings")]
        public Direction2 direction = Direction2.Horizontal;
        public float maxLength = 600;
        public float fadeDuration = 0.5f;

        /// <summary>
        /// 机械更新，没有插值。请在图表控制类中计算两数据帧的插值。
        /// </summary>
        public void SetData(IAutoSortBarChartData data)
        {
            if (direction == Direction2.Horizontal)
            {
                barTransform.sizeDelta = new Vector2(maxLength * data.Value / data.ValueMax, barTransform.sizeDelta.y);
            }
            else
            {
                barTransform.sizeDelta = new Vector2(barTransform.sizeDelta.x, maxLength * data.Value / data.ValueMax);
            }
        }

        public virtual DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> FadeIn()
        {
            return canvasGroup.DOFade(1, fadeDuration);
        }

        public virtual DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> FadeOut()
        {
            return canvasGroup.DOFade(0, fadeDuration);
        }
    }

}