using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 用于方便复用的ScriptableObject型字符串列表
    /// </summary>
    [CreateAssetMenu(menuName = "AdaptableDialogAnalyzer/StringList")]
    public class StringList : ScriptableObject
    {
        public List<string> strings;

        public string this[int index] => strings[index];
    }
}