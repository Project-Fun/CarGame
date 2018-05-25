using System;
using SplashKitSDK;
using System.Collections.Generic;

public class RaceGame
{
    private Window _window;
    private Player _player;
    private Map _map;
    private List<AI> _ai = new List<AI>();
    private Timer _timer;
    private uint _doubleSpeedTime;
    private uint _invincibleTime;
    private int _score;
    private int _level;
    private int _basicSpeed;
    private bool _reward1;
    private bool _reward2;
    public bool Restart;
    private bool _addNew;
    private bool[] _lane = new bool[5];
    public delegate string GetBitmapName(Bitmap bitmap);
    public bool ESC
    {
        get
        {
            return _player.Quit;
        }
    }
    public void LoadResource()
    {
        SplashKit.LoadBitmap("Player1", "PlayerCar1.png");
        SplashKit.LoadBitmap("Player2", "PlayerCar2.png");
        SplashKit.LoadBitmap("Player1S", "PlayerCar1S.png");
        SplashKit.LoadBitmap("Player2S", "PlayerCar2S.png");
        SplashKit.LoadBitmap("AICar1", "AICar1.png");
        SplashKit.LoadBitmap("AICar2", "AICar2.png");
        SplashKit.LoadBitmap("AICar3", "AICar3.png");
        SplashKit.LoadBitmap("Reward1", "fireball.png");
        SplashKit.LoadBitmap("Reward2", "goldegg.png");
        SplashKit.LoadBitmap("Road1", "Road1.png");
        SplashKit.LoadBitmap("Road2", "Road2.png");
        SplashKit.LoadBitmap("Road3", "Road3.png");
        SplashKit.LoadBitmap("Cactus", "Cactus.png");
        SplashKit.LoadBitmap("Key", "Key.png");
        SplashKit.LoadFont("FontC", "calibri.ttf");
        SplashKit.LoadFont("FontU", "unknown.ttf");
        SplashKit.LoadFont("FontJ", "jeebra.ttf");
    }

    public RaceGame(Window w)
    {
        _window = w;
        LoadResource();
        _player = new Player(_window);
        _map = new Map(_window);
        _timer = new Timer("gameTimer");
        _timer.Start();
    }

    public void Update()
    {
        _player.Move();
        _map.Move();
        foreach (AI ai in _ai)
        {
            ai.Move();
        }
        Collision();
        CheckOverLine();
        CheckReward();
        AddNewCar();
        RemoveAI();
        LaneStatus();
        Level();
        SetSpeed();
    }

    public void Draw()
    {
        DrawUI();
        _map.Draw();
        PlayerTwinkle();
        foreach (AI ai in _ai)
        {
            ai.Draw();
        }
    }

    public void DrawUI()
    {
        _window.DrawText($"Your Score: {_score} M", Color.Black, SplashKit.FontNamed("FontU"), 20, 20, 20);
        _window.DrawText($"Level {_level}", Color.Black, SplashKit.FontNamed("FontJ"), 40, 20, 250);
        _window.DrawBitmap(SplashKit.BitmapNamed("Key"), 0, 400);

        _window.DrawBitmap(SplashKit.BitmapNamed("Reward1"), 600, 150);
        _window.DrawText("SpeedUp", Color.Black, SplashKit.FontNamed("FontJ"), 20, 660, 210);
        if (_reward1)
            _window.DrawText($"Time left: {(_doubleSpeedTime - _timer.Ticks) / 1000} s", Color.Red, SplashKit.FontNamed("FontU"), 20, 660, 250);

        _window.DrawBitmap(SplashKit.BitmapNamed("Reward2"), 600, 300);
        _window.DrawText("Invinciable", Color.Black, SplashKit.FontNamed("FontJ"), 20, 660, 360);
        if (_reward2)
            _window.DrawText($"Time left: {(_invincibleTime - _timer.Ticks) / 1000} s", Color.Red, SplashKit.FontNamed("FontU"), 20, 660, 400);
    }

    public void Level()
    {
        _basicSpeed = 3 + _level;
        _score += _basicSpeed;
        _level = Convert.ToInt32(_timer.Ticks) / 20000;
    }

    public void SetSpeed()
    {
        if (_reward1)
        {
            _map.CactusSpeed = (_basicSpeed + 2) * 2;
            foreach (AI ai in _ai)
            {
                ai.Speed = _basicSpeed * 2;
            }
        }
        else
        {
            _map.CactusSpeed = _basicSpeed + 2;
            foreach (AI ai in _ai)
            {
                ai.Speed = _basicSpeed;
            }
        }
    }

    public void CheckReward()
    {
        _reward1 = (_timer.Ticks < _doubleSpeedTime);
        _reward2 = (_timer.Ticks < _invincibleTime);
        InvincibleBitmap(_player.CarBitmap, bitmap => SplashKit.BitmapName(bitmap));
    }

    public void InvincibleBitmap(Bitmap player, GetBitmapName getname)
    {
        if (_reward2)
        {
            if (getname(player) == "Player1")
            {
                _player.CarBitmap = SplashKit.BitmapNamed("Player1S");
            }
            if (getname(player) == "Player2")
            {
                _player.CarBitmap = SplashKit.BitmapNamed("Player2S");
            }
        }
        else
        {
            if (getname(player) == "Player1S")
            {
                _player.CarBitmap = SplashKit.BitmapNamed("Player1");
            }
            if (getname(player) == "Player2S")
            {
                _player.CarBitmap = SplashKit.BitmapNamed("Player2");
            }
        }
    }

    public void PlayerTwinkle()
    {
        if (_invincibleTime - _timer.Ticks > 700 && _invincibleTime - _timer.Ticks < 1000) { }
        else if (_invincibleTime - _timer.Ticks > 1700 && _invincibleTime - _timer.Ticks < 2000) { }
        else if (_invincibleTime - _timer.Ticks > 2700 && _invincibleTime - _timer.Ticks < 3000) { }
        else
        {
            _player.Draw();
        }
    }

    public void Collision()
    {
        foreach (AI ai in _ai)
        {
            if (ai.ColliedWith(_player))
            {
                if (SplashKit.BitmapName(ai.CarBitmap) == "Reward1")
                {
                    if (!_reward1)
                    {

                        _doubleSpeedTime = _timer.Ticks + 10000;
                    }
                    else
                        _doubleSpeedTime += 10000;
                }
                else if (SplashKit.BitmapName(ai.CarBitmap) == "Reward2")
                {
                    if (!_reward2)
                    {
                        _invincibleTime = _timer.Ticks + 10000;
                    }
                    else
                        _invincibleTime += 10000;
                }
                else if (!_reward2 && SplashKit.BitmapName(ai.CarBitmap) != "Reward1")
                {
                    SplashKit.DisplayDialog("GameOver", $"Your Score is: {_score} m", SplashKit.FontNamed("FontC"), 20);
                    Restart = true;
                }
            }
        }
    }

    public void LaneStatus()
    {
        foreach (AI ai in _ai)
        {
            _lane[ai.Lane - 1] = true;
        }
    }

    public void CheckOverLine()
    {
        foreach (AI ai in _ai)
        {
            if (ai.IsOverLine != true && ai.Y > _window.Height / 4)
            {
                _addNew = true;
                ai.IsOverLine = true;
            }
        }
    }

    public bool CheckLane(AI ai)
    {
        if (_lane[ai.Lane - 1])
        {
            ai = null;
            return false;
        }
        return true;
    }

    public void AddNewCar()
    {
        if (_addNew == true)
        {
            RandomAI();
            _addNew = false;
        }
        // else if (_lane[0] == false && _lane[1] == false && _lane[2] == false && _lane[3] == false && _lane[4] == false)
        else if (!(_lane[0] || _lane[1] || _lane[2] || _lane[3] || _lane[4]))
        {
            RandomAI();
        }
    }

    public void RandomAI()
    {
        bool rightLane = false;
        while (!rightLane)
        {
            double rnd = SplashKit.Rnd();
            if (rnd > 0.4)
            {
                AI newAI = new AICar1();
                if (CheckLane(newAI))
                {
                    _ai.Add(newAI);
                    rightLane = true;
                }
            }
            if (rnd <= 0.4 && rnd > 0.2)
            {
                AI newAI = new AICar2();
                if (CheckLane(newAI))
                {
                    _ai.Add(newAI);
                    rightLane = true;
                }
            }
            if (rnd <= 0.2 && rnd > 0.1)
            {
                AI newAI = new AICar3();
                if (CheckLane(newAI))
                {
                    _ai.Add(newAI);
                    rightLane = true;
                }
            }
            if (rnd <= 0.1 && rnd > 0.03)
            {
                AI newAI = new Reward1();
                if (CheckLane(newAI))
                {
                    _ai.Add(newAI);
                    rightLane = true;
                }
            }
            if (rnd <= 0.03)
            {
                AI newAI = new Reward2();
                if (CheckLane(newAI))
                {
                    _ai.Add(newAI);
                    rightLane = true;
                }
            }
        }
    }

    public void RemoveAI()
    {
        List<AI> _uselessAI = new List<AI>();
        foreach (AI ai in _ai)
        {
            if (ai.Y > _window.Height || ai.ColliedWith(_player))
            {
                _uselessAI.Add(ai);
                _lane[ai.Lane - 1] = false;
            }
        }
        foreach (AI r in _uselessAI)
        {
            _ai.Remove(r);
        }
    }
}