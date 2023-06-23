using AdaptableDialogAnalyzer.Games.YuRis.ExtYbn;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.Kimihane
{
    public class Chapter_Kimihane_Ybn : Chapter
    {
        Ybn ybn;
        public Ybn Ybn => ybn;

        Kimihane_LabelInfoLoader labelInfoLoader;

        public override string ExtraInfo
        {
            get
            {
                List<Inst> jumpInsts = Ybn.Insts.Where(inst => inst.Op == 28).ToList();
                string[] jumpArray = jumpInsts
                    .Select(inst => inst.Args[0].Res.Res)
                    .Select(str => Convert.FromBase64String(str))
                    .Select(b => Encoding.GetEncoding("shift-jis").GetString(b))
                    .ToArray();

                if (labelInfoLoader != null) jumpArray = jumpArray.Select(str => $"{str}({labelInfoLoader.GetContainYbnAutoJump(str.Trim('\"'))})").ToArray();

                if (jumpArray.Length > 0) return "此Ybn跳转至" + string.Join(", ", jumpArray);
                else return "此Ybn不含跳转指令";
            }
        }

        public static Chapter LoadText(string rawChapter, Kimihane_LabelInfoLoader labelYbnGetter = null)
        {
            Ybn ybn = JsonUtility.FromJson<Ybn>(rawChapter);
            Chapter_Kimihane_Ybn chapter_Kimihane_Ybn = new Chapter_Kimihane_Ybn();
            chapter_Kimihane_Ybn.ybn = ybn;
            chapter_Kimihane_Ybn.labelInfoLoader = labelYbnGetter;
            return chapter_Kimihane_Ybn;
        }

        public override BasicTalkSnippet[] GetTalkSnippets()
        {
            List<BasicTalkSnippet> basicTalkSnippets = new List<BasicTalkSnippet>();
            for (int i = 0; i < ybn.Insts.Length; i++)
            {
                Inst inst = ybn.Insts[i];

                if (inst.Op != 90) continue; // 判断运算符是否为显示文字
                byte[] bytes = Convert.FromBase64String(inst.Args[0].Res.ResRaw);
                string decodedStr = Encoding.GetEncoding("shift-jis").GetString(bytes);

                int contentStart = decodedStr.IndexOf("「");
                int contentEnd = decodedStr.LastIndexOf("」");

                string characterName;
                string content;
                int characterId;

                if (contentStart != -1 && contentEnd != -1)
                {
                    characterName = decodedStr.Substring(0, contentStart);
                    content = decodedStr.Substring(contentStart + 1, contentEnd - contentStart - 1);
                    characterId = CharacterNameLabels.GetCharacterID(characterName);
                }
                else
                {
                    characterName = null;
                    content = decodedStr.TrimStart('　');
                    characterId = 0;
                }

                BasicTalkSnippet basicTalkSnippet = new BasicTalkSnippet(i, characterId, content, characterName);
                basicTalkSnippets.Add(basicTalkSnippet);
            }
            return basicTalkSnippets.ToArray();
        }

        /// <summary>
        /// DEBUG用，寻找含有某个参数的指令
        /// </summary>
        public Inst[] FindInstWithArg(string arg, bool exactMatch)
        {
            List<Inst> insts = new List<Inst>();
            foreach (var inst in Ybn.Insts)
            {
                foreach (var argObj in inst.Args)
                {
                    foreach (var argRaw in new string[] { argObj?.Res?.ResRaw, argObj?.Res?.Res })
                    {
                        if (argRaw == null) continue;

                        //首先判断未解码前的参数是否符合
                        if (exactMatch)
                        {
                            if (argRaw.Equals(arg))
                            {
                                insts.Add(inst);
                                continue;
                            }
                        }
                        else
                        {
                            if (argRaw.Contains(arg))
                            {
                                insts.Add(inst);
                                continue;
                            }
                        }

                        //解码参数
                        byte[] bytes;
                        string decodedArg;
                        try
                        {
                            bytes = Convert.FromBase64String(argRaw);
                            decodedArg = Encoding.GetEncoding("shift-jis").GetString(bytes);
                        }
                        catch
                        {
                            continue;
                        }

                        //判断解码后的参数是否符合
                        if (exactMatch)
                        {
                            if (decodedArg.Equals(arg)) insts.Add(inst);
                        }
                        else
                        {
                            if (decodedArg.Contains(arg)) insts.Add(inst);
                        }
                    }
                }
            }
            return insts.ToArray();
        }
    }
}