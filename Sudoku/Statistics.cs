using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class Statistics : Form
    {
        public Statistics()
        {
            InitializeComponent();
        }
        public string fault { get; set; }
        public string time { get; set; }
        public string LevelString { get; set; }
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            comboBox2.Items.Add("All");
            for (int i = 0; i < games.Count; i++)
            {
                if (games[i].Contains(comboBox1.Text) == true||comboBox1.SelectedIndex==0)
                    comboBox2.Items.Add(games[i].Split('|')[0]);
            }
            comboBox2.SelectedIndex = 0;
        }
        List<string> games = new List<string>();
        private void Statistics_Load(object sender, EventArgs e)
        {
            string filepath = "Games.ini";
            if (File.Exists(filepath) == true)
            {
                FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read);
                StreamReader sw = new StreamReader(fs);
                string word = sw.ReadLine();
                while (word != null)
                {
                    games.Add(word);
                    word = sw.ReadLine();
                }
                sw.Close();
                fs.Close();
            }
            string datetime = DateTime.Now.ToString();
            FileStream fs2 = new FileStream(filepath, FileMode.Create, FileAccess.Write);
            StreamWriter sw2 = new StreamWriter(fs2);
            for (int i=0;i<games.Count;i++)
            sw2.WriteLine(games[i]);
            sw2.WriteLine(datetime + "|"+LevelString+"|"+fault+"|"+time);
            sw2.Flush();
            sw2.Close();
            fs2.Close();
            games.Add(datetime + "|" + LevelString + "|" + fault + "|" + time);

            comboBox1.SelectedItem = LevelString;
            comboBox2.SelectedItem = datetime;
            label2.Text = "Fault : "+fault;
            label3.Text = "Time : " + time;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int gamecounter = 0;
            int faultcounter = 0;
            int minutecounter = 0;
            int secondcounter = 0;

            for (int i = 1; i < comboBox2.Items.Count; i++)
            {
                if (games[i-1].Contains(comboBox2.Items[i].ToString()) == true)
                {
                        faultcounter += Convert.ToInt32(games[i-1].Split('|')[2]);
                        minutecounter += Convert.ToInt32(games[i-1].Split('|')[3].Split(':')[0]);
                        secondcounter += Convert.ToInt32(games[i-1].Split('|')[3].Split(':')[1]);
                        gamecounter += 1;
                    if (comboBox2.SelectedIndex != 0)
                        break;
                }
            }
            label5.Text = "Fault : " + faultcounter;
            label4.Text = "Time : " + minutecounter + ":" + secondcounter;
            label6.Text = "Game Played : " + gamecounter;
        }

        int Move;
        int Mouse_X;
        int Mouse_Y;
        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            Move = 0;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            Move = 1;
            Mouse_X = e.X;
            Mouse_Y = e.Y;
        }
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Move == 1)
            {
                this.SetDesktopLocation(MousePosition.X - Mouse_X, MousePosition.Y - Mouse_Y);
            }
        }
    }
}
