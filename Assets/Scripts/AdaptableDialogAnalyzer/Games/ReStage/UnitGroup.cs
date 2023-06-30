using System;

namespace AdaptableDialogAnalyzer.Games.ReStage
{
    [Flags]
    public enum UnitGroup
    {
        [LabeledEnum("なし")]
        [EnumNum(0)]
        None = 0,
        [LabeledEnum("Kirare")]
        [EnumNum(1)]
        Kirare = 1,
        [EnumNum(2)]
        [LabeledEnum("オルタンシア")]
        Ortensia = 2,
        [EnumNum(4)]
        [LabeledEnum("ステラマリス")]
        StellaMaris = 3,
        [LabeledEnum("トロワアンジュ")]
        [EnumNum(8)]
        TroisAnges = 4,
        [EnumNum(16)]
        [LabeledEnum("テトラルキア")]
        TetraRkhia = 5,
        [EnumNum(32)]
        [LabeledEnum("アスタレーヴ")]
        AsterReve = 6,
        [EnumNum(64)]
        [LabeledEnum("七森中ごらく部")]
        Gorakubu = 0x65,
        [LabeledEnum("らき☆すた")]
        [EnumNum(128)]
        LuckyStar = 0x66,
        [EnumNum(256)]
        [LabeledEnum("オンゲキ")]
        Ongeki = 0x67,
        [EnumNum(512)]
        [LabeledEnum("まちカド")]
        Machikado = 0x68,
        [LabeledEnum("オンゲキ R.E.D")]
        [EnumNum(768)]
        Ongeki2 = 0x69,
        [LabeledEnum("大運動会")]
        [EnumNum(1024)]
        Daiundoukai = 0x6A,
        [EnumNum(1280)]
        [LabeledEnum("おちこぼれフルーツタルト")]
        DropoutFruit = 0x6B,
        [EnumNum(1792)]
        [LabeledEnum("オンゲキ bright")]
        Ongeki3 = 0x6C,
        [LabeledEnum("ステラマリス2")]
        [EnumNum(2304)]
        StellaMaris2 = 0x3E7
    }
}