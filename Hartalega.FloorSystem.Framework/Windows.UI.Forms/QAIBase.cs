using System.Linq;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Framework.Windows.UI.Forms
{
    /// <summary>
    /// BAse class for QAI Module
    /// </summary>
    public partial class QAIBase : FormBase
    {

        /// <summary>
        /// Glove Type
        /// </summary>
        protected string GloveType;
        /// <summary>
        /// If Page is valid
        /// </summary>
        protected bool ISPageValid;
        /// <summary>
        /// QAI Base
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Add)
            {
                Button btnNext = (Button)this.Controls.Find("btnNext", true).FirstOrDefault();
                if (btnNext != null)
                {
                    btnNext.PerformClick();
                }
            }
            if (keyData == Keys.Escape)
            {
                /*
                Button btnCancel = (Button)this.Controls.Find("btnCancel", true).FirstOrDefault();
                if (btnCancel != null)
                {
                    btnCancel.PerformClick();
                }
                */
                this.Close();
            }

            if (keyData == Keys.Subtract)
            {
                this.Close();
            }
            if (keyData == Keys.Add || keyData == Keys.Escape || keyData == Keys.Subtract)
            {
                return true;
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }
    }
}
