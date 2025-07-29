using System;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace work.ctrl3d
{
    public static class FileExtensions
    {
        /// <summary>
        /// 로컬 이미지 파일을 Sprite로 로드합니다.
        /// </summary>
        /// <param name="imagePath">이미지 파일 경로</param>
        /// <returns>로드된 Sprite, 실패시 null</returns>
        public static Sprite LoadAsSprite(this string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
            {
                Debug.LogError($"이미지 파일이 존재하지 않습니다: {imagePath}");
                return null;
            }

            try
            {
                var fileData = File.ReadAllBytes(imagePath);
                var texture = new Texture2D(2, 2);

                if (texture.LoadImage(fileData))
                {
                    return Sprite.Create(
                        texture,
                        new Rect(0, 0, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f)
                    );
                }

                Debug.LogError($"이미지 로드에 실패했습니다: {imagePath}");
                Object.Destroy(texture);
                return null;
            }
            catch (Exception e)
            {
                Debug.LogError($"이미지 로드 중 오류가 발생했습니다: {e.Message}");
                return null;
            }
        }

        /// <summary>
        /// 로컬 이미지 파일을 Texture2D로 로드합니다.
        /// </summary>
        /// <param name="imagePath">이미지 파일 경로</param>
        /// <returns>로드된 Texture2D, 실패시 null</returns>
        public static Texture2D LoadAsTexture(this string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
            {
                Debug.LogError($"이미지 파일이 존재하지 않습니다: {imagePath}");
                return null;
            }

            try
            {
                var fileData = File.ReadAllBytes(imagePath);
                var texture = new Texture2D(2, 2);

                if (texture.LoadImage(fileData))
                {
                    return texture;
                }

                Debug.LogError($"이미지 로드에 실패했습니다: {imagePath}");
                Object.Destroy(texture);
                return null;
            }
            catch (Exception e)
            {
                Debug.LogError($"이미지 로드 중 오류가 발생했습니다: {e.Message}");
                return null;
            }
        }
    }
}