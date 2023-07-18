using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.UIElements
{
    /// <summary>
    /// 控制此物体和其所有子物体的alpha
    /// </summary>
    [Obsolete("Use CanvasGroup instead")]
    public class GraphicsAlphaController : MonoBehaviour
    {
        public GameObject targetObject;

        public float Alpha
        {
            get => _alpha;
            set
            {
                if(!Initialized)
                {
                    Initialize();
                    Initialized = true;
                }

                for (int i = 0; i < graphics.Length; i++)
                {
                    Color color = graphics[i].color;
                    color.a = Mathf.Lerp(0, alphas[i], value);
                    graphics[i].color = color;
                }
                _alpha = value;
            }
        }

        Graphic[] graphics;
        float[] alphas;
        float _alpha = 1;
        bool Initialized = false;

        public void Initialize()
        {
            graphics = targetObject.GetComponentsInChildren<Graphic>();
            alphas = new float[graphics.Length];
            for (int i = 0; i < graphics.Length; i++)
            {
                alphas[i] = graphics[i].color.a;
            }
        }
    }
}