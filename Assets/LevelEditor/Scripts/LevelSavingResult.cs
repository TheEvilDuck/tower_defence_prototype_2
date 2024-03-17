using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LevelEditor
{
    public class LevelSavingResult
    {
        private readonly ResultType _type;
        public string message;

        public LevelSavingResult(ResultType resultType)
        {
            _type = resultType;
        }
    }

    public enum ResultType
    {
        Success,
        EmptyName,
        ShortName,
        InvalidName,
        Error
    }

}