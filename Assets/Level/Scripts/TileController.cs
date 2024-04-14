using Grid = Levels.Logic.Grid;
using UnityEngine;
using UnityEngine.Tilemaps;
using Levels.Tiles;

namespace Levels.TileControl
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

            Debug.Log("A");
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
