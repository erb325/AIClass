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
        int ply = 1;

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
                if (TicTac.utility(game.CurrentState, "Max") > 0)
                    MessageBox.Show("Game over - You won!");
                else if (TicTac.utility(game.CurrentState, "Min") < 0)
                    MessageBox.Show("Game over - You lost :(");
                else
                    MessageBox.Show("Game over - It's a draw.");
                gameOver = true;
            }
        }

        private void update()
        {
            using (Graphics g = this.canvas.CreateGraphics())
            {
                var board = game.CurrentState;
                for (int row = 0; row < 19; row++)
                {
                    for (int col = 0; col < 19; col++)
                    {
                        if (board[row, col] == 1)
                            g.FillEllipse(Brushes.Black, col * 40, row * 40, 40, 40);
                        else if (board[row, col] == -1)
                            g.FillEllipse(Brushes.White, col * 40, row * 40, 40, 40);

                    }
                }
            }

            CheckForWin();
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            var board = game.CurrentState;
            for (int row = 0; row < 19; row++)
            {
                if (row > 0)
                    e.Graphics.DrawLine(Pens.Black, 0, row * 40, 760, row * 40);
                for (int col = 0; col < 19; col++)
                {
                    if (col > 0)
                        e.Graphics.DrawLine(Pens.Black, col * 40, 0, col * 40, 760);
                    if (board[row, col] == 1)
                        e.Graphics.FillEllipse(Brushes.Black, col * 40, row * 40, 40, 40);
                    else if (board[row, col] == -1)
                        e.Graphics.FillEllipse(Brushes.White, col * 40, row * 40, 40, 40);

                }
            }
        }

        private void canvas_MouseClick(object sender, MouseEventArgs e)
        {
            int x = e.X;
            int y = e.Y;

            int row = y / 40;
            int col = x / 40;

            if (game.CurrentState[row, col] != 0)
            {
                MessageBox.Show("You can't place your pebble there. Please try again.");
                return;
            }

            if (!gameOver)
            {

                game = TicTac.makeMove(row, col, "Max", 0, game);
                txtMessage.Text = "Hold on, I'm thinking ...";
                txtMessage.Refresh();
                update();


                if (!gameOver)
                {
                    game = TicTac.chooseMove(game, ply);
                    txtMessage.Text = "Your turn; click on a square.";
                    update();
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
                ply = 1;
            else
                ply = 2;
        }
    }
}