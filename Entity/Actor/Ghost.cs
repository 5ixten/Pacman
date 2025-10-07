using SFML.Graphics;

namespace Pacman;

public class Ghost : Actor
{
    private float _frozenTimer;
    
    public override FloatRect Bounds
    {
        get
        {
            FloatRect bounds = base.Bounds;
            bounds.Left += 1;
            bounds.Width -= 2;
            bounds.Top += 1;
            bounds.Height -= 2;
            return bounds;
        }
    }
    
    public Ghost() : base("pacman")
    {
        ZIndex = 1;
    }

    public override void Create(Scene scene)
    {
        direction = -1;
        moving = true;
        base.Create(scene);
        sprite.TextureRect = new IntRect(36, 0, 18, 18);
        
        originalSpeed = 60;
        speed = originalSpeed;
        scene.EventHandler.CandyEaten += OnCandyEaten;
        scene.EventHandler.LoseHealth += OnLoseHealth;
        
        animations = new Animation[]
        {
            // Up
            new Animation( new IntRect[]{
                new IntRect(36, 0, 18, 18),
                new IntRect(54, 0, 18, 18)
            }, 240),
            
            // Left
            new Animation( new IntRect[]{
                new IntRect(36, 18, 18, 18),
                new IntRect(54, 18, 18, 18)
            }, 480),
        };
    }

    public override void Update(Scene scene, float deltaTime)
    {
        if (!scene.Started) return;
        
        _frozenTimer = MathF.Max(_frozenTimer - deltaTime, 0.0f);
        speed = _frozenTimer > 0 ? 20f : originalSpeed;
        base.Update(scene, deltaTime);
    }

    private void OnLoseHealth(Scene scene, int amount)
    {
        Reset();
    }

    public override void Reset()
    {
        base.Reset();
        _frozenTimer = 0;
    }

    private void OnCandyEaten(Scene scene, int _)
    {
        _frozenTimer = 5;
    }
    

    protected override int PickDirection(Scene scene)
    {
        List<int> validMoves = new List<int>();
        
        for (int i = 0; i < 4; i++) {
            // Prevent 180 degree turn
            if ((i + 2) % 4 == direction) continue;
            if (IsFree(scene, i)) validMoves.Add(i);
        }
        
        int r = new Random().Next(0, validMoves.Count);
        return validMoves[r];
    }
    
    protected override void CollideWith(Scene scene, Entity e) {
        // No damage during respawn
        if (_spawnDelay > 0)
        {
            return;
        }
        
        if (e is Pacman) {
            if (_frozenTimer <= 0)
            {
                scene.EventHandler.PublishLoseHealth(1);
            }
            else
            {
                Reset();
            }
        }
    }

    public override void Destroy(Scene scene)
    {
        scene.EventHandler.CandyEaten -= OnCandyEaten;
        scene.EventHandler.LoseHealth -= OnLoseHealth;
        base.Destroy(scene);
    }
    
    public override void Render(RenderTarget target)
    {
        Animation currentAnim = _frozenTimer <= 0 ?  animations[0] : animations[1];
        sprite.TextureRect = currentAnim.GetCurrentTextureRect();
        base.Render(target);
    }
}