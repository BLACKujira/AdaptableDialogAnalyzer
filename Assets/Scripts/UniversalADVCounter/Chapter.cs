using System.IO;

namespace UniversalADVCounter
{
    /// <summary>
    /// 游戏中的一篇剧情、包含原始文件的名字和原始文件生成的对象
    /// </summary>
    /// <typeparam name="T">原始文件生成的对象类型</typeparam>
    public abstract class Chapter<T>
    {
        private readonly string fileName;
        protected T originalObject;

        public Chapter(string filePath)
        {
            this.fileName = Path.GetFileName(filePath);
            Initialize(filePath);
        }

        /// <summary>
        /// 原始文件名
        /// </summary>
        public string FileName => fileName;
        /// <summary>
        /// 由原始文件生成的对象
        /// </summary>
        public T OriginalObject => originalObject;
        /// <summary>
        /// 重载此函数以生成原始对象
        /// </summary>
        public abstract void Initialize(string filePath);
        /// <summary>
        /// 重载此方法以获取此剧情的基础对话信息
        /// </summary>
        /// <returns></returns>
        public virtual BasicTalkSnippet[] GetTalkSnippets() => null;
    }
}