using AdaptableDialogAnalyzer.MaterialController;
using AdaptableDialogAnalyzer.Unity;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    [RequireComponent(typeof(RectTransform))]
    public class View_BanGDream_TransitionOut_Line : MonoBehaviour
    {
        [Header("Components")]
        public UIFollower uIFollower;
        public MaterialController_HDRColorTexture materialController;

        RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
                return rectTransform;
            }
        }

        public void Initialize(View_BanGDream_ItemEffect itemEffect)
        {
            uIFollower.targetTransform = itemEffect.transform;
            itemEffect.materialController.HDRColor = materialController.startHDRColor;
        }
    }
}