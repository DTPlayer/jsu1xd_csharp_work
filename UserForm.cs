using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Notifications;
using System.Windows.Forms;
using System.Threading;
using MySQLData;
using System.IO;
using MySql.Data.MySqlClient;

namespace jsu1xd
{
    public partial class UserForm : Form
    {
        string strPath = System.Environment.GetEnvironmentVariable("TEMP");
        public int UserId { get; set; }
        public int CountMessages {  get; set; }
        public void CheckStatus()
        {
            var dbCon = DBConnection.Instance();
            while (true)
            {
                if (dbCon.IsConnect())
                {
                    var connection = dbCon.openConnect();
                    try {
                        string query = $"SELECT COUNT(*) FROM requests WHERE user={UserId} AND status='RES'";
                        var cmd = new MySqlCommand(query, connection);
                        int counter = Convert.ToInt32(cmd.ExecuteScalar());
                        if (counter > CountMessages)
                        {
                            new ToastContentBuilder()
                            .AddText("Обращение решено")
                            .AddText("Ваше обращение было решено и ждет вас во вкладке 'Мои запросы'")
                            .Show();
                        }

                        CountMessages = counter;
                        string path = Path.Combine(strPath, "jsu1xdmessage.tmp");
                        using (FileStream fs = File.Create(path))
                        {
                            byte[] info = new UTF8Encoding(true).GetBytes(CountMessages.ToString());
                            fs.Write(info, 0, info.Length);

                        }
                        connection.Close();
                    } catch (Exception ex)
                    {
                        connection.Close();
                        Thread.Sleep(5000);
                    }

                }
                Thread.Sleep(1000);
            }
        }
        public UserForm()
        {
            InitializeComponent();
            string path = Path.Combine(strPath, "jsu1xd.tmp");
            using (var sr = new StreamReader(path))
            {
                UserId = Convert.ToInt32(sr.ReadToEnd());
            }

            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect()) { }
            string query = $"SELECT fio FROM users WHERE id={UserId}";
            var cmd = new MySqlCommand(query, dbCon.Connection);
            var result = cmd.ExecuteReader();
            while (result.Read())
            {
                guna2HtmlLabel2.Text = $"{result.GetString(0)}";
            }

            try 
            {
                path = Path.Combine(strPath, "jsu1xdmessage.tmp");
                using (var sr = new StreamReader(path))
                {
                    CountMessages = Convert.ToInt32(sr.ReadToEnd());
                }
            }
            catch (FileNotFoundException) { }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Form addform = new AddUserForm();
            addform.ShowDialog();
        }

        private void UserForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private async void UserForm_Load(object sender, EventArgs e)
        {
            await Task.Run(() => CheckStatus());
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Form actionform = new UserActionsForm();
            actionform.ShowDialog();
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            Form FAQForm = new FAQForm();
            FAQForm.ShowDialog();
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }
    }
}
