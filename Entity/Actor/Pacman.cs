using SFML.Graphics;
using SFML.Window;

namespace Pacman;

public class Pacman : Actor
{
    private int _queuedDir;
    
    public Pacman() : base("pacman")
    {
        
    }

    public override void Create(Scene scene)
    {
        direction = -1;
        base.Create(scene);
        sprite.TextureRect = new IntRect(0, 0, 18, 18);
        scene.LoseHealth += OnLoseHealth;
        
        originalSpeed = 60;
        speed = originalSpeed;
    }

    private void OnLoseHealth(Scene scene, int amount)
    {
        Reset();
    }

    public override void Update(Scene scene, float deltaTime)
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.Right)) 
            _queuedDir = 0;
        else if (Keyboard.IsKeyPressed(Keyboard.Key.Up)) 
            _queuedDir = 1;
        else if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
            _queuedDir = 2;
        else if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
            _queuedDir = 3;
        
        base.Update(scene, deltaTime);
    }

    protected override int PickDirection(Scene scene)
    {
        if (IsFree(scene, _queuedDir))
        {
            moving = true;
            return _queuedDir;
        }
        if (!IsFree(scene, direction)) moving = false;
        return direction;
    }
    
    public override void Destroy(Scene scene) {
        base.Destroy(scene);
        scene.LoseHealth -= OnLoseHealth;
    }
}