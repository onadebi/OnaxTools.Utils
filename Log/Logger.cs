namespace OnaxTools
{
    public class Logger
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex">The Exception that would be logged</param>
        /// <param name="filePath">Specifiy file path to where error should be logged '''Server.MapPath("~/App_Data/LOG/");'''. File path would be created if it does not exist.</param>
        public static void ErrorLogger(Exception ex, string errorFilePath)
        {
            var currentDir = System.IO.Directory.GetCurrentDirectory();
            var currentDir4 = String.Format("{0}Log", AppDomain.CurrentDomain.BaseDirectory);

            string lineBreak = "\n======" + DateTime.Now.ToString() + "=========================================>\n";
            string fileNameWithPath = "LogFile_" + DateTime.Now.ToString("yyyy''MM''dd") + ".txt";
            StreamWriter objWriter;
            string PathCombined = errorFilePath + fileNameWithPath;

            if (System.IO.File.Exists(PathCombined))
            {
                objWriter = new StreamWriter(PathCombined, true);
            }
            else { objWriter = new StreamWriter(PathCombined); }

            try
            {
                string errorMessage = String.Empty;
                if (ex.Message != null) { errorMessage += $"ERROR MESSAGE=> {ex.Message}"; }
                if (ex.InnerException != null) { errorMessage += $"\n\n INNER EXCEPTION => {ex.InnerException.ToString()}"; }
                if (ex.StackTrace != null) { errorMessage += $"\n\n STACK TRACE => {ex.StackTrace}"; }
                objWriter.WriteLine(lineBreak + errorMessage);
                objWriter.Flush();
            }
            finally
            {
                objWriter.Close();
            }

        }

        public static void LogException(Exception ex, string message = null)
        {
            var currentDir = String.Format("{0}Log", AppDomain.CurrentDomain.BaseDirectory);

            string lineBreak = "\n======" + DateTime.Now.ToString() + "===========EXCEPTION===========>\n";
            message = !String.IsNullOrWhiteSpace(message) ? $"==={message}===\n" : String.Empty;
            string fileNameWithPath = "LogFile_" + DateTime.Now.ToString("yyyy''MM''dd") + ".txt";

            string PathCombined = Path.Combine(currentDir, fileNameWithPath);
            StreamWriter objWriter;

            if (System.IO.File.Exists(PathCombined))
            {
                objWriter = new StreamWriter(PathCombined, true);
            }
            else
            {

                if (!Directory.Exists(currentDir))
                {
                    Directory.CreateDirectory(currentDir);
                }
                objWriter = new StreamWriter(PathCombined);
            }
            try
            {
                string errorMessage = String.Empty;
                if (ex.Message != null) { errorMessage += $"ERROR MESSAGE=> {ex.Message}"; }
                if (ex.InnerException != null) { errorMessage += $"\n\n INNER EXCEPTION => {ex.InnerException.ToString()}"; }
                if (ex.StackTrace != null) { errorMessage += $"\n\n STACK TRACE => {ex.StackTrace}"; }
                objWriter.WriteLine(lineBreak + message + errorMessage);
                objWriter.Flush();
            }
            catch (Exception) { }
            finally { objWriter.Close(); }
        }

        public static string OutputException(Exception ex, string message = null)
        {
            string outputException = String.Empty;

            string lineBreak = "\n======" + DateTime.Now.ToString() + "===========EXCEPTION===========>\n";
            message = !String.IsNullOrWhiteSpace(message) ? $"==={message}===\n" : String.Empty;

            try
            {
                string errorMessage = String.Empty;
                if (ex.Message != null) { errorMessage += $"ERROR MESSAGE=> {ex.Message}"; }
                if (ex.InnerException != null) { errorMessage += $"\n\n INNER EXCEPTION => {ex.InnerException.ToString()}"; }
                if (ex.StackTrace != null) { errorMessage += $"\n\n STACK TRACE => {ex.StackTrace}"; }
                outputException = lineBreak + message + errorMessage;
            }
            catch (Exception) { }
            return outputException;
        }


        public static void LogInfo(string message = null)
        {
            var currentDir = String.Format("{0}Log\\", AppDomain.CurrentDomain.BaseDirectory);

            string lineBreak = "\n======" + DateTime.Now.ToString() + "===========INFO===========>\n";
            message = !String.IsNullOrWhiteSpace(message) ? $"{message}\n" : String.Empty;
            string fileNameWithPath = "LogFile_" + DateTime.Now.ToString("yyyy''MM''dd") + ".txt";

            string PathCombined = currentDir + fileNameWithPath;
            StreamWriter objWriter;

            if (System.IO.File.Exists(PathCombined))
            {
                objWriter = new StreamWriter(PathCombined, true);
            }
            else
            {

                if (!Directory.Exists(currentDir))
                {
                    Directory.CreateDirectory(currentDir);
                }
                objWriter = new StreamWriter(PathCombined);
            }
            try
            {

                objWriter.WriteLine(lineBreak + message);
                objWriter.Flush();
            }
            catch (Exception) { }
            finally { objWriter.Close(); }
        }

    }
}
