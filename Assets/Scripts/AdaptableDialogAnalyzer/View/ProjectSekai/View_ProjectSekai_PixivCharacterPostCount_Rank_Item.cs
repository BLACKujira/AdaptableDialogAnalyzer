using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.ProjectSekai
{

    public class View_ProjectSekai_PixivCharacterPostCount_Rank_Item : MonoBehaviour
    {
        [Header("Components")]
        public Text txtName;
        public Text txtRank;
        public Text txtData;
        public Image imgIcon;
        public IndividualColorElement individualColorElement;
        [Header("Settings")]
        public bool overrideIcon = false;
        public SpriteList iconSpriteList;
        public int fontSize_digit1 = 26;
        public int fontSize_digit2 = 20;

        public void SetData(int characterId, int rank, string data)
        {

            Character character = GlobalConfig.CharacterDefinition[characterId];
            txtData.text = data;
            individualColorElement.SetIndividualColor(character.color);
            imgIcon.sprite = iconSpriteList[characterId];

            if (txtName)
            {
                txtName.text = character.name;
            }
            if (overrideIcon)
            {
                imgIcon.sprite = iconSpriteList[characterId];
            }

            if(rank >= 10)
            {
                txtRank.fontSize = fontSize_digit2;
            }
            else
            {
                txtRank.fontSize = fontSize_digit1;
            }
            txtRank.text = rank.ToString();
        }
    }
}
