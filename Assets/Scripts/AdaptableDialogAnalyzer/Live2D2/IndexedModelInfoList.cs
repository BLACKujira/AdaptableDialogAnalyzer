using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Live2D2
{
    /// <summary>
    /// 用于方便复用的ScriptableObject型字符串列表
    /// </summary>
    [CreateAssetMenu(menuName = "AdaptableDialogAnalyzer/Live2D2/ModelInfoList")]
    public class IndexedModelInfoList : ScriptableObject
    {
        public List<IndexedModelInfo> IndexedModelInfos = new List<IndexedModelInfo>();

        public IndexedModelInfoList(List<IndexedModelInfo> indexedModelInfos)
        {
            IndexedModelInfos = indexedModelInfos;
        }

        public IndexedModelInfo this[int index] => IndexedModelInfos.Where(i=>i.index ==  index).FirstOrDefault();
    }
}