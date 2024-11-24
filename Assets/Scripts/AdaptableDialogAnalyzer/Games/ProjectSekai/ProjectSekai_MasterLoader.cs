using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.ProjectSekai
{
    public class ProjectSekai_MasterLoader : MonoBehaviour
    {
        public string masterFolder;

        MasterEvent[] events;
        public MasterEvent[] Events => events ??= LoadMaster<MasterEvent>("events");

        MasterEventStory[] eventStories;
        public MasterEventStory[] EventStories => eventStories ??= LoadMaster<MasterEventStory>("eventStories");

        MasterCharacter3D[] masterCharacter3Ds;
        public MasterCharacter3D[] MasterCharacter3Ds => masterCharacter3Ds ??= LoadMaster<MasterCharacter3D>("character3Ds");

        /// <summary>
        /// ��ȡָ�����Ƶ����ݱ�
        /// </summary>
        T[] LoadMaster<T>(string masterName)
        {
            string file = Path.Combine(masterFolder, $"{masterName}.json");
            if(!File.Exists(file))
            {
                Debug.LogError($"δ�ҵ����ݱ��ļ�{file}");
            }

            string json = File.ReadAllText(file);
            T[] t = JsonHelper.getJsonArray<T>(json);
            return t;
        }
    }
}