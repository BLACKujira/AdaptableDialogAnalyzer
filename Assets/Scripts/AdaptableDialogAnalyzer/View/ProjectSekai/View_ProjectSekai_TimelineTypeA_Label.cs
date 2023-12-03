using UnityEngine;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{
    [RequireComponent(typeof(RectTransform))]
    public class View_ProjectSekai_TimelineTypeA_Label : MonoBehaviour
    {
        RectTransform rectTransform;
        public RectTransform RectTransform
        {
            get
            {
                if (rectTransform == null)
                {
                    rectTransform = GetComponent<RectTransform>();
                }
                return rectTransform;
            }
        }
    }
}
