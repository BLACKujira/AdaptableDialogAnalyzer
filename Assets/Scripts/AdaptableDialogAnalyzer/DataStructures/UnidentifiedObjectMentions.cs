namespace AdaptableDialogAnalyzer.DataStructures
{
    /// <summary>
    /// 一个{对象}的歧义名称的匹配记录
    /// </summary>
    [System.Serializable]
    public class UnidentifiedObjectMentions : ObjectMentionedCountRow
    {
        public string unidentifiedName;

        public UnidentifiedObjectMentions(string unidentifiedName) : base(0)
        {
            this.unidentifiedName = unidentifiedName;
        }
    }
}