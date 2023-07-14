using AdaptableDialogAnalyzer.DataStructures;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    public class MentionedCountMatrixMerger : TaskWindow
    {
        [Header("Settings")]
        public string saveFile;
        [Header("Adapter")]
        public MentionedCountManagerLoader mentionedCountManagerLoader;

        private void Start()
        {
            MentionedCountManager mentionedCountManager = mentionedCountManagerLoader.MentionedCountManager;
            MentionedCountMatrix mergedMatrix = new MentionedCountMatrix(null);

            Character[] characters = GlobalConfig.CharacterDefinition.Characters;

            for (int i = 0; i < characters.Length; i++)
            {
                for (int j = 0; j < characters.Length; j++)
                {
                    Character speaker = characters[i];
                    Character mentionedPerson = characters[j];

                    CharacterMentionStats characterMentionStats = mentionedCountManager[speaker.id, mentionedPerson.id];
                    mergedMatrix[speaker.id, mentionedPerson.id].overrideCount = characterMentionStats.Total;
                }
            }

            File.WriteAllText(saveFile, JsonUtility.ToJson(mergedMatrix));
            Priority = 1;
            Progress = "Íê³É";
        }
    }
}