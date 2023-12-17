using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View
{
    public class GraphicColorTransition : ColorTransition
    {
        [Header("Components2")]
        public List<Graphic> colorGraphics;

        protected override void FadeAlpha(float alpha, float duration)
        {
            foreach (var graphic  in colorGraphics)
            {
                graphic.DOFade(alpha, duration);
            }
        }
    }
}
