using System.Drawing;
using System;
using System.Windows.Forms;

namespace Ex05.WindowsUI
{
    public class GameSettingsForm : Form
    {
        private Label m_LabelPlayers;
        private Label m_LabelPlayer1;
        private Label m_LabelPlayer2;
        private Label m_LabelBoardSize;
        private Label m_LabelRows;
        private Label m_LabelCols;

        private CheckBox m_CheckBoxForPlayer2;
        private TextBox m_TextBoxPlayer1Name;
        private TextBox m_TextBoxPlayer2Name;
        private NumericUpDown m_NumericUpDownRows;
        private NumericUpDown m_NumericUpDownCols;
        private Button m_ButtonStart;

        public GameSettingsForm()
        {
            initializeComponent();
        }

        private void initializeComponent()
        {
            initializeFormSettings();
            initializePlayersSection();
            initializePlayer1Controls();
            initializePlayer2Controls();
            initializeBoardSizeSection();
            initializeBoardSizeControls();
            initializeStartButton();
            adjustFormSize();
        }

        private void initializeFormSettings()
        {
            this.Text = "Game Settings";
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void initializePlayersSection()
        {
            m_LabelPlayers = new Label();
            m_LabelPlayers.Text = "Players:";
            m_LabelPlayers.Location = new Point(10, 10);
            m_LabelPlayers.AutoSize = true;
            this.Controls.Add(m_LabelPlayers);
        }

        private void initializePlayer1Controls()
        {
            m_LabelPlayer1 = new Label();
            m_LabelPlayer1.Text = "Player 1:";
            m_LabelPlayer1.Top = m_LabelPlayers.Bottom + 10;
            m_LabelPlayer1.Left = m_LabelPlayers.Left + 10;
            m_LabelPlayer1.AutoSize = true;
            this.Controls.Add(m_LabelPlayer1);

            m_TextBoxPlayer1Name = new TextBox();
            m_TextBoxPlayer1Name.Top = m_LabelPlayer1.Top + m_LabelPlayer1.Height / 2 - m_TextBoxPlayer1Name.Height / 2;
            m_TextBoxPlayer1Name.Left = m_LabelPlayer1.Right + 30;
            this.Controls.Add(m_TextBoxPlayer1Name);
        }

        private void initializePlayer2Controls()
        {
            m_CheckBoxForPlayer2 = new CheckBox();
            m_CheckBoxForPlayer2.Top = m_LabelPlayer1.Bottom + 20;
            m_CheckBoxForPlayer2.Left = m_LabelPlayer1.Left;
            m_CheckBoxForPlayer2.Checked = false;
            m_CheckBoxForPlayer2.AutoSize = true;
            m_CheckBoxForPlayer2.CheckedChanged += m_CheckBoxForPlayer2_CheckedChanged;
            this.Controls.Add(m_CheckBoxForPlayer2);

            m_LabelPlayer2 = new Label();
            m_LabelPlayer2.Text = "Player 2:";
            m_LabelPlayer2.Top = m_CheckBoxForPlayer2.Top + m_CheckBoxForPlayer2.Height / 2 - m_LabelPlayer2.Height / 2;
            m_LabelPlayer2.Left = m_CheckBoxForPlayer2.Right + 2;
            m_LabelPlayer2.AutoSize = true;
            this.Controls.Add(m_LabelPlayer2);

            m_TextBoxPlayer2Name = new TextBox();
            m_TextBoxPlayer2Name.Top = m_LabelPlayer2.Top + m_LabelPlayer2.Height / 2 - m_TextBoxPlayer2Name.Height / 2;
            m_TextBoxPlayer2Name.Left = m_TextBoxPlayer1Name.Left;
            m_TextBoxPlayer2Name.Text = "[Computer]";
            m_TextBoxPlayer2Name.Enabled = false;
            this.Controls.Add(m_TextBoxPlayer2Name);
        }

        private void initializeBoardSizeSection()
        {
            m_LabelBoardSize = new Label();
            m_LabelBoardSize.Text = "Board Size:";
            m_LabelBoardSize.Top = m_CheckBoxForPlayer2.Bottom + 20;
            m_LabelBoardSize.Left = m_LabelPlayers.Left;
            m_LabelBoardSize.AutoSize = true;
            this.Controls.Add(m_LabelBoardSize);
        }

        private void initializeBoardSizeControls()
        {
            m_LabelRows = new Label();
            m_LabelRows.Text = "Rows:";
            m_LabelRows.Top = m_LabelBoardSize.Bottom + 10;
            m_LabelRows.Left = m_LabelPlayer1.Left;
            m_LabelRows.AutoSize = true;
            this.Controls.Add(m_LabelRows);

            m_NumericUpDownRows = new NumericUpDown();
            m_NumericUpDownRows.Minimum = 4;
            m_NumericUpDownRows.Maximum = 10;
            m_NumericUpDownRows.Top = m_LabelRows.Top + m_LabelRows.Height / 2 - m_NumericUpDownRows.Height / 2;
            m_NumericUpDownRows.Left = m_LabelRows.Right + 5;
            m_NumericUpDownRows.Width = 40;
            m_NumericUpDownRows.Value = 6;
            this.Controls.Add(m_NumericUpDownRows);

            m_LabelCols = new Label();
            m_LabelCols.Text = "Cols:";
            m_LabelCols.Top = m_LabelRows.Top;
            m_LabelCols.Left = m_NumericUpDownRows.Right + 16;
            m_LabelCols.AutoSize = true;
            this.Controls.Add(m_LabelCols);

            m_NumericUpDownCols = new NumericUpDown();
            m_NumericUpDownCols.Minimum = 4;
            m_NumericUpDownCols.Maximum = 10;
            m_NumericUpDownCols.Top = m_LabelCols.Top + m_LabelCols.Height / 2 - m_NumericUpDownCols.Height / 2;
            m_NumericUpDownCols.Left = m_LabelCols.Right + 5;
            m_NumericUpDownCols.Width = 40;
            m_NumericUpDownCols.Value = 6;
            this.Controls.Add(m_NumericUpDownCols);
        }

        private void initializeStartButton()
        {
            m_ButtonStart = new Button();
            m_ButtonStart.Text = "Start!";
            m_ButtonStart.Width = 180;
            m_ButtonStart.Top = m_LabelRows.Bottom + 20;
            m_ButtonStart.Left = m_LabelPlayers.Left + 5;
            m_ButtonStart.Click += new EventHandler(m_ButtonStart_Click);
            this.Controls.Add(m_ButtonStart);
        }

        private void adjustFormSize()
        {
            this.ClientSize = new Size(m_ButtonStart.Right + 20, m_ButtonStart.Bottom + 20);
        }

        private void m_CheckBoxForPlayer2_CheckedChanged(object? sender, EventArgs e)
        {
            if (m_CheckBoxForPlayer2.Checked)
            {
                m_TextBoxPlayer2Name.Enabled = true;
                m_TextBoxPlayer2Name.Text = string.Empty;
            }

            else
            {
                m_TextBoxPlayer2Name.Enabled = false;
                m_TextBoxPlayer2Name.Text = "[Computer]";
            }
        }

        private void m_ButtonStart_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public string Player1Name
        {
            get { return m_TextBoxPlayer1Name.Text; }
        }

        public string Player2Name
        {
            get { return m_TextBoxPlayer2Name.Text; }
        }

        public bool IsComputer
        {
            get { return !m_CheckBoxForPlayer2.Checked; }
        }

        public int BoardRows
        {
            get { return (int)m_NumericUpDownRows.Value; }
        }

        public int BoardCols
        {
            get { return (int)m_NumericUpDownCols.Value; }
        }
    }
}
