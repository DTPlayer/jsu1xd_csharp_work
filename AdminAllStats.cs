using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySQLData;
using Windows.System;

namespace jsu1xd
{
    public partial class AdminAllStats : Form
    {
        public AdminAllStats()
        {
            InitializeComponent();
        }

        private void AdminAllStats_Load(object sender, EventArgs e)
        {
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                string query_all = $"SELECT COUNT(*) FROM requests";
                string query_wait = $"SELECT COUNT(*) FROM requests WHERE status='WAIT'";
                string query_res = $"SELECT COUNT(*) FROM requests WHERE status='RES'";

                var cmd_all = new MySqlCommand(query_all, dbCon.Connection);
                var cmd_wait = new MySqlCommand(query_wait, dbCon.Connection);
                var cmd_res = new MySqlCommand(query_res, dbCon.Connection);

                int result_all = Convert.ToInt32(cmd_all.ExecuteScalar());
                int result_wait = Convert.ToInt32(cmd_wait.ExecuteScalar());
                int result_res = Convert.ToInt32(cmd_res.ExecuteScalar());

                dbCon.Close();

                guna2HtmlLabel1.Text = $"Всего: {result_all}";
                guna2HtmlLabel2.Text = $"Ожидают: {result_wait}";
                guna2HtmlLabel3.Text = $"Решено: {result_res}";
            }
        }
    }
}
