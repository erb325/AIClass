/* Implements the Eight Puzzle game, using a 2D array of Buttons. Click on a button adjacent to the blank space to 
 *     slide it into the blank space.
 *     
 *   written by Erik Wynters  */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EightPuzzle
{
    public partial class Form1 : Form
    {
        Button[,] board = new Button[3,3];         // the current state of the game board
        Button[,] solution = new Button[3,3];      // the desired state
        int score = 0;                             // number of moves

        int[] moves;
        int nextMoveIndex = 0;
        int holeRow = 0;
        int holeCol = 0;
        int movingRow, movingCol;
        float slide_fraction = 0.0f;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int row = 0; row < 3; row++)
                for (int col = 0; col < 3; col++)
                {
                    int number = 3 * row + col + 1;
                    Button btn;

                    if (number == 9)
                        btn = btnBlank;
                    else
                    {
                        btn = new Button();
                        btn.Width = 150;
                        btn.Height = 150;
                        btn.Left = col * 150;
                        btn.Top = row * 150;
                        btn.Font = btnBlank.Font;
                        btn.Text = number.ToString();
                        btn.Name = "btn" + number.ToString();
                        Controls.Add(btn);
                        btn.Click += new EventHandler(btnBlank_Click);
                    }

                    solution[row, col] = btn;
                }

            btnBlank.Parent = null;

            // create initial game board by scrambling solution
            for (int row = 0; row < 3; row++)
                for (int col = 0; col < 3; col++)
                    board[row, col] = solution[(row + 2) % 3, (col + 2) % 3];

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            for (int row = 0; row < 3; row++)
                for (int col = 0; col < 3; col++)
                {
                    Button btn = board[row, col];
                    if (row==movingRow && col==movingCol)
                    {
                        btn.Top = (int)(slide_fraction * holeRow * 150 + (1 - slide_fraction) * row * 150);
                        btn.Left = (int)(slide_fraction * holeCol * 150 + (1 - slide_fraction) * col * 150);
                    }
                    else
                    {
                        btn.Top = row * 150;
                        btn.Left = col * 150;
                    }
                    
                }

        }

        private void SwapWithHole(int row, int col)
        {
            Button btn = board[row, col];
            movingRow = row;
            movingCol = col;
            for (slide_fraction = 0.0f; slide_fraction < 1.0f; slide_fraction += 0.02f)
            {
                Refresh();
            }
            board[row, col] = board[holeRow, holeCol];
            board[holeRow, holeCol] = btn;
            
            holeRow = row;
            holeCol = col;
        }

        // handle clicks on all 8 buttons
        private void btnBlank_Click(object sender, EventArgs e)
        {
            Button btn = (Button) sender;  // which button was clicked
            int x = btn.Left;
            int y = btn.Top;
            int bx = btnBlank.Left;
            int by = btnBlank.Top;

            // check whether button was adjacent to the blank, i.e., a legal move
            if ((x == bx && Math.Abs(y - by) == 150) || (y == by && Math.Abs(x - bx) == 150))
            {
                int row = y / 150;
                int col = x / 150;
                SwapWithHole(row, col);
                score += 1;
                this.Refresh();

                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                        if( board[i, j] != solution[i, j])
                            return;  // if the board differs from solution, stop

                MessageBox.Show("Congratulations. You solved the puzzle! Your score is " + score);
            }

            else
                MessageBox.Show("You cannot slide that tile now.", "Error"); 
        }

        
        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (moves[nextMoveIndex])
            {
                case 0: //up
                    SwapWithHole(holeRow - 1, holeCol); break;
                case 1: //down
                    SwapWithHole(holeRow + 1, holeCol); break;
                case 2: //left
                    SwapWithHole(holeRow, holeCol - 1); break;
                case 3: //right
                    SwapWithHole(holeRow, holeCol + 1); break;
            }
            Refresh();
            nextMoveIndex += 1;
            score += 1;
            if (nextMoveIndex == moves.Length)
            {
                timer1.Stop();
                MessageBox.Show("Congratulations. You solved the puzzle! Your score is " + score);
            }             
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var result = EightPuzzleSearch.gbfs(EightPuzzleSearch.theproblem);
            moves = EightPuzzleSearch.extractSolution(result);

            for (int row = 0; row < 3; row++)
                for (int col = 0; col < 3; col++)
                    board[row, col] = solution[(row + 2) % 3, (col + 2) % 3];

            slide_fraction = 0.0f;
            score = 0;
            Refresh();
            timer1.Start();
            nextMoveIndex = 0;
            holeRow = 0;
            holeCol = 0;
        }


    }
}
