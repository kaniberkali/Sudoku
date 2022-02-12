using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sudoku
{
    public partial class Select_Level : Form
    {
        public Select_Level()
        {
            InitializeComponent();
        }

        private void Selectlvl(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Form1 frm = new Form1();
            if (btn == button3)
            {
                frm.LevelString = button3.Text;
                frm.LevelCount = 60;
            }
            if (btn == button1)
            {
                frm.LevelString = button1.Text;
                frm.LevelCount = 50;
            }
            if (btn == button2)
            {
                frm.LevelString = button2.Text;
                frm.LevelCount = 40;
            }
            if (btn == button4)
            {
                frm.LevelString = button4.Text;
                frm.LevelCount = 30;
            }
            if (btn == button5)
            {
                frm.LevelString = button5.Text;
                frm.LevelCount = 20;
            }
            if (btn == button6)
            {
                frm.LevelString = button6.Text;
                frm.LevelCount = 10;
            }
            if (btn == button8)
            {
                frm.LevelString = button8.Text;
                frm.LevelCount = Convert.ToInt32(numericUpDown1.Value);
            }
            frm.Show();
            this.Hide();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
