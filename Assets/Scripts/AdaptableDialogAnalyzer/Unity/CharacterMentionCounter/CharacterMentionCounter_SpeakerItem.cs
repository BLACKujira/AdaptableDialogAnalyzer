using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    [RequireComponent(typeof(Button))]
    public class CharacterMentionCounter_SpeakerItem : MonoBehaviour
    {
        [Header("Components")]
        public IndividualColorElement individualColorElement;
        public Image imgIcon;
        public Text txtName;
        public GameObject goCheckMark;

        Button button;
        public Button Button
        {
            get
            {
                if (button == null) button = GetComponent<Button>();
                return button;
            }
        }

        /// <summary>
        /// 设置是否显示已选择标志
        /// </summary>
        public bool Checked
        {
            get => goCheckMark.activeSelf;
            set => goCheckMark.SetActive(value);
        }

        public void SetData(int characterId)
        {
            Character character = GlobalConfig.CharacterDefinition[characterId];

            imgIcon.sprite = character.icon;
            txtName.text = character.name;

            individualColorElement.SetIndividualColor(character.color);
        }
    }
}