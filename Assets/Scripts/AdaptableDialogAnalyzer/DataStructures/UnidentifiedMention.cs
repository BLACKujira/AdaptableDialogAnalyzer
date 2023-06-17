using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.DataStructures
{
    /// <summary>
    /// 一个多义昵称的匹配记录
    /// </summary>
    [System.Serializable]
    public class UnidentifiedMentions : MentionedCountGrid
    {
        public string unidentifiedNickname;

        public UnidentifiedMentions(string unidentifiedNickname)
        {
            this.unidentifiedNickname = unidentifiedNickname;
        }
    }
}