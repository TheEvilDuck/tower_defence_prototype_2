using UnityEngine;

namespace Common
{
    public class IconsMaker
    {
        private readonly Camera _renderCamera;
        private readonly RenderTexture _renderTexture;

        public IconsMaker(Camera renderCamera, RenderTexture renderTexture)
        {
            _renderCamera = renderCamera;
            _renderTexture = renderTexture;

        }

        public Texture2D Get()
        {
            RenderTexture.active = _renderTexture;

            _renderCamera.Render();

            Texture2D resultTexture = new Texture2D(_renderTexture.width,_renderTexture.height, TextureFormat.RGBA32, false);
            resultTexture.ReadPixels(new Rect(0,0,_renderTexture.width,_renderTexture.height),0,0);

            resultTexture.Apply();

            _renderCamera.Render();
            RenderTexture.active = null;
            _renderTexture.Release();
            return resultTexture;
        }
    }
}