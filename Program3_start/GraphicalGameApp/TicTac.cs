using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

public class TicTac : State
{
    public char[,] board = new char[19, 19];

    public TicTac()
    {
        for (int row = 0; row < 3; row++)
            for (int col = 0; col < 3; col++)
                board[row, col] = '.';
    }

    public override bool equals(State other)
    {
        TicTac t = (TicTac)other;
        for (int row = 0; row < 3; row++)
            for (int col = 0; col < 3; col++)
                if (board[row, col] != t.board[row, col]) return false;
        return true;
    }

    public override string toString()
    {   // don't really need for graphical games; use draw instead
        // but can implement this for debugging purposes if you want
        return "not implemented";
    }

    public void setMove(char player, int r, int c)
    {
        board[r, c] = player;
    }

    // Draws the board on a graphics context assumed to be 180 x 180 pixels,
    // with background color bg using graphical classes LetterX and LetterO.
    public void draw(Graphics g, Color bg)
    {
        //g.setColor(Color.black);
        for (int row = 0; row < 3; row++)
        {
            if (row != 0)
                g.DrawLine(Pens.Black, 0, row * 100, 300, row * 100); // horiz. line
            for (int col = 0; col < 3; col++)
            {
                if (col != 0)
                    g.DrawLine(Pens.Black, col * 100, 0, col * 100, 3000); // vert. line
                if (board[row, col] == 'X')
                {
                    LetterX lx = new LetterX(col * 100, row * 100, Color.Red, 100);
                    lx.draw(g);
                }
                else if (board[row, col] == 'O')
                {
                    LetterO lo = new LetterO(col * 100, row * 100, Color.Blue, bg, 100);
                    lo.draw(g);
                }
            }
        }
    }

    // helper function used by terminalTest
    // returns whether the board's state is such that player c ('X' or 'O')
    // has already won
    public bool wonBy(char c)
    {
        int row, col, count;

        // check for 3 in a row, horizontally
        for (row = 0; row < 3; row++)
        {
            count = 0;
            for (col = 0; col < 3; col++)
                if (board[row, col] == c)
                    count++;
            if (count == 3)
                return true;
        }

        // check for 3 in a row, vertically
        for (col = 0; col < 3; col++)
        {
            count = 0;
            for (row = 0; row < 3; row++)
                if (board[row, col] == c)
                    count++;
            if (count == 3)
                return true;
        }

        // check for 3 in a row, diagonally top-left to bot-right
        count = 0;
        for (row = 0; row < 3; row++)
        {
            col = row;
            if (board[row, col] == c)
                count++;
        }
        if (count == 3)
            return true;


        // check for 3 in a row, diagonally top-right to bot-left
        count = 0;
        for (row = 0; row < 3; row++)
        {
            col = 2 - row;
            if (board[row, col] == c)
                count++;
        }
        if (count == 3)
            return true;

        // if it got this far c doesn't have 3 in a row
        return false;
    }

}
