using SFML.Graphics;
using SFML.Window;

namespace Pacman;

public class Pacman : Actor
{
    private int _queuedDir = -1;
    private int animSpeed = 120;
    
    public Pacman() : base("pacman")
    {
        ZIndex = 2;
    }

    public override void Create(Scene scene)
    {
        direction = -1;
        base.Create(scene);
        sprite.TextureRect = new IntRect(0, 0, 18, 18);
        scene.EventHandler.LoseHealth += OnLoseHealth;
        
        originalSpeed = 60;
        speed = originalSpeed;
        
        animations = new Animation[]
        {
            // Right
            new Animation( new IntRect[]{
                new IntRect(36, 54, 18, 18),
                new IntRect(0, 0, 18, 18),
                new IntRect(18, 0, 18, 18),
                new IntRect(0, 0, 18, 18),
            }, animSpeed),
            
            // Up
            new Animation( new IntRect[]{
                new IntRect(36, 54, 18, 18),
                new IntRect(0, 18, 18, 18),
                new IntRect(18, 18, 18, 18),
                new IntRect(0, 18, 18, 18),
            }, animSpeed),
            
            // Left
            new Animation( new IntRect[]{
                new IntRect(36, 54, 18, 18),
                new IntRect(0, 36, 18, 18),
                new IntRect(18, 36, 18, 18),
                new IntRect(0, 36, 18, 18),
            }, animSpeed),
            
            // Down
            new Animation( new IntRect[]{
                new IntRect(36, 54, 18, 18),
                new IntRect(0, 54, 18, 18),
                new IntRect(18, 54, 18, 18),
                new IntRect(0, 54, 18, 18),
            }, animSpeed),

            // Still
            new Animation( new IntRect[]{
                new IntRect(36, 54, 18, 18)
            }, animSpeed),
        };
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

        scene.Started = _queuedDir >= 0;
        if (scene.Started)
        {
            base.Update(scene, deltaTime);
        }
    }

    public override void Reset()
    {
        base.Reset();
        _queuedDir = -1;
        moving = false;
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
        scene.EventHandler.LoseHealth -= OnLoseHealth;
    }
    
    public override void Render(RenderTarget target)
    {
        Animation currentAnim;
        if (moving)
        {
            currentAnim = animations[direction];
        }
        else
        {
            currentAnim = animations[4];
        }

        sprite.TextureRect = currentAnim.GetCurrentTextureRect();
        base.Render(target);
    }
}