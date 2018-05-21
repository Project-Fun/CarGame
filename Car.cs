using System;
using SplashKitSDK;

public abstract class Car : IMoveable
{
    public Bitmap _CarBitmap;
    public bool IsOverLine = false;
    public double Y;
    public double X;
    public int Lane;
    public int speed;
    public Car()
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

    public virtual void Move() { }
    public void Draw()
    {
        _CarBitmap.Draw(X, Y);
    }

    public bool ColliedWith(Player p)
    {
        return _CarBitmap.BitmapCollision(X, Y, p._CarBitmap, p.X, p.Y);
    }
}