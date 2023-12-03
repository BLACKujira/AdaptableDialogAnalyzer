using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.ProjectSekai
{
    public class ProjectSekaiHelper : MonoBehaviour
    {
        public static Characters characters = new Characters();
        public static Units units = new Units();

        public class Characters
        {
            CharacterInfo[] characterInfos =
            {
                null,
                new CharacterInfo(1,"星乃","一歌",new Color32(0x33,0xAA,0xEE,255),(8,11),Unit.Leoneed),
                new CharacterInfo(2,"天馬","咲希",new Color32(0xFF,0xDD,0x44,255),(5,9),Unit.Leoneed),
                new CharacterInfo(3,"望月","穂波",new Color32(0xEE,0x66,0x66,255),(10,27),Unit.Leoneed),
                new CharacterInfo(4,"日野森","志歩",new Color32(0xBB,0xDD,0x22,255),(1,8),Unit.Leoneed),

                new CharacterInfo(5,"花里","みのり",new Color32(0xFF,0xCC,0xAA,255),(4,14),Unit.MOREMOREJUMP),
                new CharacterInfo(6,"桐谷","遥",new Color32(0x99,0xCC,0xFF,255),(10,5),Unit.MOREMOREJUMP),
                new CharacterInfo(7,"桃井","愛莉",new Color32(0xFF,0xAA,0xCC,255),(3,19),Unit.MOREMOREJUMP),
                new CharacterInfo(8,"日野森","雫",new Color32(0x99,0xEE,0xDD,255),(12,6),Unit.MOREMOREJUMP),

                new CharacterInfo(9,"小豆沢","こはね",new Color32(0xFF,0x66,0x99,255),(3,2),Unit.VividBADSQUAD),
                new CharacterInfo(10,"白石","杏",new Color32(0x00,0xBB,0xDD,255),(7,26),Unit.VividBADSQUAD),
                new CharacterInfo(11,"東雲","彰人",new Color32(0xFF,0x77,0x22,255),(11,12),Unit.VividBADSQUAD),
                new CharacterInfo(12,"青柳","冬弥",new Color32(0x00,0x77,0xDD,255),(5,25),Unit.VividBADSQUAD),

                new CharacterInfo(13,"天馬","司",new Color32(0xFF,0xBB,0x00,255),(5,17),Unit.WonderlandsShowtime),
                new CharacterInfo(14,"鳳","えむ",new Color32(0xFF,0x66,0xBB,255),(9,9),Unit.WonderlandsShowtime),
                new CharacterInfo(15,"草薙","寧々",new Color32(0x33,0xDD,0x99,255),(7,20),Unit.WonderlandsShowtime),
                new CharacterInfo(16,"神代","類",new Color32(0xBB,0x88,0xEE,255),(6,24),Unit.WonderlandsShowtime),

                new CharacterInfo(17,"宵崎","奏",new Color32(0xBB,0x66,0x88,255),(2,10),Unit.NightCord),
                new CharacterInfo(18,"朝比奈","まふゆ",new Color32(0x88,0x88,0xCC,255),(1,27),Unit.NightCord),
                new CharacterInfo(19,"東雲","絵名",new Color32(0xCC,0xAA,0x88,255),(4,30),Unit.NightCord),
                new CharacterInfo(20,"暁山","瑞希",new Color32(0xDD,0xAA,0xCC,255),(8,27),Unit.NightCord),

                new CharacterInfo(21,"初音","ミク",new Color32(0x33,0xCC,0xBB,255),(8,27),Unit.VirtualSinger),
                new CharacterInfo(22,"鏡音","リン",new Color32(0xFF,0xCC,0x11,255),(12,27),Unit.VirtualSinger),
                new CharacterInfo(23,"鏡音","レン",new Color32(0xFF,0xEE,0x11,255),(12,27),Unit.VirtualSinger),
                new CharacterInfo(24,"巡音","ルカ",new Color32(0xFF,0xBB,0xCC,255),(1,30),Unit.VirtualSinger),
                new CharacterInfo(25,"","MEIKO",new Color32(0xDD,0x44,0x44,255),(11,5),Unit.VirtualSinger),
                new CharacterInfo(26,"","KAITO",new Color32(0x33,0x66,0xCC,255),(2,17),Unit.VirtualSinger),

                new CharacterInfo(27,"初音","ミク",new Color32(0x33,0xCC,0xBB,255),(8,27),Unit.Leoneed),
                new CharacterInfo(28,"初音","ミク",new Color32(0x33,0xCC,0xBB,255),(8,27),Unit.MOREMOREJUMP),
                new CharacterInfo(29,"初音","ミク",new Color32(0x33,0xCC,0xBB,255),(8,27),Unit.VividBADSQUAD),
                new CharacterInfo(30,"初音","ミク",new Color32(0x33,0xCC,0xBB,255),(8,27),Unit.WonderlandsShowtime),
                new CharacterInfo(31,"初音","ミク",new Color32(0x33,0xCC,0xBB,255),(8,27),Unit.NightCord),

                new CharacterInfo(32,"鏡音","リン",new Color32(0xFF,0xCC,0x11,255),(12,27),Unit.Leoneed),
                new CharacterInfo(33,"鏡音","リン",new Color32(0xFF,0xCC,0x11,255),(12,27),Unit.MOREMOREJUMP),
                new CharacterInfo(34,"鏡音","リン",new Color32(0xFF,0xCC,0x11,255),(12,27),Unit.VividBADSQUAD),
                new CharacterInfo(35,"鏡音","リン",new Color32(0xFF,0xCC,0x11,255),(12,27),Unit.WonderlandsShowtime),
                new CharacterInfo(36,"鏡音","リン",new Color32(0xFF,0xCC,0x11,255),(12,27),Unit.NightCord),

                new CharacterInfo(37,"鏡音","レン",new Color32(0xFF,0xEE,0x11,255),(12,27),Unit.Leoneed),
                new CharacterInfo(38,"鏡音","レン",new Color32(0xFF,0xEE,0x11,255),(12,27),Unit.MOREMOREJUMP),
                new CharacterInfo(39,"鏡音","レン",new Color32(0xFF,0xEE,0x11,255),(12,27),Unit.VividBADSQUAD),
                new CharacterInfo(40,"鏡音","レン",new Color32(0xFF,0xEE,0x11,255),(12,27),Unit.WonderlandsShowtime),
                new CharacterInfo(41,"鏡音","レン",new Color32(0xFF,0xEE,0x11,255),(12,27),Unit.NightCord),

                new CharacterInfo(42,"巡音","ルカ",new Color32(0xFF,0xBB,0xCC,255),(1,30),Unit.Leoneed),
                new CharacterInfo(43,"巡音","ルカ",new Color32(0xFF,0xBB,0xCC,255),(1,30),Unit.MOREMOREJUMP),
                new CharacterInfo(44,"巡音","ルカ",new Color32(0xFF,0xBB,0xCC,255),(1,30),Unit.VividBADSQUAD),
                new CharacterInfo(45,"巡音","ルカ",new Color32(0xFF,0xBB,0xCC,255),(1,30),Unit.WonderlandsShowtime),
                new CharacterInfo(46,"巡音","ルカ",new Color32(0xFF,0xBB,0xCC,255),(1,30),Unit.NightCord),

                new CharacterInfo(47,"","MEIKO",new Color32(0xDD,0x44,0x44,255),(11,5),Unit.Leoneed),
                new CharacterInfo(48,"","MEIKO",new Color32(0xDD,0x44,0x44,255),(11,5),Unit.MOREMOREJUMP),
                new CharacterInfo(49,"","MEIKO",new Color32(0xDD,0x44,0x44,255),(11,5),Unit.VividBADSQUAD),
                new CharacterInfo(50,"","MEIKO",new Color32(0xDD,0x44,0x44,255),(11,5),Unit.WonderlandsShowtime),
                new CharacterInfo(51,"","MEIKO",new Color32(0xDD,0x44,0x44,255),(11,5),Unit.NightCord),

                new CharacterInfo(52,"","KAITO",new Color32(0x33,0x66,0xCC,255),(2,17),Unit.Leoneed),
                new CharacterInfo(53,"","KAITO",new Color32(0x33,0x66,0xCC,255),(2,17),Unit.MOREMOREJUMP),
                new CharacterInfo(54,"","KAITO",new Color32(0x33,0x66,0xCC,255),(2,17),Unit.VividBADSQUAD),
                new CharacterInfo(55,"","KAITO",new Color32(0x33,0x66,0xCC,255),(2,17),Unit.WonderlandsShowtime),
                new CharacterInfo(56,"","KAITO",new Color32(0x33,0x66,0xCC,255),(2,17),Unit.NightCord),

            };

            public CharacterInfo this[Character character]
            {
                get => characterInfos[(int)character];
            }
            public CharacterInfo this[int characterID]
            {
                get => characterInfos[characterID];
            }
        }
        public class CharacterInfo
        {
            public readonly int id;
            public readonly string myouji;
            public readonly string namae;
            public readonly Color32 imageColor;
            public readonly (int month, int day) birthday;
            public readonly Unit unit;

            public string Name { get => string.IsNullOrEmpty(myouji) ? namae : myouji + ' ' + namae; }

            public CharacterInfo(int id, string myouji, string namae, Color imageColor, (int month, int day) birthday, Unit unit)
            {
                this.id = id;
                this.myouji = myouji;
                this.namae = namae;
                this.imageColor = imageColor;
                this.birthday = birthday;
                this.unit = unit;
            }
        }

        public class Units
        {
            UnitInfo[] unitInfos =
            {
                null,
                new UnitInfo(1,@"バーチャル・シンガー",new Color32(0xFF,0xFF,0xFF,255),"vs"),
                new UnitInfo(2,@"Leo/need",new Color32(0x44,0x55,0xDD,255),"l/n"),
                new UnitInfo(3,@"MORE MORE JUMP！",new Color32(0x88,0xDD,0x44,255),"mmj"),
                new UnitInfo(4,@"Vivid BAD SQUAD",new Color32(0xEE,0x11,0x66,255),"vbs"),
                new UnitInfo(5,@"ワンダーランズ×ショウタイム",new Color32(0xFF,0x99,0x00,255),"wxs"),
                new UnitInfo(6,@"25時、ナイトコードで。",new Color32(0x88,0x44,0x99,255),"25时"),
            };

            public UnitInfo this[Unit unit]
            {
                get => unitInfos[(int)unit];
            }
            public UnitInfo this[int unitID]
            {
                get => unitInfos[unitID];
            }

        }
        public class UnitInfo
        {
            public readonly int id;
            public readonly string name;

            public readonly Color32 color;
            public readonly string abbr;

            public UnitInfo(int id, string name, Color32 color, string abbr)
            {
                this.id = id;
                this.name = name;
                this.color = color;
                this.abbr = abbr;
            }
        }

        /// <summary>
        /// 注意时区问题
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime UnixTimeMSToDateTime(long unixTime)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(unixTime).DateTime;
        }

        /// <summary>
        /// 以东京时间计算
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime UnixTimeMSToDateTimeTST(long unixTime)
        {
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(unixTime);
            TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            return TimeZoneInfo.ConvertTime(dateTimeOffset, timeZoneInfo).DateTime;
        }
    }
}
