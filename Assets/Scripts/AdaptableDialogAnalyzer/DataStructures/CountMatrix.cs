using System;

namespace AdaptableDialogAnalyzer.DataStructures
{
    [Serializable]
    public abstract class CountMatrix
    {
        /// <summary>
        /// 在编辑器中使用，当修改后变成True
        /// </summary>
        [NonSerialized] private bool hasChanged = false;

        /// <summary>
        /// 统计的故事章节，运行时由MentionedCountResultLoader获取，不参与序列化
        /// </summary>
        [NonSerialized] private Chapter chapter;

        /// <summary>
        /// 序列化一部分章节的信息，以便不读取章节的情况下使用某些功能
        /// </summary>
        public ChapterInfo chapterInfo;

        public Chapter Chapter
        {
            get
            {
                if (chapter == null) throw new Exception("未加载章节，请使用MentionCountEditor加载统计数据，或在代码中设置此属性");
                return chapter;
            }
            set => chapter = value;
        }
        public bool HasChanged { get => hasChanged; set => hasChanged = value; }

        /// <summary>
        /// chapter可以为null，但此类不能有无参构造函数
        /// </summary>
        public CountMatrix(Chapter chapter)
        {
            this.chapter = chapter;
            if (chapter != null) chapterInfo = new ChapterInfo(chapter);
        }
    }
}