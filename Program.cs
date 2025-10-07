using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Text;

namespace Pacman;

class Program {
    static void Main(string[] args) {
        using (var window = new RenderWindow(
                new VideoMode(828, 900), "Pacman")) {
            
            window.SetView(new View(new FloatRect(18, 0, 414, 450)));
            window.Closed += (o, e) => window.Close();

            Clock clock = new Clock();
            Scene scene = new Scene(new SceneLoader(), new AssetManager(), new EventHandler());
            scene.Highscore = LoadHighscore();
            scene.Loader.Load("maze");
       
            while (window.IsOpen) {
                window.DispatchEvents();
                float deltaTime = clock.Restart().AsSeconds();
                deltaTime = MathF.Min(deltaTime, 0.01f);
                
                scene.UpdateAll(deltaTime);

                window.Clear(new Color(71, 44, 15));
                
                scene.RenderAll(window);

                window.Display();
            }
        }
    }

    public static void SaveHighscore(int score)
    {
        File.WriteAllText("HighScore.txt", score.ToString(), Encoding.UTF8);
    }
    
    static int LoadHighscore()
    {
        string savedContent = File.ReadAllText("HighScore.txt", Encoding.UTF8);
        if (int.TryParse(savedContent, out int score))
            return score;

        return 0; 
    }
}
