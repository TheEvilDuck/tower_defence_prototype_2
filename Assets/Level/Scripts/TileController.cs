using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels.TileControl
{
    public class TileController
    {
        private TileControllerData _tileControllerData;
        private Tilemap _groundTileMap;

        public TileController(TileControllerData tileControllerData, Tilemap groundTileMap)
        {
            _tileControllerData = tileControllerData;
            _groundTileMap = groundTileMap;
        }

        public void DrawAt(Vector2Int position)
        {
            _groundTileMap.SetTile(new Vector3Int(position.x,position.y),_tileControllerData.groundTileRule);
        }

        public void RempoveAt(Vector2Int position)
        {
            _groundTileMap.SetTile(new Vector3Int(position.x,position.y),null);
        }


    }
}
