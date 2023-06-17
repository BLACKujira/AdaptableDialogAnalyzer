using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.UIElements
{
    /// <summary>
    /// 生成一列带背景的角色名称
    /// </summary>
    public class CharacterNamePanelGenerator : MonoBehaviour
    {
        [Header("Component")]
        public RectTransform rtContent;
        [Header("Prefabs")]
        public TextWithIndividualColor nameLabelPrefab;
        public RectTransform spacePrefab;

        List<GameObject> generatedObjects = new List<GameObject>();

        /// <summary>
        /// 显示这些角色的名称
        /// </summary>
        public void SetData(int[] characterIds)
        {
            List<Character> characters = GlobalConfig.CharacterDefinition.characters;

            ClearItems();
            foreach (int characterId in characterIds) 
            {
                //如果不是第一个生成的标签则插入一个空隙
                if(generatedObjects.Count != 0) generatedObjects.Add(Instantiate(spacePrefab, rtContent).gameObject);

                TextWithIndividualColor textWithIndividualColor = Instantiate(nameLabelPrefab, rtContent);
                textWithIndividualColor.text.text = characters[characterId].name;
                textWithIndividualColor.IndividualColorElement.SetIndividualColor(characters[characterId].color);
            }
        }

        /// <summary>
        /// 清除所有的Label和标签
        /// </summary>
        public void ClearItems()
        {
            foreach (GameObject obj in generatedObjects) 
            {
                Destroy(obj);
            }
            generatedObjects.Clear();
        }
    }
}
