using AdaptableDialogAnalyzer.Unity;

namespace AdaptableDialogAnalyzer
{
    /// <summary>
    /// 读取剧情缓存
    /// </summary>
    public class Chapter_ChapterCache : Chapter
    {
        ChapterCache chapterCache;
        public ChapterCache ChapterCache => chapterCache;

        public override string ExtraInfo => chapterCache.extraInfo;

        public override BasicTalkSnippet[] GetTalkSnippets()
        {
            return chapterCache.talkSnippets;
        }

        public static Chapter LoadText(string rawChapter)
        {
            ChapterCache chapterCache = ChapterCache.LoadData(rawChapter);

            Chapter_ChapterCache chapter = new Chapter_ChapterCache();
            chapter.chapterCache = chapterCache;
            chapter.ChapterID = chapterCache.chapterID;
            chapter.ChapterType = chapterCache.chapterType;
            chapter.ChapterTitle = chapterCache.chapterTitle;

            return chapter;
        }
    }
}