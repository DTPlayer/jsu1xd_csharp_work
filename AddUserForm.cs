using System;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySQLData;

namespace jsu1xd
{
    public partial class AddUserForm : Form
    {
        string strPath = System.Environment.GetEnvironmentVariable("TEMP");
        public AddUserForm()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text == "")
            {
                MessageBox.Show("Введите известуню вам информацию");
                return;
            }

            guna2Button1.Enabled = false;

            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {

                string info = guna2TextBox1.Text;
                string comment;
                if (guna2TextBox2.Text == "")
                {
                    comment = "Не указан";
                } else
                {
                    comment = guna2TextBox2.Text;
                }

                string query;

                string path = Path.Combine(strPath, "jsu1xd.tmp");
                using (var sr = new StreamReader(path))
                {
                    int user_id = Convert.ToInt32(sr.ReadToEnd());
                    query = "INSERT INTO `requests` (`user`, `info`, `comment`, `status`)" +
                    $"VALUES ({user_id}, '{info}', '{comment}', 'WAIT')";
                }

                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Заявка оставлена успешно, ожидайте ее расмотрения!");

                dbCon.Close();

                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка подключения. Проверьте свое соединение с интернетом и попробуйте позднее");
                guna2Button1.Enabled = true;
            }

        }
    }
}
