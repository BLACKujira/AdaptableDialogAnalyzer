using System;
using UnityEngine;

namespace AdaptableDialogAnalyzer
{
    /// <summary>
    /// 游戏中的一篇剧情、包含原始文件的名字和原始文件生成的对象
    /// </summary>
    public abstract class Chapter
    {
        /// <summary>
        /// 剧情的类型，通过读取器设置
        /// </summary>
        private string chapterType = "default";

        /// <summary>
        /// 剧情的标题，通过读取器设置
        /// </summary>
        private string chapterTitle = null;

        /// <summary>
        /// 剧情的唯一ID，通过读取器设置
        /// </summary>
        private string chapterID = null;

        public string ChapterType
        {
            get => chapterType;
            set => chapterType = value;
        }
        public string ChapterTitle
        {
            get
            {
                if (string.IsNullOrEmpty(chapterTitle))
                {
                    Debug.LogWarning($"没有设置{ChapterID}的标题，在重载MultipleChapterLoader类的初始化方法时可以设置");
                    return "Untitled";
                }
                return chapterTitle;
            }
            set => chapterTitle = value;
        }
        public string ChapterID
        {
            get
            {
                if (string.IsNullOrEmpty(chapterID))
                {
                    throw new Exception("ChapterID没有设置，在重载MultipleChapterLoader类的初始化方法时，请确认每一个Chapter都设置了唯一的ChapterID");
                }
                return chapterID;
            }
            set => chapterID = value;
        }

        /// <summary>
        /// 重载此方法以获取此剧情的基础对话信息
        /// </summary>
        /// <returns></returns>
        public abstract BasicTalkSnippet[] GetTalkSnippets();
    }
}