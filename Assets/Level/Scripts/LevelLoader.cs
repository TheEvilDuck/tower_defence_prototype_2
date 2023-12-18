using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Levels.Logic
{
    public class LevelLoader
    {
        private const string LEVELS_FOLDER = "/Resources/Levels/";
        private const string LEVEL_FORMAT = ".json";
        private const string LEVEL_ICON_FORMAT = ".png";
        private const string LEVEL_ICON_PREFIX = "_ICON";

        public bool TryLoadLevel(string levelName, out LevelData levelData)
        {
            levelData = new LevelData();

            if (!LevelExists(levelName))
                return false;

            try
            {
                levelData = JsonUtility.FromJson<LevelData>(GetFullLevelPath(levelName));
                return true;
            }
            catch(IOException exception)
            {
                throw exception;
            }
            
            
        }

        public async Task SaveLevel(string levelName, LevelData levelData, Action onLevelSave, Action OnLevelSaveFailed)
        {
            string fullPath = GetFullLevelPath(levelName);

            try
            {
                await File.WriteAllTextAsync(fullPath,JsonUtility.ToJson(levelData));
                await WaitForFile(fullPath);
                await Task.Delay(5000);

            }
            catch(IOException exception)
            {
                OnLevelSaveFailed?.Invoke();
                throw exception;
            }
            onLevelSave?.Invoke();
            
        }

        public async Task CreateLevelIcon(string levelName, Texture2D resultTexture, Action OnComplete)
        {
            string fullPath = GetFillLevelIconPath(levelName);

            byte[] bytes = resultTexture.EncodeToPNG();
            await File.WriteAllBytesAsync(fullPath,bytes);
            await WaitForFile(fullPath);
            OnComplete?.Invoke();

        }

        public bool LevelExists(string levelName)
        {
            if (levelName==string.Empty)
                return false;

            return File.Exists(GetFullLevelPath(levelName));
        }

        private async Task WaitForFile(string fullPath)
        {
            while (!File.Exists(fullPath))
                await Task.Delay(1000);
        }

        private string GetFullLevelPath(string levelName)
        {
            StringBuilder stringBuilder = new StringBuilder(Application.dataPath);
            stringBuilder.Append(LEVELS_FOLDER);
            stringBuilder.Append(levelName);
            stringBuilder.Append(LEVEL_FORMAT);

            return stringBuilder.ToString();
        }

        private string GetFillLevelIconPath(string levelName)
        {
            StringBuilder stringBuilder = new StringBuilder(Application.dataPath);
            stringBuilder.Append(LEVELS_FOLDER);
            stringBuilder.Append(levelName);
            stringBuilder.Append(LEVEL_ICON_PREFIX);
            stringBuilder.Append(LEVEL_ICON_FORMAT);

            return stringBuilder.ToString();
        }
    }
}
