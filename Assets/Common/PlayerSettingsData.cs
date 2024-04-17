using UnityEngine;

namespace Common
{
    public static class PlayerSettingsData
    {
        private const string MAP_KEY = "MAP_NAME";

        public static void SaveChosenMapName(string mapName)
        {
            PlayerPrefs.SetString(MAP_KEY, mapName);
        }

        public static string LoadChosenMapName()
        {
            return PlayerPrefs.GetString(MAP_KEY);
        }
    }
}