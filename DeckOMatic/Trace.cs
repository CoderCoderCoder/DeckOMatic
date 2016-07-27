namespace DeckOMatic
{
    using System;

    public static class Trace
    {
        public static event Action<string> LogWritten;

        /// <summary>
        /// Log a trace message
        /// </summary>
        public static void Log(string formatString, params object[] parameters)
        {
            if (Trace.LogWritten != null)
            {
                string logString = String.Format(formatString, parameters);
                Trace.LogWritten(logString);
            }
        }
    }
}
