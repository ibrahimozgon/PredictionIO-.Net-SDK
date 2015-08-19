namespace io.prediction
{
    public class ApiResponse
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public string EventId { get; set; }

        public bool Succeeded
        {
            get { return !string.IsNullOrEmpty(EventId) && string.IsNullOrEmpty(Message); }
        }

        public ApiResponse(int status, string message)
        {
            Status = status;
            Message = message;
        }
    }
}