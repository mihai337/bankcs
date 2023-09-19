using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics.Tracing;
using MongoDB.Driver;
using MongoDB.Bson;
using IronPython.Hosting;
using System.Diagnostics;

namespace BankApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, EventArgs e)
        {
            var h = new Hasher();
            var username = textBox1.Text;
            var password = textBox2.Text;
            password = h.Hash(password);

            var data = new PostData
            {
                name = username,
                password = password
            };

            var client = new HttpClient();
            var endpoint = new Uri("http://192.168.1.112:8000/login");
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json , Encoding.UTF8 , "application/json");
            var response = client.PostAsync(endpoint, content).Result;

            if (response.IsSuccessStatusCode)
            {
                BalanceLbl.Text = Database.getBalance(textBox1.Text);
                UserLbl.Text = textBox1.Text;
                LoginPanel.Visible = true;
                RegPanel.Visible = true;
                UserPanel.Visible = true;

            }
            else
            {
                MessageBox.Show("Login Failed");
            }

        }
        private void RegBtn_Click(object sender, EventArgs e)
        {
            bool ok = true;

            if (textBox3.Text == "Username")
            {
                MessageBox.Show("Fill the username");
                ok = false;
            }

            if(textBox4.Text != textBox5.Text && ok)
            {
                MessageBox.Show("Passwords don't match");
                ok = false;
            }

            if(ok)
            {
                var h = new Hasher();
                var username = textBox3.Text;
                var password = textBox4.Text;
                password = h.Hash(password);

                var data = new PostData
                {
                    name = username,
                    password = password
                };

                var client = new HttpClient();
                var endpoint = new Uri("http://192.168.1.112:8000/sign-in");
                var json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = client.PostAsync(endpoint, content).Result;

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Sign in successfull, please log in");
                    RegPanel.Visible = false;
                    LoginPanel.Visible = true;

                }
                else
                {
                    MessageBox.Show("Username already exists");
                }
            }
        }
        private void TrBtn_Click(object sender, EventArgs e)
        {
            var psi = new ProcessStartInfo();
            psi.FileName = @"E:\Python\python.exe";
            var script = @"C:\Users\40756\OneDrive\Desktop\BankWin\BankApp\BankApp\consumer.py";
            var user = textBox1.Text;
            var pass = textBox2.Text;
            psi.Arguments = $"{script} {user} {pass}";

            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.RedirectStandardInput = true;

            var result = "";
            var error = "";

            using(var process = Process.Start(psi))
            {
                result = process.StandardOutput.ReadToEnd();
                error = process.StandardError.ReadToEnd();
            }
            BalanceLbl.Text = Database.getBalance(textBox1.Text);
        }

        private void RegLabel_Click(object sender, EventArgs e)
        {
            LoginPanel.Visible=false;
            UserPanel.Visible = false;
            RegPanel.Visible=true;
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if(textBox1.Text == "Username")
            {
                textBox1.Text = "";
                textBox1.ForeColor = Color.Gray;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            if(textBox1.Text == "")
            {
                textBox1.Text = "Username";
                textBox1.ForeColor = Color.Gray;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.ForeColor = Color.White;
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            if (textBox2.Text == "Password")
            {
                textBox2.PasswordChar = '*';
                textBox2.Text = "";
                textBox2.ForeColor = Color.Gray;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                textBox2.PasswordChar = '\0';
                textBox2.Text = "Password";
                textBox2.ForeColor = Color.Gray;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.ForeColor = Color.White;
        }


        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            textBox3.ForeColor = Color.White;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            textBox4.ForeColor = Color.White;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            textBox5.ForeColor = Color.White;
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            if (textBox3.Text == "Username")
            {
                textBox3.Text = "";
                textBox3.ForeColor = Color.Gray;
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (textBox3.Text == "")
            {
                textBox3.Text = "Username";
                textBox3.ForeColor = Color.Gray;
            }
        }

        private void textBox4_Enter(object sender, EventArgs e)
        {
            if (textBox4.Text == "Password")
            {
                textBox4.PasswordChar = '*';
                textBox4.Text = "";
                textBox4.ForeColor = Color.Gray;
            }
        }

        private void textBox4_Leave(object sender, EventArgs e)
        {
            if (textBox4.Text == "")
            {
                textBox4.PasswordChar = '\0';
                textBox4.Text = "Password";
                textBox4.ForeColor = Color.Gray;
            }
        }

        private void textBox5_Enter(object sender, EventArgs e)
        {
            if (textBox5.Text == "Re-Password")
            {
                textBox5.PasswordChar = '*';
                textBox5.Text = "";
                textBox5.ForeColor = Color.Gray;
            }
        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            if (textBox5.Text == "")
            {
                textBox5.PasswordChar = '\0';
                textBox5.Text = "Re-Password";
                textBox5.ForeColor = Color.Gray;
            }
        }

    }
}
