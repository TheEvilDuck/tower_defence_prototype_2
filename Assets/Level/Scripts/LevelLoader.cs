using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Levels.Logic
{
    public class LevelLoader
    {
        private const string LEVELS_FOLDER = "/Levels";
        private const string LEVEL_FORMAT = ".json";

        public bool TryLoadLevel(string levelName, out LevelData levelData)
        {
            levelData = new LevelData();

            if (!LevelExists(levelName))
                return false;

            levelData = JsonUtility.FromJson<LevelData>(GetFullLevelPath(levelName));
            
            return true;
        }

        public async void SaveLevel(string levelName, LevelData levelData, Action onLevelSave, Action onLevelSaveFailed)
        {
            try
            {
                await File.WriteAllTextAsync(GetFullLevelPath(levelName),JsonUtility.ToJson(levelData));
                onLevelSave?.Invoke();
            }
            catch(IOException exception)
            {
                onLevelSaveFailed?.Invoke();
                throw exception;
            }
            
        }

        public bool LevelExists(string levelName)
        {
            return File.Exists(GetFullLevelPath(levelName));
        }

        private string GetFullLevelPath(string levelName)
        {
            StringBuilder stringBuilder = new StringBuilder(Application.persistentDataPath);
            stringBuilder.Append(LEVELS_FOLDER);
            stringBuilder.Append(levelName);
            stringBuilder.Append(LEVEL_FORMAT);

            return stringBuilder.ToString();
        }
    }
}
