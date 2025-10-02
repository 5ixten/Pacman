using SFML.Graphics;
using SFML.System;

namespace Pacman;

public abstract class Entity
{
    private string _textureName;
    protected Sprite sprite;

    public bool Dead;

    public Vector2f Position
    {
        get { return sprite.Position; }
        set { sprite.Position = value; }
    }

    public FloatRect Bounds => sprite.GetGlobalBounds();

    public virtual bool IsSolid => false;

    public Entity(string textureName)
    {
        _textureName = textureName;
        sprite = new Sprite();
    }

    public virtual void Create(Scene scene)
    {
        sprite.Texture = scene.Assets.LoadTexture(_textureName);
    }
    
    public virtual void Destroy(Scene scene)
    {
        
    }
    
    public virtual void Update(Scene scene, float deltaTime) 
    {
        foreach (Entity found in scene.FindIntersects(Bounds)) 
        {
            CollideWith(scene, found);
        }
    }
    
    public virtual void Render(RenderTarget target)
    {
        target.Draw(sprite);
    }

    protected virtual void CollideWith(Scene s, Entity other)
    {
        
    }
}