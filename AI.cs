using System;
using SplashKitSDK;

public abstract class AI : IMovable
{
    public Bitmap CarBitmap;
    public double X;
    public double Y;
    public double Speed;
    public int Lane;
    public bool IsOverLine;
    public AI()
    {
        double r = SplashKit.Rnd();
        if (r < 0.2)
        {
            X = Map.LANE_LEFT;
            Lane = 1;
        }
        if (r >= 0.2 && r < 0.4)
        {
            X = Map.LANE_LEFT + Map.LANE_WIDTH;
            Lane = 2;
        }
        if (r >= 0.4 && r < 0.6)
        {
            X = Map.LANE_LEFT + Map.LANE_WIDTH * 2;
            Lane = 3;
        }
        if (r >= 0.6 && r < 0.8)
        {
            X = Map.LANE_LEFT + Map.LANE_WIDTH * 3;
            Lane = 4;
        }
        if (r >= 0.8)
        {
            X = Map.LANE_LEFT + Map.LANE_WIDTH * 4;
            Lane = 5;
        }
    }

    public void Draw()
    {
        CarBitmap.Draw(X, Y);
    }

    public virtual void Move() { }

    public bool ColliedWith(Player p)
    {
        return CarBitmap.BitmapCollision(X, Y, p.CarBitmap, p.X, p.Y);
    }
}