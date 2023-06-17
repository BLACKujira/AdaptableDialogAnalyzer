using System;

namespace AdaptableDialogAnalyzer.DataStructures
{
    /// <summary>
    /// 序列化一部分章节的信息，以便不读取章节的情况下使用某些功能
    /// </summary>
    [Serializable]
    public class ChapterInfo
    {
        public string chapterID;
        public string chapterType;
        public string chapterTitle;

        public ChapterInfo(Chapter chapter)
        {
            chapterID = chapter.ChapterID;
            chapterType = chapter.ChapterType;
            chapterTitle = chapter.ChapterTitle;
        }
    }
}