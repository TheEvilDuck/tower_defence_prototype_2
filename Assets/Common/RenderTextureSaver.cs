using UnityEngine;

namespace Common
{
    public class RenderTextureSaver
    {
        private Camera _renderCamera;
        private RenderTexture _renderTexture;

        public RenderTextureSaver(Camera renderCamera, RenderTexture renderTexture)
        {
            _renderCamera = renderCamera;
            _renderTexture = renderTexture;

            _renderCamera.gameObject.SetActive(false);
        }

        public Texture2D MakeLevelIcon()
        {
            _renderCamera.gameObject.SetActive(true);
            _renderCamera.Render();

            RenderTexture.active = _renderTexture;
            Texture2D resultTexture = new Texture2D(_renderTexture.width,_renderTexture.height,TextureFormat.RGBA32,false);
            resultTexture.ReadPixels(new Rect(0,0,_renderTexture.width,_renderTexture.height),0,0);
            resultTexture.Apply();

            return resultTexture;
        }
    }
}
