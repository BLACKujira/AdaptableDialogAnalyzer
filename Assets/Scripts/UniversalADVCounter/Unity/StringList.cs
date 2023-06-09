using System.Collections.Generic;
using UnityEngine;

namespace UniversalADVCounter.Unity.CharacterEditor
{
    /// <summary>
    /// 用于方便复用的ScriptableObject型字符串列表
    /// </summary>
    [System.Serializable]
    public class StringList : ScriptableObject
    {
        /// <summary>
        /// 角色昵称
        /// </summary>
        public List<string> strings;
    }
}