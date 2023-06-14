using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using System.IO;

namespace AdaptableDialogAnalyzer.Games.Kimihane
{
    public class MultipleChapterLoader_Folder_Kimihane_Ybn : MultipleChapterLoader
    {
        public string folder;

        public override Chapter[] GetChapters()
        {
            string[] files = Directory.GetFiles(folder);
            List<Chapter> chapters = new List<Chapter>();
            foreach (string file in files)
            {
                chapters.Add(Chapter_Kimihane_Ybn.LoadText(File.ReadAllText(file)));
            }
            return chapters.ToArray();
        }
    }
}