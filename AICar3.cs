using System;
using SplashKitSDK;

public class AICar3 : Car, IMoveable
{
    public AICar3()
    {
        _CarBitmap = SplashKit.BitmapNamed("AICar3");
        Y = -_CarBitmap.Height;
    }

    public override void Move()
    {
        Y += speed * 2;
    }
}