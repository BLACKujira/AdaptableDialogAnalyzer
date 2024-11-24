using AdaptableDialogAnalyzer.DataStructures;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 生成ObjectMentionedCountManager的统计结果
    /// </summary>
    public class GenerateOMCResult : TaskWindow
    {
        [Header("Components")]
        public ObjectMentionedCountManagerLoader mentionedCountManagerLoader;
        [Header("Settings")]
        public string saveFile;

        public void Start()
        {
            Generate();
        }

        public void Generate()
        {
            ObjectMentionedCountManager mentionedCountManager = mentionedCountManagerLoader.MentionedCountManager;
            List<SimpleMentionCountResultItem> simpleMentionCountResultItems = mentionedCountManager.MentionedCountDictionary.Select(kvp=>new SimpleMentionCountResultItem(kvp.Key, kvp.Value, mentionedCountManager.CountSerif(kvp.Key))).ToList();
            SimpleMentionCountResult simpleMentionCountResult = new SimpleMentionCountResult(simpleMentionCountResultItems);

            string json = JsonUtility.ToJson(simpleMentionCountResult, true);
            File.WriteAllText(saveFile, json);

            Priority = 1;
            Progress = "完成";
        }
    }
}