using System;
using SplashKitSDK;

public class Reward1 : Car, IMoveable
{
    public Reward1()
    {
        _CarBitmap = SplashKit.BitmapNamed("Reward1");
        Y = -_CarBitmap.Height;
    }

    public override void Move()
    {
        Y += speed * 2;
    }
}