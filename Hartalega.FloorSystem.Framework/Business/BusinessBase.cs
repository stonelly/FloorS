// -----------------------------------------------------------------------
// <copyright file="BusinessBase.cs" company="Avanade">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hartalega.FloorSystem.Framework.Business
{
    using System;


    /// <summary>
    /// Abstract class. Base class for all business components.
    /// </summary>
    public abstract class BusinessBase : IDisposable
    {
        /// <summary>
        /// Flag which will carry whether this class can dispose or not.
        /// </summary>
        private bool isDisposed = false;

        /// <summary>
        /// Pointer to an external unmanaged resource.
        /// </summary>
        private IntPtr handle = System.Runtime.InteropServices.Marshal.AllocHGlobal(100);

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessBase" /> class.
        /// </summary>
        #region
        protected BusinessBase()
        {
            // No implementation required
        }
        #endregion

        /// <summary>
        /// Finalizes an instance of the <see cref="BusinessBase" /> class.
        /// </summary>
        #region
        ~BusinessBase()
        {
            this.Dispose(false);
        }
        #endregion

        /// <summary>
        /// Dispose class
        /// </summary>
        #region
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        /// <summary>
        /// Handle exception
        /// </summary>
        /// <param name="exception">Exception that has to handle</param>
      
        /// <summary>
        /// Get class instance based on type T.
        /// </summary>
        /// <typeparam name="T">Type of the interface</typeparam>
        /// <returns>Class that implements type T</returns>
        #region
        protected static T GetInstance<T>() where T : class, new()
        {
            return new T();
        }
        #endregion

        /// <summary>
        /// Override method to dispose this class.
        /// </summary>
        /// <param name="isDisposing">Can Dispose this class</param>
        #region
        protected virtual void Dispose(bool isDisposing)
        {
            if (!this.isDisposed)
            {
                if (isDisposing)
                {
                    //// Disponse managed resources if any
                }

                if (this.handle != IntPtr.Zero)
                {
                    this.handle = IntPtr.Zero;
                }

                this.isDisposed = true;
            }
        }
        #endregion
    }
}
