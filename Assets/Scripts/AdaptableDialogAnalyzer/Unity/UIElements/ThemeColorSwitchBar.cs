using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.UIElements
{
    /// <summary>
    /// 在不会自动切换主题色的窗口上，选择一名角色的代表色作为主题色
    /// </summary>
    public class ThemeColorSwitchBar : MonoBehaviour
    {
        [Header("Components")]
        public RectTransform tfBar;
        public EquidistantLayoutGenerator layoutGenerator;
        [Header("Settings")]
        public float margin = 5;
        public float maxLength = 1300;
        public bool autoSelectFirst = false;

        public void Start()
        {
            Character[] characters = GlobalConfig.CharacterDefinition.Characters;
            layoutGenerator.Generate(characters.Length, (gobj, id) =>
            {
                int characterId = characters[id].id;

                CharacterInfoElement characterInfoElement = gobj.GetComponent<CharacterInfoElement>();
                characterInfoElement.SetData(characterId);

                Button button = gobj.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    GlobalColor.SetThemeColor(characters[id].color);
                });
            });

            float xSize = layoutGenerator.scorllContent.sizeDelta.x;
            xSize += margin * 2;
            xSize = xSize > maxLength ? maxLength : xSize;
            tfBar.sizeDelta = new Vector2(xSize, tfBar.sizeDelta.y);

            if(autoSelectFirst) GlobalColor.SetThemeColor(characters[0].color);
        }
    }
}