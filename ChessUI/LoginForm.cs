using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessUI
{
    public partial class LoginForm : Form
    {
        public class User
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public string Id { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }
        private IMongoCollection<User> _userCollection;
        public LoginForm()
        {
            InitializeComponent();
            InitializeMongoDB();

        }
        private void InitializeMongoDB()
        {
            const string connectionUri = "mongodb+srv://minhdai:mutoyugi@cluster0.5p2sxqb.mongodb.net/?retryWrites=true&w=majority";
            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            var database = client.GetDatabase("Chess");
            _userCollection = database.GetCollection<User>("users");
        }
    

        private void loginBtn_Click(object sender, EventArgs e)
        {
            string username = emailLogin.Text;
            string password = pwLogin.Text;

            var user = _userCollection.Find(u => u.Username == username).FirstOrDefault();

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                MessageBox.Show("Login successful!");
                Menu menu = new Menu();
                menu.Show();
            }
            else
            {
                MessageBox.Show("Invalid email or password!");
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;

        }

        private void label4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();
            string password = textBox2.Text;
         
            var user = _userCollection.Find(u => u.Username == username).FirstOrDefault();

            if (user != null)
            {
                MessageBox.Show("Username already exists! Please choose a different one.");
                return;
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var newUser = new User { Username = username, Password = hashedPassword };
            _userCollection.InsertOne(newUser);

            MessageBox.Show("Sign up successful!");
        }
    }
}
