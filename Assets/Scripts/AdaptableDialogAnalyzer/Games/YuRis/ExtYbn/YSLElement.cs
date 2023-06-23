using System;
using System.Text;

namespace AdaptableDialogAnalyzer.Games.YuRis
{
    public class YSLElement
    {
        public string name;
        public uint value1;
        public uint value2;
        public ushort ybnId;
        public ushort value4;

        public int Length => 1 + name.Length + 12;

        public YSLElement(byte[] ysl, int startIndex)
        {
            int nameLength = ysl[startIndex];  // 跳转点名称的长度
            name = Encoding.UTF8.GetString(ysl, startIndex + 1, nameLength);  // 解析跳转点名称

            // 解析4个 int 值
            value1 = BitConverter.ToUInt32(ysl, startIndex + 1 + nameLength);
            value2 = BitConverter.ToUInt32(ysl, startIndex + 1 + nameLength + 4);
            ybnId = BitConverter.ToUInt16(ysl, startIndex + 1 + nameLength + 8);
            value4 = BitConverter.ToUInt16(ysl, startIndex + 1 + nameLength + 10);
        }
    }

}
