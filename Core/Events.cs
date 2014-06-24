using System;

namespace Reclamation.Core
{

    public delegate void ProgressEventHandler(object sender, ProgressEventArgs e);
    public delegate void StatusEventHandler(object sender, StatusEventArgs e);

    public class StatusEventArgs : EventArgs
    {
        string _message;
        public string Message { get { return _message; } }
        public string Tag { get; set; }

        public StatusEventArgs(string message)
        {
            this._message = message;
        }
        public StatusEventArgs(string message, string tag)
        {
            this.Tag = tag;
            this._message = message;
        }

    }

    public class ArrayEventArgs : EventArgs
    {
        object[] m_args;

        public object[] Args
        {
            get { return m_args; }
            set { m_args = value; }
        }
        public ArrayEventArgs(object[] args)
        {
            m_args = args;
        }

    }
    /// <summary>
    /// Notification of progress of long running task.
    /// used in sybase on Row Updated events.
    /// </summary>
    public class ProgressEventArgs : EventArgs
    {
        string _message;
        int _percentComplete;
        public string Message { get { return _message; } }
        public int PrecentComplete { get { return _percentComplete; } }

        public ProgressEventArgs(string message, int percentComplete)
        {
            this._message = message;
            this._percentComplete = percentComplete;
        }
    }
    /// <summary>
    /// Notification of progress of long running task.
    /// used in sybase on Row Updated events.
    /// </summary>
    public class BytesReadEventArgs : EventArgs
    {
        string _message;
        int _bytes;
        /// <summary>
        /// Message text
        /// </summary>
        public string Message { get { return _message; } }
        public int BytesRead { get { return _bytes; } }

        public BytesReadEventArgs(string message, int bytesRead)
        {
            this._message = message;
            this._bytes = bytesRead;
        }
    }
}
