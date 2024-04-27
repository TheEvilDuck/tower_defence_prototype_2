using System;

namespace LevelEditor.LevelSaving
{
    public class LevelSavingResultFactory
    {
        private readonly LevelSavingResultDatabase _configs;
        public LevelSavingResultFactory(LevelSavingResultDatabase levelSavingResultDatabase)
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
