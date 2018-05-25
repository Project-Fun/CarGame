using System;
using SplashKitSDK;

public class AICar3 : AI, IMovable
{
    public AICar3()
    {
        CarBitmap = SplashKit.BitmapNamed("AICar3");
        Y = -CarBitmap.Height;
    }

    public override void Move()
    {
        Y += Speed / 3 * 5;
    }
}