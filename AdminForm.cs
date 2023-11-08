using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MySQLData;

namespace jsu1xd
{
    public partial class AdminForm : Form
    {
        string strPath = System.Environment.GetEnvironmentVariable("TEMP");
        public int UserId { get; set; }
        public AdminForm()
        {
            InitializeComponent();

            string path = Path.Combine(strPath, "jsu1xd.tmp");
            using (var sr = new StreamReader(path))
            {
                UserId = Convert.ToInt32(sr.ReadToEnd());
            }

            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect()) 
            { 
                string query = $"SELECT fio FROM admins WHERE id={UserId}";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var result = cmd.ExecuteReader();
                while (result.Read())
                {
                    guna2HtmlLabel2.Text = $"{result.GetString(0)}";
                }
                dbCon.Close();
            }

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Form addOperator = new AddOperatorForm();
            addOperator.ShowDialog();
        }

        private void AdminForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Form addAdmin = new AddAdminForm();
            addAdmin.ShowDialog();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Form AllStats = new AdminAllStats();
            AllStats.ShowDialog();
        }
    }
}
