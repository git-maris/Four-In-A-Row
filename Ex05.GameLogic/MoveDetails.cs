namespace Ex05.GameLogic
{
    public struct MoveDetails
    {
        public readonly int r_Row { get; }
        public readonly int r_Col { get; }
        public readonly ePlayerNum r_PlayerNum { get; }

        public MoveDetails(int i_Row, int i_Col, ePlayerNum i_Player)
        {
            r_Row = i_Row;
            r_Col = i_Col;
            r_PlayerNum = i_Player;
        }

        public int Row
        {
            get { return r_Row; }
        }

        public int Col
        {
            get { return r_Col; }
        }

        public ePlayerNum PlayerNum
        {
            get { return r_PlayerNum; }
        }
    }
}

