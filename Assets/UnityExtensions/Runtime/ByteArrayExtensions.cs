using System;
using System.Linq;
using System.Text;

namespace work.ctrl3d
{
    public static class ByteArrayExtensions
    {
        // 16진수 문자열 (StringBuilder 사용 최적화)
        public static string ToHexString(this byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return string.Empty;
                
            // 예상 길이 할당 (2글자 + 공백 1글자)
            var sb = new StringBuilder(bytes.Length * 3);
            foreach (var b in bytes)
            {
                sb.Append(b.ToString("X2")).Append(' ');
            }
                
            // 마지막 공백 제거가 필요하다면
            if (sb.Length > 0) sb.Length--;
                
            return sb.ToString();
        }

        // 16진수 문자열 (구분자 없음)
        public static string ToHexStringCompact(this byte[] bytes) =>
            string.Concat(bytes.Select(b => b.ToString("X2")));
        
        // 16진수와 ASCII를 함께 표시
        public static string ToHexDump(this byte[] bytes, int bytesPerLine = 16)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < bytes.Length; i += bytesPerLine)
            {
                // 주소 표시
                sb.Append($"{i:X8}: ");

                // 16진수 표시
                var lineBytes = bytes.Skip(i).Take(bytesPerLine).ToArray();
                foreach (var b in lineBytes)
                {
                    sb.Append($"{b:X2} ");
                }

                // 빈 공간 채우기
                for (var j = lineBytes.Length; j < bytesPerLine; j++)
                {
                    sb.Append("   ");
                }

                // ASCII 표시
                sb.Append("| ");
                foreach (var b in lineBytes)
                {
                    var c = b is >= 32 and <= 126 ? (char)b : '.';
                    sb.Append(c);
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        // 숫자 배열로 표시
        public static string ToNumberString(this byte[] bytes) =>
            $"[{string.Join(", ", bytes.Select(b => b.ToString()))}]";

        // 비트 문자열로 표시
        public static string ToBinaryString(this byte[] bytes) =>
            string.Join(" ", bytes.Select(b => Convert.ToString(b, 2).PadLeft(8, '0')));


        // 상세 정보와 함께 표시
        public static string ToDetailedString(this byte[] bytes, Encoding encoding = null)
        {
            encoding ??= Encoding.UTF8;

            var sb = new StringBuilder();
            sb.AppendLine($"Length: {bytes.Length} bytes");
            sb.AppendLine($"Hex: {bytes.ToHexString()}");

            try
            {
                var text = encoding.GetString(bytes);
                sb.AppendLine($"Text ({encoding.EncodingName}): \"{text}\"");
            }
            catch
            {
                sb.AppendLine("Text: [Binary data - cannot decode as text]");
            }

            return sb.ToString();
        }
    }
}