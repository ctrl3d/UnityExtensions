using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace work.ctrl3d
{
    public static class PathExtensions
    {
        /// <summary>
        /// 경로가 유효한 이미지 파일인지 확인합니다.
        /// </summary>
        /// <param name="path">확인할 경로</param>
        /// <returns>유효한 이미지 파일이면 true</returns>
        public static bool IsValidImagePath(this string path)
        {
            if (string.IsNullOrEmpty(path)) return false;

            var extension = Path.GetExtension(path).ToLower();
            return extension is ".png" or ".jpg" or ".jpeg" or ".bmp" or ".tga" or ".gif";
        }

        /// <summary>
        /// 경로의 이미지 파일을 Sprite로 로드합니다.
        /// </summary>
        /// <param name="imagePath">이미지 파일 경로</param>
        /// <returns>로드된 Sprite, 실패시 null</returns>
        public static Sprite LoadAsSprite(this string imagePath)
        {
            // Path.GetFullPath()로 절대 경로 정규화
            var fullPath = Path.GetFullPath(imagePath);

            if (!File.Exists(fullPath))
            {
                Debug.LogError($"이미지 파일이 존재하지 않습니다: {fullPath}");
                return null;
            }

            if (!fullPath.IsValidImagePath())
            {
                Debug.LogError($"지원하지 않는 이미지 형식입니다: {Path.GetExtension(fullPath)}");
                return null;
            }

            try
            {
                var fileData = File.ReadAllBytes(fullPath);
                var texture = new Texture2D(2, 2);

                if (texture.LoadImage(fileData))
                {
                    // 파일명을 텍스처 이름으로 설정
                    texture.name = Path.GetFileNameWithoutExtension(fullPath);

                    var sprite = Sprite.Create(
                        texture,
                        new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f)
                    );

                    // 스프라이트 이름도 설정
                    sprite.name = texture.name;
                    return sprite;
                }

                Debug.LogError($"이미지 로드에 실패했습니다: {fullPath}");
                Object.Destroy(texture);
                return null;
            }
            catch (Exception e)
            {
                Debug.LogError($"이미지 로드 중 오류가 발생했습니다 ({Path.GetFileName(fullPath)}): {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 경로의 이미지 파일을 Texture2D로 로드합니다.
        /// </summary>
        /// <param name="imagePath">이미지 파일 경로</param>
        /// <returns>로드된 Texture2D, 실패시 null</returns>
        public static Texture2D LoadAsTexture(this string imagePath)
        {
            var fullPath = Path.GetFullPath(imagePath);

            if (!File.Exists(fullPath))
            {
                Debug.LogError($"이미지 파일이 존재하지 않습니다: {fullPath}");
                return null;
            }

            if (!fullPath.IsValidImagePath())
            {
                Debug.LogError($"지원하지 않는 이미지 형식입니다: {Path.GetExtension(fullPath)}");
                return null;
            }

            try
            {
                var fileData = File.ReadAllBytes(fullPath);
                var texture = new Texture2D(2, 2);

                if (texture.LoadImage(fileData))
                {
                    texture.name = Path.GetFileNameWithoutExtension(fullPath);
                    return texture;
                }

                Debug.LogError($"이미지 로드에 실패했습니다: {fullPath}");
                Object.Destroy(texture);
                return null;
            }
            catch (Exception e)
            {
                Debug.LogError($"이미지 로드 중 오류가 발생했습니다 ({Path.GetFileName(fullPath)}): {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 디렉토리 내의 지정된 확장자 파일들을 가져옵니다.
        /// </summary>
        /// <param name="directoryPath">디렉토리 경로</param>
        /// <param name="extensions">검색할 확장자 배열 (예: "*.png", "*.txt", "cs")</param>
        /// <param name="searchOption">검색 옵션 (하위 디렉토리 포함 여부)</param>
        /// <returns>파일 경로 배열</returns>
        public static string[] GetFilesByExtensions(this string directoryPath, string[] extensions,
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            if (!Directory.Exists(directoryPath) || extensions == null || extensions.Length == 0)
                return Array.Empty<string>();

            var files = new List<string>();

            foreach (var extension in extensions)
            {
                // 확장자가 *로 시작하지 않으면 자동으로 추가
                var searchPattern = extension.StartsWith("*") ? extension : $"*.{extension.TrimStart('.')}";
                files.AddRange(Directory.GetFiles(directoryPath, searchPattern, searchOption));
            }

            return files.ToArray();
        }

        /// <summary>
        /// 디렉토리 내의 이미지 파일들을 가져옵니다. (편의 메서드)
        /// </summary>
        /// <param name="directoryPath">디렉토리 경로</param>
        /// <param name="searchOption">검색 옵션</param>
        /// <returns>이미지 파일 경로 배열</returns>
        public static string[] GetImageFiles(this string directoryPath,
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var imageExtensions = new[] { "*.png", "*.jpg", "*.jpeg", "*.bmp", "*.tga", "*.gif", "*.webp" };
            return directoryPath.GetFilesByExtensions(imageExtensions, searchOption);
        }

        /// <summary>
        /// 디렉토리 내의 오디오 파일들을 가져옵니다. (편의 메서드)
        /// </summary>
        /// <param name="directoryPath">디렉토리 경로</param>
        /// <param name="searchOption">검색 옵션</param>
        /// <returns>오디오 파일 경로 배열</returns>
        public static string[] GetAudioFiles(this string directoryPath,
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var audioExtensions = new[] { "*.mp3", "*.wav", "*.ogg", "*.aac", "*.flac" };
            return directoryPath.GetFilesByExtensions(audioExtensions, searchOption);
        }

        /// <summary>
        /// 디렉토리 내의 동영상 파일들을 가져옵니다. (편의 메서드)
        /// </summary>
        /// <param name="directoryPath">디렉토리 경로</param>
        /// <param name="searchOption">검색 옵션</param>
        /// <returns>동영상 파일 경로 배열</returns>
        public static string[] GetVideoFiles(this string directoryPath,
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var videoExtensions = new[] { "*.mp4", "*.avi", "*.mov", "*.wmv", "*.flv", "*.webm" };
            return directoryPath.GetFilesByExtensions(videoExtensions, searchOption);
        }

        /// <summary>
        /// 디렉토리 내의 텍스트 파일들을 가져옵니다. (편의 메서드)
        /// </summary>
        /// <param name="directoryPath">디렉토리 경로</param>
        /// <param name="searchOption">검색 옵션</param>
        /// <returns>텍스트 파일 경로 배열</returns>
        public static string[] GetTextFiles(this string directoryPath,
            SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var textExtensions = new[] { "*.txt", "*.md", "*.json", "*.xml", "*.csv", "*.log" };
            return directoryPath.GetFilesByExtensions(textExtensions, searchOption);
        }
    }
}