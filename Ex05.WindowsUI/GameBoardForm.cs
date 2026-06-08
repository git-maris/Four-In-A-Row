using System.Drawing;
using System;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;
using System.Text;
using Ex05.GameLogic;

namespace Ex05.WindowsUI
{
    public class GameBoardForm : Form
    {
        private const int k_ButtonSize = 50;
        private const int k_Space = 10;
        private const int k_Margin = 20;
        private const string k_Player1Sign = "X";
        private const string k_Player2Sign = "O";

        private readonly int r_Rows;
        private readonly int r_Cols;
        private readonly GameEngine r_GameEngine;

        private readonly Button[,] r_BoardGridButtons;
        private readonly Button[] r_ColsButtons;
        private Label m_LabelPlayer1Name;
        private Label m_LabelPlayer2Name;
        private Label m_LabelPlayer1Score;
        private Label m_LabelPlayer2Score;

        public GameBoardForm(Ex05.GameLogic.GameEngine i_GameLogic)
        {
            r_GameEngine = i_GameLogic;
            r_Rows = r_GameEngine.BoardRows;
            r_Cols = r_GameEngine.BoardCols;
            r_BoardGridButtons = new Button[r_Rows, r_Cols];
            r_ColsButtons = new Button[r_Cols];
            r_GameEngine.BoardChanged += r_BoardGridButtons_Changed;

            initializeComponent();
        }

        private void initializeComponent()
        {
            this.Text = "4 in a Raw !!";
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;

            initializeBoardComponent();
            initializeLabels();
            fitWindowSize();
        }

        private void initializeBoardComponent()
        {
            for (int i = 0; i < r_Cols; i++)
            {
                r_ColsButtons[i] = new Button();
                r_ColsButtons[i].Text = (i + 1).ToString();
                r_ColsButtons[i].Size = new Size(k_ButtonSize, k_ButtonSize / 2);
                r_ColsButtons[i].Top = k_Space + k_Margin;
                r_ColsButtons[i].Left = k_Margin + i * k_ButtonSize + i * k_Space;
                r_ColsButtons[i].BackColor = Color.LightGray;
                r_ColsButtons[i].Tag = i;
                r_ColsButtons[i].Click += r_ColsButton_Click;
                this.Controls.Add(r_ColsButtons[i]);
            }

            for (int row = 0; row < r_Rows; row++) 
            {
                for (int col = 0; col < r_Cols; col++)
                {
                    Button button = new Button();
                    button.Size = new Size(k_ButtonSize, k_ButtonSize);
                    button.Left = r_ColsButtons[col].Left;
                    button.Top = r_ColsButtons[col].Bottom + row * k_ButtonSize + (row + 1) * k_Space;
                    button.Enabled = false;
                    button.BackColor = Color.White;
                    this.Controls.Add(button);
                    r_BoardGridButtons[row,col] = button;
                }
            }
        }

        private void initializeLabels()
        {
            m_LabelPlayer1Name = new Label();
            m_LabelPlayer1Name.Text = string.Format("{0} {1}", r_GameEngine.Player1Name, ":");
            m_LabelPlayer1Name.AutoSize = true;
            m_LabelPlayer1Name.Left = k_Margin;
            m_LabelPlayer1Name.Top = r_BoardGridButtons[r_Rows - 1, 0].Bottom + k_Margin;
            this.Controls.Add(m_LabelPlayer1Name);

            m_LabelPlayer1Score = new Label();
            m_LabelPlayer1Score.Left = m_LabelPlayer1Name.Right + 2;
            m_LabelPlayer1Score.Top = m_LabelPlayer1Name.Top;
            m_LabelPlayer1Score.Text = r_GameEngine.Player1Score.ToString();
            m_LabelPlayer1Score.AutoSize = true;
            this.Controls.Add(m_LabelPlayer1Score);

            m_LabelPlayer2Name = new Label();
            m_LabelPlayer2Name.Text = string.Format("{0} {1}", r_GameEngine.Player2Name, ":");
            m_LabelPlayer2Name.AutoSize = true;
            m_LabelPlayer2Name.Top = m_LabelPlayer1Name.Top;
            this.Controls.Add(m_LabelPlayer2Name);

            m_LabelPlayer2Score = new Label();
            m_LabelPlayer2Score.Top = m_LabelPlayer2Name.Top;
            m_LabelPlayer2Score.Text = r_GameEngine.Player2Score.ToString();
            m_LabelPlayer2Score.AutoSize = true;
            this.Controls.Add(m_LabelPlayer2Score);

            alignPlayer2Labels();
        }

        private void alignPlayer2Labels()
        {
            int boardRightEdge = r_ColsButtons[r_Cols - 1].Right;
            m_LabelPlayer2Score.Left = boardRightEdge - m_LabelPlayer2Score.Width;
            m_LabelPlayer2Name.Left = m_LabelPlayer2Score.Left - m_LabelPlayer2Name.Width - 2;
        }

        private void fitWindowSize()
        {
            int width = r_ColsButtons[r_Cols - 1].Right + k_Margin;
            int height = m_LabelPlayer1Name.Bottom + k_Margin;

            this.ClientSize = new Size(width, height);
        }

        public void ShowGameEnded(string i_WinnerName = null)
        {
            string          message;
            string          title;
            DialogResult    userChoice;
            MessageBoxIcon  icon = MessageBoxIcon.Question;

            if (string.IsNullOrEmpty(i_WinnerName))
            {
                message = string.Format("Tie!!{0}Another Round?", Environment.NewLine);
                title = "A Tie!";
            }
            else
            {
                message = string.Format("{0} Won!!{1}Another Round?", i_WinnerName, Environment.NewLine);
                title = "A Win!";
            }

            userChoice = MessageBox.Show(message, title, MessageBoxButtons.YesNo, icon);

            if (userChoice == DialogResult.Yes) 
            {
                r_GameEngine.ResetGame();
                resetBoardVisuals();
                if (!r_GameEngine.OpponentIsComputer)
                {
                    whoStartsMessage();
                }
            }
            else
            {
                this.Close();
            }
        }

        private void resetBoardVisuals()
        {
            for (int row = 0; row < r_Rows; row++)
            {
                for (int col = 0; col < r_Cols; col++)
                {
                    r_BoardGridButtons[row, col].Text = string.Empty;
                    r_BoardGridButtons[row, col].BackColor = Color.White;
                }
            }

            foreach (Button button in r_ColsButtons)
            {
                button.Enabled = true;
            }
        }

        private void r_ColsButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            int colIndex = (int)clickedButton.Tag;
            performMove(colIndex);
        }

        private void r_BoardGridButtons_Changed(MoveDetails i_Point)
        {
            string winnerName;
            Button button = r_BoardGridButtons[i_Point.Row, i_Point.Col];

            button.Text = i_Point.PlayerNum == ePlayerNum.Player1 ? k_Player1Sign : k_Player2Sign;
            setButtonColor(button);

            r_ColsButtons[i_Point.Col].Enabled = !(i_Point.Row == 0);
            
            if (r_GameEngine.IsWin) 
            {
                updateScoreLabels();
                winnerName = i_Point.PlayerNum == ePlayerNum.Player1 ? r_GameEngine.Player1Name : r_GameEngine.Player2Name;
                ShowGameEnded(winnerName);
            }

            else if (r_GameEngine.IsTie)
            {
                ShowGameEnded();
            }
        }

        private void setButtonColor(Button button)
        {
            button.BackColor = button.Text == k_Player1Sign ? Color.Purple : Color.Yellow;
        }

        private void performMove(int i_ColIndex)
        {
            r_GameEngine.PlayMove(i_ColIndex);

            if (theGameIsStillRunningAndItsComputerTurn()) 
            {
                    r_GameEngine.PlayComputerMove();
            }
        }

        private bool theGameIsStillRunningAndItsComputerTurn()
        {
            return (!r_GameEngine.IsTie && !r_GameEngine.IsWin
                    && r_GameEngine.OpponentIsComputer
                    && r_GameEngine.CurrentPlayerTurn == (int)ePlayerNum.Player2);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            whoStartsMessage();
        }

        private void whoStartsMessage()
        {
            string message;

            message = string.Format("{0} Starts!", r_GameEngine.CurrentPlayerTurnName);
            MessageBox.Show(message, "Game Start", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void updateScoreLabels()
        {
            this.Player1Score = r_GameEngine.Player1Score;
            this.Player2Score = r_GameEngine.Player2Score;
        }

        public int Player1Score
        {
            set
            {
                m_LabelPlayer1Score.Text = value.ToString();
            }
        }

        public int Player2Score
        {
            set
            {
                m_LabelPlayer2Score.Text = value.ToString();
                alignPlayer2Labels();
            }
        }
    }
}
