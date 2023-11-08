using System;
using MySQLData;
using MySql.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace jsu1xd
{
    public partial class AddContactForm : Form
    {
        public AddContactForm()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text != "")
            {
                string address, phone, email;
                string fio = guna2TextBox1.Text; 

                if (guna2TextBox2.Text == "")
                {
                    address = "Не указан";
                } else
                {
                    address = guna2TextBox2.Text;
                }

                if (guna2TextBox3.Text == "")
                {
                    phone = "Не указан";
                }
                else
                {
                    phone = guna2TextBox3.Text;
                }

                if (guna2TextBox4.Text == "")
                {
                    email = "Не указан";
                }
                else
                {
                    email = guna2TextBox4.Text;
                }

                var dbCon = DBConnection.Instance();
                if (dbCon.IsConnect())
                {
                    string query = "INSERT INTO contacts (`fio`, `address`, `phone`, `email`) " +
                    $"VALUES ('{fio}', '{address}', '{phone}', '{email}')";
                    var cmd = new MySqlCommand(query, dbCon.Connection);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Контакт добавлен успешно!");
                    dbCon.Close();
                    this.Close();
                } else
                {
                    MessageBox.Show("Проверьте соединение с сетью");
                    this.Hide();
                }
            } else
            {
                MessageBox.Show("Заполните поле ФИО");
            }
        }
    }
}
