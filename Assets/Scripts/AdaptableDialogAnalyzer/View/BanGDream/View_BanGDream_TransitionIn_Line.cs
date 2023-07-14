using AdaptableDialogAnalyzer.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    [RequireComponent(typeof(RectTransform))]
    public class View_BanGDream_TransitionIn_Line : MonoBehaviour
    {
        [Header("Components")]
        public Image imgWhiteMask;
        public UIFollower uIFollower;

        RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if(rectTransform == null) rectTransform = GetComponent<RectTransform>();
                return rectTransform;
            }
        }
    }
}