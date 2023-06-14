using System.IO;

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
        public string chapterType = "default";

        /// <summary>
        /// 剧情的唯一ID，通过读取器设置
        /// </summary>
        public string chapterID = null;

        /// <summary>
        /// 重载此方法以获取此剧情的基础对话信息
        /// </summary>
        /// <returns></returns>
        public abstract BasicTalkSnippet[] GetTalkSnippets();
    }
}