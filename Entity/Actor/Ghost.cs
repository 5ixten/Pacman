using SFML.Graphics;

namespace Pacman;

public class Ghost : Actor
{
    public Ghost() : base("pacman")
    {
        
    }

    public override void Create(Scene scene)
    {
        direction = -1;
        moving = true;
        base.Create(scene);
        sprite.TextureRect = new IntRect(36, 0, 18, 18);
        
        originalSpeed = 60;
        speed = originalSpeed;
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
        if (e is Pacman) {
            scene.PublishLoseHealth(1);
            Reset();
        }
    }
}