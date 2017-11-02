using System;

namespace PiscesAPI.Models
{
    /// <summary>
    /// Represents error information that can be shown to user.
    /// </summary>
    public class ErrorInfoModel
    {
        /// <summary>
        /// Gets or sets error message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets error date and time.
        /// </summary>
        public DateTimeOffset TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets URI, Web API rout that has failed to complete.
        /// </summary>
        public Uri RequestUri { get; set; }

        /// <summary>
        /// <see cref="Guid"/> value that represents correlation identifier that can be used for tracking purposes.
        /// </summary>
        public Guid ErrorId { get; set; }
    }
}