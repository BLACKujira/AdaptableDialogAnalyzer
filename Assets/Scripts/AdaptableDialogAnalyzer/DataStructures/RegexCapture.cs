using System;

namespace AdaptableDialogAnalyzer.DataStructures
{
    /// <summary>
    /// 可序列化的Regex匹配结果
    /// </summary>
    [Serializable]
    public class RegexCapture
    {
        public int index;
        public int length;
        public string value;

        public RegexCapture(int index, int length, string value)
        {
            this.index = index;
            this.length = length;
            this.value = value;
        }

        public static RegexCapture FromCapture(System.Text.RegularExpressions.Capture capture)
        {
            RegexCapture regexCapture = new RegexCapture(capture.Index, capture.Length, capture.Value);
            return regexCapture;
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            RegexCapture regexCapture = obj as RegexCapture;
            if (regexCapture != null && index == regexCapture.index && length == regexCapture.length)
            {
                return true;
            }
            
            return base.Equals(obj);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            unchecked // 防止溢出
            {
                int hash = 17;
                hash = hash * 31 + index.GetHashCode();
                hash = hash * 31 + length.GetHashCode();
                return hash;
            }
        }
    }
}