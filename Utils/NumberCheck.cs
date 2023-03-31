namespace OnaxTools.Utils
{
    public static class NumberCheck
    {
        public static bool IsInteger(this object T)
        {
            if(T == null) return false;
            return int.TryParse(T.ToString().Trim(), out _);
        }

        public static bool IsDecimal(this object T)
        {
            if (T == null) return false;
            return decimal.TryParse(T.ToString().Trim(), out _);
        }
    }
}
