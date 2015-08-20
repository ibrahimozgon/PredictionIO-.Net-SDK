namespace io.prediction
{
    /// <summary>
    ///     The Response of Client Request
    /// </summary>
    public partial class ApiResponse
    {
        /// <summary>
        ///     The status code of http response
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        ///     The message of http response
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     Id of the event
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        ///     Process Succeeded
        /// </summary>
        public bool Succeeded
        {
            get { return !string.IsNullOrEmpty(EventId) && string.IsNullOrEmpty(Message); }
        }
    }
}