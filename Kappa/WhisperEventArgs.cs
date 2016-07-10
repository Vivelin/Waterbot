using System;

namespace Kappa
{
    /// <summary>
    /// Provides data for the event that is raised when a whisper is sent or
    /// received.
    /// </summary>
    public class WhisperEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WhisperEventArgs"/>
        /// class for the specified whisper.
        /// </summary>
        /// <param name="whisper">The whisper that was sent or received.</param>
        public WhisperEventArgs(Whisper whisper)
        {
            Whisper = whisper;
        }

        /// <summary>
        /// Gets the whisper that was sent or received.
        /// </summary>
        public Whisper Whisper { get; }
    }
}
