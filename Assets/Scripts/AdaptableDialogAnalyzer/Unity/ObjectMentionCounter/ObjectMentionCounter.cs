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
        [Tooltip("�ɿգ���֧��������ʽ")] public StringList removeStrings;
        [Header("Adapter")]
        public ChapterLoader chapterLoader;

        Chapter[] chapters;

        /// <summary>
        /// �Ѿ�ͳ�ƹ��ľ���
        /// </summary>
        ObjectMentionedCountManager mentionedCountManager = new ObjectMentionedCountManager();
        public ObjectMentionedCountManager MentionedCountManager => mentionedCountManager;

        /// <summary>
        /// ����ͳ������
        /// </summary>
        Dictionary<int, int> mentionedCountDictionary = new Dictionary<int, int>();
        public Dictionary<int, int> MentionedCountDictionary => mentionedCountDictionary;


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

        bool finished = false;
        public bool Finished => finished;

        private void Start()
        {
            txtTitle.text = $"ͳ�ƶ���: {objectNameDefinition.Identifier}";

            //��ʼ�������ֵ�
            Character[] characters = GlobalConfig.CharacterDefinition.Characters;
            foreach (var character in characters)
            {
                MentionedCountDictionary[character.id] = 0;
            }

            chapters = chapterLoader.Chapters;
            
            //����ͨ���ǳ��б��������ʽ����
            foreach (var regex in objectNameDefinition.CommonNameList)
            {
                regexDictionary[regex] = new Regex(regex);
            }

            //���ɽ�ɫ�����ǳ��б��������ʽ����
            foreach (var specificObjectNameList in objectNameDefinition.SpecificNameLists)
            {
                foreach (var regex in specificObjectNameList.nameList)
                {
                    regexDictionary[regex] = new Regex(regex);
                }
            }

            //����ģ���ǳ��б��������ʽ����
            foreach (var regex in objectNameDefinition.UnidentifiedNameList)
            {
                regexDictionary[regex] = new Regex(regex);
            }

            //��¼����ģ���ǳƵı�
            foreach (var regexUnidentified in objectNameDefinition.UnidentifiedNameList)
            {
                //����ͨ���ǳ��б��������ʽ����
                foreach (var regex in objectNameDefinition.CommonNameList)
                {
                    if (regexDictionary[regexUnidentified].IsMatch(regex))
                    {
                        bypassUnidentifiedDictionary[regex] = regexUnidentified;
                    }
                }

                //���ɽ�ɫ�����ǳ��б��������ʽ����
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

            //��ֹ�߳����޷���ȡ���ʵ��
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
        /// ͳ�ƾ��飬ͳ�ƺ󷵻�MentionedCountMatrix����
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
        /// ͳ��Ƭ�Σ�ͳ�ƺ�ֱ����ӵ�MentionedCountMatrix��
        /// </summary>
        void CountSnippet(BasicTalkSnippet talkSnippet, ObjectMentionedCountMatrix mentionedCountMatrix)
        {
            NicknameDefinition nicknameDefinition = GlobalConfig.NicknameDefinition;
            HashSet<string> bypassUnidentified = new HashSet<string>();

            //��¼̨��
            mentionedCountMatrix.AddSerifCount(talkSnippet.SpeakerId, 1);

            //�����ж�ͨ���ǳ�
            foreach (var regex in objectNameDefinition.CommonNameList)
            {
                Regex regexObject = regexDictionary[regex]; //��ȡ������ʽ����
                Match match = regexObject.Match(talkSnippet.Content);
                if (match.Success)
                {
                    mentionedCountMatrix.AddMatchedDialogue(talkSnippet.SpeakerId, talkSnippet.RefIdx);
                    MentionedCountDictionary[talkSnippet.SpeakerId]++; 
                    if (bypassUnidentifiedDictionary.ContainsKey(regex)) bypassUnidentified.Add(bypassUnidentifiedDictionary[regex]);
                }
            }

            //�жϽ�ɫ�����ǳ�
            SpecificObjectNameList specificNameList = objectNameDefinition.GetSpecificNameList(talkSnippet.SpeakerId);

            //����˽�ɫû�������ǳ��б��������ж�
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

            //�ж�ģ���ǳ�
            foreach (var regex in objectNameDefinition.UnidentifiedNameList)
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