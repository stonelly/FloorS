using Hartalega.FloorSystem.Business.Logic;
using Hartalega.FloorSystem.Business.Logic.DataTransferObjects;
using Hartalega.FloorSystem.Framework;
using Hartalega.FloorSystem.Framework.Common;
using Hartalega.FloorSystem.Framework.Windows.UI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Windows.UI.FinalPacking
{
    public partial class CompletedPalletSlip : FormBase
    {
        private PalletInfoDTO _palletInfoDTO {set; get;}
        public string _operatorName { set; get;}
        public string _operatorId { get; set;}

        private static int y;
        private static int x;
        private static int z;
        private static int aa;

        public CompletedPalletSlip(PalletInfoDTO palletinfoDTO)
        {
            _palletInfoDTO = palletinfoDTO;
            InitializeComponent();
        }
        /// <summary>
        /// Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>        
        private void Completed_PalletSlip_Load(object sender, EventArgs e)
        {
            txtPlant.Text = Convert.ToString(_palletInfoDTO.locationId);
            txtOperatorId.Text = _operatorId;
            txtOperatorName.Text = _operatorName;
            txtSize.Text = _palletInfoDTO.Size;
            txtCaseList.Text = _palletInfoDTO.CaseList;
            txtCases.Text = Convert.ToString(_palletInfoDTO.Casespacked);
            txtPalletId.Text = _palletInfoDTO.Palletid;
            txtOrderNum.Text = _palletInfoDTO.orderNumber;
            txtSONum.Text = _palletInfoDTO.Ponumber;
            txtBrand.Text = _palletInfoDTO.ItemName;
            txtFGCode.Text = _palletInfoDTO.ItemNumber;

            DateTime dt = CommonBLL.GetCurrentDateAndTimeFromServer();
            txtDate.Text = dt.Date.ToShortDateString();
            txtTime.Text = dt.ToString("HH:mm:ss");
            this.WindowState = FormWindowState.Maximized;
        }
        /// <summary>
        /// Printer Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            try
            {
                # region "Code for Printing - Hartalega Dev Team"               

                string[] s = _palletInfoDTO.CaseList.Split(Constants.CHARCOMMA);
                Font myFont1 = new Font("Arial", 13 - 2, FontStyle.Bold);
                Font myFont2 = new Font("Arial", 14 - 2, FontStyle.Bold);
                Font myFont3 = new Font("Arial", 12 - 2);
                Font myFont4 = new Font("Arial", 8 - 2);
                Font myFont5 = new Font("Arial", 10, FontStyle.Bold);

                Pen drawingPen = new Pen(Color.Black, 1);

                int nWidth = (int)(e.PageSettings.PaperSize.Width / e.Graphics.MeasureString("0", myFont1).Width);
                int nHeight = (int)(e.PageSettings.PaperSize.Height / e.Graphics.MeasureString("0", myFont1).Height);
                int arrLenght = _palletInfoDTO.ispreshipmentCase? (int)Math.Ceiling((Double)s.Length / 54): (int)Math.Ceiling((Double)s.Length / 90); // PSI fix to 19 Column
                int nRow = 50;//y-coordinates            
                int nColStep = 8;
                int nRowStep = (int)e.PageSettings.PaperSize.Height / nHeight;//spacing            

                int width = 816;
                int height = 523;
                Boolean fla = true;
                Single yPos = 0;
                Single leftMargin = e.MarginBounds.Left;
                Single topMargin = e.MarginBounds.Top;
                Rectangle logo = new Rectangle(40, 40, 50, 50);

                _palletInfoDTO.SlipDate = CommonBLL.GetCurrentDateAndTimeFromServer().ToString("dd/MM/yyyy");
                _palletInfoDTO.SlipTime = CommonBLL.GetCurrentDateAndTimeFromServer().ToString("HH:mm:ss");

                e.Graphics.DrawString("FINISHED GOODS TRANSFER FORM", myFont2, Brushes.Black, nColStep * 30, nRow);
                ///------------------------START--- 1st part
                e.Graphics.DrawString("FINISHED GOODS TRANSFER FORM", myFont2, Brushes.Black, nColStep * 30, nRow);

                e.Graphics.DrawString("INTERNAL USE ONLY", myFont5, Brushes.Black, nColStep * 80, nRow);
                //kamil commented for BRD0137
                //e.Graphics.DrawString("FN WL 19.4", myFont3, Brushes.Black, nColStep * 75, nRow);
                nRow += nRowStep;

                e.Graphics.DrawString("COMPLETED PALLET SLIP", myFont2, Brushes.Black, nColStep * 34, nRow);
                nRow += nRowStep;

                e.Graphics.DrawString("PLANT", myFont1, Brushes.Black, nColStep * 2, nRow);
                e.Graphics.DrawString(": " + _palletInfoDTO.locationId.ToString().Trim(), myFont1, Brushes.Black, nColStep * 12, nRow);
                e.Graphics.DrawString("PAGE", myFont1, Brushes.Black, nColStep * 75, nRow);
                e.Graphics.DrawString(": " + aa + " OF " + arrLenght.ToString().Trim(), myFont1, Brushes.Black, nColStep * 81, nRow);
                nRow += nRowStep;

                e.Graphics.DrawString("ID", myFont1, Brushes.Black, nColStep * 2, nRow);
                e.Graphics.DrawString(": " + _operatorId.Trim(), myFont1, Brushes.Black, nColStep * 12, nRow);
                e.Graphics.DrawString("DATE", myFont1, Brushes.Black, nColStep * 75, nRow);
                e.Graphics.DrawString(": " + _palletInfoDTO.SlipDate.Trim(), myFont1, Brushes.Black, nColStep * 81, nRow);
                nRow += nRowStep;

                e.Graphics.DrawString("NAME", myFont1, Brushes.Black, nColStep * 2, nRow);
                e.Graphics.DrawString(": " + _operatorName.Trim(), myFont1, Brushes.Black, nColStep * 12, nRow);
                e.Graphics.DrawString("TIME", myFont1, Brushes.Black, nColStep * 75, nRow);
                e.Graphics.DrawString(": " + _palletInfoDTO.SlipTime.Trim(), myFont1, Brushes.Black, nColStep * 81, nRow);
                nRow += nRowStep;
                //g.DrawLine(drawingPen, 16, 134, 785, 134);
                e.Graphics.DrawLine(drawingPen, nRowStep, nRow + 5, width, nRow + 5);//draw line
                nRow += nRowStep;
                ///------------------------END 1st-- part

                ///------------------------START 2nd part
                //_palletInfoDTO
                e.Graphics.DrawString("PALLET ID", myFont1, Brushes.Black, nColStep * 2, nRow);
                e.Graphics.DrawString(": " + _palletInfoDTO.Palletid.ToString().Trim(), myFont1, Brushes.Black, nColStep * 12, nRow);
                e.Graphics.DrawString("STRAPPING BAND ", myFont1, Brushes.Black, nColStep * 55, nRow);
                e.Graphics.DrawString(": Y / N ", myFont1, Brushes.Black, nColStep * 75, nRow);
                nRow += nRowStep;

                e.Graphics.DrawString("ORDER#", myFont1, Brushes.Black, nColStep * 2, nRow);
                e.Graphics.DrawString(": " + _palletInfoDTO.orderNumber.ToString().Trim(), myFont1, Brushes.Black, nColStep * 12, nRow);
                e.Graphics.DrawString("COLOUR", myFont1, Brushes.Black, nColStep * 55, nRow);
                e.Graphics.DrawString(": ______________", myFont1, Brushes.Black, nColStep * 75, nRow);
                nRow += nRowStep;

                e.Graphics.DrawString("SO#", myFont1, Brushes.Black, nColStep * 2, nRow);
                e.Graphics.DrawString(": " + _palletInfoDTO.Ponumber.ToString().Trim(), myFont1, Brushes.Black, nColStep * 12, nRow);
                nRow += nRowStep;

                e.Graphics.DrawString("BRAND", myFont1, Brushes.Black, nColStep * 2, nRow);
                e.Graphics.DrawString(": " + _palletInfoDTO.ItemName.ToString().Trim(), myFont1, Brushes.Black, nColStep * 12, nRow);
                nRow += nRowStep;

                e.Graphics.DrawString("FG CODE", myFont1, Brushes.Black, nColStep * 2, nRow);
                e.Graphics.DrawString(": " + _palletInfoDTO.ItemNumber.ToString().Trim(), myFont1, Brushes.Black, nColStep * 12, nRow);
                nRow += nRowStep;

                e.Graphics.DrawString("SIZE", myFont1, Brushes.Black, nColStep * 2, nRow);
                if (_palletInfoDTO.ispreshipmentCase)
                {
                    e.Graphics.DrawString(": "+ _palletInfoDTO.Size.ToString().Trim(), myFont1, Brushes.Black, nColStep * 12, nRow);
                }
                else
                {
                    e.Graphics.DrawString(": " + _palletInfoDTO.Size.ToString().Trim(), myFont1, Brushes.Black, nColStep * 12, nRow);
                }
                nRow += nRowStep;

                e.Graphics.DrawString("CARTON", myFont1, Brushes.Black, nColStep * 2, nRow);
                e.Graphics.DrawString(": " + _palletInfoDTO.Casespacked.ToString().Trim(), myFont1, Brushes.Black, nColStep * 12, nRow);
                nRow += nRowStep;

                e.Graphics.DrawLine(drawingPen, nRowStep, nRow + 5, width, nRow + 5);//draw line
                ///------------------------END 2nd part

                ///------------------------START 3rd part
                nRow = 440;
                e.Graphics.DrawLine(drawingPen, nRowStep, nRow, width, nRow);//draw line
                // e.Graphics.DrawString("SENDER(Packing)" + "\t\t" + "RECEIVER(WFG)" + "\t\t" + "STORING LOCATION" + "\t\t" + "PICKING", myFont3, Brushes.Black, nColStep * 2, nRow);
                // FS Revise FGTF format itrf 20220208034628296060 - remove [STORING LOCATION], [PICKING]
                e.Graphics.DrawString("\t\t\t\t" + "SENDER(Packing)" + "\t\t" + "RECEIVER(WFG)", myFont3, Brushes.Black, nColStep * 2, nRow);
                nRow += nRowStep * 3;
                e.Graphics.DrawLine(drawingPen, nRowStep, nRow, width, nRow);//draw line
                //kamil commented for BRD0137
                //e.Graphics.DrawString("AUGUST 2014", myFont4, Brushes.Black, nColStep * 2, nRow);
                e.Graphics.DrawString("Copy 1 - WAREHOUSE, Copy 2 - SENDER, Copy 3 - PALLET/PICKING", myFont4, Brushes.Black, nColStep * 29, nRow);
                //kamil commented for BRD0137
                //e.Graphics.DrawString("REV.4", myFont4, Brushes.Black, nColStep * 85, nRow);

                //MessageBox.Show(e.);
                nRow = 315;
                while (y < s.Length+1)
                {
                    y += 1;
                    if ((x < 90 && !_palletInfoDTO.ispreshipmentCase) || (x < 54 && _palletInfoDTO.ispreshipmentCase))
                    {
                        if (_palletInfoDTO.ispreshipmentCase)
                        {
                            //if (x == 14 || x == 28 || x == 42 || x == 56 || x == 70 || x == 84) Original
                            if (x == 0)
                            {
                                nColStep = 7;
                            }
                            else if (x == 9 || x == 18 || x == 27 || x == 36 || x == 45 || x == 54) // PSI Fix to 9 Column
                            {
                                nRow = nRow + nRowStep;
                                nColStep = 7;
                            }
                            e.HasMorePages = false;
                            e.Graphics.DrawString(s[z].Trim() + (s.Length == z + 1 ? "" : ","), myFont3, Brushes.Black, (nColStep) * 4, nRow); // TRIM Space
                        }
                        else
                        {
                            if (x == 15 || x == 30 || x == 45 || x == 60 || x == 75 || x == 90)
                            {
                                nRow = nRow + nRowStep;
                                nColStep = 8;
                            }

                            e.HasMorePages = false;
                            e.Graphics.DrawString(s[z] + (s.Length == z + 1 ? "" : " ,"), myFont3, Brushes.Black, (nColStep) * 3, nRow);
                        }
                        x++;
                        z++;
                        //MessageBox.Show("x : "+x.ToString());
                    }
                    else
                    {
                        aa += 1;
                        y -= 1;
                        x = 0;
                        e.HasMorePages = true;
                        return;
                    }
                    nColStep = nColStep + (_palletInfoDTO.ispreshipmentCase? 19 : 17); // Added to align PSI to 19
                }
                #endregion
            }
            catch (Exception ex)
            {
                GlobalMessageBox.Show(Messages.PRINT_EX,Constants.AlertType.Error,Messages.CONTACT_MIS,GlobalMessageBoxButtons.OK);
            }
        }
        /// <summary>
        /// To Print the FGT form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            // Log to history table
            FinalPackingBLL.InsertFPFGTFLog(_palletInfoDTO.Palletid, WorkStationDTO.GetInstance().WorkStationId, WorkStationDTO.GetInstance().LocationId, txtOperatorId.Text);

            y = 1;
            x = 0;
            z = 0;
            aa = 1;

            PrintDocument printDocument1 = new PrintDocument();
            printDocument1.PrintPage += printDocument1_PrintPage;
            printDocument1.DocumentName = Constants.PRINTING;
            printDocument1.Print();
        }
        /// <summary>
        /// Cancel button to clear controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
