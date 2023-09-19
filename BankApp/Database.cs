using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Diagnostics;
using System.Windows.Forms;

namespace BankApp
{
    public class Database
    {

        public static string getBalance(string user)
        {
            //MongoClient dbClient = new MongoClient("mongo://192.168.1.105:27017");
            //IMongoDatabase db = dbClient.GetDatabase("User");
            //var data = db.GetCollection<BsonDocument>("Data");
            //return data.Find(name);
            var psi = new ProcessStartInfo();
            psi.FileName = @"E:\Python\python.exe";
            var script = @"C:\Users\40756\OneDrive\Desktop\BankWin\BankApp\BankApp\database.py";
            // MessageBox.Show(textBox1.Text);
            psi.Arguments = $"{script} {user}";

            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.RedirectStandardInput = true;

            var result = "";
            var error = "";

            using (var process = Process.Start(psi))
            {
                result = process.StandardOutput.ReadToEnd();
                error = process.StandardError.ReadToEnd();
            }
            return result;
        }
        
    }
}
