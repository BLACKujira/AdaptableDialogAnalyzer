using AdaptableDialogAnalyzer.Games.YuRis.ExtYbn;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.Kimihane
{
    public class Chapter_Kimihane_Ybn : Chapter
    {
        Ybn ybn;
        public Ybn Ybn => ybn;

        public static Chapter LoadText(string rawChapter)
        {
            Ybn ybn = JsonUtility.FromJson<Ybn>(rawChapter);
            Chapter_Kimihane_Ybn chapter_Kimihane_Ybn = new Chapter_Kimihane_Ybn();
            chapter_Kimihane_Ybn.ybn = ybn;
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

                if (contentStart != -1 && contentEnd != -1)
                {
                    string characterName = decodedStr.Substring(0, contentStart);
                    string content = decodedStr.Substring(contentStart + 1, contentEnd - contentStart - 1);

                    int characterId = CharacterNameLabels.GetCharacterID(characterName);

                    BasicTalkSnippet basicTalkSnippet = new BasicTalkSnippet(i, characterId, content, characterName);
                    basicTalkSnippets.Add(basicTalkSnippet);
                }
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
                    byte[] bytes = Convert.FromBase64String(argObj.Res.ResRaw);
                    string decodedArg = Encoding.GetEncoding("shift-jis").GetString(bytes);

                    if(exactMatch)
                    {
                        if (decodedArg.Contains(arg)) insts.Add(inst);
                    }
                    else
                    {
                        if(decodedArg.Equals(arg)) insts.Add(inst);
                    }
                }
            }
            return insts.ToArray();
        }
    }
}