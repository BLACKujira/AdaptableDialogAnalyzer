using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using System.IO;

namespace AdaptableDialogAnalyzer.Games.Kimihane
{
    public class ChapterLoader_Folder_Kimihane_Ybn : ChapterLoader
    {
        public string folder;
        public Kimihane_LabelInfoLoader labelInfoLoader;

        public override Chapter[] InitializeChapters()
        {
            string[] files = Directory.GetFiles(folder);
            List<Chapter> chapters = new List<Chapter>();
            foreach (string file in files)
            {
                Chapter chapter = Chapter_Kimihane_Ybn.LoadText(File.ReadAllText(file), labelInfoLoader);
                chapter.ChapterID = Path.GetFileNameWithoutExtension(file);
                chapter.ChapterTitle = Path.GetFileNameWithoutExtension(file);

                if (chapter.GetTalkSnippets().Length == 0) chapter.ChapterType = "控制脚本";
                else chapter.ChapterType = "剧情脚本";

                chapters.Add(chapter);
            }
            return chapters.ToArray();
        }
    }
}