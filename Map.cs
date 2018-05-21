using System;
using SplashKitSDK;

public class Map : IMoveable
{
    private Bitmap _roadBitmap;
    private Bitmap _cactusBitmap;
    private Timer _myTimer;
    private Window _gameWindow;
    private double _cactus1X;
    private double _cactus1Y;
    private double _cactus2X;
    private double _cactus2Y;
    public int cactusSpeed;
    public const int LANE_LEFT = 250;
    public const int LANE_WIDTH = 60;
    private const int GAP = 5;

    public Map(Window window)
    {
        _cactusBitmap = SplashKit.BitmapNamed("Cactus");
        _gameWindow = window;
        _myTimer = new Timer("timer");
        _myTimer.Start();
        _cactus1X = LANE_LEFT - _cactusBitmap.Width - GAP;
        _cactus1Y = 0;
        _cactus2X = LANE_LEFT + LANE_WIDTH * 5 + GAP;
        _cactus2Y = -window.Height / 2;
    }

    public void Draw()
    {
        _roadBitmap.Draw((_gameWindow.Width - _roadBitmap.Width) / 2, 0);
        _cactusBitmap.Draw(_cactus1X, _cactus1Y);
        _cactusBitmap.Draw(_cactus2X, _cactus2Y);
    }

    public void Move()
    {
        KeepMoving();
        CactusMove();
    }
    private void KeepMoving()
    {
        if (_myTimer.Ticks < 200)
        {
            _roadBitmap = SplashKit.BitmapNamed("Road1");
        }
        if (_myTimer.Ticks >= 200 && _myTimer.Ticks < 400)
        {
            _roadBitmap = SplashKit.BitmapNamed("Road2");
        }
        if (_myTimer.Ticks >= 400 && _myTimer.Ticks < 600)
        {
            _roadBitmap = SplashKit.BitmapNamed("Road3");
        }
        if (_myTimer.Ticks >= 600)
        {
            _myTimer.Start();
        }
    }

    private void CactusMove()
    {
        _cactus1Y += cactusSpeed;
        if (_cactus1Y >= _gameWindow.Height)
        {
            _cactus1Y = 0;
        }
        _cactus2Y += cactusSpeed;
        if (_cactus2Y >= _gameWindow.Height)
        {
            _cactus2Y = 0;
        }
    }
}