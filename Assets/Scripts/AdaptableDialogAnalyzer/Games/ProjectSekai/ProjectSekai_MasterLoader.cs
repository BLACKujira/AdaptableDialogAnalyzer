using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.ProjectSekai
{
    public class ProjectSekai_MasterLoader : MonoBehaviour
    {
        public string masterFolder;

        MasterEvent[] masterEvent;
        
        public MasterEvent[] MasterEvent
        {
            get
            {
                masterEvent ??= LoadMaster<MasterEvent>("events");
                return masterEvent;
            }
        }

        /// <summary>
        /// 读取指定名称的数据表
        /// </summary>
        T[] LoadMaster<T>(string masterName)
        {
            string file = Path.Combine(masterFolder, $"{masterName}.json");
            if(!File.Exists(file))
            {
                Debug.LogError($"未找到数据表文件{file}");
            }

            string json = File.ReadAllText(file);
            T[] t = JsonHelper.getJsonArray<T>(json);
            return t;
        }
    }
}