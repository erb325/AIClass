// GameTicTac -- defines states, operators and the game class for 
//               the graphical matrix game, TicTacToe.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;




/*********  The Game class ***********************/
public class GameTicTac : MatrixGame
{
    public TicTac currentState;

    public GameTicTac()
    {
        ROWS = 3; COLS = 3;
        currentState = new TicTac();
        ops.Add(new Move());
        opnames.Add("Move");
    }

    public override bool validMove(State st, int index,
                                int row, int col, string player)
    {
        TicTac s = (TicTac)st;
        return s.board[row, col] == '.';
    }

    public override void move(State st, int index, int row, int col, string player)
    {
        TicTac s = (TicTac)st;
        if (player == "MAX")
            s.setMove('X', row, col);
        else
            s.setMove('O', row, col);
    }

    public override bool terminalTest(State st, string player)
    {
        TicTac s = (TicTac)st;
        if (s.wonBy('X') || s.wonBy('O')) return true;
        int count = 0;
        for (int row = 0; row < 3; row++)
            for (int col = 0; col < 3; col++)
                if (s.board[row, col] == '.') count++;
        return count == 0; //draw
    }

    public override float utility(State st, string player)
    {
        TicTac s = (TicTac)st;
        if (s.wonBy('X'))
            return 1;
        else if (s.wonBy('O'))
            return -1;
        else
            return 0;
    }
}

