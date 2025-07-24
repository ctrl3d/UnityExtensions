using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using UnityEngine;

namespace work.ctrl3d
{
    public static class StringExtensions
    {
        /// <summary>
        /// 지정된 구분자를 기준으로 문자열을 'Head'와 'Body'로 분리합니다.
        /// </summary>
        /// <param name="data">분리할 문자열입니다.</param>
        /// <param name="separator">문자열을 분리하는 데 사용할 구분자입니다. 기본값은 '@'입니다.</param>
        /// <returns>구분자 앞부분이 'Head', 뒷부분이 'Body'로 나뉘어진 튜플을 반환합니다. 
        /// 만약 구분자가 없으면 'Head'는 전체 문자열을 담고 'Body'는 비어있게 됩니다.</returns>
        public static (string Head, string Body) SplitHeadAndBody(this string data, string separator = "@")
        {
            if (string.IsNullOrEmpty(data))
                return (Head: string.Empty, Body: string.Empty);

            var splitData = data.Split(separator);

            var head = splitData.Length > 0 ? splitData[0] : string.Empty;
            var body = splitData.Length > 1 ? splitData[1] : string.Empty;

            return (Head: head, Body: body);
        }

        /// <summary>
        /// 지정된 구분자를 기준으로 문자열을 쪼갭니다.
        /// </summary>
        /// <param name="data">쪼갤 문자열입니다.</param>
        /// <param name="separator">문자열을 쪼개는 데 사용할 구분자입니다. 기본값은 ','입니다.</param>
        /// <returns>쪼개진 문자열이 담긴 배열을 반환합니다. 문자열에 구분자가 포함되어 있지 않은 경우 배열의 크기는 1입니다.</returns>
        public static string[] SplitStrings(this string data, string separator = ",")
        {
            if (string.IsNullOrEmpty(data))
                return new string[] { };

            var splitData = data.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);

            return splitData;
        }
        
        /// <summary>
        /// 문자열을 압축하여 바이트 배열로 반환합니다.
        /// </summary>
        public static byte[] CompressToBytes(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return Array.Empty<byte>();

            var textBytes = Encoding.UTF8.GetBytes(text);
            using var memoryStream = new MemoryStream();
            using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                gzipStream.Write(textBytes, 0, textBytes.Length);
            }
            return memoryStream.ToArray();
        }


        /// <summary>
        /// 압축된 바이트 배열을 문자열로 해제합니다.
        /// </summary>
        public static string DecompressFromBytes(this byte[] compressedBytes)
        {
            if (compressedBytes == null || compressedBytes.Length == 0)
                return string.Empty;

            using var compressedStream = new MemoryStream(compressedBytes);
            using var gzipStream = new GZipStream(compressedStream, CompressionMode.Decompress);
            using var decompressedStream = new MemoryStream();
            gzipStream.CopyTo(decompressedStream);
            var decompressedBytes = decompressedStream.ToArray();
            return Encoding.UTF8.GetString(decompressedBytes);
        }

        /// <summary>
        /// 문자열을 압축하고 Base64로 변환합니다.
        /// </summary>
        public static string Compress(this string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            var compressedBytes = text.CompressToBytes();
            return Convert.ToBase64String(compressedBytes);
        }

        /// <summary>
        /// Base64 압축 문자열을 원래 문자열로 복원합니다.
        /// </summary>
        public static string Decompress(this string compressedData)
        {
            if (string.IsNullOrEmpty(compressedData))
                return string.Empty;

            var compressedBytes = Convert.FromBase64String(compressedData);
            return compressedBytes.DecompressFromBytes();
        }
        
        /// <summary> 
        /// 지정된 문자열을 모두 빈 문자열로 바꾼 새 문자열을 반환합니다. 
        /// </summary> 
        /// <remarks> 
        /// "hello world".ReplaceEmpty("hello") → " world"
        /// </remarks> 
        public static string ReplaceEmpty(this string self, string oldValue)
        {
            return self.Replace(oldValue, string.Empty);
        }
        
        /// <summary>
        /// 줄 바꿈 코드를 제거한 문자열을 반환합니다.
        /// </summary>
        public static string RemoveNewLine(this string self)
        {
            return self.ReplaceEmpty("\n").ReplaceEmpty("\r");
        }
        
        /// <summary>
        /// 문자열이 URL 형태인지 판별
        /// </summary>
        /// <param name="input">검사할 문자열</param>
        /// <returns>URL 형태 여부</returns>
        public static bool IsUrl(this string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return false;

            // 이미 프로토콜이 있는 경우
            if (input.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                input.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                return true;

            // 로컬 파일 경로 패턴 제외
            if (input.Contains(":\\") ||
                input.StartsWith("./") ||
                input.StartsWith("../") ||
                input.StartsWith("/"))
                return false;

            // 도메인 형태 확인 (점이 포함되고 공백이 없음)
            return input.Contains(".") && !input.Contains(" ") && !input.Contains("\\");
        }

        /// <summary>
        /// URL에 프로토콜을 추가 (없는 경우에만)
        /// </summary>
        /// <param name="url">URL 문자열</param>
        /// <param name="defaultProtocol">기본 프로토콜 (기본값: https://)</param>
        /// <returns>프로토콜이 추가된 URL</returns>
        public static string EnsureProtocol(this string url, string defaultProtocol = "https://")
        {
            if (string.IsNullOrWhiteSpace(url) ||
                url.StartsWith("https://", StringComparison.OrdinalIgnoreCase) ||
                url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                return url;

            return defaultProtocol + url;
        }

        /// <summary>
        /// 문자열이 로컬 파일 경로인지 판별
        /// </summary>
        /// <param name="input">검사할 문자열</param>
        /// <returns>로컬 파일 경로 여부</returns>
        public static bool IsLocalPath(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            return input.Contains(":\\") ||
                   input.StartsWith("./") ||
                   input.StartsWith("../") ||
                   input.StartsWith("/") ||
                   !input.IsUrl();
        }
        
        /// <summary>
        /// 문자열을 Unity Color로 칠합니다.
        /// </summary>
        public static string WithColor(this string str, Color color)
        {
            var hexColor = ColorUtility.ToHtmlStringRGB(color);
            return $"<color=#{hexColor}>{str}</color>";
        }

        /// <summary>
        /// 문자열을 Color32로 칠합니다.
        /// </summary>
        public static string WithColor(this string str, Color32 color)
        {
            return $"<color=#{color.r:X2}{color.g:X2}{color.b:X2}>{str}</color>";
        }

        
    }
}