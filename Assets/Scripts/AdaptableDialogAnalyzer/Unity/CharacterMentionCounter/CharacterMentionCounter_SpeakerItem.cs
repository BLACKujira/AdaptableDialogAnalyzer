using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    [RequireComponent(typeof(Button))]
    public class CharacterMentionCounter_SpeakerItem : MonoBehaviour
    {
        [Header("Components")]
        public Image imgIcon;
        public Image imgNameBG;
        public Text txtName;
        public GameObject goCheckMark;
        [Header("Components")]
        public Color colorWhite = Color.white;
        public Color colorBlack = new Color32(68, 68, 102, 255);

        Button button;
        public Button Button
        {
            get
            {
                if(button == null) button = GetComponent<Button>();
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
            imgNameBG.color = character.color;
            txtName.text = character.name;
            txtName.color = APCA.GeBlackOrWhite(character.color, colorWhite, colorBlack);
        }
    }
}