using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{
    public class View_ProjectSekai_TimelineTypeA_LabelMonth : View_ProjectSekai_TimelineTypeA_Label
    {
        public Text text;

        public void SetText(string text)
        {
            this.text.text = text;
        }
    }
}
