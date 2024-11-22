using AdaptableDialogAnalyzer.UIElements;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    public class ObjectMentionCounterMuti_DisplayArea : MonoBehaviour
    {
        public ObjectMentionCounterMuti objectMentionCounterMuti;
        [Header("Components")]
        public Text txtTotal;
        public EquidistantLayoutGenerator2D layoutGenerator;
        [Header("Settings")]
        public int refreshFrame = 120;

        Character[] characters;
        Dictionary<int, ObjectMentionCounter_Item> items = new Dictionary<int, ObjectMentionCounter_Item>();

        public void Awake()
        {
            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;
            characters = characterDefinition.Characters;

            //初始化角色选择
            layoutGenerator.Generate(characters.Length, (gobj, id) =>
            {
                int characterId = characters[id].id;

                ObjectMentionCounter_Item item = gobj.GetComponent<ObjectMentionCounter_Item>();
                item.SetData(characterId);
                items[characterId] = item;
                item.button.onClick.AddListener(() =>
                {
                    SelectCharacter(characterId);
                });
            });

            SelectCharacter(0);
        }

        void SelectCharacter(int characterId)
        {
            GlobalColor.SetThemeColor(GlobalConfig.CharacterDefinition[characterId].color);
        }

        int currentFrame = -1;
        bool lastRefresh = false;
        private void Update()
        {
            //如果已经统计完成且完成最后一次刷新，则返回
            if (objectMentionCounterMuti.Finished && lastRefresh) return;

            //每隔refreshFrame刷新一次
            currentFrame++;
            if (currentFrame != refreshFrame) return;
            currentFrame = -1;

            //完成统计后进行最后一次刷新
            if (objectMentionCounterMuti.Finished) lastRefresh = true;

            TrueUpdate();
        }

        private void TrueUpdate()
        {
            for (int i = 0; i < characters.Length; i++)
            {
                int characterId = characters[i].id;
                items[characterId].Count = objectMentionCounterMuti.MentionedCountDictionary.ContainsKey(characterId) ? objectMentionCounterMuti.MentionedCountDictionary[characterId] : 0;
            }

            int sum = objectMentionCounterMuti.MentionedCountDictionary.Sum(kvp => kvp.Value);
            txtTotal.text = $"{sum} + Unidentified {objectMentionCounterMuti.UnidentifiedMentionCount}";
        }
    }
}