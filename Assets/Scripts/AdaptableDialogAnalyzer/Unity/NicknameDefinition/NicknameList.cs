using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 每一个角色的昵称，在集合中的位置应与角色ID相同
    /// </summary>
    [System.Serializable]
    public class NicknameList
    {
        public List<string> nicknames;

        public string this[int index] => nicknames[index];
    }
}