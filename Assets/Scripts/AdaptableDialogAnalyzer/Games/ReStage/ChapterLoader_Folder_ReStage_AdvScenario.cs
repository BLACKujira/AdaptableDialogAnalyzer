using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.ReStage
{
    public class ChapterLoader_Folder_ReStage_AdvScenario : ChapterLoader
    {
        public string folder;
        public bool passVoiceOnly = true;

        public override Chapter[] InitializeChapters()
        {
            string[] files = Directory.GetFiles(folder);
            List<Chapter> chapters = new List<Chapter>();

            int count = 0;

            foreach (string file in files)
            {
                Chapter_ReStage_AdvScenario chapter = Chapter_ReStage_AdvScenario.LoadText(File.ReadAllText(file));

                chapter.ChapterID = Path.GetFileNameWithoutExtension(file);
                chapter.ChapterTitle = $"{chapter.AdvScenario.Title1} {chapter.AdvScenario.Title2}";
                chapter.ChapterType = Chapter_ReStage_AdvScenario.GetTypeName(chapter.ChapterNameInfo);

                if (passVoiceOnly && chapter.IsVoiceOnly())
                {
                    Debug.Log(chapter.ChapterID);
                    continue;
                }

                chapters.Add(chapter);
                count++;
            }
            return chapters.ToArray();
        }
    }
}