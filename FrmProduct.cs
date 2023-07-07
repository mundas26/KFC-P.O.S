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
    public partial class FrmProduct : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        FrmProductList f;
        public string _id;
        public FrmProduct(FrmProductList f)
        {
            InitializeComponent();
            cn = new SqlConnection(dbconstring.connection);
            this.f = f;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do you want to save this record?", var._title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    MemoryStream ms = new MemoryStream();
                    pictureBox1.BackgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    pictureBox1.BackgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] arrImage = ms.GetBuffer();
                    cn.Open();
                    cm = new SqlCommand("Insert into tblProduct (discription, price, category, image) values (@discription, @price, @category, @image)", cn);
                    cm.Parameters.AddWithValue("@discription", TxtDiscription.Text);
                    cm.Parameters.AddWithValue("@price", double.Parse(TxtPrice.Text));
                    cm.Parameters.AddWithValue("@category", TxtCategory.Text);
                    cm.Parameters.AddWithValue("@image", arrImage);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    f.LoadRecordAllProduct();
                    MessageBox.Show("Record has been successfully saved!");
                    Clear();
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, var._title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        public void Clear()
        {
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            TxtDiscription.Clear();
            TxtPrice.Clear();
            TxtCategory.Clear();
            TxtDiscription.Focus();
            pictureBox1.BackgroundImage = System.Drawing.Image.FromFile(Application.StartupPath + @"\Chicken.png");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Do you want to update this record?", var._title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    MemoryStream ms = new MemoryStream();
                    pictureBox1.BackgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    pictureBox1.BackgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] arrImage = ms.GetBuffer();

                    cn.Open();
                    cm = new SqlCommand("Update tblProduct set discription=@discription, price=@price, category=@category, image=@image where id=@id", cn);
                    cm.Parameters.AddWithValue("@discription", TxtDiscription.Text);
                    cm.Parameters.AddWithValue("@price", double.Parse(TxtPrice.Text));
                    cm.Parameters.AddWithValue("@category", TxtCategory.Text);
                    cm.Parameters.AddWithValue("@image", arrImage);
                    cm.Parameters.AddWithValue("@id", _id);
                    cm.ExecuteNonQuery();
                    cn.Close();
                    MessageBox.Show("Record has been successfully Updated!", var._title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                    f.LoadRecordAllProduct();
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, var._title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
