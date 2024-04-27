using Levels.Logic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Levels.Tiles
{
    public class TileController
    {
        private TileDatabase _tileDatabase;
        private Tilemap _groundTileMap;
        private Tilemap _roadTileMap;

        public TileController(TileDatabase tileDatabase, Tilemap groundTileMap, Tilemap roadTileMap)
        {
            _tileDatabase = tileDatabase;
            _groundTileMap = groundTileMap;
            _roadTileMap = roadTileMap;
        }

        public void DrawAt(Vector2Int position, TileType tileType)
        {
            _tileDatabase.TryGetValue(tileType, out TileConfig config);

            _groundTileMap.SetTile(new Vector3Int(position.x,position.y),config.Tile);
        }

        public void DrawRoadAt(Vector2Int position)
        {
            _roadTileMap.SetTile(new Vector3Int(position.x,position.y),_tileDatabase.RoadTile);
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
