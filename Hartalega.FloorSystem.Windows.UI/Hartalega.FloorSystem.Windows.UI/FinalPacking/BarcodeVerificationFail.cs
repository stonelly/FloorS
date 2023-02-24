using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{
    public partial class BarcodeVerificationFail : FormBase
    {
        public BarcodeVerificationFail(string stationNumber, string internalNumber, string poNumber)
        {
            txtStationNo.Text = stationNumber;
            txtInnerLotNo.Text = internalNumber;
            txtPO.Text = poNumber;
            InitializeComponent();            
        }
    }
}
