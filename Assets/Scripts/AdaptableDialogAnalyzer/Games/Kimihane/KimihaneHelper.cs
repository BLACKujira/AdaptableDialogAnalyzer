using AdaptableDialogAnalyzer.Games.YuRis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdaptableDialogAnalyzer.Games.Kimihane
{
    public static class KimihaneHelper
    {
        public static string DecodeStringArg(string arg)
        {
            byte[] bytes = Convert.FromBase64String(arg);
            return Encoding.GetEncoding("shift-jis").GetString(bytes); ;
        }

        public static List<YSLElement> GetYSLElements(byte[] ysl,int startIndex)
        {
            int cursor = startIndex;

            List<YSLElement> elements = new List<YSLElement>();
            while (cursor < ysl.Length)
            {
                YSLElement ySLElement = new YSLElement(ysl, cursor);
                elements.Add(ySLElement);
                cursor += ySLElement.Length;
            }

            return elements;
        }
    }
}