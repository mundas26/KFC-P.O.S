using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace KFC_POS_
{
    public partial class Main : Form
    {
        //Para ito sa SQLCONNECTION 
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;

        private PictureBox Pic;
        private Label Price;
        private Label Discription;
        private Button Button;

        //Para ito sa kung ilang mins kana nag-take ng orders
        int sec;
        int mins;
        int hour;

        private bool PanelExtend;
        private bool ExpandTakingOrder;

        private const int VerticalStep = 40;

        public Main()
        {
            InitializeComponent();
            cn = new SqlConnection(dbconstring.connection);
        }

        private void Main_Resize(object sender, EventArgs e)
        {
            int y = Screen.PrimaryScreen.Bounds.Height;
            int x = Screen.PrimaryScreen.Bounds.Width;
            this.Height = y - 40;
            this.Width = x;
            this.Left = 0;
            this.Top = 0;
        }
        private void GetData() 
        {
            cn.Open();
            cm = new SqlCommand("Select image, price, discription from tblProduct", cn);
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                long len = dr.GetBytes(0, 0, null, 0, 0);
                byte[] array = new byte[System.Convert.ToInt32(len)+1];
                dr.GetBytes(0, 0, array, 0, System.Convert.ToInt32(len));
                
                    Pic = new PictureBox();
                    Pic.Width = 140;
                    Pic.Height = 120;
                    Pic.BorderStyle = BorderStyle.FixedSingle;
                    Pic.BackgroundImageLayout = ImageLayout.Stretch;

                Price = new Label();
                Price.Text = dr["price"].ToString();
                Price.Width = 50;
                Price.BackColor = Color.FromArgb(45, 52, 54);
                Price.ForeColor = Color.White;
                Price.TextAlign = ContentAlignment.MiddleLeft;

                Discription = new Label();
                Discription.Text = dr["discription"].ToString();
                Discription.TextAlign = ContentAlignment.MiddleRight;
                Discription.Height = 40;
                Discription.BackColor = Color.FromArgb(45, 52, 54);
                Discription.ForeColor = Color.White;
                Discription.Dock = DockStyle.Bottom;

                //Button = new Button();
                //Button.Width = 59;
                //Button.Height = 40;
                //Button.Dock = DockStyle.Bottom;

                MemoryStream ms = new MemoryStream(array);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(ms);
                Pic.BackgroundImage = bitmap;

                flpMain.Controls.Add(Pic);
                //flpMain.Controls.Add(Button);
                Pic.Controls.Add(Price);
                Pic.Controls.Add(Discription);

            }
            dr.Close();
            cn.Close();
        }
        private void Main_Load(object sender, EventArgs e)
        {
            GetData();
            tmrCounter.Start();
            lblDateNow.Text = DateTime.Now.ToLongDateString();
            lblTimeNow.Text = DateTime.Now.ToLongTimeString();
            lblBusinessDate.Text = DateTime.Today.AddDays(1).ToLongDateString();
        }

        private void PanelExtendedTimer_Tick(object sender, EventArgs e)
        {
            ShowMenu();
        }

        private void tmrCounter_Tick(object sender, EventArgs e)
        {
            sec++;
            if (sec> 59)
            {
                mins++;
                sec = 0;
            }
            if (mins> 59)
            {
                hour++;
                mins = 0;
            }
            lblHours.Text = appendZero(hour);
            lblMinutes.Text = appendZero(mins);
            lblSeconds.Text = appendZero(sec);
            if (mins == 2)
            {
                pnlRunningTime.BackColor = Color.Red;
            }
        }
        private string appendZero(double str)
        {
            if (str <= 9)
                return "0" + str;
            else
                return str.ToString();
        }

        private void btnDineInTakeOut_Click(object sender, EventArgs e)
        {
            TakingOrderTimer.Start();
        }

        private void TakingOrderTimer_Tick(object sender, EventArgs e)
        {
            TakeOrder();
        }

        private void BtnBarMenu_Click(object sender, EventArgs e)
        {
            PanelMenuExtendedTimer.Start();
        }

        private void BtnPanelHide_Click(object sender, EventArgs e)
        {
            HideMenu();
        }

        private void BtnExitPos_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want close this Application?",var._title,MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void BtnSyncProduct_Click(object sender, EventArgs e)
        {
            HideMenu();
            FrmProductList ProductList = new FrmProductList();
            ProductList.ShowDialog();
        }

        //Code ito para i-Hide ang Panel Menu..
        public void HideMenu()
        {
            PanelMenuExtendedTimer.Start();
            PanelSlider.Width -= 50;
            if (PanelSlider.Size == PanelSlider.MinimumSize)
            {
                PanelMenuExtendedTimer.Stop();
                PanelExtend = true;
                this.Refresh();
            }
        }

        //Code ito para i-Show ang Panel Menu..
        public void ShowMenu()
        {
            if (PanelExtend)
            {
                PanelSlider.Width += 50;
                if (PanelSlider.Size == PanelSlider.MaximumSize)
                {
                    PanelMenuExtendedTimer.Stop();
                    PanelExtend = false;
                }
            }
            else
            {
                PanelSlider.Width -= 50;
                if (PanelSlider.Size == PanelSlider.MinimumSize)
                {
                    PanelMenuExtendedTimer.Stop();
                    PanelExtend = true;
                }
            }
        }

        public void BtnOrder_Click(object sender, EventArgs e)
        {
            HideMenu();
        }

        //Ito ay sa Dine/TakeOut/Etc..
        public void TakeOrder()
        {
            if (ExpandTakingOrder)
            {
                DropdownTakingOrder.Width += 30;
                DropdownTakingOrder.Height += 30;
                if (DropdownTakingOrder.Size == DropdownTakingOrder.MaximumSize)
                {
                    TakingOrderTimer.Stop();
                    ExpandTakingOrder = false;
                }
            }
            else
            {
                DropdownTakingOrder.Width -= 30;
                DropdownTakingOrder.Height -= 30;
                if (DropdownTakingOrder.Size == DropdownTakingOrder.MinimumSize)
                {
                    TakingOrderTimer.Stop();
                    ExpandTakingOrder = true;
                }
            }
        }

        private void BtnScrollUP_Click(object sender, EventArgs e)
        {
            //flpMain.Top -= VerticalStep;
        }

        private void BtnScrollDown_Click(object sender, EventArgs e)
        {
            //flpMain.Top += VerticalStep;

        }
    }
}
