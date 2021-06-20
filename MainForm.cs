using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows.Forms;

namespace WinFormsLab
{
    public partial class MainForm : Form
    {
        public static int BoardWidth { get; set; } = 10;
        public static int BoardHeight { get; set; } = 10;
        public static bool ChangeLayout { get; set; } = false;

        private List<Label> columnLabels = new List<Label>();
        private List<Label> rowLabels = new List<Label>();
        private List<Button> buttons = new List<Button>();
        public static int MAX_BOARD_HEIGHT { get; } = 15;
        public static int MAX_BOARD_WIDTH { get; } = 15;
        public static Board loadedBoard { get; set; } = null;
        private bool createMode = false;
        public bool[,] board = new bool[MAX_BOARD_HEIGHT, MAX_BOARD_WIDTH];
        private bool[,] currBoard = new bool[MAX_BOARD_HEIGHT, MAX_BOARD_WIDTH];

        public MainForm()
        {
            InitializeComponent();
        }

        private bool CompareBoards()
        {
            for(int i=0;i<MAX_BOARD_HEIGHT;i++)
            {
                for(int j=0;j<MAX_BOARD_WIDTH;j++)
                {
                    if (board[i, j] != currBoard[i, j]) return false;
                }
            }
            return true;
        }

        public void LoadGame()
        {
            BoardWidth = loadedBoard.Width;
            BoardHeight = loadedBoard.Height;
            createMode = false;
            groupBox1.Visible = false;
            label1.Visible = false;
            tableLayoutPanel1.Size = new Size(BoardWidth * 30, BoardHeight * 30);
            tableLayoutPanel1.Location = new Point((Width - tableLayoutPanel1.Width) / 2, (Height - tableLayoutPanel1.Height) / 2);
            for (int i = 0; i < MAX_BOARD_WIDTH; i++)
            {
                columnLabels[i].Location = new Point(i * 30 + tableLayoutPanel1.Left, 0);
                columnLabels[i].Size = new Size(30, tableLayoutPanel1.Top);
                rowLabels[i].Location = new Point(0, i * 30 + tableLayoutPanel1.Top);
                rowLabels[i].Size = new Size(tableLayoutPanel1.Left, 30);
            }
            for (int i = 0; i < MAX_BOARD_HEIGHT; i++)
            {
                StringBuilder stringBuilder = new StringBuilder();
                int currBlacks = 0;
                for (int j = 0; j < MAX_BOARD_WIDTH; j++)
                {
                    currBoard[i, j] = false;
                    board[i, j] = loadedBoard.GameBoard[i * MAX_BOARD_WIDTH + j];
                    if (board[i, j] == true && i < BoardHeight && j < BoardWidth)
                    {
                        currBlacks++;
                    }
                    else if (i < BoardHeight && j < BoardWidth)
                    {
                        if (currBlacks > 0) stringBuilder.Append($" {currBlacks}");
                        currBlacks = 0;
                    }
                }
                if (currBlacks > 0) stringBuilder.Append($" {currBlacks}");
                if (i < BoardHeight) rowLabels[i].Text = stringBuilder.ToString();
                else rowLabels[i].Text = string.Empty;
            }
            for (int i = 0; i < MAX_BOARD_WIDTH; i++)
            {
                StringBuilder stringBuilder = new StringBuilder();
                int currBlacks = 0;
                for (int j = 0; j < MAX_BOARD_HEIGHT; j++)
                {
                    if (board[j, i] == true) currBlacks++; // uwaga, nie wiem czy dobrze
                    else
                    {
                        if (currBlacks > 0) stringBuilder.Append($"\n{currBlacks}");
                        currBlacks = 0;
                    }
                }
                if (currBlacks > 0) stringBuilder.Append($"\n{currBlacks}");
                if (i < BoardWidth) columnLabels[i].Text = stringBuilder.ToString();
                else columnLabels[i].Text = string.Empty;
            }
            foreach (var btn in buttons)
            {
                btn.BackColor = Color.White;
                btn.BackgroundImage = null;
                btn.Enabled = true;
            }
            loadedBoard = null;
        }

        private void ButtonClick(object sender, MouseEventArgs e)
        {
            Image cross = Image.FromFile("../../../cross_icon.png");
            Button button = sender as Button;
            if (e.Button == MouseButtons.Left)
            {
                button.BackgroundImage = null;
                if (button.BackColor == Color.Black)
                    button.BackColor = Color.White;
                else if (button.BackColor == Color.White) 
                    button.BackColor = Color.Black;
                currBoard[button.Location.Y / 30, button.Location.X / 30] = !currBoard[button.Location.Y / 30, button.Location.X / 30];
                if (createMode == true)
                {
                    board[button.Location.Y / 30, button.Location.X / 30] = !board[button.Location.Y / 30, button.Location.X / 30];
                    for (int i=0;i<BoardHeight;i++)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        int currBlacks = 0;
                        for (int j=0;j<BoardWidth;j++)
                        {
                            if (board[i, j] == true) currBlacks++;
                            else
                            {
                                if (currBlacks > 0) stringBuilder.Append($" {currBlacks}");
                                currBlacks = 0;
                            }
                        }
                        if (currBlacks > 0) stringBuilder.Append($" {currBlacks}");
                        rowLabels[i].Text = stringBuilder.ToString();
                        if (rowLabels[i].Text.Length == 0) rowLabels[i].Text = "0";
                    }
                    for (int i = 0; i < BoardWidth; i++)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        int currBlacks = 0;
                        for (int j = 0; j < BoardHeight; j++)
                        {
                            if (board[j, i] == true) currBlacks++;
                            else
                            {
                                if (currBlacks > 0) stringBuilder.Append($"\n{currBlacks}");
                                currBlacks = 0;
                            }
                        }
                        if (currBlacks > 0) stringBuilder.Append($"\n{currBlacks}");
                        columnLabels[i].Text = stringBuilder.ToString();
                        if (columnLabels[i].Text.Length == 0) columnLabels[i].Text = "0";
                    }
                }
            }
            else if(e.Button == MouseButtons.Right)
            {
                if (button.BackgroundImage == null) button.BackgroundImage = cross;
                else
                {
                    button.BackgroundImage = null;
                    button.BackColor = Color.White;
                    currBoard[button.Location.Y / 30, button.Location.X / 30] = false;
                }
            }
            if (CompareBoards() == true && createMode == false) // disable buttons
            {
                foreach (var btn in buttons)
                {
                    btn.Enabled = false;
                }
                label1.Visible = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for(int i=0;i<MAX_BOARD_WIDTH;i++)
            {
                Label label = new Label();
                label.Location = new Point(i * 30 + tableLayoutPanel1.Left, 0);
                label.Size = new Size(30, tableLayoutPanel1.Top);
                label.TextAlign = ContentAlignment.BottomCenter;
                columnLabels.Add(label);
                Controls.Add(label);
            }
            Random random = new Random();
            for (int i = 0; i < MAX_BOARD_HEIGHT; i++)
            {
                Label label = new Label();
                label.Location = new Point(0, i * 30 + tableLayoutPanel1.Top);
                label.Size = new Size(tableLayoutPanel1.Left, 30);
                label.TextAlign = ContentAlignment.MiddleRight;
                Controls.Add(label);
                rowLabels.Add(label);
                StringBuilder stringBuilder = new StringBuilder();
                int currBlacks = 0;
                for (int j = 0; j < MAX_BOARD_WIDTH; j++)
                {
                    int color = random.Next(0, 2);
                    Button button = new Button();
                    button.Dock = DockStyle.Fill;
                    button.FlatStyle = FlatStyle.Flat;
                    button.Margin = new Padding(0);
                    button.BackColor = Color.White;
                    if (color == 0 && i < BoardHeight && j < BoardWidth)
                    {
                        board[i, j] = true;
                        currBlacks++;
                    }
                    else if (i < BoardHeight && j < BoardWidth) 
                    {
                        if (currBlacks > 0) stringBuilder.Append($" {currBlacks}");
                        currBlacks = 0;
                    }
                    button.MouseDown += new MouseEventHandler(ButtonClick);
                    buttons.Add(button);
                    tableLayoutPanel1.Controls.Add(button, j, i);
                }
                if (currBlacks > 0) stringBuilder.Append($" {currBlacks}");
                if (i < BoardHeight) label.Text = stringBuilder.ToString();
            }
            for(int i=0;i<MAX_BOARD_WIDTH;i++)
            {
                StringBuilder stringBuilder = new StringBuilder();
                int currBlacks = 0;
                for (int j=0;j<MAX_BOARD_HEIGHT;j++)
                {
                    if (i < BoardHeight && j < BoardWidth && board[j, i] == true) currBlacks++; // uwaga, nie wiem czy dobrze
                    else
                    {
                        if (currBlacks > 0) stringBuilder.Append($"\n{currBlacks}");
                        currBlacks = 0;
                    }
                }
                if(currBlacks > 0) stringBuilder.Append($"\n{currBlacks}");
                if(i < BoardWidth) columnLabels[i].Text = stringBuilder.ToString();
            }
        }

        private void tableLayoutPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            currBoard[e.X / 30, e.Y / 30] = true;
            if (currBoard == board) MessageBox.Show("You won");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Board saveBoard = new Board();
            saveBoard.Name = textBox1.Text;
            saveBoard.Width = BoardWidth;
            saveBoard.Height = BoardHeight;
            saveBoard.Difficulty = textBox2.Text;
            for(int i=0;i<MAX_BOARD_HEIGHT;i++)
            {
                for(int j=0;j<MAX_BOARD_WIDTH;j++)
                {
                    saveBoard.GameBoard[i * MAX_BOARD_HEIGHT + j] = board[i, j];
                }
            }
            string s = JsonSerializer.Serialize(saveBoard);
            saveFileDialog1.ShowDialog();
            if(saveFileDialog1.FileName != string.Empty)
            {
                StreamWriter streamWriter = new StreamWriter(saveFileDialog1.FileName);
                streamWriter.Write(s);
                streamWriter.Close();
            }
        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if(e.ClickedItem == toolStripMenuItem4)
            {
                CreateNewGameForm.Random = true;
                CreateNewGameForm form2 = new CreateNewGameForm();
                form2.ShowDialog();
                if (ChangeLayout)
                {
                    ChangeLayout = false;
                    createMode = false;
                    groupBox1.Visible = false;
                    label1.Visible = false;
                    tableLayoutPanel1.Size = new Size(BoardWidth * 30, BoardHeight * 30);
                    tableLayoutPanel1.Location = new Point((Width - tableLayoutPanel1.Width) / 2, (Height - tableLayoutPanel1.Height) / 2);
                    for (int i = 0; i < MAX_BOARD_WIDTH; i++)
                    {
                        columnLabels[i].Location = new Point(i * 30 + tableLayoutPanel1.Left, 0);
                        columnLabels[i].Size = new Size(30, tableLayoutPanel1.Top);
                        rowLabels[i].Location = new Point(0, i * 30 + tableLayoutPanel1.Top);
                        rowLabels[i].Size = new Size(tableLayoutPanel1.Left, 30);
                    }
                    for (int i = 0; i < MAX_BOARD_HEIGHT; i++)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        int currBlacks = 0;
                        Random random = new Random();
                        for (int j = 0; j < MAX_BOARD_WIDTH; j++)
                        {
                            currBoard[i, j] = false;
                            board[i, j] = false;
                            int color = random.Next(0, 2);
                            if (color == 0 && i < BoardHeight && j < BoardWidth)
                            {
                                board[i, j] = true;
                                currBlacks++;
                            }
                            else if (i < BoardHeight && j < BoardWidth)
                            {
                                if (currBlacks > 0) stringBuilder.Append($" {currBlacks}");
                                currBlacks = 0;
                            }
                        }
                        if (currBlacks > 0) stringBuilder.Append($" {currBlacks}");
                        if (i < BoardHeight) rowLabels[i].Text = stringBuilder.ToString();
                        else rowLabels[i].Text = string.Empty;
                    }
                    for (int i = 0; i < MAX_BOARD_WIDTH; i++)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        int currBlacks = 0;
                        for (int j = 0; j < MAX_BOARD_HEIGHT; j++)
                        {
                            if (board[j, i] == true) currBlacks++; // uwaga, nie wiem czy dobrze
                            else
                            {
                                if (currBlacks > 0) stringBuilder.Append($"\n{currBlacks}");
                                currBlacks = 0;
                            }
                        }
                        if (currBlacks > 0) stringBuilder.Append($"\n{currBlacks}");
                        if (i < BoardWidth) columnLabels[i].Text = stringBuilder.ToString();
                        else columnLabels[i].Text = string.Empty;
                    }
                    foreach (var btn in buttons)
                    {
                        btn.BackColor = Color.White;
                        btn.BackgroundImage = null;
                        btn.Enabled = true;
                    }
                }
            }
            else if (e.ClickedItem == toolStripMenuItem2)
            {
                DialogResult dialogResult = openFileDialog1.ShowDialog();
                Board loadedBoard;
                if (dialogResult == DialogResult.OK && openFileDialog1.FileName != string.Empty)
                {
                    try
                    {
                        StreamReader streamReader = new StreamReader(openFileDialog1.FileName);
                        string s = streamReader.ReadToEnd();
                        streamReader.Close();
                        try
                        {
                            loadedBoard = JsonSerializer.Deserialize<Board>(s);
                        }
                        catch (JsonException)
                        {
                            MessageBox.Show("Invalid JSON file!");
                            return;
                        }
                    }
                    catch(Exception)
                    {
                        return;
                    }
                    BoardWidth = loadedBoard.Width;
                    BoardHeight = loadedBoard.Height;
                    createMode = false;
                    groupBox1.Visible = false;
                    label1.Visible = false;
                    tableLayoutPanel1.Size = new Size(BoardWidth * 30, BoardHeight * 30);
                    tableLayoutPanel1.Location = new Point((Width - tableLayoutPanel1.Width) / 2, (Height - tableLayoutPanel1.Height) / 2);
                    for (int i = 0; i < MAX_BOARD_WIDTH; i++)
                    {
                        columnLabels[i].Location = new Point(i * 30 + tableLayoutPanel1.Left, 0);
                        columnLabels[i].Size = new Size(30, tableLayoutPanel1.Top);
                        rowLabels[i].Location = new Point(0, i * 30 + tableLayoutPanel1.Top);
                        rowLabels[i].Size = new Size(tableLayoutPanel1.Left, 30);
                    }
                    for (int i = 0; i < MAX_BOARD_HEIGHT; i++)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        int currBlacks = 0;
                        for (int j = 0; j < MAX_BOARD_WIDTH; j++)
                        {
                            currBoard[i, j] = false;
                            board[i, j] = loadedBoard.GameBoard[i * MAX_BOARD_WIDTH + j];
                            if (board[i, j] == true && i < BoardHeight && j < BoardWidth)
                            {
                                currBlacks++;
                            }
                            else if (i < BoardHeight && j < BoardWidth)
                            {
                                if (currBlacks > 0) stringBuilder.Append($" {currBlacks}");
                                currBlacks = 0;
                            }
                        }
                        if (currBlacks > 0) stringBuilder.Append($" {currBlacks}");
                        if (i < BoardHeight) rowLabels[i].Text = stringBuilder.ToString();
                        else rowLabels[i].Text = string.Empty;
                    }
                    for (int i = 0; i < MAX_BOARD_WIDTH; i++)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        int currBlacks = 0;
                        for (int j = 0; j < MAX_BOARD_HEIGHT; j++)
                        {
                            if (board[j, i] == true) currBlacks++; // uwaga, nie wiem czy dobrze
                            else
                            {
                                if (currBlacks > 0) stringBuilder.Append($"\n{currBlacks}");
                                currBlacks = 0;
                            }
                        }
                        if (currBlacks > 0) stringBuilder.Append($"\n{currBlacks}");
                        if (i < BoardWidth) columnLabels[i].Text = stringBuilder.ToString();
                        else columnLabels[i].Text = string.Empty;
                    }
                    foreach (var btn in buttons)
                    {
                        btn.BackColor = Color.White;
                        btn.BackgroundImage = null;
                        btn.Enabled = true;
                    }
                }
            }
            else if (e.ClickedItem == toolStripMenuItem1)
            {
                ChoosePuzzleForm form3 = new ChoosePuzzleForm();
                form3.ShowDialog();
                if (loadedBoard != null) LoadGame();
            }
        }

        private void contextMenuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == toolStripMenuItem3)
            {
                
                    CreateNewGameForm.Random = false;
                    CreateNewGameForm form2 = new CreateNewGameForm();
                    form2.ShowDialog();
                    if (ChangeLayout)
                    {
                        ChangeLayout = false;
                        createMode = true;
                        groupBox1.Visible = true;
                        label1.Visible = false;
                        tableLayoutPanel1.Size = new Size(BoardWidth * 30, BoardHeight * 30);
                        tableLayoutPanel1.Location = new Point((Width - tableLayoutPanel1.Width) / 2, (Height - tableLayoutPanel1.Height) / 2);
                        for (int i = 0; i < MAX_BOARD_WIDTH; i++)
                        {
                            columnLabels[i].Location = new Point(i * 30 + tableLayoutPanel1.Left, 0);
                            columnLabels[i].Size = new Size(30, tableLayoutPanel1.Top);
                            rowLabels[i].Location = new Point(0, i * 30 + tableLayoutPanel1.Top);
                            rowLabels[i].Size = new Size(tableLayoutPanel1.Left, 30);
                        }
                        for (int i = 0; i < MAX_BOARD_HEIGHT; i++)
                        {
                            for (int j = 0; j < MAX_BOARD_WIDTH; j++)
                            {
                                board[i, j] = false;
                                currBoard[i, j] = false;
                            }
                            if (i < BoardHeight) rowLabels[i].Text = "0";
                            else rowLabels[i].Text = string.Empty;
                            if (i < BoardWidth) columnLabels[i].Text = "0";
                            else columnLabels[i].Text = string.Empty;
                        }
                        foreach (var btn in buttons)
                        {
                            btn.BackColor = Color.White;
                            btn.BackgroundImage = null;
                            btn.Enabled = true;
                        }
                    }
            }
            
        }
    }
    public class Board
    {
        public string Name { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Difficulty { get; set; }
        public bool[] GameBoard { get; set; } = new bool[MainForm.MAX_BOARD_HEIGHT * MainForm.MAX_BOARD_WIDTH];
    }
}
