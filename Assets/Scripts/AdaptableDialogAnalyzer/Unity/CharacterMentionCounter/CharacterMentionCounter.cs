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
        [Header("Adapter")]
        public ChapterLoader chapterLoader;

        /// <summary>
        /// ��Ҫͳ�Ƶľ��飬������ʾͳ�ƽ���
        /// </summary>
        Chapter[] chapters;

        /// <summary>
        /// �Ѿ�ͳ�ƹ��ľ���
        /// </summary>
        MentionedCountManager mentionedCountManager = new MentionedCountManager();
        public MentionedCountManager MentionedCountManager => mentionedCountManager;

        /// <summary>
        /// �Ѿ����ɺõ�������ʽ�ֵ䣬key��ģʽ��value��������ʽ���� 
        /// </summary>
        Dictionary<string, Regex> regexDictionary = new Dictionary<string, Regex>();

        /// <summary>
        /// ����ģ���ǳƵ��ж�
        /// </summary>
        Dictionary<string, string> bypassUnidentifiedDictionary = new Dictionary<string, string>();

        bool threadAbortFlag = false; //ѭ���з��������־��Ϊtrueʱ����ͳ��
        Thread countThread;

        private void Start()
        {
            chapters = chapterLoader.Chapters;

            NicknameDefinition nicknameDefinition = GlobalConfig.NicknameDefinition;

            //����ͨ���ǳ��б��������ʽ����
            foreach (var nicknameList in nicknameDefinition.commonNicknameMapping.nicknameLists)
            {
                foreach (var regex in nicknameList.nicknames)
                {
                    regexDictionary[regex] = new Regex(regex);
                }
            }

            //���ɽ�ɫ�����ǳ��б��������ʽ����
            foreach (var nicknameMapping in nicknameDefinition.nicknameMappings)
            {
                foreach (var nicknameList in nicknameMapping.nicknameLists)
                {
                    foreach (var regex in nicknameList.nicknames)
                    {
                        regexDictionary[regex] = new Regex(regex);
                    }
                }
            }

            //����ģ���ǳ��б��������ʽ����
            foreach (var regex in nicknameDefinition.unidentifiedNicknameList.nicknames)
            {
                regexDictionary[regex] = new Regex(regex);
            }

            //��¼����ģ���ǳƵı�
            foreach (var regexUnidentified in nicknameDefinition.unidentifiedNicknameList.nicknames)
            {
                //����ͨ���ǳ��б��������ʽ����
                foreach (var nicknameList in nicknameDefinition.commonNicknameMapping.nicknameLists)
                {
                    foreach (var regex in nicknameList.nicknames)
                    {
                        if (regexDictionary[regexUnidentified].IsMatch(regex))
                        {
                            bypassUnidentifiedDictionary[regex] = regexUnidentified;
                        }
                    }
                }

                //���ɽ�ɫ�����ǳ��б��������ʽ����
                foreach (var nicknameMapping in nicknameDefinition.nicknameMappings)
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

            txtProgress.text = $"�� {total} ���ļ�����ͳ�� {counted} ���ļ�";

            progressBar.Priority = (float)counted / total;
        }

        /// <summary>
        /// ͳ�Ʒ������첽����
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

                if (threadAbortFlag) return;
            }
        }

        /// <summary>
        /// ͳ�ƾ��飬ͳ�ƺ󷵻�MentionedCountMatrix����
        /// </summary>
        /// <param name="chapter"></param>
        MentionedCountMatrix CountChapter(Chapter chapter)
        {
            MentionedCountMatrix mentionedCountMatrix = new MentionedCountMatrix(chapter, GlobalConfig.CharacterDefinition.characters.Count);
            mentionedCountMatrix.Chapter = chapter;

            BasicTalkSnippet[] talkSnippets = chapter.GetTalkSnippets();
            foreach (var talkSnippet in talkSnippets)
            {
                CountSnippet(talkSnippet, mentionedCountMatrix);
            }

            return mentionedCountMatrix;
        }

        /// <summary>
        /// ͳ��Ƭ�Σ�ͳ�ƺ�ֱ����ӵ�MentionedCountMatrix��
        /// </summary>
        /// <param name="talkSnippet"></param>
        /// <param name="mentionedCountMatrix"></param>
        void CountSnippet(BasicTalkSnippet talkSnippet, MentionedCountMatrix mentionedCountMatrix)
        {
            NicknameDefinition nicknameDefinition = GlobalConfig.NicknameDefinition;
            HashSet<string> bypassUnidentified = new HashSet<string>();

            //��¼̨��
            mentionedCountMatrix[talkSnippet.SpeakerId].serifs.Add(talkSnippet.RefIdx);

            //�����ж�ͨ���ǳ�
            for (int mentionedPersonId = 0; mentionedPersonId < nicknameDefinition.commonNicknameMapping.nicknameLists.Count; mentionedPersonId++)
            {
                NicknameList nicknameList = nicknameDefinition.commonNicknameMapping.nicknameLists[mentionedPersonId];
                foreach (var regex in nicknameList.nicknames)
                {
                    Regex regexObject = regexDictionary[regex]; //��ȡ������ʽ����
                    Match match = regexObject.Match(talkSnippet.Content);
                    if (match.Success)
                        mentionedCountMatrix[talkSnippet.SpeakerId, mentionedPersonId].AddMatchedDialogue(talkSnippet.RefIdx);
                    if (bypassUnidentifiedDictionary.ContainsKey(regex)) bypassUnidentified.Add(regex);
                }
            }

            //�жϽ�ɫ�����ǳ�
            NicknameMapping nicknameMapping = nicknameDefinition.nicknameMappings[talkSnippet.SpeakerId];
            for (int mentionedPersonId = 0; mentionedPersonId < nicknameMapping.nicknameLists.Count; mentionedPersonId++)
            {
                NicknameList nicknameList = nicknameMapping.nicknameLists[mentionedPersonId];
                foreach (var regex in nicknameList.nicknames)
                {
                    Regex regexObject = regexDictionary[regex];
                    Match match = regexObject.Match(talkSnippet.Content);
                    if (match.Success)
                        mentionedCountMatrix[talkSnippet.SpeakerId, mentionedPersonId].AddMatchedDialogue(talkSnippet.RefIdx);
                    if (bypassUnidentifiedDictionary.ContainsKey(regex)) bypassUnidentified.Add(regex);
                }
            }

            //�ж�ģ���ǳ�
            foreach (var regex in nicknameDefinition.unidentifiedNicknameList.nicknames)
            {
                if (bypassUnidentified.Contains(regex)) continue; //�������жϵľ����ǳ�

                Regex regexObject = regexDictionary[regex];
                Match match = regexObject.Match(talkSnippet.Content);
                if (match.Success)
                    mentionedCountMatrix.AddUnidentifiedSerif(regex, talkSnippet.RefIdx);
            }
        }
    }
}