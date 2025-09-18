using System;

namespace Pandora.Common
{
    /// <summary>
    /// Respresents an object which implements interface IDisposable.
    /// </summary>
    public class DisposableObject : IDisposable
    {
        /// <summary>
        /// Indicating whether current object is disposed or not.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the DisposableObject class.
        /// </summary>
        public DisposableObject()
        {
            _disposed = false;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="DisposableObject"/> class.
        /// </summary>
        ~DisposableObject()
        {
            Dispose(false);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Ensures object is still alive.
        /// </summary>
        /// <exception cref="ObjectDisposedException"></exception>
        public void EnsureAlive()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        /// <summary>
        /// Release unmanaged resources.
        /// </summary>
        protected virtual void DisposeUnmanagedResources()
        { 
        }

        /// <summary>
        /// Releases managed resources.
        /// </summary>
        protected virtual void DisposeManagedResources()
        { 
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">Indicating whether to release managed resources.</param>
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    DisposeManagedResources();
                }

                DisposeUnmanagedResources();
                _disposed = true;
            }
        }
    }
}
