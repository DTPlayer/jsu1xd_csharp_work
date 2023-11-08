using MySql.Data.MySqlClient;
using System;
using MySQLData;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jsu1xd
{
    public partial class AddOperatorForm : Form
    {
        public AddOperatorForm()
        {
            InitializeComponent();
        }

        private async Task<bool> checkLogin(string login)
        {
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {

                string query_user = $"SELECT COUNT(*) FROM users WHERE login='{login}'";
                string query_operator = $"SELECT COUNT(*) FROM operators WHERE login='{login}'";
                string query_admin = $"SELECT COUNT(*) FROM admins WHERE login='{login}'";

                var cmd_user = new MySqlCommand(query_user, dbCon.Connection);
                var cmd_operator = new MySqlCommand(query_operator, dbCon.Connection);
                var cmd_admin = new MySqlCommand(query_admin, dbCon.Connection);

                int reader_user = Convert.ToInt32(cmd_user.ExecuteScalar());
                int reader_operator = Convert.ToInt32(cmd_operator.ExecuteScalar());
                int reader_admin = Convert.ToInt32(cmd_admin.ExecuteScalar());

                dbCon.Close();

                if (reader_user > 0 || reader_operator > 0 || reader_admin > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                MessageBox.Show("Ошибка подключения. Проверьте свое соединение с интернетом и попробуйте позднее");
                return false;
            }
        }

        private async void textBox2_Leave(object sender, EventArgs e)
        {
            if (guna2TextBox2.Text != "")
            {
                bool result = await checkLogin(guna2TextBox2.Text);
                guna2HtmlLabel4.Visible = true;
                if (result)
                {
                    guna2HtmlLabel5.Visible = true;
                    guna2HtmlLabel5.Text = "<span color='green'>Логин доступен</span>";
                }
                else
                {
                    guna2HtmlLabel5.Visible = true;
                    guna2HtmlLabel5.Text = "<span color='red'>Логин недоступен</span>";
                }
            }
        }

        private async void guna2Button2_Click(object sender, EventArgs e)
        {
            string fio = guna2TextBox1.Text;
            string login = guna2TextBox2.Text;
            string password = guna2TextBox3.Text;

            bool result = await checkLogin(login);

            if (fio != "" && login != "" && password != "" && result)
            {
                var dbCon = DBConnection.Instance();
                if (dbCon.IsConnect())
                {
                    string query_create = $"INSERT INTO operators (`fio`, `login`, `password`) values ('{fio}', '{login}', '{password}')";

                    var create_command = new MySqlCommand(query_create, dbCon.Connection);

                    create_command.ExecuteNonQuery();
                    MessageBox.Show("Оператор добавлен успешно");
                    dbCon.Close();
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("Все поля должны быть заполнены, а логин - уникальным");
            }
        }
    }
}
