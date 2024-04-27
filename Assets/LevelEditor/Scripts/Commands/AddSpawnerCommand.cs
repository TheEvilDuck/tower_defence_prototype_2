using UnityEngine;

namespace LevelEditor.Commands
{
    public class AddSpawnerCommand : ICommand
    {
        private readonly SpawnerPositions _spawnerPositions;
        private readonly Vector2Int _position;

        public AddSpawnerCommand(SpawnerPositions spawnerPositions, Vector2Int position)
        {
            _spawnerPositions = spawnerPositions;
            _position = position;
        }

        public bool Execute()
        {
            return _spawnerPositions.TryAdd(_position);
        }

        public void Undo()
        {
            _spawnerPositions.Remove(_position);
        }
    }
}