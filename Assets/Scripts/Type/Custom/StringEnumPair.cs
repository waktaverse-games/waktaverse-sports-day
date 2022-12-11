using System;

namespace GameHeaven.Temp
{
    [Serializable]
    public class StringEnumPair<T> where T : Enum
    {
        public StringEnumPair(string typeString, T type)
        {
            TypeString = typeString;
            Type = type;
        }

        public string TypeString { get; }

        public T Type { get; }
    }
}