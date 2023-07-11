using UnityEngine;

namespace AdaptableDialogAnalyzer
{
    /// <summary>
    /// 处理音频相关的事务
    /// </summary>
    public static class AudioHelper
    {
        /// <summary>
        /// 取自2.1版本的live2dsdk
        /// </summary>
        public static float GetCurrentVolume(AudioSource audio)
        {
            float[] data = new float[256];
            float sum = 0;
            audio.GetOutputData(data, 0);
            foreach (float s in data)
            {
                sum += Mathf.Abs(s);
            }
            return sum / 256.0f;
        }
    }
}