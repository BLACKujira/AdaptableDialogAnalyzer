using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Games.BanGDream;
using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using System.Linq;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_MostAnalysis_ItemMostOtherUnit : View_BanGDream_MostAnalysis_ItemTypeA
    {
        protected override void Initialize(MentionedCountManager mentionedCountManager, int speakerId, bool mainCharacterOnly, bool passSelf)
        {
            List<CharacterMentionStats> characterMentionStatsList = mentionedCountManager.GetMentionStatsList(speakerId, false, passSelf);

            if (mainCharacterOnly)
            {
                characterMentionStatsList = characterMentionStatsList
                    .Where(cms => BanGDreamHelper.IsMainCharacter(cms.MentionedPersonId))
                    .ToList();
            }

            int countSize = characterMentionStatsList.Count;
            int mentionTotal = characterMentionStatsList.Sum(cms => cms.Total);

            characterMentionStatsList = characterMentionStatsList
                .Where(cms => BanGDreamHelper.GetCharacterBand(cms.SpeakerId) != BanGDreamHelper.GetCharacterBand(cms.MentionedPersonId))
                .OrderBy(cms => -cms.Total)
                .ToList();

            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;
            int mentionedPersonId = characterMentionStatsList[0].MentionedPersonId;

            string speaker = characterDefinition[speakerId].Namae;
            string mentionedPerson = characterDefinition[mentionedPersonId].Namae;

            int count = characterMentionStatsList[0].Total;
            float percent = (float)count / mentionTotal;

            txtDescription.text =
$@"在统计的 {mentionedCountManager.mentionedCountMatrices.Count} 篇剧情中
{speaker} 共提到其他 {countSize} 名角色 {mentionTotal} 次
其中提到 {mentionedPerson} {count} 次，占比 {percent * 100:00.00}%";

            infoBar.SetData(speakerId, mentionedPersonId, $"组合外提及次数最多: {count}次");
            SetSdChara(speakerId, mentionedPersonId);
        }
    }
}