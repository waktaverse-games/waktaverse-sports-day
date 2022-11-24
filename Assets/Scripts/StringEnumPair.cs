using System;

namespace GameHeaven.Temp
{
    [Serializable]
    public class StringEnumPair<T> where T : Enum
    {
        public StringEnumPair(string typeString, T type)
        {
            this.TypeString = typeString;
            this.Type = type;
        }

        public string TypeString { get; }

        public T Type { get; }
    }
}