using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
        }
        public string LevelString { get; set; }
        public int LevelCount { get; set; }
        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 9; i++)
                for (int q = 0; q < 9; q++)
                    alllocations.Add(i + "," + q);
            label1.Text = "Sudoku - " + LevelString;
            this.Text = label1.Text;
            creatematris();
            Thread th = new Thread(writematris);
            th.Start();
        }
        List<string> alllocations = new List<string>();
        public void writematris()
        {
            Random rnd = new Random();
            List<int> randomlist = new List<int>();
            for (int i=1;i<=81;i++) { randomlist.Add(i); }
            for (int i = 0; i < LevelCount; i++)
            {
                int number = randomlist[rnd.Next(randomlist.Count)];
                TextBox text = this.Controls.Find("txt" + number, true).FirstOrDefault() as TextBox;
                string location = alllocations[number-1];
                int x = Convert.ToInt32(location.Split(',')[0]);
                int y = Convert.ToInt32(location.Split(',')[1]);
                text.Text = matris[x,y].ToString();
                randomlist.Remove(number);
                text.ReadOnly = true;
                text.BackColor = Color.LightGreen;
            }
        }

        static int[,] matris = new int[9, 9];
        static void creatematris()
        {
            Init(ref matris);
            Update(ref matris, 10);
        }
        static void Init(ref int[,] grid)
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    grid[i, j] = (i * 3 + i / 3 + j) % 9 + 1;
        }

        static void ChangeTwoCell(ref int[,] grid, int findValue1, int findValue2)
        {
            int xParm1, yParm1, xParm2, yParm2;
            xParm1 = yParm1 = xParm2 = yParm2 = 0;
            for (int i = 0; i < 9; i += 3)
            {
                for (int k = 0; k < 9; k += 3)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        for (int z = 0; z < 3; z++)
                        {
                            if (grid[i + j, k + z] == findValue1)
                            {
                                xParm1 = i + j;
                                yParm1 = k + z;

                            }
                            if (grid[i + j, k + z] == findValue2)
                            {
                                xParm2 = i + j;
                                yParm2 = k + z;

                            }
                        }
                    }
                    grid[xParm1, yParm1] = findValue2;
                    grid[xParm2, yParm2] = findValue1;
                }
            }
        }

        static void Update(ref int[,] grid, int shuffleLevel)
        {
            for (int repeat = 0; repeat < shuffleLevel; repeat++)
            {
                Random rand = new Random(Guid.NewGuid().GetHashCode());
                Random rand2 = new Random(Guid.NewGuid().GetHashCode());
                ChangeTwoCell(ref grid, rand.Next(1, 9), rand2.Next(1, 9));
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void txt81_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void TextsControl(object sender, KeyEventArgs e)
        {
            try
            {
                TextBox gettext = sender as TextBox;
                int textnumber = Convert.ToInt32(gettext.Name.Split('t')[2]);
                if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Tab&&textnumber-1 > 0)
                    textnumber--;
                if (e.KeyCode == Keys.Right && textnumber + 1 <= 81)
                    textnumber++;
                if (e.KeyCode == Keys.Up && textnumber - 9 > 0)
                    textnumber -= 9;
                if (e.KeyCode == Keys.Down && textnumber + 9 <= 81)
                    textnumber += 9;
                TextBox text = this.Controls.Find("txt" + textnumber.ToString(), true).FirstOrDefault() as TextBox;
                ((TextBox)text).Focus();
                    foreach (Control texts in this.Controls)
                        if (texts is TextBox && ((TextBox)texts).ReadOnly == true)
                            texts.BackColor = Color.LightGreen;
                if (text.ReadOnly == true)
                {
                    foreach (Control texts in this.Controls)
                    {
                        if (texts is TextBox &&
                            ((TextBox)texts).ReadOnly == true &&
                            text.Text == texts.Text)
                        {
                            texts.BackColor = Color.Blue;
                        }
                    }
                }
            }
            catch { }
        }
        private void colorback(TextBox textbox)
        {
            textbox.ReadOnly = true;
            Thread.Sleep(200);
            textbox.BackColor = Color.White;
            textbox.ReadOnly = false;
        }
        private void colorback(Label label)
        {
            Thread.Sleep(200);
            label.ForeColor = Color.White;
        }

        int truecounter = 0;
        int faultcounter = 0;
        string alltext = "";
        private void TextsCheck(object sender, EventArgs e)
        {
            TextBox gettext = sender as TextBox;
            try
            {
                if (gettext.Text != "")
                {
                    int textnumber = Convert.ToInt32(gettext.Name.Split('t')[2]);
                    int x = Convert.ToInt32(alllocations[textnumber-1].Split(',')[0]);
                    int y = Convert.ToInt32(alllocations[textnumber-1].Split(',')[1]);
                    if (matris[x, y] == Convert.ToInt32(gettext.Text))
                    {
                        gettext.ReadOnly = true;
                        gettext.BackColor = Color.LightGreen;
                        Label lbl = this.Controls.Find("lbl" + gettext.Text, true).FirstOrDefault() as Label;
                        lbl.ForeColor = Color.Blue;
                        Thread th = new Thread(() => colorback(lbl)); th.Start();
                        truecounter += 1;
                        alltext += gettext.Text;
                        foreach (Control texts in this.Controls)
                            if (texts is TextBox && ((TextBox)texts).ReadOnly == true)
                                texts.BackColor = Color.LightGreen;
                            foreach (Control texts in this.Controls)
                            {
                                if (texts is TextBox &&
                                    ((TextBox)texts).ReadOnly == true &&
                                    gettext.Text == texts.Text)
                                {
                                    texts.BackColor = Color.Blue;
                                }
                            }
                    }
                    else
                    {
                        faultcounter++;
                        gettext.BackColor = Color.Red;
                        Label lbl = this.Controls.Find("lbl" + gettext.Text, true).FirstOrDefault() as Label;
                        lbl.ForeColor = Color.Red;
                            Thread th = new Thread(() => colorback(gettext)); th.Start();
                        th = new Thread(() => colorback(lbl)); th.Start();
                        gettext.Text = "";
                        label2.Text = "Fault : " + faultcounter;
                    }
                }
            }
            catch
            {

            }
            for (int i = 1; i <= 9; i++)
            {
                Label lbl = this.Controls.Find("lbl" + i.ToString(), true).FirstOrDefault() as Label;
                int allcount = alltext.Length;
                int tempcount = alltext.Replace(i.ToString(), "").Length;
                int count = allcount - tempcount;
                if (lbl.Visible == true && count == 9)
                    lbl.Visible = false;
            }
            if (truecounter == 81)
            {
                this.Hide();
                Statistics statistics = new Statistics();
                statistics.fault = label2.Text.Replace("Fault : ","");
                statistics.time = label3.Text;
                statistics.LevelString = LevelString;
                statistics.Show();
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            
        }

        private void LabelsClick(object sender, EventArgs e)
        {
            Label selectlabel = sender as Label;
            string text = selectlabel.Name.Split('l')[2];
            SendKeys.Send(text);
        }
        int secondcounter = 0;
        private void timer_Tick(object sender, EventArgs e)
        {
            secondcounter++;
            int minute = secondcounter / 60;
            int second = secondcounter % 60;
            label3.Text = minute + ":" + second;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 9; i++)
                for (int q = 0; q < 9; q++)
                    matris[i, q] = 0;
            foreach (Control text in this.Controls)
                if (text is TextBox)
                {
                    ((TextBox)text).Text = "";
                    ((TextBox)text).ReadOnly = false;
                    text.BackColor = Color.White;
                }
            for (int i = 1; i <= 9; i++)
            {
                Label lbl = this.Controls.Find("lbl" + i, true).FirstOrDefault() as Label;
                lbl.Visible = true;
            }
            truecounter = 0;
            faultcounter = 0;
            creatematris();
            Thread th = new Thread(writematris);
            th.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void TextsClick(object sender, EventArgs e)
        {
            foreach (Control texts in this.Controls)
                if (texts is TextBox && ((TextBox)texts).ReadOnly == true)
                    texts.BackColor = Color.LightGreen;
            TextBox gettext = sender as TextBox;
            if (gettext.ReadOnly == true)
            {
                foreach (Control texts in this.Controls)
                {
                    if (texts is TextBox &&
                        ((TextBox)texts).ReadOnly == true &&
                        gettext.Text == texts.Text)
                    {
                        texts.BackColor = Color.Blue;
                    }
                }
            }
        }

        private void TextsCheckNumber(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
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