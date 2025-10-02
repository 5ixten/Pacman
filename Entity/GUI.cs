using SFML.Graphics;
using SFML.System;

namespace Pacman;

public class GUI : Entity
{
    private Text scoreText;
    private int maxHealth = 3;
    private int currentHealth;
    private int currentScore;
    
    public GUI() : base("pacman")
    {
        scoreText = new Text();
    }
    
    public override void Create(Scene scene) {
        base.Create(scene);
        sprite.Scale = new Vector2f(2, 2);

        scoreText.Font = scene.Assets.LoadFont("pixel-font");
        scoreText.DisplayedString = "Score";
        currentHealth = maxHealth;
    }

    public override void Render(RenderTarget target)
    {
        // Place hearts
        for (int i = 0; i < maxHealth; i++) {
            sprite.TextureRect = i < currentHealth
                ? new IntRect(72, 36, 18, 18) // Full heart
                : new IntRect(72, 0, 18, 18); // Empty heart
            base.Render(target);
            sprite.Position = new Vector2f(36 + i * 36, 396);
        }
        
        scoreText.DisplayedString = $"Score: {currentScore}";
        scoreText.Position = new Vector2f(
            414 - scoreText.GetGlobalBounds().Width, 396
        );
        
        target.Draw(scoreText);
    }
    
    public override void Update(Scene scene, float deltaTime) {}
}