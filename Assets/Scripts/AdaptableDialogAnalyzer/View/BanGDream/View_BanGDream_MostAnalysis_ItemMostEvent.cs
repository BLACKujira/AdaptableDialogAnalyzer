using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Games.BanGDream;
using AdaptableDialogAnalyzer.Unity;
using Google.Protobuf.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using EventStoryInfo = AdaptableDialogAnalyzer.Games.BanGDream.EventStoryInfo;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_MostAnalysis_ItemMostEvent : View_BanGDream_MostAnalysis_ItemTypeB
    {
        [Header("Loader")]
        public BanGDream_EventLogoLoader eventLogoLoader;

        protected override void Initialize(MentionedCountManager mentionedCountManager, int speakerId, bool mainCharacterOnly, bool passSelf)
        {
            (EventStoryInfo ev, MentionedCountMatrix m)[][] evCountMatrices= mentionedCountManager.mentionedCountMatrices
                .Where(m => m.chapterInfo.chapterType.Equals(ChapterLoader_Folder_BanGDream_Scenario.TYPE_EVENTSTORY))
                .Select(m => (BanGDreamHelper.GetEventStoryInfo(m.chapterInfo.chapterID), m))
                .Where(t => t.Item1 != null)
                .GroupBy(t=>t.Item1.eventId)
                .Select(g=>g.ToArray())
                .ToArray();

            Character[] characters = GlobalConfig.CharacterDefinition.Characters
                .Where(c=> !mainCharacterOnly || BanGDreamHelper.IsMainCharacter(c.id))
                .Where(c=> !passSelf || c.id != speakerId)
                .ToArray();

            List<(int eventId, int characterId, int mentionCount)> mentionCounts = new List<(int, int, int)>();

            foreach ((EventStoryInfo ev, MentionedCountMatrix m)[] evCountMatrix in evCountMatrices)
            {
                int eventId = evCountMatrix[0].ev.eventId;

                foreach (Character character in characters)
                {
                    int mentionCount = 0;

                    for (int i = 0; i < evCountMatrix.Length; i++)
                    {
                        MentionedCountGrid grid = evCountMatrix[i].m[speakerId, character.id];
                        mentionCount += (grid != null) ? grid.Count : 0;
                    }

                    mentionCounts.Add((eventId, character.id, mentionCount));
                }
            }

            (int eventId, int characterId, int mentionCount) mostMention = mentionCounts
                .OrderByDescending(t => t.mentionCount)
                .First();

            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;
            int mentionedPersonId = mostMention.characterId;

            string speaker = characterDefinition[speakerId].Namae;
            string mentionedPerson = characterDefinition[mentionedPersonId].Namae;

            int mostEventId = mostMention.eventId;
            int serifCount = mentionedCountManager.CountSerif(speakerId);
            int count = mostMention.mentionCount;

            MapField<uint, MasterEvent> events = eventLogoLoader.suiteMasterLoader.SuiteMasterGetResponse.MasterEventMapForExchanges.Entries;
            string eventName = events.ContainsKey((uint)mostEventId) ? events[(uint)mostEventId].EventName : "未知活动";

            txtDescription.text =
$@"在第 {mostEventId} 期活动 
{eventName} 中，
{speaker} 一共提到了 {mentionedPerson} {count} 次";

            infoBar.SetData(speakerId, mentionedPersonId, $"单次活动提及次数最多: {count}次");
            rimgIcon.texture = eventLogoLoader.GetLogo(mostEventId);
        }
    }
}