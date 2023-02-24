using Hartalega.FloorSystem.Framework.Database;
using Hartalega.FloorSystem.Framework.DbExceptionLog;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Drawing.Imaging;
using System.IO.Ports;
using Hartalega.FloorSystem.Framework.BarCodeLib;

namespace Hartalega.FloorSystem.Framework
{
    public class FSIntegration
    {
    }
    /// <summary>
    /// Class to store printing data
    /// </summary>
    public class PrintDTO
    {
        /// <summary>
        /// batchnumber of batch
        /// </summary>
        public string BatchNumber { get; set; }
        /// <summary>
        /// resource of batch
        /// </summary>
        public string Resource { get; set; }
        /// <summary>
        /// weight of batch
        /// </summary>
        public string BatchWeight { get; set; }
        /// <summary>
        /// size of gloves
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// weight of ten gloves
        /// </summary>
        public string TenPcsWeight { get; set; }
        /// <summary>
        /// weight of ten Pcs
        /// </summary>
        public bool TenPcsMsg { get; set; }
        /// /// <summary>
        /// weight of Batch Msg
        /// </summary>
        public bool BatchWeightMsg { get; set; }
        /// <summary>
        /// desc of glovetype
        /// </summary>
        public string GloveDesc { get; set; }
        /// <summary>
        /// side of Tier
        /// </summary>
        public string Side { get; set; }
        /// <summary>
        /// Image of barcode generated
        /// </summary>
        public Image Bmp { get; set; }
        /// <summary>
        /// Date when barcode was generated
        /// </summary>
        public string DateTime { get; set; }
        /// <summary>
        /// Serial Number of the batch
        /// </summary>
        public string SerialNumber { get; set; }
        /// <summary>
        /// For Batch Card or Reject Sticker
        /// </summary>
        public string Template { get; set; }
        /// <summary>
        /// is Reprint
        /// </summary>
        public bool IsReprint { get; set; }
        /// <summary>
        /// Tier Side
        /// </summary>
        public string TierSide { get; set; }
        /// <summary>
        /// Glove Category
        /// </summary>
        public string GloveCategory { get; set; }
        /// <summary>
        /// Second Grade Type
        /// </summary>
        public string SecondGradeType { get; set; }
        /// <summary>
        /// Second Grade Type
        /// </summary>
        public string SecondGradePCs { get; set; }
        /// <summary>
        /// Packing Size
        /// </summary>
        public string PackingSize { get; set; }
        /// <summary>
        /// Inner Box
        /// </summary>
        public string InnerBox { get; set; }
        /// <summary>
        /// Total Glove Quantity
        /// </summary>
        public string TotalGloveQty { get; set; }

        public bool IsManual { get; set; }
    }

    public class PalletInfoDTO
    {
        public int locationId { set; get; }
        public string OperatorId { set; get; }
        public string Workstationnumber { set; get; }
        public string Packdate { set; get; }
        public string Palletid { set; get; }
        public string orderNumber { get; set; }
        public string Ponumber { set; get; }
        public string ItemName { set; get; }
        public string Size { set; get; }
        public int Casespacked { set; get; }
        public string Name { set; get; }
        public string CaseList { set; get; }
        public Boolean Isavailable { set; get; }
        public Boolean ispreshipmentCase { set; get; }
        public string ItemNumber { set; get; }
        public string SlipDate { set; get; }

        public string SlipTime { set; get; }

        public string Location { set; get; }

        public string CustomerRefPo { set; get; }

        
    }

    /// <summary>
    /// This is used to have method which integrate with hard devices
    /// </summary>
    public class FSDeviceIntegration : IDisposable 
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #region properties

        public DateTime ServerCurrentDateTime
        {
            get
            {
                return Convert.ToDateTime(FloorDBAccess.ExecuteScalar("USP_GET_CurrentDateAndTime"));
            }
        }
        #endregion

        public FSDeviceIntegration()
        {
            // Empty Contructor
        }

        #region Static Variables        
        /// <summary>
        /// Width of Barcode
        /// </summary>
        public const int BarcodeWidth = 200;
        /// <summary>
        /// For OnlineByPass Print
        /// </summary>
        public const string ByPassHeading = "Loose Reprint";
        /// <summary>
        /// Height of Barcode
        /// </summary>
        public const int BarcodeHeight = 50;
        /// <summary>
        /// Height of Drawing
        /// </summary>
        public const int DrawingHeight = 80;
        /// <summary>
        /// Height of Drawing
        /// </summary>
        public const string DimensionUnit = "0.0400";
        /// <summary>
        /// 13
        /// </summary>
        public const int THIRTEEN = 13;
        /// <summary>
        /// 27
        /// </summary>
        public const int TWENTYSEVEN = 27;
        /// <summary>
        ///   /// Font1 bold
        /// </summary>
        Font myFont1Bold = new Font("Arial", 10, FontStyle.Bold);
        /// Font1
        /// </summary>
        Font myFont1 = new Font("Arial", 10);
        /// <summary>
        /// Font2
        /// </summary>
        Font myFont2 = new Font("Arial", 20, FontStyle.Bold);
        /// <summary>
        /// Font3
        /// </summary>
        Font myFont3 = new Font("Arial", 13, FontStyle.Bold);
        /// <summary>
        /// Font4
        /// </summary>
        Font myFont4 = new Font("Arial", 8);
        /// <summary>
        /// Font5
        /// </summary>
        /// 
        Font myFont5 = new Font("Arial", 9);       
        /// <summary>
        /// Font6
        /// </summary>
        /// 
        Font myFont6 = new Font("Arial", 8, FontStyle.Underline);
        /// <summary>
        /// Font7
        /// </summary>
        /// 
        Font myFont7 = new Font("Arial", 11, FontStyle.Regular);
        /// <summary>
        /// Font8
        /// </summary>
        /// 
        Font myFont8 = new Font("Arial", 10, FontStyle.Regular);
        /// <summary>
        /// Font9
        /// </summary>
        /// 
        Font myFont9 = new Font("Arial", 8, FontStyle.Regular);
        /// <summary>
        /// KG
        /// </summary>
        /// 
        public const string KG = " Kg";
        /// <summary>
        /// GMS
        /// </summary>
        public const string GMS = " gms";
        #endregion

        #region Printing Methods
        /// <summary>
        /// Get Barcode Image
        /// </summary>

        private Image PrintBarcodeText(string serialNumber)
        {
            Image Image;
            try
            {
                Barcode b = new Barcode();
                TYPE type = TYPE.Interleaved2of5;
                Image = b.Encode(type, serialNumber.Trim(), Color.Black, Color.White, BarcodeWidth, BarcodeHeight);
                var newImage = new Bitmap(Image.Width, Image.Height + 20);
                var gr = Graphics.FromImage(newImage);
                gr.Clear(Color.White);
                gr.DrawImageUnscaled(Image, 0, 0);
                gr.DrawString(serialNumber, new Font("Arial", 9), Brushes.Black, 45, Image.Height);
                gr.Save();
                return newImage;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Constants.BARCODE_ERROR, Constants.INTEGRATION, ex);
            }
        }

        /// <summary>
        /// To Print data
        /// </summary>
        /// <param name="printData"></param>       
        public void Print(PrintDTO printData)
        {
            PaperSize psize = new PaperSize("Tubling", 600, 550);
            printData.Bmp = PrintBarcodeText(printData.SerialNumber);
            PrintDocument printDocument1 = new PrintDocument();
            printDocument1.PrintPage += delegate(object sender, PrintPageEventArgs e) { PrintPageEventHandler_Print( e, printData); };
            if (printData.Template != Constants.REJECT_GLOVE_SCREEN)
            printDocument1.DefaultPageSettings.PaperSize = psize;

            printDocument1.DocumentName = Constants.PRINTING;
            printDocument1.Print();            
        }

        /// <summary>
        /// To Print data
        /// </summary>
        /// <param name="printData"></param>       
        public void PrintON2G(PrintDTO printData)
        {
            PaperSize psize = new PaperSize("ON2G", 600, 550);

            PrintDocument printDocument1 = new PrintDocument();
            printDocument1.PrintPage += delegate (object sender, PrintPageEventArgs e) { ON2GPrintPageEventHandler(e, printData); };
            printDocument1.DocumentName = Constants.PRINTING;
            printDocument1.DefaultPageSettings.PaperSize = psize;
            printDocument1.Print();
            //log.InfoFormat("FSIntegration - End Print SerialNo {0}", printData.SerialNumber);
            

            //PaperSize psize = new PaperSize("Tubling", 600, 550);
            //printData.Bmp = PrintBarcodeText(printData.SerialNumber);
            //PrintDocument printDocument1 = new PrintDocument();
            //printDocument1.PrintPage += delegate (object sender, PrintPageEventArgs e) { PrintPageEventHandler_Print(e, printData); };
            //if (printData.Template != Constants.REJECT_GLOVE_SCREEN)
            //    printDocument1.DefaultPageSettings.PaperSize = psize;

            //printDocument1.DocumentName = Constants.PRINTING;
            //printDocument1.Print();
        }

        /// <summary>
        /// To print FP 
        /// </summary>
        /// <param name="printData"></param>
        public void PrintFPForm(PalletInfoDTO printData)
        {
            PrintDocument printDocument1 = new PrintDocument();
            printDocument1.PrintPage += delegate(object sender, PrintPageEventArgs e) { PrintPageEventHandler_FP( e, printData); };
            printDocument1.DocumentName = Constants.PRINTING;
            printDocument1.Print();
            string st1 = Character(TWENTYSEVEN) + "j" + Character(THIRTEEN);
            for (int i = Constants.ZERO; i < 5; i++)
            {
                st1 += Character(TWENTYSEVEN) + "j" + Character(THIRTEEN);
            }
            try
            {
                Framework.DirectPrinter.SendToPrinter("", st1, printDocument1.PrinterSettings.PrinterName);
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.PRINTING_ERROR, Constants.INTEGRATION, ex);
            }
        }


        /// <summary>
        /// User defines Print Page Event Handler to assign values accordingly
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ev"></param>
        /// <param name="printData"></param>
        private void PrintPageEventHandler_Print( PrintPageEventArgs ev, PrintDTO printData)
        {
            try
            {
                Graphics g = ev.Graphics;
                // #Azrul 13/07/2018: Merged from Live AX6 Start
                // 2018-06-01 Azman Start

                string gloveDesc = printData.GloveDesc.Replace("\r\n\t", "|");
                String[] glove = gloveDesc.Split('|');
                int gloveLength = glove[0].Length;

                StringFormat drawFormat = new StringFormat();
                drawFormat.Alignment = StringAlignment.Far;

                // 2018-06-01 Azman End 
                // #Azrul 13/07/2018: Merged from Live AX6 End

                if (printData.Template != Constants.REJECT_GLOVE_SCREEN)
                {
                    if (!String.IsNullOrEmpty(printData.TierSide))
                        g.DrawString(ByPassHeading, myFont1, Brushes.Black, 80, 0);
                    g.DrawString(printData.DateTime, myFont1, Brushes.Black, 10, 35);
                    g.DrawImage(printData.Bmp, 180, 0);
                    g.DrawString(printData.BatchNumber, myFont1, Brushes.Black, 10, 75);
                    if (printData.BatchWeightMsg == true)
                    {
                        g.DrawString((Convert.ToDouble(printData.BatchWeight)).ToString() + KG, myFont1, Brushes.Black, 10, 118);
                        g.DrawString(Messages.WEIGHT_OUT_OF_RANGE, myFont4, Brushes.Red, 145, 112);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(printData.BatchWeight))
                            g.DrawString(printData.BatchWeight + KG, myFont1, Brushes.Black, 10, 118);
                    }
                    g.DrawString(printData.Size, myFont2, Brushes.Black, 270, 104);
                    if (printData.TenPcsMsg == true)
                    {
                        g.DrawString((Convert.ToDouble(printData.TenPcsWeight) * 10).ToString() + GMS, myFont1, Brushes.Black, 10, 132);
                        g.DrawString(Messages.WEIGHT_OUT_OF_RANGE, myFont4, Brushes.Red, 145, 126);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(printData.TenPcsWeight))
                            g.DrawString((Convert.ToDouble(printData.TenPcsWeight) * 10).ToString() + GMS, myFont1, Brushes.Black, 10, 155);
                    }
                    g.DrawString(Convert.ToString(printData.TierSide), myFont3, Brushes.Black, 340, 114);
                    //g.DrawString(printData.GloveDesc, myFont3, Brushes.Black, 150, 180);
                    // #Azrul 13/07/2018: Merged from Live AX6 Start
                    // Azman 30/05/2018 To accomodate new glove code length
                    if (gloveLength < 27)
                        g.DrawString(printData.GloveDesc, myFont3, Brushes.Black, 150, 180);
                    if ((gloveLength > 26) && (gloveLength < 28))
                    {
                        string glovePrint = glove[0] + "\r\n\t\t" + glove[1];
                        g.DrawString(glovePrint, myFont3, Brushes.Black, 90, 180);
                    }
                    else if (gloveLength > 28)
                    {
                        string glovePrint = glove[0] + "\r\n\t\t" + glove[1];
                        g.DrawString(glovePrint, myFont3, Brushes.Black, 40, 180);
                    }
                    // Azman 30/05/2018 End
                    // #Azrul 13/07/2018: Merged from Live End

                    // Pang Start : P066 Puller information does not appear after reprint batch card
                    if (printData.Template == Constants.TUMBLING_REPRINT_BATCH_CARD && !string.IsNullOrEmpty(printData.Resource))
                        g.DrawString(Convert.ToString(printData.Resource), myFont3, Brushes.Black, 380, 210, drawFormat);
                    // Pang End
                }
                else
                {
                    g.DrawString(printData.DateTime, myFont5, Brushes.Black, 2, 6);
                    g.DrawImage(printData.Bmp, 190, 5);
                    g.DrawString(printData.BatchNumber, myFont5, Brushes.Black, 2, 26);
                    if (printData.BatchWeightMsg == true)
                    {
                        g.DrawString((Convert.ToDouble(printData.BatchWeight)).ToString() + KG, myFont1, Brushes.Black, 2, 46);
                        g.DrawString(Messages.WEIGHT_OUT_OF_RANGE, myFont4, Brushes.Red, 53, 22);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(printData.BatchWeight))
                            g.DrawString(printData.BatchWeight + KG, myFont1, Brushes.Black, 2, 46);
                    }
                    // g.DrawString(printData.BatchWeight + KG, myFont5, Brushes.Black, 8, 45);
                    //g.DrawString(printData.GloveDesc, myFont3, Brushes.Black, 90, 85);

                    // #Azrul 13/07/2018: Merged from Live AX6 Start
                    // Azman 30/05/2018 To accomodate new glove code length
                    if (gloveLength < 27)
                        g.DrawString(printData.GloveDesc, myFont3, Brushes.Black, 90, 85);
                    if ((gloveLength > 26) && (gloveLength < 28))
                        g.DrawString(printData.GloveDesc, myFont3, Brushes.Black, 30, 85);
                    else if (gloveLength > 28)
                        g.DrawString(printData.GloveDesc, myFont3, Brushes.Black, 0, 85);
                    // Azman 30/05/2018 End
                    // #Azrul 13/07/2018: Merged from Live AX6 End

                    //g.DrawImage(printData.Bmp, 190, 5);
                }
                myFont1.Dispose();
                myFont2.Dispose();
                myFont3.Dispose();
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.DRAWING_EXCEPTION, Constants.INTEGRATION, ex);
            }
        }

        private void PrintPageEventHandler_FP( PrintPageEventArgs ev, PalletInfoDTO printData)
        {
            try
            {
                Graphics g = ev.Graphics;
                g.DrawString(Convert.ToDateTime(ServerCurrentDateTime).ToLongTimeString(), myFont1, Brushes.Black, 25, 24);
                g.DrawString(Convert.ToString(printData.Size), myFont2, Brushes.Black, 250, 90);
                myFont1.Dispose();
                myFont2.Dispose();
                myFont3.Dispose();
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.DRAWING_EXCEPTION, Constants.INTEGRATION, ex);
            }
        }

        /// <summary>
        /// Get Barcode Image
        /// </summary>
        public void PrintTestSlip(PrintTestSlipDTO printData)
        {
            try
            {
                PrintDetails(printData);
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.DRAWING_EXCEPTION, Constants.INTEGRATION, ex);
            }
        }

        /// <summary>
        /// Get Barcode Image for Second Grade Sticker
        /// </summary>
        public void PrintSticker(PrintDTO printData)
        {
            try
            {
                printData.Bmp = PrintBarcodeText(printData.SerialNumber.Trim());
                PrintStickerDetails(printData);
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.DRAWING_EXCEPTION, Constants.INTEGRATION, ex);
            }
        }

        /// <summary>
        /// To Print data
        /// </summary>
        /// <param name="printData"></param>       
        public void PrintDetails(PrintTestSlipDTO printData)
        {
            PrintDocument printDocument1 = new PrintDocument();
            PaperSize psize = new PaperSize("PrintSlip", 900, 550);
            printDocument1.DefaultPageSettings.PaperSize = psize;           
            printDocument1.PrintPage += delegate(object sender, PrintPageEventArgs e) { PrintPageEventHandler_PrintSlip( e, printData); };

            printDocument1.DocumentName = Constants.PRINTING;
            printDocument1.Print();
            string st1 = Character(TWENTYSEVEN) + "j" + Character(THIRTEEN);
            for (int i = Constants.ZERO; i < 5; i++)
            {
                st1 += Character(TWENTYSEVEN) + "j" + Character(THIRTEEN);
            }
            try
            {
                Framework.DirectPrinter.SendToPrinter("", st1, printDocument1.PrinterSettings.PrinterName);
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.PRINTING_ERROR, Constants.INTEGRATION, ex);
            }
        }

        /// <summary>
        /// To Print Sticker data
        /// </summary>
        /// <param name="printData"></param>       
        public void PrintStickerDetails(PrintDTO printData)
        {
            try
            {
            PrintDocument printDocument1 = new PrintDocument();
            printDocument1.PrintPage += delegate(object sender, PrintPageEventArgs e) { PrintPageEventHandler_Slip(sender, e, printData); };

            printDocument1.DocumentName = Constants.PRINTING;
            printDocument1.Print();           
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.PRINTING_ERROR, Constants.INTEGRATION, ex);
            }
        }

        /// <summary>
        /// User defines Print Page Event Handler to assign values accordingly
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ev"></param>
        /// <param name="printData"></param>
        private void PrintPageEventHandler_PrintSlip( PrintPageEventArgs ev, PrintTestSlipDTO printData)
        {
            try
            {
                Graphics g = ev.Graphics;
                myFont2 = new Font("Arial", 20, FontStyle.Bold);
                Font myFont5 = new Font("Arial", 15);
                Font myFont6 = new Font("Arial", 10, FontStyle.Bold);
                Font myFont7 = new Font("Arial", 10, FontStyle.Bold | FontStyle.Underline);

                // Create rectangle.
                Rectangle rect = new Rectangle(0, 0, 790, 440);

                // Draw rectangle to screen.
                g.DrawRectangle(new Pen(Color.Black, 1), rect);

                System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.OrangeRed);
                g.FillRectangle(myBrush, new Rectangle(0, 0, 790, 35));
                myBrush.Dispose();
                g.DrawString(printData.TestSlipName + " Test", myFont5, Brushes.White, 05, 05);

                g.DrawString("Time", myFont1, Brushes.Black, 30, 50);
                g.DrawString(":", myFont1, Brushes.Black, 145, 50);
                g.DrawString(Convert.ToDateTime(printData.DateTime).ToLongTimeString(), myFont1, Brushes.Black, 155, 50);
                g.DrawImage(PrintBarcodeText(printData.SerialNumber.Trim()), 270, 50);
                g.DrawString("Ref No: ", myFont6, Brushes.Black, 525, 50);
                g.DrawImage(PrintBarcodeText(printData.ReferenceId.Trim()), 575, 50);

                g.DrawString("Batch No", myFont1, Brushes.Black, 30, 80);
                g.DrawString(":", myFont1, Brushes.Black, 145, 80);
                g.DrawString(printData.BatchNumber, myFont1, Brushes.Black, 155, 80);

                g.DrawString("Batch Weight", myFont1, Brushes.Black, 30, 110);
                g.DrawString(":", myFont1, Brushes.Black, 145, 110);
                g.DrawString(printData.BatchWeight + "kg", myFont1, Brushes.Black, 155, 110);
                g.DrawString(printData.TestSlipName + " Test Result", myFont7, Brushes.Black, 450, 110);

                g.DrawString("100 Pcs Weight", myFont1, Brushes.Black, 30, 140);
                g.DrawString(":", myFont1, Brushes.Black, 145, 140);
                g.DrawString(printData.HundredPcsWeight + "gms", myFont1, Brushes.Black, 155, 140);
                g.DrawString("Date tested", myFont1, Brushes.Black, 450, 140);
                g.DrawString(": ", myFont1, Brushes.Black, 640, 140);
                Pen blackPen = new Pen(Color.Black, 1);
                Point point1 = new Point(648, 152);
                Point point2 = new Point(780, 152);
                g.DrawLine(blackPen, point1, point2);


                g.DrawString("ID", myFont1, Brushes.Black, 30, 170);
                g.DrawString(":", myFont1, Brushes.Black, 145, 170);
                g.DrawString(printData.TesterId, myFont1, Brushes.Black, 155, 170);
                g.DrawString("Tested by", myFont1, Brushes.Black, 450, 175);
                g.DrawString(": ", myFont1, Brushes.Black, 640, 175);
                Point point3 = new Point(648, 187);
                Point point4 = new Point(780, 187);
                g.DrawLine(blackPen, point3, point4);

                // use smaller font for operator id MYAdamas 9/10/2017
                // g.DrawString(printData.TesterName, myFont3, Brushes.Black, 250, 170);
                g.DrawString(printData.TesterName, myFont1Bold, Brushes.Black, 250, 170);

                g.DrawString(printData.GloveType, myFont3, Brushes.Black, 05, 210);
                g.DrawString(printData.Size, myFont2, Brushes.Black, 330, 240);
                g.DrawString("Remarks", myFont1, Brushes.Black, 450, 210);
                g.DrawString(": ", myFont1, Brushes.Black, 640, 210);
                Point point5 = new Point(648, 225);
                Point point6 = new Point(780, 225);
                g.DrawLine(blackPen, point5, point6);
                g.DrawLine(blackPen, new Point(450, 260), new Point(780, 260));
                g.DrawLine(blackPen, new Point(450, 295), new Point(780, 295));

                if (printData.TestSlipName == Constants.HOTBOX_TEST)
                {
                    g.DrawString("PT Date", myFont1, Brushes.Black, 30, 290);
                    g.DrawString(":", myFont1, Brushes.Black, 115, 290);
                    g.DrawString(Convert.ToDateTime(printData.PTDate).Date.ToString("dd/MM/yyyy"), myFont1, Brushes.Black, 125, 290);
                    g.DrawString("PT Time", myFont1, Brushes.Black, 30, 320);
                    g.DrawString(":", myFont1, Brushes.Black, 115, 320);
                    g.DrawString(Convert.ToDateTime(printData.PTDate).ToLongTimeString(), myFont1, Brushes.Black, 125, 320);
                    g.DrawString("Cyclone No", myFont1, Brushes.Black, 30, 350);
                    g.DrawString(":", myFont1, Brushes.Black, 115, 350);
                    g.DrawString(printData.DryerNumber, myFont1, Brushes.Black, 125, 350);
                    g.DrawString("Program", myFont1, Brushes.Black, 190, 350);
                    g.DrawString(":", myFont1, Brushes.Black, 255, 350);
                    g.DrawString(printData.DryerProgram, myFont1, Brushes.Black, 270, 350);
                    g.DrawString("Washer No", myFont1, Brushes.Black, 30, 380);
                    g.DrawString(":", myFont1, Brushes.Black, 115, 380);
                    g.DrawString(printData.WasherNumber, myFont1, Brushes.Black, 125, 380);
                    g.DrawString("Program", myFont1, Brushes.Black, 190, 380);
                    g.DrawString(":", myFont1, Brushes.Black, 255, 380);
                    g.DrawString(printData.WasherProgram, myFont1, Brushes.Black, 270, 380);
                }


                else if (printData.TestSlipName == Constants.POWDER_TEST)
                {
                    g.DrawString("Filter Paper", myFont1, Brushes.Black, 450, 320);
                    g.DrawString(":", myFont1, Brushes.Black, 640, 320);
                    g.DrawLine(blackPen, new Point(648, 332), new Point(760, 332));
                    g.DrawString("g", myFont1, Brushes.Black, 762, 320);

                    g.DrawString("Filter Paper+Residue Powder", myFont1, Brushes.Black, 450, 350);
                    g.DrawString(":", myFont1, Brushes.Black, 640, 350);
                    g.DrawLine(blackPen, new Point(648, 362), new Point(760, 362));
                    g.DrawString("g", myFont1, Brushes.Black, 762, 350);

                    g.DrawString("Residue Powder per glove", myFont1, Brushes.Black, 450, 380);
                    g.DrawString(":", myFont1, Brushes.Black, 640, 380);
                    g.DrawLine(blackPen, new Point(648, 392), new Point(760, 392));
                    g.DrawString("mg", myFont1, Brushes.Black, 762, 380);
                }

                g.DrawString("Rework No", myFont1, Brushes.Black, 30, 410);
                g.DrawString(":", myFont1, Brushes.Black, 115, 410);
                g.DrawString(printData.ReworkCount, myFont1, Brushes.Black, 125, 410);
                g.DrawString("Pass:( )/Fail:( )", myFont1, Brushes.Black, 450, 410);
                g.DrawString("Data Entered:( )", myFont1, Brushes.Black, 670, 410);

                myFont1.Dispose();
                myFont2.Dispose();
                myFont3.Dispose();
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.DRAWING_EXCEPTION, Constants.INTEGRATION, ex);
            }
        }

        /// <summary>
        /// User defines Print Page Event Handler to assign values accordingly
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ev"></param>
        /// <param name="printData"></param>
        private void PrintPageEventHandler_Slip(Object obj, PrintPageEventArgs ev, PrintDTO printData)
        {
            try
            {
                Graphics g = ev.Graphics;
                g.DrawString("TIME", myFont4, Brushes.Black, 8, 5);
                g.DrawString(":", myFont4, Brushes.Black, 120, 5);
                g.DrawString(Convert.ToDateTime(printData.DateTime).ToLongTimeString(), myFont4, Brushes.Black, 130, 5);
                g.DrawImage(printData.Bmp, 190, 5);
                g.DrawString("BATCH NO.", myFont4, Brushes.Black, 8, 25);
                g.DrawString(":", myFont4, Brushes.Black, 120, 25);
                g.DrawString(Convert.ToString(printData.BatchNumber), myFont4, Brushes.Black, 130, 25);

                //g.DrawString("2nd GRADE TYPE", myFont4, Brushes.Black, 8, 45);
                g.DrawString("QUANTITY", myFont4, Brushes.Black, 8, 45);
                g.DrawString(":", myFont4, Brushes.Black, 120, 45);
                //g.DrawString(Convert.ToString(printData.SecondGradeType), myFont4, Brushes.Black, 130, 45);
                g.DrawString(printData.SecondGradePCs + " (Pcs)", myFont4, Brushes.Black, 130, 45);

                g.DrawString(Convert.ToString(printData.GloveDesc), myFont3, Brushes.Black, 70, 95);
                myFont1.Dispose();
                myFont2.Dispose();
                myFont3.Dispose();
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.DRAWING_EXCEPTION, Constants.INTEGRATION, ex);
            }
        }


        /// <summary>
        /// To convert integer to character
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public static Char Character(int i)
        {
            return Convert.ToChar(i);
        }

        #region "Hourly Batch card Printing"
        public void HBCPrint(List<PrintDTO> printDataLst)
        {
            PaperSize psize = new PaperSize("HBC", 600, 550);
            foreach (PrintDTO printData in printDataLst)
            {
                string printInd = printData.IsReprint ? "Reprint" : printData.IsManual ? "Manual Print" : "HBC Print";

                PrintDocument printDocument1 = new PrintDocument();
                log.InfoFormat("FSIntegration - Start {0} SerialNo {1}", printInd,  printData.SerialNumber);
                printDocument1.PrintPage += delegate (object sender, PrintPageEventArgs e) { HBCPrintPageEventHandler(e, printData); };
                printDocument1.DocumentName = Constants.PRINTING;
                printDocument1.DefaultPageSettings.PaperSize = psize;
                printDocument1.Print();
                log.InfoFormat("FSIntegration - End Print SerialNo {0}", printData.SerialNumber);
            }
        }

        /// <summary>
        /// User defines Print Page Event Handler to assign values accordingly
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ev"></param>
        /// <param name="printData"></param>
        private void HBCPrintPageEventHandler(PrintPageEventArgs ev, PrintDTO printData)
        {
            try
            {
                Graphics g = ev.Graphics;
                StringFormat drawFormat = new StringFormat();
                drawFormat.Alignment = StringAlignment.Far;
                if (!printData.IsReprint)
                {
                    g.DrawString(printData.DateTime, myFont1, Brushes.Black, 35, 35);
                    g.DrawImage(PrintBarcodeText(printData.SerialNumber), 185, 0);
                    g.DrawString(Convert.ToString(printData.BatchNumber), myFont1, Brushes.Black, 35, 75);
                    g.DrawString("Pack Size:  " + Convert.ToString(printData.PackingSize), myFont5, Brushes.Black, 35, 113);
                    g.DrawString("Inner Box:  " + Convert.ToString(printData.InnerBox), myFont5, Brushes.Black, 35, 125);
                    g.DrawString("Total:  " + Convert.ToString(printData.TotalGloveQty), myFont5, Brushes.Black, 35, 170);
                    g.DrawString(Convert.ToString(printData.Size), myFont2, Brushes.Black, 250, 90);

                    // #Azrul 13/07/2018: Merged from Live AX6 Start
                    // Azman 30/05/2018 To accomodate new glove code length
                    if (printData.GloveDesc.Length < 27)
                        g.DrawString(Convert.ToString(printData.GloveDesc), myFont3, Brushes.Black, 150, 150);
                    if ((printData.GloveDesc.Length > 26) && (printData.GloveDesc.Length < 28))
                        g.DrawString(Convert.ToString(printData.GloveDesc), myFont3, Brushes.Black, 90, 150);
                    else if (printData.GloveDesc.Length > 28)
                        g.DrawString(Convert.ToString(printData.GloveDesc), myFont3, Brushes.Black, 40, 150);
                    // Azman 30/05/2018 End
                    // #Azrul 13/07/2018: Merged from Live AX6 End


                    g.DrawString(Convert.ToString(printData.GloveCategory), myFont3, Brushes.Black, 250, 170);
                    g.DrawString(Convert.ToString(printData.Resource), myFont3, Brushes.Black, 380, 200, drawFormat);
                }
                else
                {
                    g.DrawString(printData.DateTime, myFont1, Brushes.Black, 35, 35);
                    g.DrawString("Reprint", myFont1, Brushes.Black, 205, 0);
                    g.DrawImage(PrintBarcodeText(printData.SerialNumber), 185, 18);
                    g.DrawString(Convert.ToString(printData.BatchNumber), myFont1, Brushes.Black, 35, 75);
                    g.DrawString("Pack Size: " + Convert.ToString(printData.PackingSize), myFont5, Brushes.Black, 35, 113);
                    g.DrawString("Inner Box: " + Convert.ToString(printData.InnerBox), myFont5, Brushes.Black, 35, 125);
                    g.DrawString("Total: " + Convert.ToString(printData.TotalGloveQty), myFont5, Brushes.Black, 35, 190);
                    g.DrawString(Convert.ToString(printData.Size), myFont2, Brushes.Black, 250, 110);

                    // #Azrul 13/07/2018: Merged from Live AX6 Start
                    // Azman 30/05/2018 To accomodate new glove code length
                    if (printData.GloveDesc.Length < 27)
                        g.DrawString(Convert.ToString(printData.GloveDesc), myFont3, Brushes.Black, 150, 168);
                    if ((printData.GloveDesc.Length > 26) && (printData.GloveDesc.Length < 28))
                        g.DrawString(Convert.ToString(printData.GloveDesc), myFont3, Brushes.Black, 90, 168);
                    else if (printData.GloveDesc.Length > 28)
                        g.DrawString(Convert.ToString(printData.GloveDesc), myFont3, Brushes.Black, 40, 168);
                    // Azman 30/05/2018 End
                    // #Azrul 13/07/2018: Merged from Live AX6 End

                    g.DrawString(Convert.ToString(printData.GloveCategory), myFont3, Brushes.Black, 250, 188);
                    g.DrawString(Convert.ToString(printData.Resource), myFont3, Brushes.Black, 380, 218, drawFormat);
                }

            }
            catch (Exception ex)
            {
                string reprintInd = printData.IsReprint ? "Re" : "";
                log.ErrorFormat("Error: {0}; FSIntegration - Draw {0}print layout - SerialNo {1} | {2}", reprintInd, printData.SerialNumber, ex.Message);
                throw new FloorSystemException(Messages.DRAWING_EXCEPTION, Constants.INTEGRATION, ex);
            }
        }


        /// <summary>
        /// User defines Print Page Event Handler to assign values accordingly
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ev"></param>
        /// <param name="printData"></param>
        private void ON2GPrintPageEventHandler(PrintPageEventArgs ev, PrintDTO printData)
        {
            try
            {
                Graphics g = ev.Graphics;
                StringFormat drawFormat = new StringFormat();
                drawFormat.Alignment = StringAlignment.Far;
                
                g.DrawString(printData.DateTime, myFont1, Brushes.Black, 35, 35);
                g.DrawString("Reprint", myFont1, Brushes.Black, 205, 0);
                g.DrawImage(PrintBarcodeText(printData.SerialNumber), 185, 18);
                g.DrawString(Convert.ToString(printData.BatchNumber), myFont1, Brushes.Black, 35, 75);
                g.DrawString("Pack Size: " + Convert.ToString(printData.PackingSize), myFont5, Brushes.Black, 35, 113);
                g.DrawString("Inner Box: " + Convert.ToString(printData.InnerBox), myFont5, Brushes.Black, 35, 125);
                g.DrawString("Total: " + Convert.ToString(printData.TotalGloveQty), myFont5, Brushes.Black, 35, 190);
                g.DrawString(Convert.ToString(printData.Size), myFont2, Brushes.Black, 250, 110);

                // #Azrul 13/07/2018: Merged from Live AX6 Start
                // Azman 30/05/2018 To accomodate new glove code length
                if (printData.GloveDesc.Length < 27)
                    g.DrawString(Convert.ToString(printData.GloveDesc), myFont3, Brushes.Black, 150, 168);
                if ((printData.GloveDesc.Length > 26) && (printData.GloveDesc.Length < 28))
                    g.DrawString(Convert.ToString(printData.GloveDesc), myFont3, Brushes.Black, 90, 168);
                else if (printData.GloveDesc.Length > 28)
                    g.DrawString(Convert.ToString(printData.GloveDesc), myFont3, Brushes.Black, 40, 168);
                // Azman 30/05/2018 End
                // #Azrul 13/07/2018: Merged from Live AX6 End

                g.DrawString(Convert.ToString(printData.GloveCategory), myFont3, Brushes.Black, 250, 188);
                g.DrawString(Convert.ToString(printData.Resource), myFont3, Brushes.Black, 380, 218, drawFormat);

            }
            catch (Exception ex)
            {
                string reprintInd = printData.IsReprint ? "Re" : "";
                log.ErrorFormat("Error: {0}; FSIntegration - Draw {0}print layout - SerialNo {1} | {2}", reprintInd, printData.SerialNumber, ex.Message);
                throw new FloorSystemException(Messages.DRAWING_EXCEPTION, Constants.INTEGRATION, ex);
            }
        }
        #endregion

        #region "Surgical Batch Card Printing"
        public void SRBCPrint(List<PrintDTO> printDataLst)
        {
            PaperSize psize = new PaperSize("SRBC", 600, 550);
            foreach (PrintDTO printData in printDataLst)
            {
                PrintDocument printDocument1 = new PrintDocument();
                printDocument1.PrintPage += delegate (object sender, PrintPageEventArgs e) { SRBCPrintPageEventHandler(e, printData); };
                printDocument1.DocumentName = Constants.PRINTING;
                printDocument1.DefaultPageSettings.PaperSize = psize;
                printDocument1.Print();

            }
        }

        /// <summary>
        /// User defines Print Page Event Handler to assign values accordingly
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="ev"></param>
        /// <param name="printData"></param>
        private void SRBCPrintPageEventHandler(PrintPageEventArgs ev, PrintDTO printData)
        {
            try
            {
                Graphics g = ev.Graphics;
                StringFormat drawFormat = new StringFormat();
                drawFormat.Alignment = StringAlignment.Far;
                if (!printData.IsReprint)
                {
                    g.DrawString(printData.DateTime, myFont1, Brushes.Black, 35, 35);
                    g.DrawImage(PrintBarcodeText(printData.SerialNumber), 185, 0);
                    g.DrawString(Convert.ToString(printData.BatchNumber), myFont1, Brushes.Black, 35, 75);
                    //g.DrawString("Batch Weight", myFont6, Brushes.Black, 35, 113);
                    g.DrawString(Convert.ToString(printData.BatchWeight), myFont3, Brushes.Black, 35, 125);
                    //#Azrul 20200112: Temp disabled, Print SRBC Qty default to 0. START
                    //g.DrawString("Total", myFont6, Brushes.Black, 35, 168);
                    //g.DrawString(Convert.ToString(printData.TotalGloveQty), myFont8, Brushes.Black, 35, 180);
                    //#Azrul 20200112: Temp disabled, Print SRBC Qty default to 0. END
                    g.DrawString(Convert.ToString(printData.Size), myFont2, Brushes.Black, 250, 90);

                    // #Azrul 13/07/2018: Merged from Live AX6 Start
                    // Azman 30/05/2018 To accomodate new glove code length
                    if (printData.GloveDesc.Length < 27)
                        g.DrawString(Convert.ToString(printData.GloveDesc), myFont3, Brushes.Black, 150, 150);
                    if ((printData.GloveDesc.Length > 26) && (printData.GloveDesc.Length < 28))
                        g.DrawString(Convert.ToString(printData.GloveDesc), myFont3, Brushes.Black, 90, 150);
                    else if (printData.GloveDesc.Length > 28)
                        g.DrawString(Convert.ToString(printData.GloveDesc), myFont3, Brushes.Black, 40, 150);
                    // Azman 30/05/2018 End
                    // #Azrul 13/07/2018: Merged from Live AX6 End

                    g.DrawString(Convert.ToString(printData.GloveCategory), myFont3, Brushes.Black, 250, 170);
                    g.DrawString(Convert.ToString(printData.Resource), myFont3, Brushes.Black, 380, 200, drawFormat);
                }
                else
                {
                    g.DrawString(printData.DateTime, myFont1, Brushes.Black, 35, 35);
                    g.DrawString("Reprint", myFont1, Brushes.Black, 205, 0);
                    g.DrawImage(PrintBarcodeText(printData.SerialNumber), 185, 18);
                    g.DrawString(Convert.ToString(printData.BatchNumber), myFont1, Brushes.Black, 35, 75);
                    //g.DrawString("Batch Weight", myFont6, Brushes.Black, 35, 113);
                    g.DrawString(Convert.ToString(printData.BatchWeight), myFont3, Brushes.Black, 35, 125);
                    //#Azrul 20200112: Temp disabled, Print SRBC Qty default to 0. START
                    //g.DrawString("Total", myFont6, Brushes.Black, 35, 168);
                    //g.DrawString(Convert.ToString(printData.TotalGloveQty), myFont8, Brushes.Black, 35, 180);
                    //#Azrul 20200112: Temp disabled, Print SRBC Qty default to 0. END
                    g.DrawString(Convert.ToString(printData.Size), myFont2, Brushes.Black, 250, 90);

                    // #Azrul 13/07/2018: Merged from Live AX6 Start
                    // Azman 30/05/2018 To accomodate new glove code length
                    if (printData.GloveDesc.Length < 27)
                        g.DrawString(Convert.ToString(printData.GloveDesc), myFont3, Brushes.Black, 150, 150);
                    if ((printData.GloveDesc.Length > 26) && (printData.GloveDesc.Length < 28))
                        g.DrawString(Convert.ToString(printData.GloveDesc), myFont3, Brushes.Black, 90, 150);
                    else if (printData.GloveDesc.Length > 28)
                        g.DrawString(Convert.ToString(printData.GloveDesc), myFont3, Brushes.Black, 40, 150);
                    // Azman 30/05/2018 End
                    // #Azrul 13/07/2018: Merged from Live AX6 End

                    g.DrawString(Convert.ToString(printData.GloveCategory), myFont3, Brushes.Black, 250, 170);
                    g.DrawString(Convert.ToString(printData.Resource), myFont3, Brushes.Black, 380, 200, drawFormat);
                }

            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Messages.DRAWING_EXCEPTION, Constants.INTEGRATION, ex);
            }
        }

        #endregion
        #endregion

        #region ScalingMethods
        /// <summary>
        /// Get Batch Weight
        /// </summary>
        /// <returns></returns>
        public static double GetBatchWeight(string comPort, string baudRate, string parity, string dataBit, string stopBit, string readSec, bool lBucket, decimal pallentinerWeight, string platformScalingSystem)
        {
            try
            {
                double val = 0;
                while (val == 0)
                {
                    HSBHWInterface hw = new HSBHWInterface(comPort);
                    hw.BaudRate = Convert.ToInt16(baudRate);
                    hw.ReadSec = Convert.ToInt16(readSec);
                    hw.DataBit = Convert.ToInt16(dataBit);
                    if (parity == Constants.EVEN_PARITY)
                        hw.Parity = Parity.Even;
                    else if (parity == Constants.ODD_PARITY)
                        hw.Parity = Parity.Odd;
                    else if (parity == Constants.MARK_PARITY)
                        hw.Parity = Parity.Mark;
                    else if (parity == Constants.SPACE_PARITY)
                        hw.Parity = Parity.Space;
                    else
                        hw.Parity = Parity.None;
                    if (Convert.ToInt16(stopBit) == Constants.ONE)
                        hw.StopBits = StopBits.One;
                    else if (Convert.ToInt16(stopBit) == Constants.TWO)
                        hw.StopBits = StopBits.Two;
                    else
                        hw.StopBits = StopBits.None;
                    if (lBucket == true)
                        hw.Basket = Convert.ToDouble(pallentinerWeight);
                    else
                        hw.Basket = Constants.ZERO;
                    if (platformScalingSystem.ToUpper() == Constants.PLATFORMSCALE_DI10)
                        val = hw.PlatformScaleDI10();
                    else if (platformScalingSystem.ToUpper() == Constants.PLATFORMSCALE_DI28)
                        val = hw.PlatformScaleDI28();
                    else
                        val = hw.PlatformScaleYamato();
                }
                return val;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Constants.WEIGHT_ERROR, Constants.INTEGRATION, ex);
            }
        }

        /// <summary>
        /// Get Ten Pcs Weight
        /// </summary>
        /// <returns></returns>
        public static double GetTenPcsWeight(string comPort, string baudRate, string parity, string dataBit, string stopBit, string readSec, string smallScalingSystem)
        {
            try
            {
                double val = 0;
                while (val == 0)
                {
                    HSBHWInterface hw = new HSBHWInterface(comPort);
                    hw.BaudRate = Convert.ToInt16(baudRate);
                    hw.DataBit = Convert.ToInt16(dataBit);
                    if (parity == Constants.EVEN_PARITY)
                        hw.Parity = Parity.Even;
                    else if (parity == Constants.ODD_PARITY)
                        hw.Parity = Parity.Odd;
                    else if (parity == Constants.MARK_PARITY)
                        hw.Parity = Parity.Mark;
                    else if (parity == Constants.SPACE_PARITY)
                        hw.Parity = Parity.Space;
                    else
                        hw.Parity = Parity.None;
                    hw.ReadSec = Convert.ToInt16(readSec);
                    if (Convert.ToInt16(stopBit) == Constants.ONE)
                        hw.StopBits = StopBits.One;
                    else if (Convert.ToInt16(stopBit) == Constants.TWO)
                        hw.StopBits = StopBits.Two;
                    else
                        hw.StopBits = StopBits.None;
                    if (smallScalingSystem.ToLower() == Constants.SMALLSCALE_OHAUS)
                        val = hw.SmallScaleOhaus();
                    else if (smallScalingSystem.ToLower() == Constants.SMALLSCALE_TXB622L)
                        val = hw.SmallScaleShimadzuTXB622L();
                    else
                        val = hw.SmallScaleShimadzu();
                }
                return val;
            }
            catch (Exception ex)
            {
                throw new FloorSystemException(Constants.WEIGHT_ERROR, Constants.INTEGRATION, ex);
            }
        }

        #endregion
        /// <summary>
        /// to dispose objects
        /// </summary>
        public void Dispose()
        {
            try
            {
                //this can be deleted if it doesn't affect the functionality
               //GC.SuppressFinalize(this);
            }
            catch (Exception)
            {
                throw ;
            }
        }
    }
}
