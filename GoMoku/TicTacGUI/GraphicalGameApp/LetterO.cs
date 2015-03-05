using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

/**
    A graphical version of the letter O that can draw itself.
*/
public class LetterO
{
    private int topLeftX;     // x-coordinate of top-left corner of bounding box
    private int topLeftY;     // y-coordinate of top-left corner of bounding box
    private Color drawColor;  // color used to draw the O
    private Color background; // color inside the O
    private int size;         // size of bounding square

    public LetterO(int x, int y, Color c, Color bg, int drawSize)
    {
        topLeftX = x;
        topLeftY = y;
        drawColor = c;
        background = bg;
        size = drawSize;
    }

    public void draw(Graphics g)
    {

        g.FillEllipse(new SolidBrush(drawColor), topLeftX + size / 20, topLeftY + size / 20, size * 9 / 10, size * 9 / 10);

        int inner = size / 5;
        g.FillEllipse(new SolidBrush(background), topLeftX + inner, topLeftY + inner, size - 2 * inner, size - 2 * inner);
    }
}