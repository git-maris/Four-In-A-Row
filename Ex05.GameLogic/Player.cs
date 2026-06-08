using Ex05.GameLogic;

namespace Ex05.GameLogic
{
    public class Player
    {
        private readonly ePlayerNum r_PlayerNum;
        private readonly ePlayerType r_PlayerType;
        private readonly string r_PlayerName;
        private int m_Score = 0;

        public Player(ePlayerType i_PlayerType, ePlayerNum i_PlayerNum, string i_PlayerName)
        {
            r_PlayerType = i_PlayerType;
            r_PlayerNum = i_PlayerNum;
            r_PlayerName = i_PlayerName;
        }

        public ePlayerType Type
        {
            get { return r_PlayerType; }
        }

        public int Score
        {
            get { return m_Score; }
        }

        public string Name
        {
            get { return r_PlayerName; }
        }

        public void IncreaseScore()
        {
            m_Score++;
        }
    }
}