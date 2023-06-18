using AdaptableDialogAnalyzer.DataStructures;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    public class DiscrepancyCheckItem : MonoBehaviour
    {
        [Header("Components")]
        public IndividualColorElement iceCharColorA;
        public IndividualColorElement iceCharColorB;
        public Image imgCharIconA;
        public Image imgCharIconB;
        public Text txtCountAToB;
        public Text txtCountBToA;
        public Text txtReason;

        public void SetData(CharacterMentionStats statsAToB, CharacterMentionStats statsBToA, string reason)
        {
            List<Character> characters = GlobalConfig.CharacterDefinition.characters;
            Character characterA = characters[statsAToB.SpeakerId];
            Character characterB = characters[statsBToA.SpeakerId];

            iceCharColorA.SetIndividualColor(characterA.color);
            iceCharColorB.SetIndividualColor(characterB.color);
            imgCharIconA.sprite = characterA.icon;
            imgCharIconB.sprite = characterB.icon;

            txtCountAToB.text = statsAToB.Total.ToString();
            txtCountBToA.text = statsBToA.Total.ToString();
            txtReason.text = reason;
        }
    }
}