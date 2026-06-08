using System;
using System.Collections.Generic;
using System.Text;

namespace Ex05.GameLogic
{
    public class Board
    {
        private readonly eCellType[,] r_BoardMatrix;
        private readonly int r_Rows;
        private readonly int r_Cols;

        public Board(int i_Rows, int i_Cols)
        {
            r_Rows = i_Rows;
            r_Cols = i_Cols;
            r_BoardMatrix = new eCellType[i_Rows, i_Cols];
        }

        public void InitializeBoard()
        {
            for (int i = 0; i < r_Rows; i++)
            {
                for (int j = 0; j < r_Cols; j++)
                {
                    r_BoardMatrix[i, j] = eCellType.Empty;
                }
            }
        }

        public bool TryInsertCoinToCol(int i_ColIndex, out int o_RowIndex, ePlayerNum i_PlayerNum, out bool o_WinFlag)
        {
            int         i;
            bool        insertSucceeded = false;
            eCellType   cellType = playerNumToCellType(i_PlayerNum);

            o_RowIndex = -1;
            o_WinFlag = false;

            for (i = r_Rows - 1; i >= 0; i--)
            {
                if (r_BoardMatrix[i, i_ColIndex] == eCellType.Empty)
                {
                    r_BoardMatrix[i, i_ColIndex] = cellType;
                    insertSucceeded = true;
                    o_RowIndex = i;
                    o_WinFlag = checkIfWin(i, i_ColIndex, cellType);
                    break;
                }
            }

            return insertSucceeded;
        }

        private bool checkIfWin(int i_Row, int i_col, eCellType i_PlayerNum)
        {
            bool winFlag = false;
            winFlag = checkHorizontalWin(i_Row, i_PlayerNum) ||
                      checkVerticalWin(i_Row, i_col, i_PlayerNum) ||
                      checkDiagonalWin(i_Row, i_col, i_PlayerNum);

            return winFlag;
        }

        private bool checkHorizontalWin(int i_Row, eCellType i_PlayerNum)
        {
            int     count = 0;
            bool    winFlag = false;

            for (int i = 0; i < r_Cols; i++)
            {
                if (isFourInARow(r_BoardMatrix[i_Row, i], i_PlayerNum, ref count))
                {
                    winFlag = true;
                    break;
                }
            }

            return winFlag;
        }

        private bool checkVerticalWin(int i_Row, int i_Col, eCellType i_PlayerNum)
        {
            int     count = 0;
            bool    winFlag = false;

            //check if its even possible to have four in a row
            if (i_Row <= r_Rows - 4)
            {
                for (int i = i_Row; i < r_Rows; i++)
                {
                    if (isFourInARow(r_BoardMatrix[i, i_Col], i_PlayerNum, ref count))
                    {
                        winFlag = true;
                        break;
                    }
                }
            }

            return winFlag;
        }

        private bool checkDiagonalWin(int i_Row, int i_Col, eCellType i_PlayerNum)
        {
            return (checkDiagonalWinFromTopLeftCorner(i_Row, i_Col, i_PlayerNum) ||
                    checkDiagonalWinFromTopRightCorner(i_Row, i_Col, i_PlayerNum));
        }

        private bool checkDiagonalWinFromTopRightCorner(int i_Row, int i_Col, eCellType i_PlayerNum)
        {
            bool    winFlag = false;
            int     count = 0;
            int     distanceToTopRightCell = r_Cols - 1 - i_Col;
            int     stepBack = Math.Min(i_Row, distanceToTopRightCell);
            int     topRightCellRow = i_Row - stepBack;
            int     topRightCellCol = i_Col + stepBack;


            /* check if its even possible to have a four in a row in this diagonal
             * and if so, check for a 4 in a row*/
            if (r_Rows - topRightCellRow >= 4 && topRightCellCol >= 3)
            {
                while (topRightCellRow < r_Rows && topRightCellCol >= 0)
                {
                    if (isFourInARow(r_BoardMatrix[topRightCellRow, topRightCellCol], i_PlayerNum, ref count))
                    {
                        winFlag = true;
                        break;
                    }

                    //move to the next cell in the diagonal
                    topRightCellRow++;
                    topRightCellCol--;
                }
            }
            return winFlag;
        }

        private bool checkDiagonalWinFromTopLeftCorner(int i_Row, int i_Col, eCellType i_PlayerNum)
        {
            bool    winFlag = false;
            int     count = 0;
            int     stepBack = Math.Min(i_Row, i_Col);
            int     topLeftCellRow = i_Row - stepBack;
            int     topLeftCellCol = i_Col - stepBack;

            /* check if its even possible to have a four in a row in this diagonal
             * and if so, check for a 4 in a row*/
            if (r_Rows - topLeftCellRow >= 4 && r_Cols - topLeftCellCol >= 4)
            {
                while (topLeftCellRow < r_Rows && topLeftCellCol < r_Cols)
                {
                    if (isFourInARow(r_BoardMatrix[topLeftCellRow, topLeftCellCol], i_PlayerNum, ref count))
                    {
                        winFlag = true;
                        break;
                    }

                    //move to the next cell in the diagonal
                    topLeftCellRow++;
                    topLeftCellCol++;
                }
            }

            return winFlag;
        }

        /* function that gets a cell, a target sign, and a counter. 
         * if the counter reaches 4, it returns true to end the loop in the "check win" functions type*/
        private bool isFourInARow(eCellType i_CurrentCell, eCellType i_TargetSign, ref int io_count)
        {
            bool winFlag = false;

            if (i_CurrentCell == i_TargetSign)
            {
                io_count++;
                if (io_count == 4)
                {
                    winFlag = true;
                }
            }
            else
            {
                io_count = 0;
            }

            return winFlag;
        }

        public int Rows
        {
            get { return r_Rows; } 
        }

        public int Cols
        {
            get { return r_Cols; }
        }

        private eCellType playerNumToCellType(ePlayerNum i_PlayerNum)
        {
            eCellType res;

            if (i_PlayerNum == ePlayerNum.Player1)
            {
                res = eCellType.Player1;
            }
            else
            {
                res = eCellType.Player2;
            }

            return res;
        }

        public bool IsWinningMove(ref int o_Col, eCellType i_TypeToCheckIfWin)
        {
            bool isWinningMove = false;

            for (int j = 0; j < r_Cols; j++)
            {
                for (int i = r_Rows - 1; i >= 0; i--)
                {
                    if (r_BoardMatrix[i, j] == eCellType.Empty)
                    {
                        r_BoardMatrix[i, j] = i_TypeToCheckIfWin;
                        
                        if (checkIfWin(i, j, i_TypeToCheckIfWin))
                        {
                            o_Col = j;
                            isWinningMove = true;
                        }

                        r_BoardMatrix[i, j] = eCellType.Empty;
                        break;
                    }
                }
                if (isWinningMove)
                {
                    break;
                }
            }

            return isWinningMove;
        }

        public bool IsColumnAvailable(int i_Col)
        {
            bool avilable = false;

            for (int i = r_Rows - 1; i >= 0; i--)
            {
                if (r_BoardMatrix[i, i_Col] == eCellType.Empty)
                {
                    avilable = true;
                    break;
                }
            }

            return avilable;
        }
    }
}

