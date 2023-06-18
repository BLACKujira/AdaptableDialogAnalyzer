using AdaptableDialogAnalyzer.Games.YuRis.ExtYbn;
using AdaptableDialogAnalyzer.YAML;
using System;
using System.Collections.Generic;
using UnityEngine;
using MonoBehaviour = UnityEngine.MonoBehaviour;

namespace AdaptableDialogAnalyzer.Games.Kimihane
{
    /// <summary>
    /// 寻找含有某个参数的指令
    /// </summary>
    public class Kimihane_Debug_ArgumentFinder : MonoBehaviour
    {
        [Header("Components")]
        public ChapterLoader_Folder_Kimihane_Ybn chapterLoader;
        [Header("Settings")]
        public string argument;
        public bool exactMatch = false;

        private void Start()
        {
            Chapter[] chapters = chapterLoader.Chapters;
            foreach (var chapter in chapters)
            {
                Chapter_Kimihane_Ybn chapterYbn = chapter as Chapter_Kimihane_Ybn;
                if (chapterYbn == null) throw new Exception("请使用Kimihane的剧情加载器");

                Inst[] insts = chapterYbn.FindInstWithArg(argument, exactMatch);
                List<Inst> instList = new List<Inst> (chapterYbn.Ybn.Insts);
                foreach (var inst in insts)
                {
                    Debug.Log($"位于：{chapter.ChapterID} 指令：{inst.Op} 索引：{instList.IndexOf(inst)}");
                }
            }
        }
    }
}