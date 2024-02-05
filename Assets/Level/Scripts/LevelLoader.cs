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
        public static readonly string LEVELS_FOLDER = "/Resources/Levels/";
        public static readonly string LEVEL_FORMAT = ".json";
        public static readonly string LEVEL_ICON_FORMAT = ".png";
        public static readonly string LEVEL_ICON_PREFIX = "_ICON";

        public bool TryLoadLevel(string levelName, out LevelData levelData)
        {
            levelData = new LevelData();

            if (!LevelExists(levelName))
                return false;

            try
            {
                string json = File.ReadAllText(GetFullLevelPath(levelName));
                levelData = JsonUtility.FromJson<LevelData>(json);
                return true;
            }
            catch(IOException exception)
            {
                throw exception;
            }
            
            
        }

        public bool TryLoadLevelIcon(string levelName, out Texture2D resultTexture)
        {
            resultTexture = null;

            if (!LevelExists(levelName))
                return false;

            try
            {
                byte[] data = File.ReadAllBytes(GetFullLevelIconPath(levelName));
                resultTexture = new Texture2D(2,2);
                resultTexture.LoadImage(data);
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
            string fullPath = GetFullLevelIconPath(levelName);

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

        public string[] GetAllMapsNames()
        {
            StringBuilder stringBuilder = new StringBuilder(Application.dataPath);
            stringBuilder.Append(LEVELS_FOLDER);

            DirectoryInfo directoryInfo = new DirectoryInfo(stringBuilder.ToString());
            FileInfo[] fileInfos = directoryInfo.GetFiles($"*{LEVEL_FORMAT}");
            List<string>result = new List<string>();

            foreach (FileInfo fileInfo in fileInfos)
            {
                string name = fileInfo.Name;
                int jsonIndex = name.IndexOf(LEVEL_FORMAT);
                string resultName = name.Remove(jsonIndex,name.Length-jsonIndex);
                result.Add(resultName);
            }
            return result.ToArray();
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

        private string GetFullLevelIconPath(string levelName)
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
