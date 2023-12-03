using AdaptableDialogAnalyzer.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{
    public class View_ProjectSekai_TimelineTypeA_LabelBirthday : View_ProjectSekai_TimelineTypeA_Label
    {
        public IndividualColorElement individualColorElement;
        public Image icon;
        [Header("settings")]
        public SpriteList charIconList;

        public void SetData(int characterID)
        {
            icon.sprite = charIconList[characterID];
            Character character = GlobalConfig.CharacterDefinition[characterID];
            individualColorElement.SetIndividualColor(character.color);
        }
    }
}
