using AdaptableDialogAnalyzer.Unity;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{
    public class View_ProjectSekai_TimelineTypeA_LabelAnniversary : View_ProjectSekai_TimelineTypeA_Label
    {
        public IndividualColorElement elementBg;
        [Header("Settings")]
        public ColorList colorList;

        public void SetData(int index)
        {
            elementBg.SetIndividualColor(colorList[index]);
        }
    }
}
