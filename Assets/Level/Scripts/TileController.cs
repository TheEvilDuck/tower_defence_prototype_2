using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels.TileControl
{
    public class TileController
    {
        private TileConfig _tileConfig;
        private Tilemap _groundTileMap;

        public TileController(TileConfig tileConfig, Tilemap groundTileMap)
        {
            _tileConfig = tileConfig;
            _groundTileMap = groundTileMap;
        }

        public void DrawAt(Vector2Int position)
        {
            _groundTileMap.SetTile(new Vector3Int(position.x,position.y),_tileConfig.GroundTile);
        }

        public void RempoveAt(Vector2Int position)
        {
            _groundTileMap.SetTile(new Vector3Int(position.x,position.y),null);
        }


    }
}
