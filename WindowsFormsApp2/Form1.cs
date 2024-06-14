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

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        int cR, cG, cB;
        public Form1()
        {
            InitializeComponent();
            Form1_Load(this, EventArgs.Empty);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            Bitmap bmp = new Bitmap(openFileDialog1.FileName);
            pictureBox1.Image = bmp;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            int R = int.Parse(textBox1.Text);
            int G = int.Parse(textBox2.Text);
            int B = int.Parse(textBox3.Text);
            string textura = textBox4.Text;
            string color = textBox5.Text;

            string connectionString = "Data Source=LAPTOP-CIPD7E2U; Initial Catalog=examen; Integrated Security=true;";
            string query = "INSERT INTO puntos (textura, color, R, G, B) VALUES (@textura, @color, @R, @G, @B)";

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@textura", textura);
                    cmd.Parameters.AddWithValue("@color", color);
                    cmd.Parameters.AddWithValue("@R", R);
                    cmd.Parameters.AddWithValue("@G", G);
                    cmd.Parameters.AddWithValue("@B", B);

                    try
                    {
                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Datos guardados exitosamente.");
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al guardar los datos: " + ex.Message);
                    }
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseClick_1(object sender, MouseEventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Image);
            Color c = new Color();
            int x, y, mR = 0, mG = 0, mB = 0;
            x = e.X; y = e.Y;
            for (int i = x; i < x + 10; i++)
                for (int j = y; j < y + 10; j++)
                {
                    c = bmp.GetPixel(i, j);
                    mR = mR + c.R;
                    mG = mG + c.G;
                    mB = mB + c.B;
                }
            mR = mR / 100;
            mG = mG / 100;
            mB = mB / 100;
            cR = mR;
            cG = mG;
            cB = mB;
            textBox1.Text = cR.ToString();
            textBox2.Text = cG.ToString();
            textBox3.Text = cB.ToString();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            Bitmap bmp2 = new Bitmap(openFileDialog1.FileName);
            pictureBox2.Image = bmp2;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private DataTable GetData()
        {
            string connectionString = "Data Source=LAPTOP-CIPD7E2U; Initial Catalog=examen; Integrated Security=true;";

            string query = "SELECT * FROM puntos";
            DataTable dataTable = new DataTable();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    try
                    {
                        con.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error al obtener los datos: " + ex.Message);
                    }
                }
            }

            return dataTable;
        }

        private void button3_Click(object sender, EventArgs e)
        {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = "Data Source=LAPTOP-CIPD7E2U; Initial Catalog=examen; Integrated Security=true;";
                string query = "SELECT * FROM puntos";
                SqlCommand comando = new SqlCommand(query, con);
                SqlDataAdapter data = new SqlDataAdapter(comando);
                DataTable tabla = new DataTable();
                con.Open();
                data.Fill(tabla);
                con.Close();

                Bitmap bmp2 = new Bitmap(pictureBox2.Image);
                Bitmap cpoa2 = new Bitmap(bmp2.Width, bmp2.Height);
                HashSet<string> uniqueMessages = new HashSet<string>();

                for (int i = 0; i < bmp2.Width; i += 10)
                {
                    for (int j = 0; j < bmp2.Height; j += 10)
                    {
                        int totalR = 0, totalG = 0, totalB = 0;
                        for (int k = i; k < i + 10 && k < bmp2.Width; k++)
                        {
                            for (int l = j; l < j + 10 && l < bmp2.Height; l++)
                            {
                                Color pixelColor = bmp2.GetPixel(k, l);
                                totalR += pixelColor.R;
                                totalG += pixelColor.G;
                                totalB += pixelColor.B;
                            }
                        }
                        int avgR = totalR / 100;
                        int avgG = totalG / 100;
                        int avgB = totalB / 100;
                        bool colorChanged = false;
                        for (int row = 0; row < tabla.Rows.Count; row++)
                        {
                            int dbR, dbG, dbB;
                            if (int.TryParse(tabla.Rows[row]["R"].ToString(), out dbR) &&
                                int.TryParse(tabla.Rows[row]["G"].ToString(), out dbG) &&
                                int.TryParse(tabla.Rows[row]["B"].ToString(), out dbB))
                            {
                                if (Math.Abs(avgR - dbR) <= 10 && Math.Abs(avgG - dbG) <= 10 && Math.Abs(avgB - dbB) <= 10)
                                {
                                    string colorName = tabla.Rows[row]["color"].ToString().Trim();
                                    Color replaceColor = Color.FromName(colorName);

                                    for (int x = i; x < i + 10 && x < bmp2.Width; x++)
                                    {
                                        for (int y = j; y < j + 10 && y < bmp2.Height; y++)
                                        {
                                            cpoa2.SetPixel(x, y, replaceColor);
                                        }
                                    }
                                    colorChanged = true;

                                    string textura = tabla.Rows[row]["textura"].ToString();
                                    uniqueMessages.Add($"Textura '{textura}' con RGB ({dbR}, {dbG}, {dbB}) cambiada a color {colorName}");

                                    break;
                                }
                            }
                            else
                            {

                            }
                        }

                        if (!colorChanged)
                        {
                            for (int x = i; x < i + 10 && x < bmp2.Width; x++)
                            {
                                for (int y = j; y < j + 10 && y < bmp2.Height; y++)
                                {
                                    cpoa2.SetPixel(x, y, bmp2.GetPixel(x, y));
                                }
                            }
                        }
                    }
                }

                pictureBox2.Image = cpoa2;
                if (uniqueMessages.Count > 0)
                {
                    string resultMessage = string.Join(Environment.NewLine, uniqueMessages);
                    MessageBox.Show(resultMessage, "Texturas y colores cambiados");
                }
                else
                {
                    MessageBox.Show("No se encontraron coincidencias de RGB para cambiar.", "Resultado");
                }

        }

        private void LoadData()
        {
            DataTable dataTable = GetData();
            dataGridView1.DataSource = dataTable;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }



    }
}
