using System;
using SplashKitSDK;

public class Reward2 : Car, IMoveable
{
    public Reward2()
    {
        _CarBitmap = SplashKit.BitmapNamed("Reward2");
        Y = -_CarBitmap.Height;
    }

    public override void Move()
    {
        Y += speed * 2;
    }
}