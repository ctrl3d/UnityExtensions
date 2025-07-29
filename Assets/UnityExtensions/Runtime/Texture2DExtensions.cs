using UnityEngine;

namespace work.ctrl3d
{
    public static class Texture2DExtensions
    {
        public static Texture2D Decompress(this Texture2D source)
        {
            var renderTex = RenderTexture.GetTemporary(
                source.width,
                source.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            var previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            var readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }
    
        public static Sprite Texture2DToSprite(Texture2D texture)
        {
            var rect = new Rect(0, 0, texture.width, texture.height);
            var sprite = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));
            return sprite;
        }
    }
}