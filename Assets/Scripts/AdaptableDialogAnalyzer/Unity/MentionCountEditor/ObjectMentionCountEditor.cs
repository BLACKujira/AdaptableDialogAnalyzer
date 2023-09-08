using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.UIElements;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    public class ObjectMentionCountEditor : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public ObjectMentionedCountManagerLoader mentionedCountManagerLoader;
        public EquidistantLayoutGenerator2D layoutGenerator;
        public Text txtTitle;
        public Text txtCountTotal;
        public Text txtChanged;
        [Header("Settings")]
        public ObjectNameDefinition objectNameDefinition;

        ObjectMentionedCountManager mentionedCountManager;

        private void Start()
        {
            mentionedCountManager = mentionedCountManagerLoader.MentionedCountManager;
            Initialize();
        }

        private void Initialize()
        {
            txtTitle.text = $"统计对象: {(objectNameDefinition == null ? "未知" : objectNameDefinition.Identifier)}";
            Character[] characters = GlobalConfig.CharacterDefinition.Characters;
            SelectSpeaker(characters[0].id);
            Refresh();
        }

        private void SelectSpeaker(int characterId)
        {
            GlobalColor.SetThemeColor(GlobalConfig.CharacterDefinition[characterId].color);
        }

        private void Refresh()
        {
            layoutGenerator.ClearItems();
            Dictionary<int, int> mentionedCountDictionary = mentionedCountManager.MentionedCountDictionary;

            Character[] characters = GlobalConfig.CharacterDefinition.Characters;
            layoutGenerator.Generate(characters.Length, (gobj, id) =>
            {
                int characterId = characters[id].id;

                ObjectMentionCounter_Item item = gobj.GetComponent<ObjectMentionCounter_Item>();
                item.SetData(characterId);

                int count = mentionedCountDictionary.ContainsKey(characterId) ? mentionedCountDictionary[characterId] : 0;
                item.Count = count;

                item.button.onClick.AddListener(() =>
                {
                    SelectSpeaker(characterId);
                });
            });

            int countTotal = mentionedCountDictionary.Sum(kvp => kvp.Value);

            txtCountTotal.text = $"共 {countTotal} 次";
            txtChanged.text = $"{mentionedCountManagerLoader.ChangedMatricesCount} 个文件已更改";
        }

        public void SaveChangedMatrices()
        {
            mentionedCountManagerLoader.SaveChangedMatrices();
            Refresh();
        }
    }
}