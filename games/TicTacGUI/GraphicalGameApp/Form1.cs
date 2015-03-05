using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TicTacApp
{
    public partial class Form1 : Form
    {
        TicTac.MatrixGame game;
        bool gameOver;
       
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            game = TicTac.game;
            txtMessage.Text = "Your turn; click on a square.";
            gameOver = false;
        }

        private void CheckForWin()
        {
            if (TicTac.gameOver(game.CurrentState))
            {
                if (TicTac.utility(game.CurrentState, "Max")>0)
                    MessageBox.Show("Game over - You won!");
                else if (TicTac.utility(game.CurrentState, "Min")<0)
                    MessageBox.Show("Game over - You lost :(");
                else
                    MessageBox.Show("Game over - It's a draw.");
                gameOver = true;
            }
        } 

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            var board = game.CurrentState;
            for (int row = 0; row < 3; row++)
            {
                if (row > 0)
                    e.Graphics.DrawLine(Pens.Black, 0, row*100, 300, row*100);
                for (int col = 0; col < 3; col++)
                {
                    if (col > 0)
                        e.Graphics.DrawLine(Pens.Black, col * 100, 0, col * 100, 300);
                    if (board[row, col] == 1)
                        e.Graphics.DrawString("X", canvas.Font, Brushes.Red, col * 100, row * 100);
                    else if (board[row, col] == -1)
                        e.Graphics.DrawString("O", canvas.Font, Brushes.Blue, col * 100, row * 100);

                }
            }
            CheckForWin();  
        }

        private void canvas_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;

            int row = y / 100;
            int col = x / 100;

            if (!gameOver)
            {
                    
                game = TicTac.makeMove(row, col, "Max", 0, game);
                Refresh();

                if (!gameOver)
                {
                    txtMessage.Text = "Hold on, I'm thinking ...";
                    Refresh();

                    game = TicTac.chooseMove(game);
                    txtMessage.Text = "Your turn; click on a square.";
                    Refresh();
                }
            }
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            game = TicTac.game;
            txtMessage.Text = "Your turn; click on a square.";
            gameOver = false;
            Refresh();
        }
    }
}
