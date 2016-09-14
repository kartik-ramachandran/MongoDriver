using NLog;

namespace MongoDriver.ErrorHandling
{
    public class ErrorHandler
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public void LogInfoMessage<T>(T value, string message)
        {
            logger.Info(message, value);
        }

        public void LogErrorMessage<T>(T value, string message)
        {
            logger.Error(message, value);
        }

        public void LogWarnMessage<T>(T value, string message)
        {
            logger.Warn(message, value);
        }

        public void LogFatalMessage<T>(T value, string message)
        {
            logger.Fatal(message, value);
        }

        public void LogTraceMessage<T>(T value, string message)
        {
            logger.Trace(message, value);
        }

        public void LogDebugMessage<T>(T value, string message)
        {
            logger.Debug(message, value);
        }
    }
}
