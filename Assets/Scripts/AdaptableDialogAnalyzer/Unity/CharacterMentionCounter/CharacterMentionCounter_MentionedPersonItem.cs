using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    public class CharacterMentionCounter_MentionedPersonItem : MonoBehaviour
    {
        [Header("Components")]
        public Image imgIcon;
        public Image imgNameBG;
        public Text txtName;
        public Image imgCountBG;
        public Text txtCount;
        [Header("Settings")]
        public Color colorWhite = Color.white;
        public Color colorBlack = new Color32(68, 68, 102, 255);

        public int Count { set => txtCount.text = value.ToString(); }

        public void SetData(int characterId)
        {
            Character character = GlobalConfig.CharacterDefinition[characterId];
            imgIcon.sprite = character.icon;

            imgNameBG.color = character.color;
            txtName.text = character.name;
            txtName.color = APCA.GeBlackOrWhite(character.color, colorWhite, colorBlack);

            imgCountBG.color = character.color;
            txtCount.text = "0";
            txtCount.color = APCA.GeBlackOrWhite(character.color, colorWhite, colorBlack);
        }
    }
}