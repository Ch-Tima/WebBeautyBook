namespace BLL
{
    /// <summary>
    /// Represents a response object used to communicate the success or failure of a service operation along with an optional message.
    /// </summary>
    public class ServiceResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether the service operation was successful.
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Gets or sets an optional message associated with the service operation.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceResponse"/> class with the specified success status and message.
        /// </summary>
        /// <param name="isSuccess">A value indicating whether the service operation was successful.</param>
        /// <param name="message">An optional message associated with the service operation.</param>
        public ServiceResponse(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}
