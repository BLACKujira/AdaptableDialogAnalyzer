using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    public class CharacterMentionCounter_MentionedPersonItem : MonoBehaviour
    {
        [Header("Components")]
        public IndividualColorElement individualColorElement;
        public Image imgIcon;
        public Text txtName;
        public Text txtCount;

        public int Count { set => txtCount.text = value.ToString(); }

        public void SetData(int characterId)
        {
            Character character = GlobalConfig.CharacterDefinition[characterId];

            imgIcon.sprite = character.icon;
            txtName.text = character.name;
            txtCount.text = "0";

            individualColorElement.SetIndividualColor(character.color);
        }
    }
}