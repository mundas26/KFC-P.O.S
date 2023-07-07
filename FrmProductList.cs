using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace KFC_POS_
{
    public partial class FrmProductList : Form
    {
        SqlConnection cn;
        SqlCommand cm;
        SqlDataReader dr;
        public string _id;
        public FrmProductList()
        {
            InitializeComponent();
            cn = new SqlConnection(dbconstring.connection);
        }
        public void LoadRecordAllProduct()
        {
            try
            {
                ViewProduct.Rows.Clear();
                cn.Open();
                cm = new SqlCommand("Select *from tblProduct", cn);
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    ViewProduct.Rows.Add(dr["id"].ToString(), dr["discription"].ToString(), dr["price"].ToString(), dr["category"].ToString());
                }
                dr.Close();
                cn.Close();
                ViewProduct.ClearSelection();
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, var._title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FrmProductList_Load(object sender, EventArgs e)
        {
            LoadRecordAllProduct();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FrmProduct f = new FrmProduct(this);
            f.ShowDialog();
        }

        private void ViewProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string colname = ViewProduct.Columns[e.ColumnIndex].Name;
                if (colname == "btnEdit")
                {
                    FrmProduct Product = new FrmProduct(this);
                    cn.Open();
                    cm = new SqlCommand("Select image as Picture, *from tblProduct where id like '" + ViewProduct.Rows[e.RowIndex].Cells[0].Value.ToString() + "'", cn);
                    dr = cm.ExecuteReader();
                    dr.Read();
                    if (dr.HasRows)
                    {
                        long len = dr.GetBytes(0, 0, null, 0, 0);
                        byte[] array = new byte[System.Convert.ToInt32(len) + 1];
                        dr.GetBytes(0, 0, array, 0, System.Convert.ToInt32(len));

                        //Product.lblID.Text = dr["id"].ToString();
                        //Product.lblDiscription.Text = dr["discription"].ToString();
                        //Product.lblPrice.Text = dr["price"].ToString();
                        //Product.lblCategory.Text = dr["category"].ToString();

                        Product._id = dr["id"].ToString();
                        Product.TxtDiscription.Text = dr["discription"].ToString();
                        Product.TxtPrice.Text = dr["price"].ToString();
                        Product.TxtCategory.Text = dr["category"].ToString();

                        MemoryStream ms = new MemoryStream(array);
                        Bitmap bitmap = new Bitmap(ms);
                        Product.pictureBox1.BackgroundImage = bitmap;
                    }
                    Product.btnSave.Enabled = false;
                    dr.Close();
                    cn.Close();
                    Product.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                cn.Close();
                MessageBox.Show(ex.Message, var._title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            //try
            //{
            //    string colname = ViewProduct.Columns[e.ColumnIndex].Name;
            //    if (colname == "btnEdit")
            //    {
            //        cn.Open();
            //        cm = new SqlCommand("Select image as picture, *from tblProduct where id like '" + ViewProduct.Rows[e.RowIndex].Cells[0].Value.ToString() + "'", cn);
            //        dr = cm.ExecuteReader();
            //        dr.Read();
            //        if (dr.HasRows)
            //        {
            //            long len = dr.GetBytes(0, 0, null, 0, 0);
            //            byte[] array = new byte[System.Convert.ToInt32(len) + 1];
            //            dr.GetBytes(0, 0, array, 0, System.Convert.ToInt32(len));

            //            _id = dr["id"].ToString();
            //            txtDiscription.Text = dr["discription"].ToString();
            //            txtPrice.Text = dr["price"].ToString();
            //            txtCategory.Text = dr["category"].ToString();

            //            MemoryStream ms = new MemoryStream(array);
            //            Bitmap bitmap = new Bitmap(ms);
            //            PicImage.BackgroundImage = bitmap;
            //        }
            //        btnSave.Enabled = false;
            //        dr.Close();
            //        cn.Close();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    cn.Close();
            //    MessageBox.Show(ex.Message, var._title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (MessageBox.Show("Do you want to save this record?", var._title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //    {
            //        MemoryStream ms = new MemoryStream();
            //        PicImage.BackgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            //        PicImage.BackgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            //        byte[] arrImage = ms.GetBuffer();
            //        cn.Open();
            //        cm = new SqlCommand("Insert into tblProduct (discription, price, category, image) values (@discription, @price, @category, @image)", cn);
            //        cm.Parameters.AddWithValue("@discription", txtDiscription.Text);
            //        cm.Parameters.AddWithValue("@price", double.Parse(txtPrice.Text));
            //        cm.Parameters.AddWithValue("@category", txtCategory.Text);
            //        cm.Parameters.AddWithValue("@image", arrImage);
            //        cm.ExecuteNonQuery();
            //        cn.Close();
            //        LoadRecordAllProduct();
            //        MessageBox.Show("Record has been successfully saved!");
            //        Clear();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    cn.Close();
            //    MessageBox.Show(ex.Message, var._title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
        }
        public void Clear()
        {
            //txtDiscription.Clear();
            //txtCategory.Clear();
            //txtPrice.Clear();
            //txtDiscription.Focus();
            //PicImage.BackgroundImage = System.Drawing.Image.FromFile(Application.StartupPath + @"\Chicken.png");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    if (MessageBox.Show("Do you want to update this record?", var._title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //    {
            //        MemoryStream ms = new MemoryStream();
            //        PicImage.BackgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            //        PicImage.BackgroundImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            //        byte[] arrImage = ms.GetBuffer();

            //        cn.Open();
            //        cm = new SqlCommand("Update tblProduct set discription=@discription, price=@price, category=@category, image=@image where id=@id", cn);
            //        cm.Parameters.AddWithValue("@discription", txtDiscription.Text);
            //        cm.Parameters.AddWithValue("@price", double.Parse(txtPrice.Text));
            //        cm.Parameters.AddWithValue("@category", txtCategory.Text);
            //        cm.Parameters.AddWithValue("@image", arrImage);
            //        cm.Parameters.AddWithValue("@id", _id);
            //        cm.ExecuteNonQuery();
            //        cn.Close();
            //        MessageBox.Show("Record has been successfully Updated!", var._title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        Clear();
            //        LoadRecordAllProduct();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    cn.Close();
            //    MessageBox.Show(ex.Message, var._title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //}
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            FrmProduct Product = new FrmProduct(this);
            Product.Clear();
            Product.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
