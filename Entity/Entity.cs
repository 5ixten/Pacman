using SFML.Graphics;
using SFML.System;

namespace Pacman;

public abstract class Entity
{
    private string _textureName;
    protected Sprite sprite;
    protected Animation[] animations;
    protected float _spawnDelay = 0;
    
    public int ZIndex;
    public bool DontDestroyOnLoad;
    public bool Dead;

    public Vector2f Position
    {
        get { return sprite.Position; }
        set { sprite.Position = value; }
    }

    public virtual FloatRect Bounds => sprite.GetGlobalBounds();

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
        _spawnDelay = MathF.Max(_spawnDelay - deltaTime, 0.0f);
        if (!scene.Started) return;
        if (_spawnDelay > 0) return;
        
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