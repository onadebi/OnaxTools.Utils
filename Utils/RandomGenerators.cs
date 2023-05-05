namespace OnaxTools.Utils
{
    public static class RandomGenerators
    {
        /// <summary>
        /// This function generates a unique random string based of randomly generated number combined with datetime, to values of milliseconds.
        /// </summary>
        /// <param name="startPoint">This is the range starting point from which a number should be generated. Defaults to 1</param>
        /// <param name="endPoint">This is the range ending point from which a number should be generated. Defaults to 5000</param>
        /// <param name="paddingSwapCount">This specifies the count of starting and ending point numbers that should be swapped using the characterSwap parameter. This defaults to 2 </param>
        /// <param name="characterSwap">This specifies the string character(s) that is used to replace the paddingSwapCount. This defaults to '00' </param>
        /// <returns></returns>
        public static string GenerateRandomReferenceString(int startPoint = 1, int endPoint = 5000, int paddingSwapCount = 2, string characterSwap = "00" )
        {
            Random rnd = new();
            string charsLength = Convert.ToString(Math.Abs(rnd.Next(startPoint, endPoint)));
            paddingSwapCount = paddingSwapCount < 0 ? Math.Abs(paddingSwapCount) : paddingSwapCount;
            string frontPadding = charsLength.Length > paddingSwapCount ? string.Concat(characterSwap, charsLength[paddingSwapCount..]) : charsLength;
            string endPadding = charsLength.Length > paddingSwapCount ? string.Concat(charsLength[..paddingSwapCount], characterSwap) : charsLength;
            return string.Concat(frontPadding, DateTime.Now.ToString("yyyyMMddhhMMssffffff"), endPadding);
        }
    }
}
