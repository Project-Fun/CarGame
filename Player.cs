using System;
using SplashKitSDK;

public class Player : IMoveable
{
    private Window _gameWindow;
    public Bitmap _CarBitmap;
    public double X;
    public double Y;
    public bool Quit;

    public delegate string GetBitmapName(Bitmap bitmap);

    // GetBitmapName PlayerBitmap = delegate (Bitmap bitmap)
    // {
    //     return SplashKit.BitmapName(bitmap);
    // };

    public void SwtichPlayer(Bitmap player, GetBitmapName getname)
    {
        if (getname(player) == "Player1")
        {
            _CarBitmap = SplashKit.BitmapNamed("Player2");
        }
        if (getname(player) == "Player2")
        {
            _CarBitmap = SplashKit.BitmapNamed("Player1");
        }
        if (getname(player) == "Player1S")
        {
            _CarBitmap = SplashKit.BitmapNamed("Player2S");
        }
        if (getname(player) == "Player2S")
        {
            _CarBitmap = SplashKit.BitmapNamed("Player1S");
        }
    }
    public Player(Window window)
    {
        _CarBitmap = SplashKit.BitmapNamed("Player1");
        _gameWindow = window;
        X = Map.LANE_LEFT + Map.LANE_WIDTH * 2;
        Y = _gameWindow.Height - _CarBitmap.Height;
    }

    public void Draw()
    {
        _CarBitmap.Draw(X, Y);
    }

    public void Move()
    {
        HandleInput();
        StayInTrack();
    }

    private void HandleInput()
    {
        int movement = Map.LANE_WIDTH;
        int speed = 4;
        SplashKit.ProcessEvents();
        if (SplashKit.KeyReleased(KeyCode.RightKey) || SplashKit.KeyReleased(KeyCode.DKey))
        {
            X += movement;
        }
        if (SplashKit.KeyReleased(KeyCode.LeftKey) || SplashKit.KeyReleased(KeyCode.AKey))
        {
            X -= movement;
        }
        if (SplashKit.KeyDown(KeyCode.UpKey) || SplashKit.KeyDown(KeyCode.WKey))
        {
            Y -= speed;
        }
        if (SplashKit.KeyDown(KeyCode.DownKey) || SplashKit.KeyDown(KeyCode.SKey))
        {
            Y += speed;
        }
        if (SplashKit.KeyReleased(KeyCode.LeftCtrlKey))
        {
            SwtichPlayer(_CarBitmap, bitmap => SplashKit.BitmapName(bitmap));
        }
        if (SplashKit.KeyReleased(KeyCode.EscapeKey))
        {
            Quit = true;
        }
    }

    private void StayInTrack()
    {
        if (X > Map.LANE_LEFT + Map.LANE_WIDTH * 4) //the right side of track
        {
            X -= Map.LANE_WIDTH;
        }
        if (X < Map.LANE_LEFT) //the left side of track
        {
            X += Map.LANE_WIDTH;
        }
        if (Y > _gameWindow.Height - _CarBitmap.Height)
        {
            Y = _gameWindow.Height - _CarBitmap.Height;
        }
        if (Y < 0)
        {
            Y = 0;
        }
    }
}