using AdaptableDialogAnalyzer.Games.YuRis.ExtYbn;
using System;

using System.Xml.Linq;

namespace AdaptableDialogAnalyzer.Games.YuRis
{

}

namespace AdaptableDialogAnalyzer.Games.YuRis.ExtYbn
{
    /// <summary>
    /// 由ExtYbn反编译得到的JSON对象
    /// </summary>
    [Serializable]
    public class Ybn
    {
        public Header Header;
        public Inst[] Insts;
        public long[] Offs;
    }

    [Serializable]
    public class Header
    {
        public int[] Magic;
        public int Version;
        public int InstCnt;
        public int CodeSize;
        public int ArgSize;
        public int ResourceSize;
        public int OffSize;
        public int Resv;
    }

    [Serializable]
    public class Inst
    {
        public int Op;
        public int Unk;
        public Arg[] Args;
    }

    [Serializable]
    public class Arg
    {
        public int Value;
        public int Type;
        public ResData Res;
    }

    [Serializable]
    public class ResData
    {
        public int Type;
        public string Res;
        public string ResRaw;
    }

}