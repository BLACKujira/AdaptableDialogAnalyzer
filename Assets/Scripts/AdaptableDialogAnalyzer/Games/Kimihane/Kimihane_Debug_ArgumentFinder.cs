using AdaptableDialogAnalyzer.Games.YuRis.ExtYbn;
using System;
using System.Collections.Generic;
using System.Text;
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
                List<Inst> instList = new List<Inst>(chapterYbn.Ybn.Insts);
                foreach (var inst in insts)
                {
                    foreach (var arg in inst.Args)
                    {
                        byte[] bytes;
                        string decodedArg = "解码失败";
                        byte[] bytesRaw;
                        string decodedArgRaw = "解码失败";
                        try
                        {
                            bytes = Convert.FromBase64String(arg.Res.Res);
                            decodedArg = Encoding.GetEncoding("shift-jis").GetString(bytes);
                        }
                        catch { }

                        try
                        {
                            bytesRaw = Convert.FromBase64String(arg.Res.ResRaw);
                            decodedArgRaw = Encoding.GetEncoding("shift-jis").GetString(bytesRaw);
                        }
                        catch { }

                        string outStr = $"位于：{chapter.ChapterID} 指令：{inst.Op} 索引：{instList.IndexOf(inst)}";
                        if (arg.Res.Res != null) outStr += $" Res参数：{arg.Res.Res} Res解码参数：{decodedArg}";
                        if (arg.Res.ResRaw != null) outStr += $" ResRaw参数：{arg.Res.ResRaw} ResRaw解码参数：{decodedArgRaw}";
                        Debug.Log(outStr);
                    }
                }
            }
            Debug.Log("完成");
        }
    }
}