using System;
using MySQLData;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Types;
using System.IO;

namespace jsu1xd
{
    public partial class OperatorRequestForm : Form
    {
        string strPath = System.Environment.GetEnvironmentVariable("TEMP");
        public int UserId { get; set; }
        public OperatorRequestForm()
        {
            InitializeComponent();

            string path = Path.Combine(strPath, "jsu1xd.tmp");
            using (var sr = new StreamReader(path))
            {
                UserId = Convert.ToInt32(sr.ReadToEnd());
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            int requestId = Convert.ToInt32((guna2ComboBox1.SelectedItem as ComboboxItem).Value);

            var dbCon = DBConnection.Instance();     
            if (dbCon.IsConnect())
            {
                string query = $"SELECT info, comment FROM requests WHERE id={requestId}";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var result = cmd.ExecuteReader();
                while (result.Read())
                {
                    MessageBox.Show($"Текст обращения: {result.GetString(0)}\nКомментарий: {result.GetString(1)}");
                }
                dbCon.Close();
            }
            else
            {
                MessageBox.Show("Проверьте подключение к сети");
            }
        }

        private void OperatorRequestForm_Load(object sender, EventArgs e)
        {
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                string query = $"SELECT COUNT(*) FROM contacts";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                int counter = Convert.ToInt32(cmd.ExecuteScalar());
                if (counter == 0)
                {
                    MessageBox.Show("Сначала требуется добавить контакты");
                    dbCon.Close();
                    this.BeginInvoke(new MethodInvoker(this.Close));
                    return;
                }

                query = $"SELECT COUNT(*) FROM requests WHERE status='WAIT'";
                cmd = new MySqlCommand(query, dbCon.Connection);
                counter = Convert.ToInt32(cmd.ExecuteScalar());
                if (counter == 0)
                {
                    MessageBox.Show("Простите, но нет заявок");
                    dbCon.Close();
                    this.BeginInvoke(new MethodInvoker(this.Close));
                    return;
                }

                query = $"SELECT id, info FROM requests WHERE status='WAIT'";
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

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                string query = $"SELECT COUNT(*) FROM contacts";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                int counter = Convert.ToInt32(cmd.ExecuteScalar());
                if (counter == 0)
                {
                    MessageBox.Show("Простите, но нет контактов");
                    dbCon.Close();
                    this.Hide();
                    return;
                }

                query = $"SELECT id, fio FROM contacts";
                cmd = new MySqlCommand(query, dbCon.Connection);
                var result = cmd.ExecuteReader();
                int i = 0;
                while (result.Read())
                {
                    Console.WriteLine(result.GetString(1));
                    ComboboxItem item = new ComboboxItem();
                    item.Text = result.GetString(1);
                    item.Value = result.GetInt32(0);
                    guna2ComboBox2.Items.Add(item);
                    i++;
                }
                dbCon.Close();
            }
            guna2Button1.Visible = true;
            guna2Button2.Visible = true;
            guna2ComboBox2.Visible = true;
            guna2HtmlLabel2.Visible = true;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            int requestId = Convert.ToInt32((guna2ComboBox1.SelectedItem as ComboboxItem).Value);
            int contactId = Convert.ToInt32((guna2ComboBox2.SelectedItem as ComboboxItem).Value);

            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                string query = $"UPDATE requests SET contact={contactId}, operator={UserId}, status='RES' WHERE id={requestId}";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                cmd.ExecuteNonQuery();
                dbCon.Close();
                MessageBox.Show("Данные были предоставлены");
                this.Close();
            }
        }
    }
}
