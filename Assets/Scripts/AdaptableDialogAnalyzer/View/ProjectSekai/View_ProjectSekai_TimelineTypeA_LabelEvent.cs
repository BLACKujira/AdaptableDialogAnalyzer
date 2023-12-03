using AdaptableDialogAnalyzer.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{
    public class View_ProjectSekai_TimelineTypeA_LabelEvent : View_ProjectSekai_TimelineTypeA_Label
    {
        public Image icon;
        [Header("settings")]
        public SpriteList eventIconList;

        public void SetData(int eventID)
        {
            icon.sprite = eventIconList[eventID];
        }
    }
}
