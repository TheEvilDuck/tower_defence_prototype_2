using System;
using System.Linq;
using Common;
using LevelEditor.Commands.Factory;
using LevelEditor.LevelSaving;
using LevelEditor.Selectors;
using LevelEditor.Tools;
using LevelEditor.UI;
using LevelEditor.UI.WavesEditing;
using Levels.Logic;
using Levels.Tiles;
using Levels.View;
using UnityEngine;

namespace LevelEditor.Mediators
{
    public class LevelEditorMediator: IDisposable
    {
        private LevelEditor _levelEditor;
        private Level _level;
        private TileController _tileController;
        private LevelSavingUI _levelSavingUI;
        private Tool _drawTool;
        private BrushSelector _brushSelector;
        private WavesEditor _waveEditor;
        private SpawnerPositions _spawnerPositions;
        private SpawnersView _spawnersView;
        private TowersSettingsMenu _towersSettingsMenu;

        public LevelEditorMediator
        (
            LevelEditor levelEditor, 
            Level level, 
            TileController tileController,
            LevelSavingUI levelSavingUI,
            Tool drawTool,
            BrushSelector brushSelector,
            WavesEditor wavesEditor,
            SpawnerPositions spawnerPositions,
            SpawnersView spawnersView,
            TowersSettingsMenu towersSettingsMenu
        )
        {
            _levelEditor = levelEditor;
            _level = level;
            _tileController = tileController;
            _levelSavingUI = levelSavingUI;
            _drawTool = drawTool;
            _brushSelector = brushSelector;
            _waveEditor = wavesEditor;
            _spawnerPositions = spawnerPositions;
            _spawnersView = spawnersView;
            _towersSettingsMenu = towersSettingsMenu;

            _levelEditor.ChangeSelector(_brushSelector);
            _levelEditor.ChangeTool(_drawTool);
        
            _level.Grid.cellChanged+=OnCellChanged;
            _level.Grid.cellRemoved+=OnCellRemoved;
            _level.Grid.cellAdded+=OnCellAdded;
            _levelEditor.levelSaveTried+=OnLevelSaveTried;
            _spawnerPositions.placed+=OnSpawnerAddedAt;
            _spawnerPositions.removed+=OnSpawnerRemovedAt;
            
        }


        public void Dispose()
        {

            _level.Grid.cellRemoved-=OnCellRemoved;
            _level.Grid.cellAdded-=OnCellAdded;
        
            _spawnerPositions.placed-=OnSpawnerAddedAt;
            _spawnerPositions.removed-=OnSpawnerRemovedAt;
            
        }

        private void OnCellChanged(Vector2Int cellId, CellData cell)
        {
            if (cell.HasRoad)
                _tileController.DrawRoadAt(cellId);
            else
                _tileController.RemoveRoadAt(cellId);
        }

        private void OnCellAdded(Vector2Int cellId, TileType tileType)
        {
            _tileController.DrawAt(cellId, tileType);
        }

        private void OnCellRemoved(Vector2Int cellId)
        {
            _tileController.RempoveAt(cellId);
            _tileController.RemoveRoadAt(cellId);
        }

        private void OnLevelSaveTried(LevelSavingResult levelSavingResult)
        {
            if (levelSavingResult.Type == ResultType.MapOverride)
            {
                _levelSavingUI.ShowCancelButton();
                _levelSavingUI.OkButtonPressed.AddListener(OnOkSaveButtonClickedWhileOverriding);
            }

            _levelSavingUI.Show();
            _levelSavingUI.OnLevelSaveTried(levelSavingResult.message);
        }
        

        private void OnOkSaveButtonClickedWhileOverriding()
        {
            _levelSavingUI.OkButtonPressed.RemoveListener(OnOkSaveButtonClickedWhileOverriding);
            
            _levelEditor.SaveLevel(_waveEditor.WaveDatas.ToArray(), _towersSettingsMenu.SelectedTowers.ToArray(), true);
        }
        

        private void OnSpawnerAddedAt(Vector2Int position)
        {
            _spawnersView.CreateViewAt(position);
        }

        private void OnSpawnerRemovedAt(Vector2Int position)
        {
            _spawnersView.RemoveViewAt(position);
        }
    }
}
