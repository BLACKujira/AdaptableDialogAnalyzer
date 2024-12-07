using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Games.BanGDream;
using AdaptableDialogAnalyzer.Live2D2;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 负责生成角色的最常用表情和动作统计
    /// </summary>
    public class BanGDream_GenerateOMCDisplayAnimationSet : TaskWindow
    {
        [Header("Components")]
        public ObjectMentionedCountManagerLoader mentionedCountManagerLoader;
        [Header("Settings")]
        public string modelPath = "Assets/View/BanGDream/live2d";
        public string saveFile = "Assets/View/BanGDream/prefab/TypeC/AnimationList.asset";

        List<ObjectMentionedCountMatrix> countMatrices; // 存储符合条件的提及次数矩阵

        private void Start()
        {
            Process();
        }

        // 主处理逻辑，统计每个角色的最常用表情和动作
        void Process()
        {
            // 获取需要的章节数据
            countMatrices = GetRequiredChapter();

            Dictionary<int, string[]> animations2 = new Dictionary<int, string[]>();

            // 遍历角色 ID (假设 1 到 40 是角色 ID 范围)
            for (int i = 1; i <= 40; i++)
            {
                // 获取该角色的所有对话片段
                List<ScenarioSnippetTalk> scenarioSnippetTalks = GetAllTalkSnippets(i);

                // 统计最常用表情和动作
                Dictionary<string, int> mostUsedExpression = GetMostUsedExpression(scenarioSnippetTalks, i);
                Dictionary<string, int> mostUsedMotion = GetMostUsedMotion(scenarioSnippetTalks, i);

                // 移除默认表情
                string[] removeMotions = { "","idle01", "default" };
                foreach (var motionName in removeMotions)
                {
                    if (mostUsedExpression.ContainsKey(motionName) && mostUsedExpression.Count > 1) mostUsedExpression.Remove(motionName);
                    if (mostUsedMotion.ContainsKey(motionName) && mostUsedMotion.Count > 1) mostUsedMotion.Remove(motionName);
                }

                // 获取使用次数最多的表情和动作的名称
                string mostUsedExpressionName = mostUsedExpression.OrderByDescending(kvp => kvp.Value).FirstOrDefault().Key;
                string mostUsedMotionName = mostUsedMotion.OrderByDescending(kvp => kvp.Value).FirstOrDefault().Key;

                // 输出统计结果
                Debug.Log($"角色{i}的最常用表情是{mostUsedExpressionName} ({mostUsedExpression[mostUsedExpressionName]}),最常用动作是{mostUsedMotionName} ({mostUsedMotion[mostUsedMotionName]})");

                animations2.Add(i, new string[] { mostUsedExpressionName, mostUsedMotionName });
            }
            GenerateScriptableObject(animations2);
        }

        // 获取符合要求的章节的提及次数矩阵
        List<ObjectMentionedCountMatrix> GetRequiredChapter()
        {
            // 定义需要的章节类型
            string[] requireChapterType = {
                ChapterLoader_Folder_BanGDream_Scenario.TYPE_BANDSTORY, // 乐队故事
                ChapterLoader_Folder_BanGDream_Scenario.TYPE_AREATALK,  // 区域对话
                ChapterLoader_Folder_BanGDream_Scenario.TYPE_CARDSTORY, // 卡牌故事
                ChapterLoader_Folder_BanGDream_Scenario.TYPE_EVENTSTORY // 活动故事
            };

            // 过滤提及次数矩阵，保留属于指定章节类型的矩阵
            return mentionedCountManagerLoader.MentionedCountManager.mentionedCountMatrices
                .Where(m => requireChapterType.Contains(m.chapterInfo.chapterType)).ToList();
        }

        // 获取指定角色的所有对话片段
        List<ScenarioSnippetTalk> GetAllTalkSnippets(int characterId)
        {
            List<ScenarioSnippetTalk> scenarioSnippetTalks = new List<ScenarioSnippetTalk>();

            // 遍历每个提及次数矩阵
            foreach (var countMatrix in countMatrices)
            {
                // 跳过未提及该角色的矩阵
                if (countMatrix[characterId] == null || countMatrix[characterId].Count == 0) continue;

                // 获取章节数据并确保类型正确
                Chapter_BanGDream_Scenario chapter_BanGDream_Scenario = countMatrix.Chapter as Chapter_BanGDream_Scenario;
                if (chapter_BanGDream_Scenario == null) continue;

                // 获取与角色相关的对话片段索引
                foreach (var refIdx in countMatrix[characterId].MatchedIndexSet)
                {
                    // 根据索引找到对话数据并加入列表
                    ScenarioSnippet scenarioSnippet = chapter_BanGDream_Scenario.ScenarioSceneData.snippets[refIdx];
                    ScenarioSnippetTalk scenarioSnippetTalk = chapter_BanGDream_Scenario.ScenarioSceneData.talkData[scenarioSnippet.referenceIndex];
                    scenarioSnippetTalks.Add(scenarioSnippetTalk);
                }
            }

            return scenarioSnippetTalks;
        }

        // 统计最常用的表情
        Dictionary<string, int> GetMostUsedExpression(List<ScenarioSnippetTalk> scenarioSnippetTalks, int characterId)
        {
            Dictionary<string, int> mostUsedExpression = new Dictionary<string, int>();

            // 遍历对话片段
            foreach (var scenarioSnippetTalk in scenarioSnippetTalks)
            {
                // 找到与指定角色相关的动作信息
                ScenarioSnippetTalkMotion scenarioSnippetTalkMotion = scenarioSnippetTalk.motions.Where(m => m.characterId == characterId).FirstOrDefault();

                // 如果没有找到，跳过
                if (scenarioSnippetTalkMotion == null) continue;

                // 将表情名加入统计字典
                if (!mostUsedExpression.ContainsKey(scenarioSnippetTalkMotion.expressionName))
                {
                    mostUsedExpression.Add(scenarioSnippetTalkMotion.expressionName, 0);
                }

                // 表情使用次数增加
                mostUsedExpression[scenarioSnippetTalkMotion.expressionName] += 1;
            }

            return mostUsedExpression;
        }

        // 统计最常用的动作
        Dictionary<string, int> GetMostUsedMotion(List<ScenarioSnippetTalk> scenarioSnippetTalks, int characterId)
        {
            Dictionary<string, int> mostUsedMotion = new Dictionary<string, int>();

            // 遍历对话片段
            foreach (var scenarioSnippetTalk in scenarioSnippetTalks)
            {
                // 找到与指定角色相关的动作信息
                ScenarioSnippetTalkMotion scenarioSnippetTalkMotion = scenarioSnippetTalk.motions.Where(m => m.characterId == characterId).FirstOrDefault();

                // 如果没有找到，跳过
                if (scenarioSnippetTalkMotion == null) continue;

                // 将动作名加入统计字典
                if (!mostUsedMotion.ContainsKey(scenarioSnippetTalkMotion.motionName))
                {
                    mostUsedMotion.Add(scenarioSnippetTalkMotion.motionName, 0);
                }

                // 动作使用次数增加
                mostUsedMotion[scenarioSnippetTalkMotion.motionName] += 1;
            }

            return mostUsedMotion;
        }

        void GenerateScriptableObject(Dictionary<int, string[]> animations2)
        {
#if UNITY_EDITOR // 此部分代码仅在Unity编辑器环境下执行
            // 定义一个列表，用于存储所有的动画序列对象
            List<IndexedLive2D2AnimationSequence> indexedLive2D2AnimationSequences = new List<IndexedLive2D2AnimationSequence>();

            // 循环生成每个角色的动画数据（假设角色ID从1到40）
            for (int i = 1; i <= 40; i++)
            {
                // 定义角色模型的基本路径，文件夹名为角色编号
                string basePath = Path.Combine(modelPath, $"{i:000}_general");

                // 定义角色的基础表情文件和动作文件路径
                string expression1Path = Path.Combine(basePath, "expressions/idle01.exp.json");
                string motion1Path = Path.Combine(basePath, "motions/idle01.mtn.bytes");

                // 定义角色的自定义表情文件和动作文件路径，文件名从animations2字典中获取
                string expression2Path = Path.Combine(basePath, $"expressions/{animations2[i][0]}.exp.json");
                string motion2Path = Path.Combine(basePath, $"motions/{animations2[i][1]}.mtn.bytes");

                // 定义可能出现的拼写错误路径（用于容错处理）
                string expression1Path_mistake = Path.Combine(basePath, "exppressions/idle01.exp.json");
                string expression2Path_mistake = Path.Combine(basePath, $"expressions/{animations2[i][0]}.exp.json");

                // 加载基础表情和动作文件的资源
                TextAsset expression1 = AssetDatabase.LoadAssetAtPath<TextAsset>(expression1Path);
                TextAsset motion1 = AssetDatabase.LoadAssetAtPath<TextAsset>(motion1Path);
                TextAsset expression2 = AssetDatabase.LoadAssetAtPath<TextAsset>(expression2Path);
                TextAsset motion2 = AssetDatabase.LoadAssetAtPath<TextAsset>(motion2Path);

                // 如果基础表情或自定义表情加载失败，尝试使用容错路径加载
                if (expression1 == null)
                    expression1 = AssetDatabase.LoadAssetAtPath<TextAsset>(expression1Path_mistake);
                if (expression2 == null)
                    expression2 = AssetDatabase.LoadAssetAtPath<TextAsset>(expression1Path_mistake);

                // 创建一个动画序列，包括基础和自定义的表情与动作
                List<Live2D2Animation> animationSequence = new List<Live2D2Animation>()
                {
                    new Live2D2Animation(motion1,expression1), // 基础动画
                    new Live2D2Animation(motion2,expression2), // 自定义动画
                };

                // 将角色ID和对应的动画序列封装到IndexedLive2D2AnimationSequence对象中，并加入列表
                indexedLive2D2AnimationSequences.Add(new IndexedLive2D2AnimationSequence(i, animationSequence));
            }

            // 创建一个ScriptableObject，用于保存所有的动画序列数据
            IndexedLive2D2AnimationSequenceList indexedLive2D2AnimationSequenceList = ScriptableObject.CreateInstance<IndexedLive2D2AnimationSequenceList>();
            indexedLive2D2AnimationSequenceList.indexedLive2D2AnimationSequences = indexedLive2D2AnimationSequences;

            // 将创建的ScriptableObject保存为一个文件
            AssetDatabase.CreateAsset(indexedLive2D2AnimationSequenceList, saveFile);

            // 保存所有更改并刷新资源数据库
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // 显示一个提示框，通知用户生成成功
            EditorUtility.DisplayDialog("Success", "Generated successfully!", "OK");
#endif
        }
    }
}