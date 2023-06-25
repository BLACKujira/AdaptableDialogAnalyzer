using System.Collections.Generic;
using System.IO;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 读取剧情缓存
    /// </summary>
    public class ChapterLoader_ChapterCache : ChapterLoader
    {
        public string folder;

        public override Chapter[] InitializeChapters()
        {
            string[] files = Directory.GetFiles(folder);
            List<Chapter> chapters = new List<Chapter>();
            foreach (string file in files) 
            {
                Chapter chapter = Chapter_ChapterCache.LoadText(File.ReadAllText(file));
                chapters.Add(chapter);
            }
            return chapters.ToArray();
        }
    }
}