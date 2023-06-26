namespace AdaptableDialogAnalyzer.DataStructures
{
    /// <summary>
    /// һ�������ǳƵ�ƥ���¼
    /// </summary>
    [System.Serializable]
    public class UnidentifiedMentions : MentionedCountGrid
    {
        public string unidentifiedNickname;

        public UnidentifiedMentions(string unidentifiedNickname) : base(0)
        {
            this.unidentifiedNickname = unidentifiedNickname;
        }
    }
}