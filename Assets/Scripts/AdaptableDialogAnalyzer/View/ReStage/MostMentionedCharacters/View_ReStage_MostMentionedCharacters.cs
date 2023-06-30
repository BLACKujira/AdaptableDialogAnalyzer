using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Games.ReStage;
using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.ReStage
{
    public class View_ReStage_MostMentionedCharacters : MonoBehaviour
    {
        [Header("Components")]
        public View_ReStage_MostMentionedCharacters_Item itemMostAll;
        public View_ReStage_MostMentionedCharacters_Item itemMostDifferentUnit;
        [Header("Settings")]
        public bool passZero = true;
        public bool passSelf = true;
        public int speakerId = 1;
        [Header("Adapter")]
        public MentionedCountManagerLoader mentionedCountManagerLoader;

        MentionedCountManager mentionedCountManager;

        private void Start()
        {
            mentionedCountManager = mentionedCountManagerLoader.MentionedCountManager;
            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;

            List<CharacterMentionStats> characterMentionStatsList = mentionedCountManager.GetMentionStatsList(speakerId, true, true);
            int serifCount = mentionedCountManager.CountSerif(speakerId);

            CharacterMentionStats mostAll = characterMentionStatsList
                .OrderBy(cms => -cms.Total)
                .First();

            CharacterMentionStats mostDifferentUnit = characterMentionStatsList
                .Where(cms => !ReStageHelper.IsInSameGroup(cms.SpeakerId, cms.MentionedPersonId))
                .OrderBy(cms => -cms.Total)
                .First();

            Character charSpeaker = characterDefinition[speakerId];
            Character charMostAll = characterDefinition[mostAll.MentionedPersonId];
            Character charDifferentUnit = characterDefinition[mostDifferentUnit.MentionedPersonId];

            string colorSpeaker = ColorUtility.ToHtmlStringRGB(charSpeaker.color);
            string colorMostAll = ColorUtility.ToHtmlStringRGB(charMostAll.color);
            string colorDifferentUnit = ColorUtility.ToHtmlStringRGB(charDifferentUnit.color);

            itemMostAll.SetData(mostAll.SpeakerId, mostAll.MentionedPersonId,
                $"�ἰ������ࣺ<color=#{colorMostAll}>{charMostAll.name}</color>",
                $@"��<color=#{colorSpeaker}>{charSpeaker.Namae}</color>��{serifCount}��̨����
���ἰ<color=#{colorMostAll}>{charMostAll.Namae}</color>{mostAll.Total}�Σ�ռ��{(float)mostAll.Total / serifCount * 100:0.0}%
ƽ��ÿ{serifCount / mostAll.Total:0.0}��̨���ᵽһ��<color=#{colorMostAll}>{charMostAll.Namae}</color>");

            itemMostDifferentUnit.SetData(mostDifferentUnit.SpeakerId, mostDifferentUnit.MentionedPersonId,
                $"������ἰ��ࣺ<color=#{colorDifferentUnit}>{charDifferentUnit.name}</color>",
                $@"��<color=#{colorSpeaker}>{charSpeaker.Namae}</color>��{serifCount}��̨����
���ἰ<color=#{colorDifferentUnit}>{charDifferentUnit.Namae}</color>{mostDifferentUnit.Total}�Σ�ռ��{(float)mostDifferentUnit.Total / serifCount * 100:0.0}%
ƽ��ÿ{serifCount / mostDifferentUnit.Total:0.0}��̨���ᵽһ��<color=#{colorDifferentUnit}>{charDifferentUnit.Namae}</color>");
        }
    }
}