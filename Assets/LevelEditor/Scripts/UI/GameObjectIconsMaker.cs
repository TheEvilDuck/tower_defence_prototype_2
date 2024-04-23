using UnityEngine;

public class GameObjectIconsMaker
{
    private readonly Camera _renderCamera;
    private readonly RenderTexture _renderTexture;
    private readonly Transform _gameObjectParent;

    public GameObjectIconsMaker(Camera renderCamera, RenderTexture renderTexture, Transform gameObjectParent)
    {
        _renderCamera = renderCamera;
        _renderTexture = renderTexture;
        _gameObjectParent = gameObjectParent;

    }

    public Texture2D Get<TObject>(TObject prefab) where TObject: MonoBehaviour
    {
        RenderTexture.active = _renderTexture;

        TObject obj = GameObject.Instantiate(prefab, _gameObjectParent);
        obj.transform.localPosition = Vector3.zero;

        _renderCamera.Render();

        Texture2D resultTexture = new Texture2D(_renderTexture.width,_renderTexture.height, TextureFormat.RGBA32, false);
        resultTexture.ReadPixels(new Rect(0,0,_renderTexture.width,_renderTexture.height),0,0);


        resultTexture.Apply();

        GameObject.Destroy(obj.gameObject);

        _renderCamera.Render();
        RenderTexture.active = null;
        _renderTexture.Release();
        return resultTexture;
    }

    public Texture2D GetWithTemp<TObject>(TObject prefab) where TObject: MonoBehaviour
    {
        RenderTexture temp = RenderTexture.GetTemporary(_renderTexture.descriptor);
        RenderTexture.active = temp;

        TObject obj = GameObject.Instantiate(prefab, _gameObjectParent);
        obj.transform.localPosition = Vector3.zero;

        _renderCamera.targetTexture = temp;
        _renderCamera.Render();

        Texture2D resultTexture = new Texture2D(temp.width,temp.height, TextureFormat.RGBA32, false);
        resultTexture.ReadPixels(new Rect(0,0,temp.width,temp.height),0,0);

        resultTexture.Apply();

        GameObject.Destroy(obj.gameObject);

        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(temp);
        return resultTexture;

    } 
}