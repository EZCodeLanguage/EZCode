namespace EZCodeLanguage
{
    [Serializable]
    public class EZException : Exception
    {
        public string Message { get; set; }
        public string Id { get; set; }
        public string StackTrace { get; set; }
        public Exception InnerException { get; set; }
        public EZException() { }
        public EZException(string message, string id, string stacktrace, Exception innerException) : base(message, innerException)
        {
            Message = message;
            Id = id;
            StackTrace = stacktrace;
            InnerException = innerException;
            base.Source = "EZCodeLanguage";
        }
    }
}
