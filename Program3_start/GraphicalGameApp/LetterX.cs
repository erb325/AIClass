using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
/**
    A graphical version of the letter X that can draw itself.
*/
public class LetterX
{
    private int topLeftX;    // x-coordinate of top-left corner of bounding box
    private int topLeftY;    // y-coordinate of top-left corner of bounding box
    private Color drawColor; // color used to
    private int size;        // size of bounding square


    public LetterX(int x, int y, Color c, int drawSize)
    {
        size = drawSize;
        topLeftX = x;
        topLeftY = y;
        topLeftX = x + size / 10;
        topLeftY = y + size / 10;
        size = drawSize * 8 / 10;
        drawColor = c;
    }

    public void draw(Graphics g)
    {
        int[] x1 = new int[4];
        int[] y1 = new int[4];
        int[] x2 = new int[4];
        int[] y2 = new int[4];
        x1[0] = topLeftX;
        x1[1] = topLeftX + size / 4;
        x1[2] = topLeftX + size;
        x1[3] = topLeftX + size - size / 4;
        y1[0] = topLeftY;
        y1[1] = topLeftY;
        y1[2] = topLeftY + size;
        y1[3] = topLeftY + size;
        x2[0] = topLeftX;
        x2[1] = topLeftX + size / 4;
        x2[2] = topLeftX + size;
        x2[3] = topLeftX + size - size / 4;
        y2[0] = topLeftY + size;
        y2[1] = topLeftY + size;
        y2[2] = topLeftY;
        y2[3] = topLeftY;

        Point[] pts1 = new Point[4];
        Point[] pts2 = new Point[4];
        for (int i = 0; i < 4; i++)
        {
            pts1[i] = new Point(x1[i], y1[i]);
            pts2[i] = new Point(x2[i], y2[i]);
        }
        g.FillPolygon(new SolidBrush(drawColor), pts1);
        g.FillPolygon(new SolidBrush(drawColor), pts2);
    }
}
