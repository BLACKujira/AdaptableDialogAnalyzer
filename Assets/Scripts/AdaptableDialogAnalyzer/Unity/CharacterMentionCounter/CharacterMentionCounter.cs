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
    public class CharacterMentionCounter : MonoBehaviour
    {
        [Header("Components")]
        public CharacterMentionCounter_DisplayArea displayArea;
        public Text txtProgress;
        public ProgressBar progressBar;
        [Header("Settings")]
        public string saveFolder;
        [Tooltip("可空，不支持正则表达式")] public StringList removeStrings;
        [Header("Adapter")]
        public ChapterLoader chapterLoader;

        /// <summary>
        /// 需要统计的剧情，用于显示统计进度
        /// </summary>
        Chapter[] chapters;

        /// <summary>
        /// 已经统计过的剧情
        /// </summary>
        MentionedCountManager mentionedCountManager = new MentionedCountManager();
        public MentionedCountManager MentionedCountManager => mentionedCountManager;

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
            chapters = chapterLoader.Chapters;

            NicknameDefinition nicknameDefinition = GlobalConfig.NicknameDefinition;

            //生成通用昵称列表的正则表达式对象
            foreach (var nicknameList in nicknameDefinition.CommonNicknameMapping.nicknameLists)
            {
                foreach (var regex in nicknameList.nicknames)
                {
                    regexDictionary[regex] = new Regex(regex);
                }
            }

            //生成角色特殊昵称列表的正则表达式对象
            foreach (var nicknameMapping in nicknameDefinition.SpecificNicknameMappings)
            {
                foreach (var nicknameList in nicknameMapping.nicknameLists)
                {
                    foreach (var regex in nicknameList.nicknames)
                    {
                        regexDictionary[regex] = new Regex(regex);
                    }
                }
            }

            //生成模糊昵称列表的正则表达式对象
            foreach (var regex in nicknameDefinition.UnidentifiedNicknameList.nicknames)
            {
                regexDictionary[regex] = new Regex(regex);
            }

            //记录跳过模糊昵称的表
            foreach (var regexUnidentified in nicknameDefinition.UnidentifiedNicknameList.nicknames)
            {
                //生成通用昵称列表的正则表达式对象
                foreach (var nicknameList in nicknameDefinition.CommonNicknameMapping.nicknameLists)
                {
                    foreach (var regex in nicknameList.nicknames)
                    {
                        if (regexDictionary[regexUnidentified].IsMatch(regex))
                        {
                            bypassUnidentifiedDictionary[regex] = regexUnidentified;
                        }
                    }
                }

                //生成角色特殊昵称列表的正则表达式对象
                foreach (var nicknameMapping in nicknameDefinition.SpecificNicknameMappings)
                {
                    foreach (var nicknameList in nicknameMapping.nicknameLists)
                    {
                        foreach (var regex in nicknameList.nicknames)
                        {
                            if (regexDictionary[regexUnidentified].IsMatch(regex))
                            {
                                bypassUnidentifiedDictionary[regex] = regexUnidentified;
                            }
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
                MentionedCountMatrix mentionedCountMatrix = CountChapter(chapter);
                mentionedCountManager.mentionedCountMatrices.Add(mentionedCountMatrix);

                string savePath = Path.Combine(saveFolder, chapter.ChapterID + ".mcm");
                File.WriteAllText(savePath, JsonUtility.ToJson(mentionedCountMatrix));

                if (threadAbortFlag)
                {
                    finished  = true;
                    return;
                }
            }
            finished = true;
        }

        /// <summary>
        /// 统计剧情，统计后返回MentionedCountMatrix对象
        /// </summary>
        /// <param name="chapter"></param>
        MentionedCountMatrix CountChapter(Chapter chapter)
        {
            MentionedCountMatrix mentionedCountMatrix = new MentionedCountMatrix(chapter);
            mentionedCountMatrix.Chapter = chapter;

            BasicTalkSnippet[] talkSnippets = chapter.GetTalkSnippets();
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
        /// <param name="talkSnippet"></param>
        /// <param name="mentionedCountMatrix"></param>
        void CountSnippet(BasicTalkSnippet talkSnippet, MentionedCountMatrix mentionedCountMatrix)
        {
            NicknameDefinition nicknameDefinition = GlobalConfig.NicknameDefinition;
            HashSet<string> bypassUnidentified = new HashSet<string>();

            //记录台词
            mentionedCountMatrix[talkSnippet.SpeakerId].serifs.Add(talkSnippet.RefIdx);

            //首先判断通用昵称
            for (int i = 0; i < nicknameDefinition.CommonNicknameMapping.nicknameLists.Count; i++)
            {
                int mentionedPersonId = nicknameDefinition.CommonNicknameMapping.nicknameLists[i].mentionedPersonId;

                NicknameList nicknameList = nicknameDefinition.CommonNicknameMapping.nicknameLists[i];
                foreach (var regex in nicknameList.nicknames)
                {
                    Regex regexObject = regexDictionary[regex]; //获取正则表达式对象
                    Match match = regexObject.Match(talkSnippet.Content);
                    if (match.Success)
                    {
                        mentionedCountMatrix[talkSnippet.SpeakerId, mentionedPersonId].AddMatchedDialogue(talkSnippet.RefIdx);
                        if (bypassUnidentifiedDictionary.ContainsKey(regex)) bypassUnidentified.Add(bypassUnidentifiedDictionary[regex]);
                    }
                }
            }

            //判断角色特殊昵称
            NicknameMapping nicknameMapping = nicknameDefinition.GetSpecificNicknameMapping(talkSnippet.SpeakerId);

            //如果此角色没有特殊昵称列表则跳过判断
            if (nicknameMapping != null)
            {
                for (int i = 0; i < nicknameMapping.nicknameLists.Count; i++)
                {
                    int mentionedPersonId = nicknameMapping.nicknameLists[i].mentionedPersonId;

                    NicknameList nicknameList = nicknameMapping.nicknameLists[i];
                    foreach (var regex in nicknameList.nicknames)
                    {
                        Regex regexObject = regexDictionary[regex];
                        Match match = regexObject.Match(talkSnippet.Content);
                        if (match.Success)
                        {
                            mentionedCountMatrix[talkSnippet.SpeakerId, mentionedPersonId].AddMatchedDialogue(talkSnippet.RefIdx);
                            if (bypassUnidentifiedDictionary.ContainsKey(regex)) bypassUnidentified.Add(bypassUnidentifiedDictionary[regex]);
                        }
                    }
                }
            }

            //判断模糊昵称
            foreach (var regex in nicknameDefinition.UnidentifiedNicknameList.nicknames)
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