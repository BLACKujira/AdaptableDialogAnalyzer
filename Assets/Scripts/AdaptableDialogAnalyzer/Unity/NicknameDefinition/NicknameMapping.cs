using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 定义角色对其他角色的昵称，在集合中的位置应与角色ID相同
    /// </summary>
    [CreateAssetMenu(menuName = "AdaptableDialogAnalyzer/NicknameDefinition/NicknameCollection")]
    public class NicknameMapping : ScriptableObject
    {
        public List<NicknameList> nicknameLists;

        public NicknameList this[int mentionedPersonId] => nicknameLists[mentionedPersonId];
    }
}