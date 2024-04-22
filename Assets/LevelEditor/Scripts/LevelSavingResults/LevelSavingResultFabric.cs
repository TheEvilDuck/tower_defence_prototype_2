using System;

namespace LevelEditor
{
    public class LevelSavingResultFabric
    {
        private readonly LevelSavingResultDatabase _configs;
        public LevelSavingResultFabric(LevelSavingResultDatabase levelSavingResultDatabase)
        {
            _configs = levelSavingResultDatabase;
        }

        public LevelSavingResult Get(ResultType resultType)
        {
            LevelSavingResult levelSavingResult = new LevelSavingResult(resultType);

            if (!_configs.TryGetValue(resultType, out LevelSavingResultConfig config))
            {
                throw new ArgumentException($"There is no saving result presenting {resultType} in current database");
            }

            levelSavingResult.message = config.message;

            return levelSavingResult;
        }
    }
}
