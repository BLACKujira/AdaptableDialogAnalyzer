using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Games.BanGDream;
using AdaptableDialogAnalyzer.Games.ProjectSekai;
using AdaptableDialogAnalyzer.Unity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_MostAnalysis_ItemMost : View_BanGDream_MostAnalysis_ItemTypeA
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

            characterMentionStatsList = characterMentionStatsList
                .OrderBy(cms => -cms.Total)
                .ToList();

            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;
            int mentionedPersonId = characterMentionStatsList[0].MentionedPersonId;

            string speaker = characterDefinition[speakerId].Namae;
            string mentionedPerson = characterDefinition[mentionedPersonId].Namae;

            int serifCount = mentionedCountManager.CountSerif(speakerId);
            int count = characterMentionStatsList[0].Total;
            float percent = (float)count / serifCount;

            txtDescription.text =
$@"在 {speaker} 的 {serifCount} 句台词中
一共提到 {mentionedPerson} {count} 次，占比 {percent * 100:00.00}%
也就是平均 {1 / percent:0.0} 句就会提到一次 {mentionedPerson}";

            infoBar.SetData(speakerId, mentionedPersonId, $"提及次数最多: {count}次");
            SetSdChara(speakerId, mentionedPersonId);
        }
    }
}