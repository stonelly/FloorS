using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.ProductionDefect
{
    public partial class MainMenu : FormBase
    {
        public MainMenu()
        {
            InitializeComponent();
        }

        private void btnProductionDefectList_Click(object sender, EventArgs e)
        {
            new ProductionDefect.ProductionDefectList().ShowDialog();
        }
    }
}
