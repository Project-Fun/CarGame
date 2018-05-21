using System;
using SplashKitSDK;

public interface IMoveable
{
    void Move();
}
public class Program
{
    public static void Main()
    {
        Window window = new Window("RaceGame", 800, 800);
        RaceGame rGame = new RaceGame(window);
        while (!window.CloseRequested && !rGame.ESC)
        {
            if (rGame.Restart)
            {
                rGame = new RaceGame(window);
            }
            window.Clear(Color.RGBColor(193, 154, 107));
            rGame.Update();
            rGame.Draw();
            window.Refresh();
        }
        window.Close();
        window = null;
    }
}

