using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 定义角色的昵称
    /// </summary>
    [CreateAssetMenu(menuName = "AdaptableDialogAnalyzer/NicknameDefinition/NicknameDefinition")]
    public class NicknameDefinition : ScriptableObject
    {
        /// <summary>
        /// 每一个角色对其他角色的昵称列表
        /// </summary>
        [Header("每一个角色对其他角色的昵称")] public List<NicknameMapping> nicknameMappings;
        /// <summary>
        /// 对某一个角色的通用昵称列表
        /// </summary>
        [Header("角色的通用昵称")] public NicknameMapping commonNicknameMapping;
        /// <summary>
        /// 指代不明的昵称列表
        /// </summary>
        [Header("指代不明的昵称")] public NicknameList unidentifiedNicknameList;

        public NicknameList this[int speakerId, int mentionedPersonId] => nicknameMappings[speakerId][mentionedPersonId];
    }
}