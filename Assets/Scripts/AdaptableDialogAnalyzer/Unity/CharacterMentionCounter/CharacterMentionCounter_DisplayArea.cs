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

        CharacterMentionCounter_SpeakerItem[] speakerItems;
        CharacterMentionCounter_MentionedPersonItem[] mentionedPersonItems;

        public void Awake()
        {
            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;
            speakerItems = new CharacterMentionCounter_SpeakerItem[characterDefinition.characters.Count];
            mentionedPersonItems = new CharacterMentionCounter_MentionedPersonItem[characterDefinition.characters.Count];

            //初始化角色选择
            elgSpeaker.Generate(characterDefinition.characters.Count, (gobj, id) =>
            {
                CharacterMentionCounter_SpeakerItem speakerItem = gobj.GetComponent<CharacterMentionCounter_SpeakerItem>();
                speakerItem.SetData(id);
                speakerItems[id] = speakerItem;
                speakerItem.Button.onClick.AddListener(() =>
                {
                    SelectCharacter(id);
                });
            });

            //初始化信息显示
            elgMentionedPerson.Generate(characterDefinition.characters.Count, (gobj, id) =>
            {
                CharacterMentionCounter_MentionedPersonItem mentionedPersonItem = gobj.GetComponent<CharacterMentionCounter_MentionedPersonItem>();
                mentionedPersonItem.SetData(id);
                mentionedPersonItems[id] = mentionedPersonItem;
            });

            SelectCharacter(0);
        }

        void SelectCharacter(int characterId)
        {
            selectedCharacterId = characterId;
            foreach (var speakerItem in speakerItems)
            {
                if (speakerItem.Checked) speakerItem.Checked = false;
            }
            speakerItems[characterId].Checked = true;
        }

        private void Update()
        {
            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;
            for (int i = 0; i < characterDefinition.characters.Count; i++)
            {
                CharacterMentionStats characterMentionStats = characterMentionCounter.MentionedCountManager[selectedCharacterId, i];
                mentionedPersonItems[i].Count = characterMentionStats.Total;
            }
        }
    }
}