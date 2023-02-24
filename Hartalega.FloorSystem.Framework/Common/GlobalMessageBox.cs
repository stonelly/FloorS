using Hartalega.FloorSystem.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hartalega.FloorSystem.Framework.Common
{
    public class GlobalMessageBoxLabel : Label
    {
        public static Label Set(string Text = "", Font Font = null, Color ForeColor = new Color(), Color BackColor = new Color())
        {
            Label l = new Label();
            l.Text = Text;
            l.Font = (Font == null) ? new Font("Microsoft Sans Serif", 18) : Font;
            l.ForeColor = (ForeColor == new Color()) ? Color.Black : ForeColor;
            l.BackColor = (BackColor == new Color()) ? SystemColors.Control : BackColor;
            l.AutoSize = true;
            return l;
        }
    }
    public class GlobalMessageBoxButton : Button
    {
        public static Button Set(string Text = "", int Width = 102, int Height = 30, Font Font = null, Color ForeColor = new Color(), Color BackColor = new Color())
        {
            Button b = new Button();
            b.Text = Text;
            b.Width = Width;
            b.Height = Height;
            b.Font = (Font == null) ? new Font("Microsoft Sans Serif", 18) : Font;
            b.ForeColor = (ForeColor == new Color()) ? Color.Black : ForeColor;
            b.BackColor = (BackColor == new Color()) ? SystemColors.Control : BackColor;
            b.UseVisualStyleBackColor = (b.BackColor == SystemColors.Control);
            return b;
        }
    }
    public class GlobalMessageBoxImage : PictureBox
    {
        public static PictureBox Set(string ImagePath = null, int Width = 60, int Height = 60)
        {
            PictureBox i = new PictureBox();
            if (ImagePath != null)
            {
                i.BackgroundImageLayout = ImageLayout.Zoom;
                i.Location = new Point(9, 9);
                i.Margin = new Padding(3, 3, 2, 3);
                i.Size = new Size(Width, Height);
                i.TabStop = false;
                i.Visible = true;
                i.BackgroundImage = Image.FromFile(ImagePath);
            }
            else
            {
                i.Visible = true;
                i.Size = new Size(0, 0);
            }
            return i;
        }
    }
    public partial class GlobalMessageBox : Form
    {
        private GlobalMessageBox()
        {
            this.panText = new FlowLayoutPanel();
            this.panButtons = new FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // panText
            // 
            this.panText.Parent = this;
            this.panText.AutoScroll = true;
            this.panText.AutoSize = true;
            this.panText.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            //this.panText.Location = new Point(90, 90);
            this.panText.Margin = new Padding(0);
            this.panText.MaximumSize = new Size(500, 300);
            this.panText.MinimumSize = new Size(108, 50);
            this.panText.Size = new Size(108, 50);
            // 
            // panButtons
            // 
            this.panButtons.AutoSize = true;
            this.panButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.panButtons.FlowDirection = FlowDirection.RightToLeft;
            this.panButtons.Location = new Point(89, 89);
            this.panButtons.Margin = new Padding(0);
            this.panButtons.MaximumSize = new Size(580, 150);
            this.panButtons.MinimumSize = new Size(108, 0);
            this.panButtons.Size = new Size(108, 35);
            // 
            // MyMessageBox
            // 
            this.AutoScaleDimensions = new SizeF(8F, 19F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(206, 133);
            this.Controls.Add(this.panButtons);
            this.Controls.Add(this.panText);
            this.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Margin = new Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new Size(168, 132);
            this.Name = "MyMessageBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();
        }
       
        public static string Show(string Label, string Title = "", List<Button> Buttons = null, Icon icon = null, PictureBox Image = null)
        {
            Constants.AlertType type = Constants.AlertType.Warning;
            icon = (icon == null) ? SystemIcons.Information : icon;
            if (icon.Equals(SystemIcons.Information))
                type = Constants.AlertType.Information;
            else if (icon.Equals(SystemIcons.Warning))
                type = Constants.AlertType.Warning;
            else if (icon.Equals(SystemIcons.Error))
                type = Constants.AlertType.Error;
            else if (icon.Equals(SystemIcons.Exclamation))
                type = Constants.AlertType.Exclamation;
            else if (icon.Equals(SystemIcons.Question))
                type = Constants.AlertType.Question;

            return Show(Label, type, Title, Buttons);


        }

        public static string ShowMessageInRedFont(string Label, Constants.AlertType type, string title = "", List<Button> Buttons = null)
        {

            List<Label> Labels = new List<Label>();
            Labels.Add(GlobalMessageBoxLabel.Set(Label,null,Color.Red));
            return Show(type, Labels, title, Buttons);
        }

        public static string Show(string Label, Constants.AlertType type, string title = "", List<Button> Buttons = null)
        {

            List<Label> Labels = new List<Label>();
            Labels.Add(GlobalMessageBoxLabel.Set(Label));
            return Show(type, Labels, title, Buttons);
        }

        private static string Show(Constants.AlertType type, List<Label> Labels = null, string titile = "", List<Button> Buttons = null)
        {
            if (Labels == null) Labels = new List<Label>();
            if (Labels.Count == 0) Labels.Add(GlobalMessageBoxLabel.Set(string.Empty));
            if (Buttons == null) Buttons = new List<Button>();
            if (Buttons.Count == 0) Buttons.Add(GlobalMessageBoxButton.Set("OK"));
            List<Button> buttons = new List<Button>(Buttons);
            buttons.Reverse();

            int ImageWidth = 0;
            int ImageHeight = 0;
            int LabelWidth = 0;
            int LabelHeight = 0;
            int ButtonWidth = 0;
            int ButtonHeight = 0;
            int TotalWidth = 0;
            int TotalHeight = 0;

            GlobalMessageBox mb = new GlobalMessageBox();
            PictureBox Image = new PictureBox();
            Icon icon = null;

            switch (type)
            {
                case Constants.AlertType.Warning:
                    mb.Text = (String.IsNullOrEmpty(titile)) ? Constants.AlertType.Warning.ToString() : titile;
                    icon = SystemIcons.Warning;
                    break;
                case Constants.AlertType.Error:
                    mb.Text = (String.IsNullOrEmpty(titile)) ? Constants.AlertType.Error.ToString() : titile;
                    icon = SystemIcons.Error;
                    break;
                case Constants.AlertType.Information:
                    mb.Text = (String.IsNullOrEmpty(titile)) ? Constants.AlertType.Information.ToString() : titile;
                    icon = SystemIcons.Information;
                    break;
                case Constants.AlertType.Question:
                    mb.Text = (String.IsNullOrEmpty(titile)) ? Constants.AlertType.Question.ToString() : titile;
                    icon = SystemIcons.Question;
                    break;
                case Constants.AlertType.Exclamation:
                    mb.Text = (String.IsNullOrEmpty (titile)) ? Constants.AlertType.Exclamation.ToString() : titile;
                    icon = SystemIcons.Exclamation;
                    break;
            }

            mb.Controls.Add(Image);
            Image.MaximumSize = new Size(150, 300);
            ImageWidth = 50;//Image.Width + Image.Margin.Horizontal;
            ImageHeight = 50; //Image.Height + Image.Margin.Vertical;
            if (icon != null)
                Image.Image = icon.ToBitmap();
            List<int> il = new List<int>();
            mb.panText.Location = new Point(5 + ImageWidth, 9);
            foreach (Label l in Labels)
            {
                mb.panText.Controls.Add(l);
                l.Location = new Point(200, 50);
                l.MaximumSize = new Size(480, 2000);
                l.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                il.Add(l.Width);
            }
            il.ToString();
            Labels.ForEach(l => l.MinimumSize = new Size(Labels.Max(x => x.Width), 1));
            mb.panText.Height = Labels.Sum(l => l.Height);
            mb.panText.MinimumSize = new Size(Labels.Max(x => x.Width) + mb.ScrollBarWidth(Labels), ImageHeight);
            mb.panText.MaximumSize = new Size(Labels.Max(x => x.Width) + mb.ScrollBarWidth(Labels), 300);
            LabelWidth = mb.panText.Width;
            LabelHeight = mb.panText.Height;
            Image.Location = new Point(15, mb.panText.Location.Y);
            foreach (Button b in buttons)
            {
                mb.panButtons.Controls.Add(b);
                b.Location = new Point(3, 3);
                b.TabIndex = Buttons.FindIndex(i => i.Text == b.Text);
                b.Click += new EventHandler(mb.Button_Click);
                b.Size = new Size(130, 39);
            }
            ButtonWidth = mb.panButtons.Width;
            ButtonHeight = mb.panButtons.Height;

            //Set Widths
            if (ButtonWidth > ImageWidth + LabelWidth)
            {
                Labels.ForEach(l => l.MinimumSize = new Size(ButtonWidth - ImageWidth - mb.ScrollBarWidth(Labels), 1));
                mb.panText.Height = Labels.Sum(l => l.Height);
                mb.panText.MinimumSize = new Size(Labels.Max(x => x.Width) + mb.ScrollBarWidth(Labels), ImageHeight);
                mb.panText.MaximumSize = new Size(Labels.Max(x => x.Width) + mb.ScrollBarWidth(Labels), 300);
                LabelWidth = mb.panText.Width;
                LabelHeight = mb.panText.Height;
            }
            TotalWidth = ImageWidth + LabelWidth;

            //Set Height
            TotalHeight = LabelHeight + ButtonHeight;

            // mb.panButtons.Location = new Point(TotalWidth - ButtonWidth + 9, mb.panText.Location.Y + mb.panText.Height);
            try
            {
                mb.panButtons.Location = new Point(((TotalWidth - ButtonWidth) / 2) + 9, mb.panText.Location.Y + mb.panText.Height);
            }
            catch(Exception ex)
            {
                mb.panButtons.Location = new Point(TotalWidth - ButtonWidth + 9, mb.panText.Location.Y + mb.panText.Height);
            }


            mb.StartPosition = FormStartPosition.CenterScreen;
            mb.Size = new Size(TotalWidth + 25, TotalHeight + 47);
            mb.ShowDialog();
            return mb.Result;
        }

        //public static string Show(List<Label> Labels = null, string Title = "", List<Button> Buttons = null, PictureBox Image = null, Icon icon = null)
        //{
        //    if (Labels == null) Labels = new List<Label>();
        //    if (Labels.Count == 0) Labels.Add(GlobalMessageBoxLabel.Set(string.Empty));
        //    if (Buttons == null) Buttons = new List<Button>();
        //    if (Buttons.Count == 0) Buttons.Add(GlobalMessageBoxButton.Set("OK"));
        //    List<Button> buttons = new List<Button>(Buttons);
        //    buttons.Reverse();

        //    int ImageWidth = 0;
        //    int ImageHeight = 0;
        //    int LabelWidth = 0;
        //    int LabelHeight = 0;
        //    int ButtonWidth = 0;
        //    int ButtonHeight = 0;
        //    int TotalWidth = 0;
        //    int TotalHeight = 0;

        //    GlobalMessageBox mb = new GlobalMessageBox();

        //    mb.Text = Title;

        //    //Image
        //    if (Image != null)
        //    {
        //        mb.Controls.Add(Image);
        //        Image.MaximumSize = new Size(150, 300);
        //        ImageWidth = 50;//Image.Width + Image.Margin.Horizontal;
        //        ImageHeight = 50; //Image.Height + Image.Margin.Vertical;
        //        if (icon != null)
        //            Image.Image = icon.ToBitmap();

        //    }

        //    //Labels
        //    List<int> il = new List<int>();
        //    mb.panText.Location = new Point(5 + ImageWidth, 9);
        //    foreach (Label l in Labels)
        //    {
        //        mb.panText.Controls.Add(l);
        //        l.Location = new Point(200, 50);
        //        l.MaximumSize = new Size(480, 2000);
        //        l.Font = new Font("Microsoft Sans Serif", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
        //        il.Add(l.Width);
        //    }
        //    il.ToString();
        //    Labels.ForEach(l => l.MinimumSize = new Size(Labels.Max(x => x.Width), 1));
        //    mb.panText.Height = Labels.Sum(l => l.Height);
        //    mb.panText.MinimumSize = new Size(Labels.Max(x => x.Width) + mb.ScrollBarWidth(Labels), ImageHeight);
        //    mb.panText.MaximumSize = new Size(Labels.Max(x => x.Width) + mb.ScrollBarWidth(Labels), 300);
        //    LabelWidth = mb.panText.Width;
        //    LabelHeight = mb.panText.Height;
        //    if (Image != null)
        //        Image.Location = new Point(15, mb.panText.Location.Y);
        //    //Buttons
        //    foreach (Button b in buttons)
        //    {
        //        mb.panButtons.Controls.Add(b);
        //        b.Location = new Point(3, 3);
        //        b.TabIndex = Buttons.FindIndex(i => i.Text == b.Text);
        //        b.Click += new EventHandler(mb.Button_Click);
        //        b.Size = new Size(130, 39);
        //    }
        //    ButtonWidth = mb.panButtons.Width;
        //    ButtonHeight = mb.panButtons.Height;

        //    //Set Widths
        //    if (ButtonWidth > ImageWidth + LabelWidth)
        //    {
        //        Labels.ForEach(l => l.MinimumSize = new Size(ButtonWidth - ImageWidth - mb.ScrollBarWidth(Labels), 1));
        //        mb.panText.Height = Labels.Sum(l => l.Height);
        //        mb.panText.MinimumSize = new Size(Labels.Max(x => x.Width) + mb.ScrollBarWidth(Labels), ImageHeight);
        //        mb.panText.MaximumSize = new Size(Labels.Max(x => x.Width) + mb.ScrollBarWidth(Labels), 300);
        //        LabelWidth = mb.panText.Width;
        //        LabelHeight = mb.panText.Height;
        //    }
        //    TotalWidth = ImageWidth + LabelWidth;

        //    //Set Height
        //    TotalHeight = LabelHeight + ButtonHeight;

        //    // mb.panButtons.Location = new Point(TotalWidth - ButtonWidth + 9, mb.panText.Location.Y + mb.panText.Height);
        //    try
        //    {
        //        mb.panButtons.Location = new Point(((TotalWidth - ButtonWidth) / 2) + 9, mb.panText.Location.Y + mb.panText.Height);
        //    }
        //    catch(Exception ex)
        //    {
        //        mb.panButtons.Location = new Point(TotalWidth - ButtonWidth + 9, mb.panText.Location.Y + mb.panText.Height);
        //    }


        //    mb.StartPosition = FormStartPosition.CenterScreen;
        //    mb.Size = new Size(TotalWidth + 25, TotalHeight + 47);
        //    mb.ShowDialog();
        //    return mb.Result;
        //}

        private FlowLayoutPanel panText;
        private FlowLayoutPanel panButtons;
        private int ScrollBarWidth(List<Label> Labels)
        {
            return (Labels.Sum(l => l.Height) > 300) ? 23 : 6;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Result = ((Button)sender).Text;
            Close();
        }

        private string Result = string.Empty;
    }

    public static class GlobalMessageBoxButtons
    {
        public static List<Button> OK = new List<Button>() { new Button() { Text = "OK" } };
        public static List<Button> OKCancel = new List<Button>() { new Button() { Text = "OK" }, new Button() { Text = "Cancel" } };
        public static List<Button> YesNo = new List<Button>() { new Button() { Text = "Yes" }, new Button() { Text = "No" } };
    }



}

