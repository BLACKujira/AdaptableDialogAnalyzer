using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Games.BanGDream;
using AdaptableDialogAnalyzer.Unity;
using Google.Protobuf.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_MostAnalysis_ItemMostCard : View_BanGDream_MostAnalysis_ItemTypeB
    {
        [Header("Loader")]
        public BanGDream_CardIconLoader cardIconLoader;
        public View_BanGDream_CardIcon cardIcon;

        protected override void Initialize(MentionedCountManager mentionedCountManager, int speakerId, bool mainCharacterOnly, bool passSelf)
        {
            (CardStoryInfo card, MentionedCountMatrix m)[] cardCountMatrices = mentionedCountManager.mentionedCountMatrices
                .Where(m => m.chapterInfo.chapterType.Equals(ChapterLoader_Folder_BanGDream_Scenario.TYPE_CARDSTORY))
                .Select(m => (BanGDreamHelper.GetCardStoryInfo(m.chapterInfo.chapterID), m))
                .Where(t => t.Item1 != null)
                .ToArray();

            Character[] characters = GlobalConfig.CharacterDefinition.Characters
                .Where(c => !mainCharacterOnly || BanGDreamHelper.IsMainCharacter(c.id))
                .Where(c => !passSelf || c.id != speakerId)
                .ToArray();

            List<(CardStoryInfo card, int characterId, int mentionCount)> mentionCounts = new List<(CardStoryInfo, int, int)>();

            foreach (var cardCountMatrix in cardCountMatrices)
            {
                foreach (Character character in characters)
                {
                    int mentionCount = 0;

                    MentionedCountGrid grid = cardCountMatrix.m[speakerId, character.id];
                    mentionCount += (grid != null) ? grid.Count : 0;

                    if (mentionCount > 0)
                    {
                        mentionCounts.Add((cardCountMatrix.card, character.id, mentionCount));
                    }
                }
            }

            var mostMention = mentionCounts
                .OrderByDescending(t => t.mentionCount)
                .First();

            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;
            int mentionedPersonId = mostMention.characterId;

            string speaker = characterDefinition[speakerId].Namae;
            string mentionedPerson = characterDefinition[mentionedPersonId].Namae;

            CardStoryInfo cardInfo = mostMention.card;
            int serifCount = mentionedCountManager.CountSerif(speakerId);
            int count = mostMention.mentionCount;

            MapField<uint, MasterCharacterSituation> cards = cardIconLoader.masterLoader.SuiteMasterGetResponse.MasterCharacterSituationMap.Entries;
            string cardCharacter;
            string cardName;
            string storyName;
            if (cards.ContainsKey((uint)cardInfo.cardId))
            {
                MasterCharacterSituation masterCharacterSituation = cards[(uint)cardInfo.cardId];
                cardCharacter = characterDefinition[(int)masterCharacterSituation.CharacterId].Namae;
                cardName = masterCharacterSituation.Prefix;
                storyName = masterCharacterSituation.Episodes.Entries
                    .Where(e=>e.EpisodeType == cardInfo.EpisodeType)
                    .FirstOrDefault()?.Title ?? "未知剧情";
            }
            else
            {
                cardCharacter = "?";
                cardName = "未知卡片";
                storyName = "未知剧情";
            }

            txtDescription.text =
$@"在 {cardCharacter} 的卡片 {cardName} 的剧情
「{storyName}」 中，
{speaker} 一共提到了 {mentionedPerson} {count} 次";

            infoBar.SetData(speakerId, mentionedPersonId, $"单篇卡片剧情提及次数最多: {count}次");
            if(cards.ContainsKey((uint)cardInfo.cardId))
            {
                MasterCharacterSituation masterCharacterSituation = cards[(uint)cardInfo.cardId];
                cardIcon.SetData(masterCharacterSituation);
                cardIcon.rimgCardPreview.texture = cardIconLoader.GetIcon((int)masterCharacterSituation.SituationId);
            }
        }
    }
}