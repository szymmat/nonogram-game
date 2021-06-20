using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace WinFormsLab
{
    public partial class CreateNewGameForm : Form
    {
        public static bool Random { get; set; } = false;
        public CreateNewGameForm()
        {
            InitializeComponent();
            if (Random) Text = "New Random puzzle";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                int num = int.Parse(textBox2.Text);
                if(num < 2 || num > 15)
                {
                    errorProvider2.SetError(textBox2, "Height must be integer number in range 2-15");
                    e.Cancel = true;
                    return;
                }
                errorProvider2.SetError(textBox2, string.Empty);
                e.Cancel = false;
            }
            catch(Exception)
            {
                errorProvider2.SetError(textBox2, "Height must be integer number in range 2-15");
                e.Cancel = true;
            }
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                int num = int.Parse(textBox1.Text);
                if (num < 2 || num > 15)
                {
                    errorProvider1.SetError(textBox1, "Width must be integer number in range 2-15");
                    e.Cancel = true;
                    return;
                }
                errorProvider1.SetError(textBox1, string.Empty);
                e.Cancel = false;
            }
            catch (Exception)
            {
                errorProvider1.SetError(textBox1, "Width must be integer number in range 2-15");
                e.Cancel = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
            {
                MainForm.BoardWidth = int.Parse(textBox1.Text);
                MainForm.BoardHeight = int.Parse(textBox2.Text);
                MainForm.ChangeLayout = true;
                Close();
            }
        }
    }
}
