using System;
using System.Text;
using UnityEngine;

// Texture2D 사용을 위해 필요

namespace work.ctrl3d
{
    public static class ByteArrayExtensions
    {
        public static string ToHexString(this byte[] bytes, string separator = " ")
        {
            if (bytes == null || bytes.Length == 0) return string.Empty;
            
            var capacity = bytes.Length * (2 + separator.Length);
            var sb = new StringBuilder(capacity);

            foreach (var b in bytes)
            {
                sb.Append(b.ToString("X2")).Append(separator);
            }
            
            if (separator.Length > 0 && sb.Length > 0)
                sb.Length -= separator.Length;

            return sb.ToString();
        }
        
        public static string ToHexDump(this byte[] bytes, int bytesPerLine = 16)
        {
            if (bytes == null || bytes.Length == 0) return "<Empty>";

            var sb = new StringBuilder();
            var length = bytes.Length;

            for (var i = 0; i < length; i += bytesPerLine)
            {
                sb.Append($"{i:X8}: ");
                
                for (var j = 0; j < bytesPerLine; j++)
                {
                    if (i + j < length)
                        sb.Append($"{bytes[i + j]:X2} ");
                    else
                        sb.Append("   "); // 데이터가 없을 때 공백 채움
                }

                sb.Append("| ");
                
                for (var j = 0; j < bytesPerLine; j++)
                {
                    if (i + j >= length) continue;
                    var b = bytes[i + j];
                    var c = b is >= 32 and <= 126 ? (char)b : '.';
                    sb.Append(c);
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }
        
        public static string ToDetailedString(this byte[] bytes)
        {
            if (bytes == null) return "null";
            
            var sb = new StringBuilder();
            sb.AppendLine($"[Byte Array Info] Length: {bytes.Length:N0} bytes");

            sb.AppendLine(bytes.Length > 256
                ? $"Head (First 256): {bytes.ToHexString().Substring(0, 256 * 3)}..."
                : $"Hex: {bytes.ToHexString()}");
            
            try
            {
                var utf8Text = Encoding.UTF8.GetString(bytes);
                // 제어 문자가 너무 많으면 텍스트가 아닐 확률이 높음 (간단 체크)
                if (!string.IsNullOrEmpty(utf8Text) && utf8Text.Length < 1000) 
                    sb.AppendLine($"Text (UTF8): \"{utf8Text}\"");
            }
            catch
            {
                // ignored
            }

            return sb.ToString();
        }
        
        /// <summary>
        /// 바이트 배열을 Base64 문자열로 빠르게 변환합니다. (API 전송용)
        /// </summary>
        public static string ToBase64(this byte[] bytes)
        {
            if (bytes == null) return null;
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// 바이트 배열을 Unity Texture2D로 변환합니다. (이미지 수신용)
        /// </summary>
        public static Texture2D ToTexture2D(this byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;
            
            var tex = new Texture2D(2, 2);
            if (tex.LoadImage(bytes))
            {
                tex.Apply();
                return tex;
            }
            
            UnityEngine.Object.Destroy(tex);
            return null;
        }
    }
}