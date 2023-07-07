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

        public virtual string ExtraInfo => "无附加信息";

        int serifCount = -1;
        public int SerifCount
        {
            get
            {
                if(serifCount < 0) 
                {
                    serifCount = TalkSnippets.Length;
                }
                return serifCount;
            }
        }

        BasicTalkSnippet[] talkSnippets = null;

        /// <summary>
        /// 此剧情的对话片段。已替代GetTalkSnippets，减少性能消耗。有缓存，非必要请勿修改内容
        /// </summary>
        public BasicTalkSnippet[] TalkSnippets
        {
            get
            {
                if(talkSnippets == null) talkSnippets = GetTalkSnippets();
                return talkSnippets;
            }
        }

        /// <summary>
        /// 重载此方法以获取此剧情的基础对话信息，每次都会重新获取，请注意性能消耗
        /// </summary>
        /// <returns></returns>
        public abstract BasicTalkSnippet[] GetTalkSnippets();
    }
}