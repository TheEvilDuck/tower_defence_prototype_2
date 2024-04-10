using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileConfig", menuName = "Configs/New tile config")]
public class TileConfig : ScriptableObject
{
    [field: SerializeField] public TileBase Tile {get; private set;}
}
