using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    [CreateAssetMenu(menuName = "AdaptableDialogAnalyzer/ObjectNameDefinition/ObjectNameDefinition")]
    public class ObjectNameDefinition : ScriptableObject
    {
        [Header("����ı������/��׼��/ѧ��")][SerializeField] string identifier;

        [Header("�����ͨ������")][SerializeField] List<string> commonNameList = new List<string>();

        [Header("��ɫ�Դ˶�����ر�����")][SerializeField] List<SpecificObjectNameList> specificNameLists;

        [Header("ָ������������")][SerializeField] List<string> unidentifiedNameList;

        public string Identifier => identifier;
        public List<string> CommonNameList => commonNameList;
        public List<SpecificObjectNameList> SpecificNameLists => specificNameLists;
        public List<string> UnidentifiedNameList => unidentifiedNameList;

        /// <summary>
        /// δ�ҵ�ʱ����null 
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
