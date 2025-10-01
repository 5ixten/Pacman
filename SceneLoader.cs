using System.Text;
using SFML.System;

namespace Pacman;

public class SceneLoader
{
    private readonly Dictionary<char, Func<Entity>> loaders;
    private string currentScene = "", nextScene = "";

    public SceneLoader()
    {
        loaders = new Dictionary<char, Func<Entity>> {
            {'#', () => new Wall()},
            {'g', () => new Ghost()},
            {'p', () => new Pacman()},
            {'.', () => new Coin()},
            {'c', () => new Candy()}
        };
    }
    
    public void HandleSceneLoad(Scene scene) {
        if (nextScene == "") return;
        scene.Clear();
        
        string file = $"assets/{nextScene}.txt";
        string[] lines = File.ReadLines(file, Encoding.UTF8).ToArray();

        for (int y = 0; y < lines.Length; y++)
        {
            char[] chars = lines[y].ToCharArray();
            for (int x = 0; x < chars.Length; x++)
            {
                if (Create(chars[x], out Entity created))
                {
                    scene.Spawn(created);
                    created.Position = new Vector2f(x * 18, y * 18);
                }
            }
        }

        currentScene = nextScene;
        nextScene = "";
    }
    
    public void Load(string scene) => nextScene = scene;
    public void Reload() => nextScene = currentScene;
    
    private bool Create(char symbol, out Entity created) 
    {
        if (loaders.TryGetValue(symbol, out Func<Entity> loader)) 
        {
            created = loader();
            return true;
        }
        
        created = null;
        return false;
    }
}