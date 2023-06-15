using UnityEngine;

namespace AdaptableDialogAnalyzer
{
    public static class APCA
    {
        /// <summary>
        /// 在黑白两种色彩中选择，由于程序基本配色为黑底白字，白色被选中的几率更高
        /// </summary>
        public static Color GetBlackOrWhite(Color baseColor, Color colorWhite, Color colorBlack)
        {
            return GetMostVisibleColor(baseColor, colorWhite, colorBlack, 1, 0.9f);
        }

        /// <summary>
        /// 选择两种颜色在基础色彩上可视度最高的颜色
        /// </summary>
        /// <param name="baseColor"></param>
        /// <param name="colorOptionA"></param>
        /// <param name="colorOptionB"></param>
        /// <param name="probabilityOptionA">颜色选项A的选中概率</param>
        /// <param name="probabilityOptionB">颜色选项B的选中概率</param>
        /// <returns></returns>
        public static Color GetMostVisibleColor(Color baseColor, Color colorOptionA, Color colorOptionB, float probabilityOptionA = 1, float probabilityOptionB = 1)
        {
            // 分别计算两种颜色的可视度
            float optionAVisibility = CalculateContrast(baseColor, colorOptionA) * probabilityOptionA;
            float optionBVisibility = CalculateContrast(baseColor, colorOptionB) * probabilityOptionB;

            // 返回可见度最高的可选色彩
            return optionAVisibility > optionBVisibility ? colorOptionA : colorOptionB;
        }

        // 计算两个颜色的对比度
        public static float CalculateContrast(Color colorA, Color colorB)
        {
            // 转换为LMS颜色空间
            Vector3 lmsColor1 = RGBToLMS(colorA);
            Vector3 lmsColor2 = RGBToLMS(colorB);

            // 计算LMS对比度
            float lmsContrast = Vector3.Distance(lmsColor1, lmsColor2);

            // 转换为可感知的对比度
            float perceptualContrast = LMSToPerceptualContrast(lmsContrast);

            return perceptualContrast;
        }

        // 将RGB颜色值转换为LMS颜色空间
        private static Vector3 RGBToLMS(Color color)
        {
            // 使用转换矩阵将RGB颜色值转换为LMS颜色值
            float l = 0.3811f * color.r + 0.5783f * color.g + 0.0402f * color.b;
            float m = 0.1967f * color.r + 0.7244f * color.g + 0.0782f * color.b;
            float s = 0.0241f * color.r + 0.1288f * color.g + 0.8444f * color.b;

            return new Vector3(l, m, s);
        }

        // 将LMS对比度转换为可感知的对比度
        private static float LMSToPerceptualContrast(float lmsContrast)
        {
            // 根据实验数据和数学模型计算可感知的对比度
            float perceptualContrast = Mathf.Pow(lmsContrast, 0.43f);

            return perceptualContrast;
        }
    }
}