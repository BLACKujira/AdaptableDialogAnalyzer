using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.UIElements;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 实时信息显示界面
    /// </summary>
    public class CharacterMentionCounter_DisplayArea : MonoBehaviour
    {
        public CharacterMentionCounter characterMentionCounter;
        [Header("Components")]
        public EquidistantLayoutGenerator elgSpeaker;
        public EquidistantLayoutGenerator elgMentionedPerson;

        int selectedCharacterId = 0;

        Dictionary<int, CharacterMentionCounter_SpeakerItem> speakerItems = new Dictionary<int, CharacterMentionCounter_SpeakerItem>();
        Dictionary<int, CharacterMentionCounter_MentionedPersonItem> mentionedPersonItems = new Dictionary<int, CharacterMentionCounter_MentionedPersonItem>();

        public void Awake()
        {
            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;
            Character[] characters = characterDefinition.Characters;

            //初始化角色选择
            elgSpeaker.Generate(characters.Length, (gobj, id) =>
            {
                int characterId = characters[id].id;

                CharacterMentionCounter_SpeakerItem speakerItem = gobj.GetComponent<CharacterMentionCounter_SpeakerItem>();
                speakerItem.SetData(characterId);
                speakerItems[characterId] = speakerItem;
                speakerItem.button.onClick.AddListener(() =>
                {
                    SelectCharacter(characterId);
                });
            });

            //初始化信息显示
            elgMentionedPerson.Generate(characters.Length, (gobj, id) =>
            {
                int characterId = characters[id].id;

                CharacterMentionCounter_MentionedPersonItem mentionedPersonItem = gobj.GetComponent<CharacterMentionCounter_MentionedPersonItem>();
                mentionedPersonItem.SetData(characterId);
                mentionedPersonItems[characterId] = mentionedPersonItem;
            });

            SelectCharacter(0);
        }

        void SelectCharacter(int characterId)
        {
            selectedCharacterId = characterId;
            foreach (var speakerItem in speakerItems)
            {
                if (speakerItem.Value.Checked) speakerItem.Value.Checked = false;
            }
            speakerItems[characterId].Checked = true;
            GlobalColor.SetThemeColor(GlobalConfig.CharacterDefinition[characterId].color);
        }

        private void Update()
        {
            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;
            Character[] characters = characterDefinition.Characters;
            for (int i = 0; i < characters.Length; i++)
            {
                int characterId = characters[i].id;

                CharacterMentionStats characterMentionStats = characterMentionCounter.MentionedCountManager[selectedCharacterId, characterId];
                mentionedPersonItems[characterId].Count = characterMentionStats.Total;
            }
        }
    }
}