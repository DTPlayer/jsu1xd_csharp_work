using MySql.Data.MySqlClient;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySQLData;
using System.IO;
using System.Text;

namespace jsu1xd
{
    public partial class LoginForm : Form
    {
        string strPath = System.Environment.GetEnvironmentVariable("TEMP");

        public LoginForm()
        {
            InitializeComponent();
        }

        private async Task<bool> checkLogin(string login, string password)
        {
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {

                string query_user = $"SELECT COUNT(*) FROM users WHERE login='{login}' and password='{password}'";
                string query_operator = $"SELECT COUNT(*) FROM operators WHERE login='{login}' and password='{password}'";
                string query_admin = $"SELECT COUNT(*) FROM admins WHERE login='{login}' and password='{password}'";

                var cmd_user = new MySqlCommand(query_user, dbCon.Connection);
                var cmd_operator = new MySqlCommand(query_operator, dbCon.Connection);
                var cmd_admin = new MySqlCommand(query_admin, dbCon.Connection);

                int reader_user = Convert.ToInt32(cmd_user.ExecuteScalar());
                int reader_operator = Convert.ToInt32(cmd_operator.ExecuteScalar());
                int reader_admin = Convert.ToInt32(cmd_admin.ExecuteScalar());

                if (reader_user > 0)
                {
                    string query = $"SELECT id FROM users WHERE login='{login}' and password='{password}'";
                    var cmd = new MySqlCommand(query, dbCon.Connection);
                    var result = cmd.ExecuteReader();
                    while (result.Read())
                    {
                        string path = Path.Combine(strPath, "jsu1xd.tmp");
                        using (FileStream fs = File.Create(path))
                        {
                            byte[] info = new UTF8Encoding(true).GetBytes(result.GetString(0));
                            fs.Write(info, 0, info.Length);
                        }
                    }

                    MessageBox.Show("Вы успешно вошли, Пользователь!");
                    dbCon.Close();
                    Form Userform = new UserForm();
                    Userform.Show();
                    return true;
                }
                else if (reader_operator > 0)
                {
                    string query = $"SELECT id FROM operators WHERE login='{login}' and password='{password}'";
                    var cmd = new MySqlCommand(query, dbCon.Connection);
                    var result = cmd.ExecuteReader();
                    while (result.Read())
                    {
                        string path = Path.Combine(strPath, "jsu1xd.tmp");
                        using (FileStream fs = File.Create(path))
                        {
                            byte[] info = new UTF8Encoding(true).GetBytes(result.GetString(0));
                            fs.Write(info, 0, info.Length);
                        }
                    }

                    MessageBox.Show("Вы успешно вошли, Оператор!");
                    dbCon.Close();
                    Form OperFrom = new OperatorForm();
                    OperFrom.Show();
                    return true;
                }
                else if (reader_admin > 0)
                {
                    string query = $"SELECT id FROM admins WHERE login='{login}' and password='{password}'";
                    var cmd = new MySqlCommand(query, dbCon.Connection);
                    var result = cmd.ExecuteReader();
                    while (result.Read())
                    {
                        string path = Path.Combine(strPath, "jsu1xd.tmp");
                        using (FileStream fs = File.Create(path))
                        {
                            byte[] info = new UTF8Encoding(true).GetBytes(result.GetString(0));
                            fs.Write(info, 0, info.Length);
                        }
                    }

                    MessageBox.Show("Вы успешно вошли, Администратор!");
                    dbCon.Close();
                    Form AdminForm = new AdminForm();
                    AdminForm.Show();
                    return true;
                } else
                {
                    MessageBox.Show("Проверьте логин и пароль");
                    dbCon.Close();
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Ошибка подключения. Проверьте свое соединение с интернетом и попробуйте позднее");
                guna2Button1.Enabled = true;
                guna2Button2.Enabled = true;
                return false;
            }
        }

    private async void button1_Click(object sender, EventArgs e)
        {
            guna2Button1.Enabled = false;
            guna2Button2.Enabled = false;
            bool result = await checkLogin(guna2TextBox1.Text, guna2TextBox2.Text);
            if (result is false) {
                guna2Button1.Enabled = true;
                guna2Button2.Enabled = true;
            } else
            {
                this.Hide();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form RegisterForm = new RegForm();
            RegisterForm.ShowDialog();
        }
    }
}
