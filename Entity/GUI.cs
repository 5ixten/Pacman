using SFML.Graphics;

namespace Pacman;

public class GUI : Entity
{
    private Text scoreText;
    private int maxHealth;
    private int currentHealth;
    private int currentScore;
    
    public GUI() : base("pacman")
    {
        scoreText = new Text();
    }
    
    public override void Create(Scene scene) {
        base.Create(scene);
        sprite.TextureRect = new IntRect(72, 36, 18, 18);
    }
    
    public override void Update(Scene scene, float deltaTime) {}
}