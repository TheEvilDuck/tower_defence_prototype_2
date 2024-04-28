using UnityEngine;

namespace LevelEditor.Commands
{
    public class RemoveSpawnerCommand : ICommand
    {
        private readonly SpawnerPositions _spawnerPositions;
        private readonly Vector2Int _position;

        public RemoveSpawnerCommand(SpawnerPositions spawnerPositions, Vector2Int position)
        {
            _spawnerPositions = spawnerPositions;
            _position = position;
        }

        public bool Execute()
        {
            return _spawnerPositions.Remove(_position);
        }

        public void Undo()
        {
            _spawnerPositions.TryAdd(_position);
        }
    }
}