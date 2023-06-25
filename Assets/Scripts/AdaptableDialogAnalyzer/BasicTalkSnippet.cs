namespace AdaptableDialogAnalyzer
{
    /// <summary>
    /// 基础的对话片段、对应原始文件中的一句台词
    /// </summary>
    [System.Serializable]
    public class BasicTalkSnippet
    {
        //由于需要序列化没有设置成私有，最好不要使用
        public int refIdx;
        public int speakerId;
        public string displayName;
        public string content;

        /// <summary>
        /// 构造函数，没什么特别的
        /// </summary>
        /// <param name="refIdx">标记此对话在原文件中的某个位置，可以为行数或此对话在数组中的位置</param>
        /// <param name="speakerId">当前台词所属角色的ID、若原文件中未写明可通过对话窗口显示的名称或显示的立绘获取</param>
        /// <param name="content">对话内容（不含说话者的名字）</param>
        public BasicTalkSnippet(int refIdx, int speakerId, string content, string displayName = null)
        {
            this.refIdx = refIdx;
            this.speakerId = speakerId;
            this.content = content;
            this.displayName = displayName;
        }

        /// <summary>
        /// 标记此对话在原文件中的某个位置，可以为行数或在此对话数组中的位置
        /// </summary>
        public int RefIdx => refIdx;
        /// <summary>
        /// 当前台词所属角色的ID，与角色列表中的一致
        /// </summary>
        public int SpeakerId => speakerId;
        /// <summary>
        /// 对话内容（不含说话者的名字）
        /// </summary>
        public string Content => content;
        /// <summary>
        /// 窗口显示名称，用于查看，如未设置则为角色定义中的角色名
        /// </summary>
        public string DisplayName => displayName;

        public BasicTalkSnippet Clone()
        {
            return new BasicTalkSnippet(RefIdx, SpeakerId, Content, DisplayName);
        }
    }
}