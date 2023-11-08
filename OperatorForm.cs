using Microsoft.Toolkit.Uwp.Notifications;
using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MySQLData;
using System.Threading.Tasks;

namespace jsu1xd
{
    public partial class OperatorForm : Form
    {
        string strPath = System.Environment.GetEnvironmentVariable("TEMP");
        public int CountMessages { get; set; }

        public int UserId { get; set; }

        public void CheckStatus()
        {
            var dbCon = DBConnection.Instance();
            while (true)
            {
                if (dbCon.IsConnect())
                {
                    try
                    {
                        var connection = dbCon.openConnect();
                        string query = $"SELECT COUNT(*) FROM requests WHERE status='WAIT'";
                        var cmd = new MySqlCommand(query, connection);
                        int counter = Convert.ToInt32(cmd.ExecuteScalar());
                        if (counter > CountMessages)
                        {
                            new ToastContentBuilder()
                            .AddText("Новое обращение")
                            .AddText("Было получено новое общращение, посмотреть его можно во вкладке 'Запросы пользователей'")
                            .Show();
                        }

                        CountMessages = counter;
                        string path = Path.Combine(strPath, "jsu1xdmessageoperator.tmp");
                        using (FileStream fs = File.Create(path))
                        {
                            byte[] info = new UTF8Encoding(true).GetBytes(CountMessages.ToString());
                            fs.Write(info, 0, info.Length);
                        connection.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        dbCon.Close();
                        Console.WriteLine(ex.ToString());
                        Thread.Sleep(5000);
                    }

                }
                Thread.Sleep(1000);
            }
        }
        public OperatorForm()
        {
            InitializeComponent();

            string path = Path.Combine(strPath, "jsu1xd.tmp");
            using (var sr = new StreamReader(path))
            {
                UserId = Convert.ToInt32(sr.ReadToEnd());
            }

            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect()) { }
            string query = $"SELECT fio FROM operators WHERE id={UserId}";
            var cmd = new MySqlCommand(query, dbCon.Connection);
            var result = cmd.ExecuteReader();
            while (result.Read())
            {
                guna2HtmlLabel2.Text = $"{result.GetString(0)}";
            }
            dbCon.Close();

            try
            {
                path = Path.Combine(strPath, "jsu1xdmessageoperator.tmp");
                using (var sr = new StreamReader(path))
                {
                    CountMessages = Convert.ToInt32(sr.ReadToEnd());
                }
            }
            catch (FileNotFoundException) { }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Form AddContactForm = new AddContactForm();
            AddContactForm.ShowDialog();
        }

        private void OperatorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private async void OperatorForm_Load(object sender, EventArgs e)
        {
            await Task.Run(() => CheckStatus());
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Form requestsForm = new OperatorRequestForm();
            requestsForm.ShowDialog();
        }
    }
}
