using System;
using SplashKitSDK;

public class Reward1 : AI, IMovable
{
    public Reward1()
    {
        CarBitmap = SplashKit.BitmapNamed("Reward1");
        Y = -CarBitmap.Height;
    }

    public override void Move()
    {
        Y += Speed * 2;
    }
}