using System;
using SplashKitSDK;

public class Reward2 : AI, IMovable
{
    public Reward2()
    {
        CarBitmap = SplashKit.BitmapNamed("Reward2");
        Y = -CarBitmap.Height;
    }

    public override void Move()
    {
        Y += Speed * 2;
    }
}