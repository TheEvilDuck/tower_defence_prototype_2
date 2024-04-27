using UnityEngine;

namespace Common
{
    public class GameObjectIconsMaker: IconsMaker
    {
        private readonly Camera _renderCamera;
        private readonly RenderTexture _renderTexture;
        private readonly Transform _gameObjectParent;

        public GameObjectIconsMaker(Camera renderCamera, RenderTexture renderTexture, Transform gameObjectParent) : base(renderCamera, renderTexture)
        {
            _gameObjectParent = gameObjectParent;
        }

        public Texture2D Get<TObject>(TObject prefab) where TObject: MonoBehaviour
        {
            TObject obj = GameObject.Instantiate(prefab, _gameObjectParent);
            obj.transform.localPosition = Vector3.zero;
            Texture2D resultTexture = Get();
            GameObject.Destroy(obj.gameObject);

            return resultTexture;
        }
    }
}