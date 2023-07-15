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
        public RectTransform rtStar;

        RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if(rectTransform == null) rectTransform = GetComponent<RectTransform>();
                return rectTransform;
            }
        }

        private void Awake()
        {
            rtStar.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360f));
        }
    }
}