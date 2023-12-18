using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{
    public class LevelIconMaker
    {
        private Camera _renderCamera;
        private RenderTexture _renderTexture;

        public LevelIconMaker(Camera renderCamera, RenderTexture renderTexture)
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
            Texture2D resultTexture = new Texture2D(_renderTexture.width,_renderTexture.height,TextureFormat.RGB24,false);
            resultTexture.ReadPixels(new Rect(0,0,_renderTexture.width,_renderTexture.height),0,0);
            resultTexture.Apply();

            return resultTexture;
        }
    }
}
