using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.DataStructures
{
    public abstract class CountManager
    {
        public abstract CountMatrix[] CountMatrices { get; }

        /// <summary>
        /// 获取以剧情类型分类的统计矩阵 
        /// </summary>
        /// <returns>以剧情类型为键，对应类型的统计矩阵列表为值的字典</returns>
        public Dictionary<string, List<CountMatrix>> GetCountMatrixByType()
        {
            Dictionary<string, List<CountMatrix>> dictionary = new Dictionary<string, List<CountMatrix>>();
            foreach (var countMatrix in CountMatrices)
            {
                if (!dictionary.ContainsKey(countMatrix.Chapter.ChapterType))
                {
                    dictionary[countMatrix.Chapter.ChapterType] = new List<CountMatrix>();
                }
                dictionary[countMatrix.Chapter.ChapterType].Add(countMatrix);
            }
            return dictionary;
        }
    }
}