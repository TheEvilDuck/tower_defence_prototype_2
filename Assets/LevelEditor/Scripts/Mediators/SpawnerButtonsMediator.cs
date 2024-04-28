using System;
using LevelEditor.Tools;
using LevelEditor.UI;

namespace LevelEditor.Mediators
{
    public class SpawnerButtonsMediator: IDisposable
    {
        private readonly SpawnersMenu _spawnersMenu;
        private readonly LevelEditor _levelEditor;
        private readonly Tool _removeSpawnersTool;
        private readonly Tool _addSpawnersTool;

        public SpawnerButtonsMediator(SpawnersMenu spawnersMenu, LevelEditor levelEditor, Tool removeSpawnersTool, Tool addSpawnersTool)
        {
            _spawnersMenu = spawnersMenu;
            _levelEditor = levelEditor;
            _removeSpawnersTool = removeSpawnersTool;
            _addSpawnersTool = addSpawnersTool;

            _spawnersMenu.drawButtonClicked += OnDrawButtonPressed;
            _spawnersMenu.deleteButtonClicked += OnDeleteButtonPressed;
        }

        public void Dispose()
        {
            _spawnersMenu.drawButtonClicked -= OnDrawButtonPressed;
            _spawnersMenu.deleteButtonClicked -= OnDeleteButtonPressed;
        }

        private void OnDrawButtonPressed()
        {
            _levelEditor.ChangeTool(_addSpawnersTool);
        }

        private void OnDeleteButtonPressed()
        {
            _levelEditor.ChangeTool(_removeSpawnersTool);
        }
    }
}