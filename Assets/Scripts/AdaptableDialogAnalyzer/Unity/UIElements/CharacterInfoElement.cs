using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.UIElements
{
    /// <summary>
    /// 一个通用的角色信息UI实现
    /// </summary>
    public class CharacterInfoElement : MonoBehaviour
    {
        [Header("Components")]
        public List<IndividualColorElement> iceCharacterColor;
        public List<Image> imgCharacterIcon;
        public List<Text> txtCharacterName;

        public void SetData(int characterId)
        {
            Character character = GlobalConfig.CharacterDefinition.characters[characterId];
            foreach (var individualColorElement in iceCharacterColor) individualColorElement.SetIndividualColor(character.color);
            foreach (var image in imgCharacterIcon) image.sprite = character.icon;
            foreach (var text in txtCharacterName) text.text = character.name;
        }
    }
}