using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    [CreateAssetMenu(menuName = "AdaptableDialogAnalyzer/ObjectNameDefinition/ObjectNameDefinition")]
    public class ObjectNameDefinition : ScriptableObject
    {
        [Header("对象的标记名称/标准名/学名")][SerializeField] string identifier;

        [Header("对象的通用名称")][SerializeField] List<string> commonNameList = new List<string>();

        [Header("角色对此对象的特别名称")][SerializeField] List<SpecificObjectNameList> specificNameLists;

        [Header("指代不明的名称")][SerializeField] List<string> unidentifiedNameList;

        public string Identifier => identifier;
        public List<string> CommonNameList => commonNameList;
        public List<SpecificObjectNameList> SpecificNameLists => specificNameLists;
        public List<string> UnidentifiedNameList => unidentifiedNameList;

        /// <summary>
        /// 未找到时返回null 
        /// </summary>
        public SpecificObjectNameList GetSpecificNameList(int speakerId)
        {
            foreach (var nameList in specificNameLists)
            {
                if (nameList.speakerId == speakerId) return nameList;
            }
            return null;
        }
    }
}
