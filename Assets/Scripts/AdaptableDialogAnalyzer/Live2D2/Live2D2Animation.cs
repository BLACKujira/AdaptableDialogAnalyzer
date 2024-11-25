using live2d.framework;
using live2d;
using System;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Live2D2
{
    [Serializable]
    public class Live2D2Animation
    {
        public TextAsset motionAsset;
        public TextAsset expressionAsset;

        [NonSerialized]
        public Live2DMotion live2DMotion;
        public Live2DMotion Live2DMotion
        {
            get
            {
                if (live2DMotion == null) LoadMotion();
                return live2DMotion;
            }
        }
        [NonSerialized]
        public L2DExpressionMotion live2DExpression;

        public L2DExpressionMotion Live2DExpression
        {
            get
            {
                if (live2DExpression == null) LoadExpression();
                return live2DExpression;
            }
        }

        void LoadMotion()
        {
            if (motionAsset != null) live2DMotion = Live2DMotion.loadMotion(motionAsset.bytes);
        }

        void LoadExpression()
        {
            if (expressionAsset != null) live2DExpression = L2DExpressionMotion.loadJson(expressionAsset.bytes);
        }

        public Live2D2Animation(TextAsset motionAsset, TextAsset expressionAsset)
        {
            this.motionAsset = motionAsset;
            this.expressionAsset = expressionAsset;
        }

        public Live2D2Animation()
        {
        }
    }
}