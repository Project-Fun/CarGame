using System;
using SplashKitSDK;

public class AICar1 : Car, IMoveable
{
    public AICar1()
    {
        _CarBitmap = SplashKit.BitmapNamed("AICar1");
        Y = -_CarBitmap.Height;
    }

    public override void Move()
    {
        Y += speed;
    }
}