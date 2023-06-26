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
        [Header("每一个角色对其他角色的昵称")] [SerializeField] List<NicknameMapping> specificNicknameMappings;
        /// <summary>
        /// 对某一个角色的通用昵称列表
        /// </summary>
        [Header("角色的通用昵称")][SerializeField] NicknameMapping commonNicknameMapping;
        /// <summary>
        /// 指代不明的昵称列表
        /// </summary>
        [Header("指代不明的昵称")][SerializeField] NicknameList unidentifiedNicknameList;

        public NicknameMapping[] SpecificNicknameMappings => specificNicknameMappings.ToArray();
        public NicknameMapping CommonNicknameMapping => commonNicknameMapping;
        public NicknameList UnidentifiedNicknameList => unidentifiedNicknameList;

        /// <summary>
        /// 返回某说话角色的角色特殊昵称列表未找到时返回null 
        /// </summary>
        public NicknameMapping GetSpecificNicknameMapping(int speakerId)
        {
            foreach (var nicknameMapping in specificNicknameMappings)
            {
                if (nicknameMapping.speakerId == speakerId) return nicknameMapping;
            }
            return null;
        }
    }
}