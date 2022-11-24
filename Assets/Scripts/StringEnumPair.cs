namespace DefaultNamespace
{
    [System.Serializable]
    public class StringEnumPair<T> where T : System.Enum
    {
        private string typeString;
        private T type;

        public StringEnumPair(string typeString, T type)
        {
            this.typeString = typeString;
            this.type = type;
        }

        public string TypeString => typeString;
        public T Type => type;
    }
}