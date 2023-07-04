using AdaptableDialogAnalyzer.UIElements;
using DG.Tweening;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.ReStage
{
    public class View_ReStage_FirstPage : MonoBehaviour
    {
        [Header("Components")]
        public GraphicsAlphaController alphaController;
        public RuntimeScaleController scaleController;
        [Header("Components")]
        public float startScale = 0.8f;
        public float fadeTime = 0.5f;


        private void Start()
        {
            alphaController.Alpha = 0;
            scaleController.ScaleRatio = startScale;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space)) 
            {
                Play();
            }
        }

        void Play()
        {
            scaleController.DoScale(1, fadeTime);
            alphaController.DoFade(1, fadeTime);
        }
    }
}