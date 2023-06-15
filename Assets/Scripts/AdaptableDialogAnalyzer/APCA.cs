using UnityEngine;

namespace AdaptableDialogAnalyzer
{
    public static class APCA
    {
        /// <summary>
        /// �ںڰ�����ɫ����ѡ�����ڳ��������ɫΪ�ڵװ��֣���ɫ��ѡ�еļ��ʸ���
        /// </summary>
        public static Color GetBlackOrWhite(Color baseColor, Color colorWhite, Color colorBlack)
        {
            return GetMostVisibleColor(baseColor, colorWhite, colorBlack, 1, 0.9f);
        }

        /// <summary>
        /// ѡ��������ɫ�ڻ���ɫ���Ͽ��Ӷ���ߵ���ɫ
        /// </summary>
        /// <param name="baseColor"></param>
        /// <param name="colorOptionA"></param>
        /// <param name="colorOptionB"></param>
        /// <param name="probabilityOptionA">��ɫѡ��A��ѡ�и���</param>
        /// <param name="probabilityOptionB">��ɫѡ��B��ѡ�и���</param>
        /// <returns></returns>
        public static Color GetMostVisibleColor(Color baseColor, Color colorOptionA, Color colorOptionB, float probabilityOptionA = 1, float probabilityOptionB = 1)
        {
            // �ֱ����������ɫ�Ŀ��Ӷ�
            float optionAVisibility = CalculateContrast(baseColor, colorOptionA) * probabilityOptionA;
            float optionBVisibility = CalculateContrast(baseColor, colorOptionB) * probabilityOptionB;

            // ���ؿɼ�����ߵĿ�ѡɫ��
            return optionAVisibility > optionBVisibility ? colorOptionA : colorOptionB;
        }

        // ����������ɫ�ĶԱȶ�
        public static float CalculateContrast(Color colorA, Color colorB)
        {
            // ת��ΪLMS��ɫ�ռ�
            Vector3 lmsColor1 = RGBToLMS(colorA);
            Vector3 lmsColor2 = RGBToLMS(colorB);

            // ����LMS�Աȶ�
            float lmsContrast = Vector3.Distance(lmsColor1, lmsColor2);

            // ת��Ϊ�ɸ�֪�ĶԱȶ�
            float perceptualContrast = LMSToPerceptualContrast(lmsContrast);

            return perceptualContrast;
        }

        // ��RGB��ɫֵת��ΪLMS��ɫ�ռ�
        private static Vector3 RGBToLMS(Color color)
        {
            // ʹ��ת������RGB��ɫֵת��ΪLMS��ɫֵ
            float l = 0.3811f * color.r + 0.5783f * color.g + 0.0402f * color.b;
            float m = 0.1967f * color.r + 0.7244f * color.g + 0.0782f * color.b;
            float s = 0.0241f * color.r + 0.1288f * color.g + 0.8444f * color.b;

            return new Vector3(l, m, s);
        }

        // ��LMS�Աȶ�ת��Ϊ�ɸ�֪�ĶԱȶ�
        private static float LMSToPerceptualContrast(float lmsContrast)
        {
            // ����ʵ�����ݺ���ѧģ�ͼ���ɸ�֪�ĶԱȶ�
            float perceptualContrast = Mathf.Pow(lmsContrast, 0.43f);

            return perceptualContrast;
        }
    }
}