using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.UIElements;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.Unity
{
    public class ObjectMentionCounter : MonoBehaviour
    {
        [Header("Components")]
        public ObjectMentionCounter_DisplayArea displayArea;
        public Text txtTitle;
        public Text txtProgress;
        public ProgressBar progressBar;
        [Header("Settings")]
        public ObjectNameDefinition objectNameDefinition;
        public string saveFolder;
        [Tooltip("可空，不支持正则表达式")] public StringList removeStrings;
        [Header("Adapter")]
        public ChapterLoader chapterLoader;

        Chapter[] chapters;

        /// <summary>
        /// 已经统计过的剧情
        /// </summary>
        ObjectMentionedCountManager mentionedCountManager = new ObjectMentionedCountManager();
        public ObjectMentionedCountManager MentionedCountManager => mentionedCountManager;

        /// <summary>
        /// 缓存统计数据
        /// </summary>
        Dictionary<int, int> mentionedCountDictionary = new Dictionary<int, int>();
        public Dictionary<int, int> MentionedCountDictionary => mentionedCountDictionary;


        /// <summary>
        /// 已经生成好的正则表达式字典，key：模式，value：正则表达式对象 
        /// </summary>
        Dictionary<string, Regex> regexDictionary = new Dictionary<string, Regex>();

        /// <summary>
        /// 跳过模糊昵称的判断
        /// </summary>
        Dictionary<string, string> bypassUnidentifiedDictionary = new Dictionary<string, string>();

        bool threadAbortFlag = false; //循环中发现这个标志变为true时结束统计
        Thread countThread;

        bool finished = false;
        public bool Finished => finished;

        private void Start()
        {
            txtTitle.text = $"统计对象: {objectNameDefinition.Identifier}";

            //初始化缓存字典
            Character[] characters = GlobalConfig.CharacterDefinition.Characters;
            foreach (var character in characters)
            {
                MentionedCountDictionary[character.id] = 0;
            }

            chapters = chapterLoader.Chapters;
            
            //生成通用昵称列表的正则表达式对象
            foreach (var regex in objectNameDefinition.CommonNameList)
            {
                regexDictionary[regex] = new Regex(regex);
            }

            //生成角色特殊昵称列表的正则表达式对象
            foreach (var specificObjectNameList in objectNameDefinition.SpecificNameLists)
            {
                foreach (var regex in specificObjectNameList.nameList)
                {
                    regexDictionary[regex] = new Regex(regex);
                }
            }

            //生成模糊昵称列表的正则表达式对象
            foreach (var regex in objectNameDefinition.UnidentifiedNameList)
            {
                regexDictionary[regex] = new Regex(regex);
            }

            //记录跳过模糊昵称的表
            foreach (var regexUnidentified in objectNameDefinition.UnidentifiedNameList)
            {
                //生成通用昵称列表的正则表达式对象
                foreach (var regex in objectNameDefinition.CommonNameList)
                {
                    if (regexDictionary[regexUnidentified].IsMatch(regex))
                    {
                        bypassUnidentifiedDictionary[regex] = regexUnidentified;
                    }
                }

                //生成角色特殊昵称列表的正则表达式对象
                foreach (var specificObjectNameList in objectNameDefinition.SpecificNameLists)
                {
                    foreach (var regex in specificObjectNameList.nameList)
                    {
                        if (regexDictionary[regexUnidentified].IsMatch(regex))
                        {
                            bypassUnidentifiedDictionary[regex] = regexUnidentified;
                        }
                    }
                }
            }

            //防止线程中无法获取组件实例
            GlobalConfig.ForceInitialize();

            countThread = new Thread(() => Count(chapters));
            countThread.Start();
        }

        private void OnDestroy()
        {
            threadAbortFlag = true;
        }

        private void Update()
        {
            int counted = mentionedCountManager.mentionedCountMatrices.Count;
            int total = chapters.Length;

            txtProgress.text = $"共 {total} 个文件，已统计 {counted} 个文件";

            progressBar.Priority = (float)counted / total;
        }

        /// <summary>
        /// 统计方法，异步调用
        /// </summary>
        /// <param name="chapters"></param>
        void Count(Chapter[] chapters)
        {
            foreach (var chapter in chapters)
            {
                ObjectMentionedCountMatrix mentionedCountMatrix = CountChapter(chapter);
                mentionedCountManager.mentionedCountMatrices.Add(mentionedCountMatrix);

                string savePath = Path.Combine(saveFolder, chapter.ChapterID + ".omcm");
                mentionedCountMatrix.SerializeAndSave(savePath);

                if (threadAbortFlag)
                {
                    finished = true;
                    return;
                }
            }
            finished = true;
        }

        /// <summary>
        /// 统计剧情，统计后返回MentionedCountMatrix对象
        /// </summary>
        ObjectMentionedCountMatrix CountChapter(Chapter chapter)
        {
            ObjectMentionedCountMatrix mentionedCountMatrix = new ObjectMentionedCountMatrix(chapter);
            mentionedCountMatrix.Chapter = chapter;

            BasicTalkSnippet[] talkSnippets = chapter.TalkSnippets;
            foreach (var talkSnippet in talkSnippets)
            {
                BasicTalkSnippet matchSnippet = talkSnippet;
                if (removeStrings != null)
                {
                    foreach (var str in removeStrings.strings)
                    {
                        matchSnippet.content = matchSnippet.content.Replace(str, "");
                    }
                }
                CountSnippet(matchSnippet, mentionedCountMatrix);
            }

            return mentionedCountMatrix;
        }

        /// <summary>
        /// 统计片段，统计后直接添加到MentionedCountMatrix中
        /// </summary>
        void CountSnippet(BasicTalkSnippet talkSnippet, ObjectMentionedCountMatrix mentionedCountMatrix)
        {
            NicknameDefinition nicknameDefinition = GlobalConfig.NicknameDefinition;
            HashSet<string> bypassUnidentified = new HashSet<string>();

            //记录台词
            mentionedCountMatrix.AddSerifCount(talkSnippet.SpeakerId, 1);

            //首先判断通用昵称
            foreach (var regex in objectNameDefinition.CommonNameList)
            {
                Regex regexObject = regexDictionary[regex]; //获取正则表达式对象
                Match match = regexObject.Match(talkSnippet.Content);
                if (match.Success)
                {
                    mentionedCountMatrix.AddMatchedDialogue(talkSnippet.SpeakerId, talkSnippet.RefIdx);
                    MentionedCountDictionary[talkSnippet.SpeakerId]++; 
                    if (bypassUnidentifiedDictionary.ContainsKey(regex)) bypassUnidentified.Add(bypassUnidentifiedDictionary[regex]);
                }
            }

            //判断角色特殊昵称
            SpecificObjectNameList specificNameList = objectNameDefinition.GetSpecificNameList(talkSnippet.SpeakerId);

            //如果此角色没有特殊昵称列表则跳过判断
            if (specificNameList != null)
            {
                foreach (var regex in specificNameList.nameList)
                {
                    Regex regexObject = regexDictionary[regex];
                    Match match = regexObject.Match(talkSnippet.Content);
                    if (match.Success)
                    {
                        mentionedCountMatrix.AddMatchedDialogue(talkSnippet.SpeakerId, talkSnippet.RefIdx);
                        MentionedCountDictionary[talkSnippet.SpeakerId]++;
                        if (bypassUnidentifiedDictionary.ContainsKey(regex)) bypassUnidentified.Add(bypassUnidentifiedDictionary[regex]);
                    }
                }
            }

            //判断模糊昵称
            foreach (var regex in objectNameDefinition.UnidentifiedNameList)
            {
                if (bypassUnidentified.Contains(regex)) continue; //跳过已判断的具体昵称

                Regex regexObject = regexDictionary[regex];
                Match match = regexObject.Match(talkSnippet.Content);
                if (match.Success)
                    mentionedCountMatrix.AddUnidentifiedSerif(regex, talkSnippet.RefIdx);
            }
        }
    }
}