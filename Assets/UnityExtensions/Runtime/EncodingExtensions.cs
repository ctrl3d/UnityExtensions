using System;
using System.Text;

namespace work.ctrl3d
{
    [Serializable]
    public enum EncodingType
    {
        UTF8 = 65001,
        ASCII = 20127,
        Unicode = 1200,
        BigEndianUnicode = 1201,
        UTF32 = 12000,
        EucKr = 51949,        // 한국어
        ShiftJis = 932,       // 일본어
        Gb2312 = 936,         // 중국어 간체
        Big5 = 950,           // 중국어 번체
        Windows1252 = 1252,   // 서유럽
        Iso88591 = 28591      // Latin-1
    }

    public static class EncodingExtensions
    {
        public static Encoding ToEncoding(this EncodingType encodingType)
        {
            return Encoding.GetEncoding((int)encodingType);
        }
    
        public static int GetCodePage(this EncodingType encodingType)
        {
            return (int)encodingType;
        }
    }
}