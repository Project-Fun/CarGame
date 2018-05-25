using System;
using SplashKitSDK;

public class AICar1 : AI, IMovable
{
    public AICar1()
    {
        CarBitmap = SplashKit.BitmapNamed("AICar1");
        Y = -CarBitmap.Height;
    }

    public override void Move()
    {
        Y += Speed;
    }
}