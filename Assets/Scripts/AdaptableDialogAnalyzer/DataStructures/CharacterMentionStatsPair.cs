using UnityEngine;

namespace AdaptableDialogAnalyzer.DataStructures
{
    /// <summary>
    /// 由A提到B和B提到A的CharacterMentionStats组成的对，用于计算和值和差值
    /// </summary>
    public class CharacterMentionStatsPair
    {
        CharacterMentionStats statsAToB;
        CharacterMentionStats statsBToA;

        public int CharacterAId => statsAToB.SpeakerId;
        public int CharacterBId => statsAToB.MentionedPersonId;

        public CharacterMentionStats StatsAToB { get => statsAToB; }
        public CharacterMentionStats StatsBToA { get => statsBToA; }

        public int Sum => statsAToB.Total + statsBToA.Total;

        /// <summary>
        /// 获取A提到B的占比与B提到A的占比的差值（不是与台词数的比例）
        /// </summary>
        /// <returns></returns>
        public float GetRatioDifference(int metionTotalA, int metionTotalB)
        {
            float ratioA = (float)statsAToB.Total / metionTotalA;
            float ratioB = (float)statsBToA.Total / metionTotalB;
            return Mathf.Abs(ratioA - ratioB);
        }

        /// <summary>
        /// 获取A提到B的占比与B提到A的占比的差值（不是与台词数的比例）
        /// </summary>
        /// <returns></returns>
        public float GetRatioDifference(MentionedCountManager mentionedCountManager, bool passZero, bool passSelf)
        {
            return GetRatioDifference(mentionedCountManager.CountMentionTotal(CharacterAId, passZero, passSelf), mentionedCountManager.CountMentionTotal(CharacterBId, passZero, passSelf));
        }

        /// <summary>
        /// 交换两个CharacterMentionStats
        /// </summary>
        public void Swap()
        {
            (statsAToB, statsBToA) = (statsBToA, statsAToB);
        }

        public CharacterMentionStatsPair(CharacterMentionStats statsAToB, CharacterMentionStats statsBToA)
        {
            this.statsAToB = statsAToB;
            this.statsBToA = statsBToA;
        }
    }
}