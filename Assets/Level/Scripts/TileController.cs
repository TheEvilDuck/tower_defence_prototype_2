using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels.TileControl
{
    public class TileController
    {
        private TileConfig _tileConfig;
        private Tilemap _groundTileMap;
        private Tilemap _roadTileMap;

        public TileController(TileConfig tileConfig, Tilemap groundTileMap, Tilemap roadTileMap)
        {
            _tileConfig = tileConfig;
            _groundTileMap = groundTileMap;
            _roadTileMap = roadTileMap;
        }

        public void DrawAt(Vector2Int position)
        {
            _groundTileMap.SetTile(new Vector3Int(position.x,position.y),_tileConfig.GroundTile);
        }

        public void DrawRoadAt(Vector2Int position)
        {
            _roadTileMap.SetTile(new Vector3Int(position.x,position.y),_tileConfig.RoadTile);
        }

        public void RempoveAt(Vector2Int position)
        {
            _groundTileMap.SetTile(new Vector3Int(position.x,position.y),null);
        }

        public void RemoveRoadAt(Vector2Int position)
        {
            _roadTileMap.SetTile(new Vector3Int(position.x,position.y),null);
        }


    }
}
