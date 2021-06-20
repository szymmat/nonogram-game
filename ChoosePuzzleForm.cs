using System;
using System.Windows.Forms;
using System.IO;
using System.Text.Json;

namespace WinFormsLab
{
    public partial class ChoosePuzzleForm : Form
    {
        public ChoosePuzzleForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = folderBrowserDialog1.ShowDialog();
            if(dialogResult == DialogResult.OK && folderBrowserDialog1.SelectedPath != string.Empty)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
                var files = Directory.GetFiles(folderBrowserDialog1.SelectedPath,"*.json");
                listView1.Items.Clear();
                foreach(string path in files)
                {
                    StreamReader streamReader = new StreamReader(path);
                    string s = streamReader.ReadToEnd();
                    streamReader.Close();
                    Board board = JsonSerializer.Deserialize<Board>(s);
                    if(board.Height >= 2 && board.Height <= 15 && board.Width >= 2 && board.Height <= 15)
                    {
                        ListViewItem listViewItem = new ListViewItem(board.Name);
                        listViewItem.SubItems.Add(board.Width.ToString());
                        listViewItem.SubItems.Add(board.Height.ToString());
                        listViewItem.SubItems.Add(board.Difficulty);
                        listView1.Items.Add(listViewItem);
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems != null)
            {
                try
                {
                    var files = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.json");
                    StreamReader streamReader = new StreamReader(files[listView1.SelectedIndices[0]]);
                    string s = streamReader.ReadToEnd();
                    streamReader.Close();
                    Board board = JsonSerializer.Deserialize<Board>(s);
                    if (board.Height >= 2 && board.Height <= 15 && board.Width >= 2 && board.Height <= 15)
                    {
                        MainForm.loadedBoard = board;
                    }
                }
                catch(Exception)
                {

                }
            }
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(folderBrowserDialog1.SelectedPath != string.Empty)
            {
                var files = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*.json");
                listView1.Items.Clear();
                foreach (string path in files)
                {
                    StreamReader streamReader = new StreamReader(path);
                    string s = streamReader.ReadToEnd();
                    streamReader.Close();
                    Board board = JsonSerializer.Deserialize<Board>(s);
                    if (board.Height >= 2 && board.Height <= 15 && board.Width >= 2 && board.Height <= 15)
                    {
                        ListViewItem listViewItem = new ListViewItem(board.Name);
                        listViewItem.SubItems.Add(board.Width.ToString());
                        listViewItem.SubItems.Add(board.Height.ToString());
                        listViewItem.SubItems.Add(board.Difficulty);
                        listView1.Items.Add(listViewItem);
                    }
                }
            }
            
        }
    }
}
