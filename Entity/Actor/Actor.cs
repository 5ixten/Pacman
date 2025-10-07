using SFML.Graphics;
using SFML.System;

namespace Pacman;

public abstract class Actor : Entity
{
    private bool _wasAligned;
    
    protected float speed;
    protected int direction;
    protected bool moving;
    protected Vector2f? originalPosition;
    protected float originalSpeed;
    
    protected bool IsAligned =>
        (int) MathF.Floor(Position.X) % 18 == 0 &&
        (int) MathF.Floor(Position.Y) % 18 == 0;
    
    protected bool IsFree(Scene scene, int dir) {
        Vector2f at = Position + new Vector2f(9, 9);
        at += 18 * ToVector(dir);
        FloatRect rect = new FloatRect(at.X, at.Y, 1, 1);
        return !scene.FindIntersects(rect).Any(e => e.IsSolid);
    }

    protected Actor(string textureName) : base(textureName)
    {
        
    }
    
    protected static Vector2f ToVector(int dir) {
        switch (dir)
        {
            case 0:
                return new Vector2f(1, 0);
            case 1:
                return new Vector2f(0, -1);
            case 2:
                return new Vector2f(-1, 0);
            default:
                return new Vector2f(0, 1);
        }
    }

    public virtual void Reset()
    {
        _wasAligned = false;
        speed = originalSpeed;
        Position = (Vector2f)originalPosition;
        _spawnDelay = 1;
    }

    public override void Update(Scene scene, float deltaTime)
    {
        if (!scene.Started) return;
        base.Update(scene, deltaTime);
        if (_spawnDelay > 0) return;

        if (originalPosition == null)
        {
            originalPosition = Position;
        }
        
        if (IsAligned)
        {
            // If just got aligned
            if (!_wasAligned)
            {
                direction = PickDirection(scene);
            }

            if (moving)
            {
                _wasAligned = true;
            }
        }
        else
        {
            _wasAligned = false;
        }
        
        if (!moving) return;
        Position += ToVector(direction) * (speed * deltaTime);
        Position = MathF.Floor(Position.X) switch {
            < 0 => new Vector2f(432, Position.Y),
            > 432 => new Vector2f( 0, Position.Y),
            _ => Position
        };
    }

    protected abstract int PickDirection(Scene scene);
}