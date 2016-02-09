namespace Waterbot.Common
{
    /// <summary>
    /// Defines an event to log.
    /// </summary>
    public interface ILogEvent
    {
        /// <summary>
        /// Gets the message to log.
        /// </summary>
        string Message { get; }
    }
}