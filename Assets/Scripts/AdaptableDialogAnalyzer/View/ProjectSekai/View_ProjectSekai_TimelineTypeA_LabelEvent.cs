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

        int eventID;
        public int EventID => eventID;

        public void SetData(int eventID)
        {
            this.eventID = eventID;

            if(eventID >= eventIconList.sprites.Count)
            {
                Debug.Log($"活动 {eventID} 没有对应的图标");
                return;
            }

            icon.sprite = eventIconList[eventID];
        }
    }
}
