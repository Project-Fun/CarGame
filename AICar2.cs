using System;
using SplashKitSDK;

public class AICar2 : Car, IMoveable
{
    public AICar2()
    {
        _CarBitmap = SplashKit.BitmapNamed("AICar2");
        Y = -_CarBitmap.Height;
    }

    public override void Move()
    {
        Y += speed / 2 * 3;
    }
}