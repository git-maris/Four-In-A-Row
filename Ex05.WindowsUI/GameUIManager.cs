using System;
using System.Windows.Forms;
using Ex05.GameLogic;

namespace Ex05.WindowsUI
{
    public class GameUIManager
    {
        private readonly GameSettingsForm r_GameSettingsForm;
        private readonly GameBoardForm r_GameBoardForm;
        private readonly GameEngine r_GameEngine;

        public GameUIManager()
        {
            int rows;
            int cols;
            string player1Name;
            string player2Name;
            ePlayerType opponentPlayerType;

            r_GameSettingsForm = new GameSettingsForm();

            if (r_GameSettingsForm.ShowDialog() == DialogResult.OK)
            {
                if (r_GameSettingsForm.IsComputer) 
                {
                    opponentPlayerType = ePlayerType.Computer;
                    player2Name = "Computer";
                }
                else
                {
                    opponentPlayerType = ePlayerType.Human;
                    player2Name = r_GameSettingsForm.Player2Name;
                }

                rows = r_GameSettingsForm.BoardRows;
                cols = r_GameSettingsForm.BoardCols;
                player1Name = r_GameSettingsForm.Player1Name;

                r_GameEngine = new GameEngine(rows, cols, opponentPlayerType, player1Name, player2Name);
                r_GameBoardForm = new GameBoardForm(r_GameEngine);
            }
        }

        public void Run()
        {
            if (r_GameBoardForm != null)
            {
                r_GameBoardForm.ShowDialog();
            }
        }
    }
}
