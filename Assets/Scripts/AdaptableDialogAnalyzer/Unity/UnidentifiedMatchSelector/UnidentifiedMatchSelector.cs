using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.UIElements;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 选择一个多义昵称并进入对应的剧情选择界面
    /// </summary>
    public class UnidentifiedMatchSelector : MonoBehaviour
    {
        public Window window;
        [Header("Components")]
        public EquidistantLayoutGenerator2D layoutGenerator;
        [Header("Prefabs")]
        public Window chapterSelectorPrefab;

        public void Initialize(MentionedCountManager mentionedCountManager)
        {
            Dictionary<string, List<MentionedCountMatrix>> dictionary = mentionedCountManager.GetMatricesWithUnidentifiedMatches();
            List<KeyValuePair<string, List<MentionedCountMatrix>>> list = dictionary.ToList();
            layoutGenerator.Generate(list.Count, (gobj, id) =>
            {
                UnidentifiedMatchItem unidentifiedMatchItem = gobj.GetComponent<UnidentifiedMatchItem>();
                unidentifiedMatchItem.SetData(list[id].Key, list[id].Value.Sum(m=>m.GetUnidentifiedMentions(list[id].Key).Count));
                unidentifiedMatchItem.button.onClick.AddListener(()=>
                {
                    ChapterSelectorUnidentified chapterSelectorUnidentified = window.OpenWindow<ChapterSelectorUnidentified>(chapterSelectorPrefab);
                    chapterSelectorUnidentified.Initialize(mentionedCountManager, list[id].Key);
                });
            });
        }
    }
}