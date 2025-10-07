using SFML.Graphics;
using SFML.System;

namespace Pacman;

public class Coin : Entity
{
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
    
    public Coin() : base("pacman")
    {

    }
    
    public override void Create(Scene scene) {
        base.Create(scene);
        sprite.TextureRect = new IntRect(36, 36, 18, 18);
    }
    
    protected override void CollideWith(Scene scene, Entity e) {
        if (e is Pacman) {
            scene.EventHandler.PublishGainScore(100);
            Dead = true;
        }
    }
}