using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace AdaptableDialogAnalyzer.View
{
    public class SpriteColorTransition : ColorTransition
    {
        [Header("Components2")]
        public List<SpriteRenderer> colorSprites;

        protected override void FadeAlpha(float alpha, float duration)
        {
            foreach (var spriteRenderer in colorSprites)
            {
                spriteRenderer.DOFade(alpha, duration);
            }
        }
    }
}
