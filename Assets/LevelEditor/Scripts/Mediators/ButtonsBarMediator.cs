using System;
using System.Linq;
using Builder;
using Common;
using LevelEditor.Selectors;
using LevelEditor.Tools;
using LevelEditor.UI;
using LevelEditor.UI.WavesEditing;
using Levels.Logic;
using UnityEngine;
using Grid = Levels.Logic.Grid;

namespace LevelEditor.Mediators
{
    public class ButtonsBarMediator: IDisposable
    {
        private readonly ButtonsBar _buttonsBar;
        private readonly SceneLoader _sceneLoader;
        private readonly MenuParentsManager _barMenus;
        private readonly SettingsMenu _settingsMenu;
        private readonly WavesEditor _waveEditor;
        private readonly LoadMenu _loadMenu;
        private readonly TowersMenu _towersMenu;
        private readonly ToolButtons _toolButtons;
        private readonly LevelEditor _levelEditor;
        private readonly BrushSelector _brushSelector;
        private readonly DeleteSelector _deleteSelector;
        private readonly Tool _drawTool;
        private readonly Grid _grid;
        private readonly LevelSavingUI _levelSavingUI;
        private readonly SpawnerPlacamentSelector _spawnerPlacementSelector;
        private readonly Tool _spawnerPlacer;
        private readonly TowersSettingsMenu _towersSettingsMenu;
        private readonly Tool _deletingPlacableTool;
        private readonly PlacablesContainer _placablesContainer;
        private readonly TowersPlaceMenu _towersPlaceMenu;
        private readonly SpawnersMenu _spawnersMenu;
        private readonly SpawnerPositions _spawnerPositions;


        public ButtonsBarMediator
        (
            ButtonsBar buttonsBar,
            SceneLoader sceneLoader, 
            MenuParentsManager barMenus,
            SettingsMenu settingsMenu,
            WavesEditor wavesEditor,
            LoadMenu loadMenu,
            TowersMenu towersMenu,
            ToolButtons toolButtons,
            LevelEditor levelEditor,
            BrushSelector brushSelector,
            Tool drawTool,
            Grid grid,
            LevelSavingUI levelSavingUI,
            SpawnerPlacamentSelector spawnerPlacamentSelector,
            Tool spawnerPlacer,
            TowersSettingsMenu towersSettingsMenu,
            Tool deletingPlacableTool,
            PlacablesContainer placablesContainer,
            TowersPlaceMenu towersPlaceMenu,
            DeleteSelector deleteSelector,
            SpawnersMenu spawnersMenu,
            SpawnerPositions spawnerPositions
        )
        {
            _buttonsBar = buttonsBar;
            _sceneLoader = sceneLoader;
            _barMenus = barMenus;
            _settingsMenu = settingsMenu;
            _waveEditor = wavesEditor;
            _loadMenu = loadMenu;
            _towersMenu = towersMenu;
            _toolButtons = toolButtons;
            _levelEditor = levelEditor;
            _brushSelector = brushSelector;
            _drawTool = drawTool;
            _grid = grid;
            _levelSavingUI = levelSavingUI;
            _spawnerPlacementSelector = spawnerPlacamentSelector;
            _spawnerPlacer = spawnerPlacer;
            _towersSettingsMenu = towersSettingsMenu;
            _deletingPlacableTool = deletingPlacableTool;
            _placablesContainer = placablesContainer;
            _towersPlaceMenu = towersPlaceMenu;
            _deleteSelector = deleteSelector;
            _spawnersMenu = spawnersMenu;
            _spawnerPositions = spawnerPositions;

            _buttonsBar.saveButtonPressed+=OnSaveKeyCombinationDown;
            _buttonsBar.exitButtonPressed+=OnExitButtonPressed;
            _buttonsBar.settingsButtonPressed+=OnSettingsButtonPressed;
            _buttonsBar.wavesButtonPressed+=OnWavesButtonPressed;
            _buttonsBar.loadButtonPressed+=OnLoadButtonPressed;
            _buttonsBar.deleteButtonPressed+=OnDeleteButtonPressed;
            _buttonsBar.newButtonPressed+=OnNewButtonPressed;
            _buttonsBar.spawnerButtonPressed+=OnSpawnerButtonPressed;
            _buttonsBar.toolsButtonPressed += OnToolButtonPresed;
            _buttonsBar.towersButtonPressed += OnTowersButtonPressed;
        }

        public void Dispose()
        {
            _buttonsBar.saveButtonPressed-=OnSaveKeyCombinationDown;
            _buttonsBar.exitButtonPressed-=OnExitButtonPressed;
            _buttonsBar.settingsButtonPressed-=OnSettingsButtonPressed;
            _buttonsBar.wavesButtonPressed-=OnWavesButtonPressed;
            _buttonsBar.loadButtonPressed-=OnLoadButtonPressed;
            _buttonsBar.deleteButtonPressed-=OnDeleteButtonPressed;
            _buttonsBar.newButtonPressed-=OnNewButtonPressed;
            _buttonsBar.toolsButtonPressed -= OnToolButtonPresed;
            _buttonsBar.spawnerButtonPressed-=OnSpawnerButtonPressed;
            _buttonsBar.towersButtonPressed -= OnTowersButtonPressed;
        }

        private void OnExitButtonPressed() => _sceneLoader.LoadMainMenu();
        private void OnSettingsButtonPressed() => _barMenus.Show(_settingsMenu);
        private void OnWavesButtonPressed() => _barMenus.Show(_waveEditor);
        private void OnLoadButtonPressed() => _barMenus.Show(_loadMenu);
        private void OnTowersButtonPressed()
        {
            _barMenus.Show(_towersMenu);
            _levelEditor.ChangeTool(_deletingPlacableTool);
            _levelEditor.ChangeSelector(_deleteSelector);
        }
        private void OnToolButtonPresed()
        {
            _barMenus.Show(_toolButtons);
            _levelEditor.ChangeSelector(_brushSelector);
            _levelEditor.ChangeTool(_drawTool);
        }
        private void OnDeleteButtonPressed()
        {
            _settingsMenu.RestoreDefaultValues();
            _levelEditor.CleaerCommandsBuffer();
            _grid.Clear();
            _waveEditor.DeleteCurrentData();
            _placablesContainer.DestroyAll();
            _towersPlaceMenu.ClearData();
            _towersSettingsMenu.ClearData();
            _spawnerPositions.Clear();
            _placablesContainer.DestroyAll();
        }

        private void OnNewButtonPressed()
        {
            OnDeleteButtonPressed();
        }
        private void OnSpawnerButtonPressed()
        {
            _levelEditor.ChangeTool(_spawnerPlacer);
            _levelEditor.ChangeSelector(_spawnerPlacementSelector);
            _barMenus.Show(_spawnersMenu);
        }

        private void OnSaveKeyCombinationDown()
        {
            _waveEditor.FillWaveDatasWithEnemyDatas();
            _levelSavingUI.Show();

            foreach (var id in _towersSettingsMenu.SelectedTowers)
                Debug.Log(id);
            
            _levelEditor.SaveLevel(_waveEditor.WaveDatas.ToArray(), _towersSettingsMenu.SelectedTowers.ToArray(), false);
        }
    }
}