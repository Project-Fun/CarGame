using System;
using SplashKitSDK;
using System.Collections.Generic;

public class RaceGame
{
    private Window _window;
    private Player _player;
    private Map _map;
    private List<Car> _car = new List<Car>();
    private Timer _timer;
    private uint _doubleSpeedTime;
    private uint _invincibleTime;
    private int _score;
    private int _level;
    private int _basicSpeed;
    private bool _addNew;
    private bool _reward1;
    private bool _reward2;
    private bool[] Lane = new bool[5];
    public bool Restart;
    public delegate string GetBitmapName(Bitmap bitmap);

    public bool ESC
    {
        get
        {
            return _player.Quit;
        }
    }

    public RaceGame(Window window)
    {
        LoadResource();
        _window = window;
        _timer = new Timer("time");
        _timer.Start();
        _player = new Player(_window);
        _map = new Map(_window);
    }

    public void UI()
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
        _score += _basicSpeed;
        _level = _score / 3000;
        _basicSpeed = 3 + _level;
    }
    public void Update()
    {
        _player.Move();
        _map.Move();
        foreach (Car ai in _car)
        {
            ai.Move();
        }
        Collision();
        CheckCar();
        CheckReward();
        AddNewCar();
        RemoveCar();
        Level();
        ChangeSpeed();
    }

    public void CheckReward()
    {
        _reward1 = (_timer.Ticks < _doubleSpeedTime);
        _reward2 = (_timer.Ticks < _invincibleTime);
        InvincibleBitmap(_player._CarBitmap, bitmap => SplashKit.BitmapName(bitmap));
    }
    public void ChangeSpeed()
    {
        _map.cactusSpeed = _basicSpeed + 2;
        if (_reward1)
        {
            foreach (Car car in _car)
            {
                car.speed = _basicSpeed * 2;
                _map.cactusSpeed = (_basicSpeed + 2) * 2;
            }
        }
        else
        {
            foreach (Car car in _car)
            {
                car.speed = _basicSpeed;
            }
        }
    }
    public void InvincibleBitmap(Bitmap player, GetBitmapName getname)
    {
        if (_reward2)
        {
            if (getname(player) == "Player1")
            {
                _player._CarBitmap = SplashKit.BitmapNamed("Player1S");
            }
            if (getname(player) == "Player2")
            {
                _player._CarBitmap = SplashKit.BitmapNamed("Player2S");
            }
        }
        else
        {
            if (getname(player) == "Player1S")
            {
                _player._CarBitmap = SplashKit.BitmapNamed("Player1");
            }
            if (getname(player) == "Player2S")
            {
                _player._CarBitmap = SplashKit.BitmapNamed("Player2");
            }
        }
    }

    public void Collision()
    {
        foreach (Car ai in _car)
        {
            if (ai.ColliedWith(_player))
            {
                if (SplashKit.BitmapName(ai._CarBitmap) == "Reward1")
                {
                    if (!_reward1)
                    {

                        _doubleSpeedTime = _timer.Ticks + 10000;
                    }
                    else
                        _doubleSpeedTime += 10000;
                }
                if (SplashKit.BitmapName(ai._CarBitmap) == "Reward2")
                {
                    if (!_reward2)
                    {
                        _invincibleTime = _timer.Ticks + 10000;
                    }
                    else
                        _invincibleTime += 10000;
                }
                else if (!_reward2 && SplashKit.BitmapName(ai._CarBitmap) != "Reward1")
                {
                    SplashKit.DisplayDialog("GameOver", $"Your Score is: {_score} m", SplashKit.FontNamed("FontC"), 20);
                    Restart = true;
                }
            }
        }
    }

    public void Twinkle()
    {
        if (_invincibleTime - _timer.Ticks > 700 && _invincibleTime - _timer.Ticks < 1000) { }
        else if (_invincibleTime - _timer.Ticks > 1700 && _invincibleTime - _timer.Ticks < 2000) { }
        else if (_invincibleTime - _timer.Ticks > 2700 && _invincibleTime - _timer.Ticks < 3000) { }
        else
        {
            _player.Draw();
        }
    }
    public void Draw()
    {
        UI();
        _map.Draw();
        Twinkle();
        foreach (Car ai in _car)
        {
            ai.Draw();
        }
    }

    public void CheckCar()
    {
        foreach (Car ai in _car)
        {
            if (ai.IsOverLine != true && ai.Y > 200)
            {
                _addNew = true;
                ai.IsOverLine = true;
            }
            Lane[ai.Lane - 1] = true;
        }
    }

    public bool CheckLane(Car car)
    {
        if (Lane[car.Lane - 1])
        {
            car = null;
            return false;
        }
        return true;
    }

    public void AddNewCar()
    {
        if (_addNew == true)
        {
            RandomCar();
            _addNew = false;
        }
        else if (Lane[0] == false && Lane[1] == false && Lane[2] == false && Lane[3] == false && Lane[4] == false)
        {
            RandomCar();
        }
    }

    public void RandomCar()
    {
        bool rightLane = false;
        while (!rightLane)
        {
            double rnd = SplashKit.Rnd();
            if (rnd > 0.4)
            {
                Car car = new AICar1();
                if (CheckLane(car))
                {
                    _car.Add(car);
                    rightLane = true;
                }
            }
            if (rnd <= 0.4 && rnd > 0.2)
            {
                Car car = new AICar2();
                if (CheckLane(car))
                {
                    _car.Add(car);
                    rightLane = true;
                }
            }
            if (rnd <= 0.2 && rnd > 0.1)
            {
                Car car = new AICar3();
                if (CheckLane(car))
                {
                    _car.Add(car);
                    rightLane = true;
                }
            }
            if (rnd <= 0.1 && rnd > 0.03)
            {
                Car car = new Reward1();
                if (CheckLane(car))
                {
                    _car.Add(car);
                    rightLane = true;
                }
            }
            if (rnd <= 0.03)
            {
                Car car = new Reward2();
                if (CheckLane(car))
                {
                    _car.Add(car);
                    rightLane = true;
                }
            }
        }
    }

    public void RemoveCar()
    {
        List<Car> _removecar = new List<Car>();
        foreach (Car ai in _car)
        {
            if (ai.Y > _window.Height || ai.ColliedWith(_player))
            {
                _removecar.Add(ai);
                Lane[ai.Lane - 1] = false;
            }
        }
        foreach (Car rai in _removecar)
        {
            _car.Remove(rai);
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
        SplashKit.LoadFont("FontJ", "jeebra.ttf");
        SplashKit.LoadFont("FontU", "unknown.ttf");
    }
}