using System;
using System.Collections.Generic;
using System.Text;

namespace Ex05.GameLogic
{
    public class GameEngine
    {
        private readonly Board r_Board;
        private readonly Player[] r_Players;
        private Random m_Random;
        private ePlayerNum m_CurrentPlayerTurn;
        private int m_CounterToCheckIfTie;
        private bool m_IsTie;
        private bool m_IsWin;
        public event Action<MoveDetails> BoardChanged;

        public GameEngine(int i_Rows,
                          int i_Cols,
                          ePlayerType i_OpponentPlayerType,
                          string i_Player1Name,
                          string i_Player2Name)
        {
            r_Board = new Board(i_Rows, i_Cols);
            r_Players = new Player[2];
            r_Players[0] = new Player(ePlayerType.Human, ePlayerNum.Player1, i_Player1Name);
            r_Players[1] = new Player(i_OpponentPlayerType, ePlayerNum.Player2, i_Player2Name);
            m_Random = new Random();
            m_IsTie = false;
            m_IsWin = false;
            m_CounterToCheckIfTie = i_Cols * i_Rows;
            setWhoStart();
        }

        public void PlayMove(int i_Col)
        {
            int row;
            MoveDetails moveDetails;
            ePlayerNum playerWhoJustPlayed = m_CurrentPlayerTurn;

            r_Board.TryInsertCoinToCol(i_Col, out row, m_CurrentPlayerTurn, out m_IsWin);
            tieCheckAndHandle();

            if (m_IsWin)
            {
                r_Players[(int)m_CurrentPlayerTurn].IncreaseScore();
            }
            else if (!m_IsTie)
            {
                changeTurn();
            }

            moveDetails = new MoveDetails(row, i_Col, playerWhoJustPlayed);
            BoardChanged.Invoke(moveDetails);
        }

        private void setWhoStart()
        {
            if (r_Players[1].Type == ePlayerType.Computer)
            {
                m_CurrentPlayerTurn = ePlayerNum.Player1;
            }
            else
            {
                getRandomStartingPlayer();
            }
        }

        private void getRandomStartingPlayer()
        {
            int randomWhoStart;

            randomWhoStart = m_Random.Next(1, 3);
            m_CurrentPlayerTurn = (randomWhoStart == 1) ? ePlayerNum.Player1 : ePlayerNum.Player2;
        }

        private void changeTurn()
        {
            m_CurrentPlayerTurn = (m_CurrentPlayerTurn == ePlayerNum.Player1 ? ePlayerNum.Player2 : ePlayerNum.Player1);
        }

        public void ResetGame()
        {
            r_Board.InitializeBoard();
            setWhoStart();
            m_IsTie = false;
            m_IsWin = false;
            m_CounterToCheckIfTie = r_Board.Rows * r_Board.Cols;
        }

        public void PlayComputerMove()
        {
            int     col = 0;
            bool    winMoveForComputer;
            bool    winMoveForOpp;
            bool    foundMove = false;

            winMoveForComputer = r_Board.IsWinningMove(ref col, eCellType.Player2);
            if (!winMoveForComputer)
            {
                winMoveForOpp = r_Board.IsWinningMove(ref col, eCellType.Player1);
                if (!winMoveForOpp)
                {
                    while (!foundMove)
                    {
                        col = m_Random.Next(0, r_Board.Cols);
                        foundMove = r_Board.IsColumnAvailable(col);
                    }
                }
            }
            PlayMove(col);
        }

        private void tieCheckAndHandle()
        {
            m_CounterToCheckIfTie--;
            m_IsTie = m_CounterToCheckIfTie == 0 && !m_IsWin;
        }

        public int BoardRows
        {
            get { return r_Board.Rows; }
        }

        public int BoardCols
        {
            get { return r_Board.Cols; }
        }

        public string Player1Name
        {
            get {  return r_Players[0].Name; }
        }

        public string Player2Name
        {
            get {  return r_Players[1].Name; }
        }

        public int Player1Score
        {
            get { return r_Players[0].Score; }
        }

        public int Player2Score
        {
            get { return r_Players[1].Score; }
        }

        public string CurrentPlayerTurnName
        {
            get { return r_Players[(int)m_CurrentPlayerTurn].Name; }
        }

        public int CurrentPlayerTurn
        {
            get { return (int)m_CurrentPlayerTurn; }
        }

        public bool IsWin
        {
            get { return m_IsWin; }
        }

        public bool IsTie
        {
            get { return m_IsTie; }
        }

        public bool OpponentIsComputer
        {
            get { return r_Players[1].Type == ePlayerType.Computer; }
        }
    }
}
