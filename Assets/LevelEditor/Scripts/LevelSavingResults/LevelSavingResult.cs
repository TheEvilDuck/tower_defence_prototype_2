namespace LevelEditor.LevelSaving
{
    public class LevelSavingResult
    {
        public readonly ResultType Type;
        public string message;

        public LevelSavingResult(ResultType resultType)
        {
            Type = resultType;
        }
    }

    public enum ResultType
    {
        Success,
        EmptyName,
        ShortName,
        InvalidName,
        Error,
        MapOverride
    }

}