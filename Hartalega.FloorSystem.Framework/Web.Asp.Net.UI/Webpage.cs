// -----------------------------------------------------------------------
// <copyright file="Webpage.cs" company="Accenture">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Hartalega.FloorSystem.Framework.Web.Asp.Net.UI
{
    using System;
   // using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

    /// <summary>
    /// Cannot create instance and can only be inheritable.
    /// All the web pages can be inherit this class and use the common features.
    /// </summary>
    public abstract class Webpage : System.Web.UI.Page
    {
        /// <summary>
        /// Page Initialize event
        /// </summary>
        /// <param name="e">Event Argument</param>
        #region
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }
        #endregion
    }
}
