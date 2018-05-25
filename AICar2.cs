using System;
using SplashKitSDK;

public class AICar2 : AI, IMovable
{
    public AICar2()
    {
        CarBitmap = SplashKit.BitmapNamed("AICar2");
        Y = -CarBitmap.Height;
    }

    public override void Move()
    {
        Y += Speed / 3 * 4;
    }
}