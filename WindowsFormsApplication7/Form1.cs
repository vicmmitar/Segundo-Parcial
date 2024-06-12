using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication7
{

    public partial class Form1 : Form
    {
        int cR, cG, cB;

        public Form1()
        {
            InitializeComponent();
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "archivos jpg|*.jpg";
            openFileDialog1.ShowDialog();
            Bitmap bmp = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = bmp;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            /*Bitmap bmp = new Bitmap(pictureBox1.Image);
            Color c = new Color();
            c = bmp.GetPixel(e.X, e.Y);
            textBox1.Text = c.R.ToString();
            textBox2.Text = c.G.ToString();
            textBox3.Text = c.B.ToString();
             */
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Color c = new Color();
            int sR, sG, sB;
            sR = 0;
            sG = 0;
            sB = 0;
            for (int i = e.X; i < e.X + 10;i++)
                for (int j = e.Y; j < e.Y + 10; j++)
                { 
                    c = bmp.GetPixel(i, j);
                    sR = sR + c.R;
                    sG = sG + c.G;
                    sB = sB + c.B;
                }
            sR = sR/100;
            sG = sG/100;
            sB = sB/100;
            cR = sR;
            cG = sG;
            cB = sB;
            textBox1.Text = sR.ToString();
            textBox2.Text = sG.ToString();
            textBox3.Text = sB.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height);
            Color c = new Color();
            for (int i=0;i<bmp.Width;i++)
                for (int j = 0; j < bmp.Height; j++)
                {
                    c = bmp.GetPixel(i, j);
                    if (((230 <= c.R) && (c.R <= 240)) && ((185 <= c.G) && (c.G <= 195)) && ((0 <= c.B) && (c.B <= 10)))
                        bmp2.SetPixel(i, j, Color.Black);
                    else
                        bmp2.SetPixel(i, j, Color.FromArgb(c.R, c.G, c.B));
                }
            pictureBox1.Image = bmp2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Bitmap bmp2 = new Bitmap(bmp.Width, bmp.Height);
            Color c = new Color();
            int sR, sG, sB;
            for (int i = 0; i < bmp.Width-10; i=i+10)
                for (int j = 0; j < bmp.Height-10; j=j+10)
                {
                    sR = 0; sG = 0; sB = 0;
                    for (int ip = i; ip < i + 10; ip++)
                        for (int jp = j; jp < j + 10; jp++)
                        {
                            c = bmp.GetPixel(ip, jp);
                            sR = sR + c.R;
                            sG = sG + c.G;
                            sB = sB + c.B;
                        }
                    sR = sR / 100;
                    sG = sG / 100;
                    sB = sB / 100;

                    if (tablaDatos.SelectedRows.Count > 0)
                    {
                        int precision = 20;
                        int R = Int16.Parse(tablaDatos.CurrentRow.Cells["cR"].Value.ToString());
                        int G = Int16.Parse(tablaDatos.CurrentRow.Cells["cG"].Value.ToString());
                        int B = Int16.Parse(tablaDatos.CurrentRow.Cells["cB"].Value.ToString());
                        String Col = tablaDatos.CurrentRow.Cells["colorpintar"].Value.ToString();

                        int liR = R - precision;
                        int lsR = R + precision;
                        int liG = G - precision;
                        int lsG = G + precision;
                        int liB = B - precision;
                        int lsB = B + precision;
                        if (((liR <= sR) && (sR <= lsR)) && ((liG <= sG) && (sG <= lsG)) && ((liB <= sB) && (sB <= lsB)))
                        {
                            for (int ip = i; ip < i + 10; ip++)
                                for (int jp = j; jp < j + 10; jp++)
                                {
                                    bmp2.SetPixel(ip, jp, Color.FromName(Col));
                                }
                        }
                        else
                        {
                            for (int ip = i; ip < i + 10; ip++)
                                for (int jp = j; jp < j + 10; jp++)
                                {
                                    c = bmp.GetPixel(ip, jp);
                                    bmp2.SetPixel(ip, jp, Color.FromArgb(c.R, c.G, c.B));
                                }
                        }
                    }
                    else
                        MessageBox.Show("seleccione una fila por favor");
                    pictureBox1.Image = bmp2;
                }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mostrar();
        }

        private void mostrar()
        {
            OdbcConnection con = new OdbcConnection();
            OdbcDataAdapter ada = new OdbcDataAdapter();
            con.ConnectionString = "DSN=MySql";
            ada.SelectCommand = new OdbcCommand();
            ada.SelectCommand.Connection = con;
            ada.SelectCommand.CommandText = "select * from texturas";
            DataSet ds = new DataSet();
            ada.Fill(ds);
            tablaDatos.DataSource = ds.Tables[0];
        }


        private void btnGuardar_Click(object sender, EventArgs e)
        {
            OdbcConnection con = new OdbcConnection();
            OdbcCommand cmd = new OdbcCommand();
            con.ConnectionString = "DSN=MySql";
            cmd.CommandText = "insert into texturas  (descripcion,cR,cG,cB,colorpintar) ";
            cmd.CommandText += "values ('" + textBox4.Text + "'," + textBox1.Text + "," + textBox2.Text + "," + textBox3.Text + ",'" + textBox5.Text + "')";
            cmd.CommandType = CommandType.Text;

            cmd.Connection = con;

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            mostrar();

        }
    }
}
