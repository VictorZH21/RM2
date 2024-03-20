using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace RM2._1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "сотрудники_предприятияDataSet.Сотрудники1". При необходимости она может быть перемещена или удалена.
            this.сотрудники1TableAdapter.Fill(this.сотрудники_предприятияDataSet.Сотрудники1);
            
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].HeaderText = "Названия предпрития";
            dataGridView1.Columns[1].Width = 150;
            dataGridView1.Columns[2].HeaderText = "ФИО сотрудника";
            dataGridView1.Columns[2].Width = 180;
            dataGridView1.Columns[3].HeaderText = "Должность";
            dataGridView1.Columns[3].Width = 200;
            dataGridView1.Columns[4].HeaderText = "Зарплата";
            dataGridView1.Columns[4].Width = 120;
            dataGridView1.Columns[5].HeaderText = "Фото";
            dataGridView1.Columns[5].Width = 100;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            if (f.ShowDialog() == DialogResult.OK)
            {
                string photoPath = "0.jpg";
                if (!string.IsNullOrWhiteSpace(f.label5.Text))
                {
                    photoPath = f.label5.Text;
                }
                string sql = "INSERT INTO[dbo].[Сотрудники1] " +
                    " ([Название_предприятия]" +
                    ",[ФИО_сотрудников]" +
                    ",[Должность] " +
                    ",[Зарплата] " +
                    ",[Фото])" +
                    "VALUES" +
                    "('" + f.textBox1.Text + "', '" + f.textBox2.Text + "', '" + f.textBox3.Text + "'" +
                    "," + f.numericUpDown1.Value + ", '" + photoPath + "')";

                string connectionString = "Data Source=WIN-JO0SOU9O4VS\\SQLEXPRESS;Initial Catalog=Сотрудники_предприятия;Integrated Security=True";
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

                this.сотрудники1TableAdapter.Fill(this.сотрудники_предприятияDataSet.Сотрудники1);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                int n = dataGridView1.CurrentCell.RowIndex;
                int id = Convert.ToInt32(dataGridView1[0, n].Value);
                string sql = "DELETE [dbo].[Сотрудники1] where [ID] =" + id;

                string connectionString = "Data Source=WIN-JO0SOU9O4VS\\SQLEXPRESS;Initial Catalog=Сотрудники_предприятия;Integrated Security=True";
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                sqlCommand.ExecuteNonQuery();
                sqlConnection.Close();

                this.сотрудники1TableAdapter.Fill(this.сотрудники_предприятияDataSet.Сотрудники1);
            }
            else
            {
                MessageBox.Show("Не выбрали данные для удаления");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell != null)
            {
                int n = dataGridView1.CurrentCell.RowIndex;
                int id = Convert.ToInt32(dataGridView1[0, n].Value);
                Form2 f = new Form2();
                f.textBox1.Text = dataGridView1[1, n].Value.ToString();
                f.textBox2.Text = dataGridView1[2, n].Value.ToString();
                f.textBox3.Text = dataGridView1[3, n].Value.ToString();
                f.numericUpDown1.Value = Convert.ToInt32(dataGridView1[4, n].Value);
                f.label5.Text = dataGridView1[5, n].Value.ToString();

                if (dataGridView1[5, n].Value.ToString().Trim() != "")
                {
                    f.pictureBox1.Load("Фото/" + dataGridView1[5, n].Value.ToString());
                }
                else
                {
                    pictureBox1.Load("Фото/0.jpg");
                }

                if (f.ShowDialog() == DialogResult.OK)
                {
                    if (dataGridView1.CurrentCell != null)

                    {
                        string sql = "UPDATE [dbo].[Сотрудники1]" +
                            "SET [Название_предприятия] = '" + f.textBox1.Text + "'" +
                            ",[ФИО_сотрудников] = '" + f.textBox2.Text + "'" +
                            ",[Должность] = '" + f.textBox3.Text + "'" +
                            ",[Зарплата] = '" + f.numericUpDown1.Value + "'" +
                            ",[Фото] = '" + f.label5.Text + "'" +
                            "WHERE id =" + id;

                        string connectionString = "Data Source=WIN-JO0SOU9O4VS\\SQLEXPRESS;Initial Catalog=Сотрудники_предприятия;Integrated Security=True";
                        SqlConnection sqlConnection = new SqlConnection(connectionString);
                        sqlConnection.Open();
                        SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                        sqlCommand.ExecuteNonQuery();
                        sqlConnection.Close();

                        this.сотрудники1TableAdapter.Fill(this.сотрудники_предприятияDataSet.Сотрудники1);
                    }
                }
                else
                {
                    MessageBox.Show("Не выбрали данные для редактирования");

                }
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                numericUpDown1.Visible = true;
                numericUpDown2.Visible = true;
                textBox1.Visible = false;
            }
            else
            {
                numericUpDown1.Visible = false;
                numericUpDown2.Visible = false;
                textBox1.Visible = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                string sql = "SELECT * from [dbo].[Сотрудники1] where [Должность] Like '%" + textBox1.Text+ "%'";

                string connectionString = "Data Source=WIN-JO0SOU9O4VS\\SQLEXPRESS;Initial Catalog=Сотрудники_предприятия;Integrated Security=True";
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sql, sqlConnection);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];

                sqlConnection.Close();
            }
    }

        private void button5_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * from [dbo].[Сотрудники1]";

            string connectionString = "Data Source=WIN-JO0SOU9O4VS\\SQLEXPRESS;Initial Catalog=Сотрудники_предприятия;Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connectionString);// открыли подключение к бд
            sqlConnection.Open();
            //SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
            SqlDataAdapter adapter = new SqlDataAdapter(sql, sqlConnection);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];

            sqlConnection.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int n = dataGridView1.CurrentCell.RowIndex;
            string photoPath = dataGridView1[5, n].Value.ToString().Trim(); 

            if (!string.IsNullOrWhiteSpace(photoPath))
            {
                try
                {
                    pictureBox1.Load("Фото/" + photoPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при загрузки фотографии:" + ex.Message);
                }
            }
            else
            {
                pictureBox1.Image = Image.FromFile("Фото/0.jpg");
            }
        }
    }
}
