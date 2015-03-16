class Move : MatrixOperator
{
    public State successorOf(State st, int r, int c, string player)
    {
        TicTac s = (TicTac)st;
        if (s.board[r, c] != '.') return s;
        TicTac t = new TicTac();
        for (int row = 0; row < 3; row++)
            for (int col = 0; col < 3; col++)
                t.board[row, col] = s.board[row, col];
        if (player == "MAX")
            t.board[r, c] = 'X';
        else
            t.board[r, c] = 'O';
        return t;
    }
}