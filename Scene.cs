using SFML.Graphics;

namespace Pacman;

public class Scene
{ 
    private List<Entity> _entities = new List<Entity>();

    public readonly SceneLoader Loader;
    public readonly AssetManager Assets;

    public Scene(SceneLoader loader,  AssetManager assets)
    {
        _entities = new();
        Loader = loader;
        Assets = assets;
    }

    public void Spawn(Entity entity)
    {
        _entities.Add(entity);
        entity.Create(this);
    }

    public void Clear()
    {
        for (int i = _entities.Count - 1; i >= 0; i--) {
            Entity entity = _entities[i];
            _entities.RemoveAt(i);
            entity.Destroy(this);
        }
    }

    public void UpdateAll(float deltaTime)
    {
        Loader.HandleSceneLoad(this);
        foreach (var entity in _entities)
        {
            entity.Update(this, deltaTime);
        }
    }

    public void RenderAll(RenderTarget target)
    {
        foreach (var entity in _entities)
        {
            entity.Render(target);
        }
    }
    
    public bool FindByType<T>(out T found) where T : Entity 
    {
        foreach (var entity in _entities)
        {
            if (!entity.Dead && entity is T typed) {
                found = typed;
                return true;
            }
        }
        
        found = default(T);
        return false;
    }

    public IEnumerable<Entity> FindIntersects(FloatRect bounds) 
    {
        int lastEntity = _entities.Count - 1;
        
        for (int i = lastEntity; i >= 0; i--) 
        {
            Entity entity = _entities[i];
            if (entity.Dead) continue;
            if (entity.Bounds.Intersects(bounds)) 
            {
                yield return entity;
            }
        }
    }
}