using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using System.IO;

namespace AdaptableDialogAnalyzer.Games.Kimihane
{
    public class ChapterLoader_Folder_Kimihane_Ybn : ChapterLoader
    {
        public string folder;

        public override Chapter[] InitializeChapters()
        {
            string[] files = Directory.GetFiles(folder);
            List<Chapter> chapters = new List<Chapter>();
            foreach (string file in files)
            {
                Chapter chapter = Chapter_Kimihane_Ybn.LoadText(File.ReadAllText(file));
                chapter.ChapterID = Path.GetFileNameWithoutExtension(file);
                chapter.ChapterTitle = Path.GetFileNameWithoutExtension(file);
                chapters.Add(chapter);
            }
            return chapters.ToArray();
        }
    }
}