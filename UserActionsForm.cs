using MySql.Data.MySqlClient;
using System;
using MySQLData;
using System.Windows.Forms;
using System.IO;
using Types;
using Microsoft.Toolkit.Uwp.Notifications;

namespace jsu1xd
{
    public partial class UserActionsForm : Form
    {
        string strPath = System.Environment.GetEnvironmentVariable("TEMP");
        public int UserId { get; set; }
        public UserActionsForm()
        {
            InitializeComponent();
            string path = Path.Combine(strPath, "jsu1xd.tmp");
            using (var sr = new StreamReader(path))
            {
                UserId = Convert.ToInt32(sr.ReadToEnd());
            }
        }

        private void UserActionsForm_Shown(object sender, EventArgs e)
        {
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                string query = $"SELECT COUNT(*) FROM requests WHERE user={UserId} AND status='RES'";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                int counter = Convert.ToInt32(cmd.ExecuteScalar());
                if (counter == 0) {
                    MessageBox.Show("Простите, но нет рассмотренных заявок");
                    dbCon.Close();
                    this.Hide();
                    return;
                }

                query = $"SELECT id, info FROM requests WHERE user={UserId} AND status='RES'";
                cmd = new MySqlCommand(query, dbCon.Connection);
                var result = cmd.ExecuteReader();
                int i = 0;
                while (result.Read())
                {
                    ComboboxItem item = new ComboboxItem();
                    item.Text = result.GetString(1);
                    item.Value = result.GetInt32(0);
                    guna2ComboBox1.Items.Add(item);
                    i++;
                }
                dbCon.Close();
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            int requestId = Convert.ToInt32((guna2ComboBox1.SelectedItem as ComboboxItem).Value.ToString());

            var dbcon = DBConnection.Instance();
            if (dbcon.IsConnect())
            {
                string query = "SELECT contacts.fio, contacts.address, contacts.phone, contacts.email FROM contacts JOIN " +
                    "requests ON contacts.id = requests.contact " +
                    $"WHERE requests.id = {requestId}";
                var cmd = new MySqlCommand (query, dbcon.Connection);
                var result = cmd.ExecuteReader();
                while (result.Read())
                {
                    MessageBox.Show($"ФИО: {result.GetString(0)}\n" +
                        $"Адрес: {result.GetString(1)}\n" +
                        $"Номер телефона: {result.GetString(2)}\n" +
                        $"Почта: {result.GetString(3)}");
                }
                dbcon.Close();
            } else
            {
                MessageBox.Show("Проверьте соединение с интернетом");
                this.Hide();
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            int requestId = Convert.ToInt32((guna2ComboBox1.SelectedItem as ComboboxItem).Value.ToString());

            var dbcon = DBConnection.Instance();
            if (dbcon.IsConnect())
            {
                string query = "SELECT contacts.fio, contacts.address, contacts.phone, contacts.email FROM contacts " +
                    $"JOIN requests ON contacts.id = requests.contact WHERE requests.id = {requestId}";
                var cmd = new MySqlCommand(query, dbcon.Connection);
                var result = cmd.ExecuteReader();
                while (result.Read())
                {
                    Clipboard.SetText($"ФИО: {result.GetString(0)}\n" +
                        $"Адрес: {result.GetString(1)}\n" +
                        $"Номер телефона: {result.GetString(2)}\n" +
                        $"Почта: {result.GetString(3)}");
                }
                dbcon.Close();
                new ToastContentBuilder()
                .AddText("Данные скопированны")
                .AddText("Данные были успешно скопированны")
                .Show();
            }
            else
            {
                MessageBox.Show("Проверьте соединение с интернетом");
                this.Hide();
            }
        }
    }
}
