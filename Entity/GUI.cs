using SFML.Graphics;
using SFML.System;

namespace Pacman;

public class GUI : Entity
{
    private Text scoreText;
    private int maxHealth = 3;
    private int currentHealth;
    private int currentScore;
    private bool showHighScore = true;

    private bool firstInit = true; // Ugly fix, but deadline is soon :P
    private Scene _scene;
    
    public GUI() : base("pacman")
    {
        scoreText = new Text();
        ZIndex = 10;
        DontDestroyOnLoad = true;
    }
    
    private void OnLoseHealth(Scene scene, int amount) {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            showHighScore = true;
            firstInit = true;
            scene.Started = false;
            scene.Loader.Reload();
        }
    }

    
    public override void Create(Scene scene) {
        base.Create(scene);
        _scene = scene;
        sprite.Scale = new Vector2f(2, 2);

        scoreText.Font = scene.Assets.LoadFont("pixel-font");
        scoreText.DisplayedString = "Score";
        scoreText.CharacterSize = 16;
        currentHealth = maxHealth;
        
        scene.EventHandler.LoseHealth += OnLoseHealth;
        scene.EventHandler.GainScore += OnGainedScore;
    }

    private void OnGainedScore(Scene scene, int amount)
    {
        currentScore += amount;
        if (!scene.FindByType<Coin>(out _)) {
            scene.Loader.Reload();
        }
        
        if (scene.Highscore < currentScore)
        {
            scene.Highscore = currentScore;
            Program.SaveHighscore(scene.Highscore);
        }
    }

    public override void Render(RenderTarget target)
    {
        // Place hearts
        if (!showHighScore)
        {
            RenderHearts(target);
        }
        else
        {
            scoreText.DisplayedString = $"HighScore: {_scene.Highscore}";
            scoreText.Position = new Vector2f(
                18, 396
            );
            target.Draw(scoreText);
        }
        
        scoreText.DisplayedString = $"Score: {currentScore}";
        scoreText.Position = new Vector2f(
            414 - scoreText.GetGlobalBounds().Width, 396
        );
        
        target.Draw(scoreText);
    }

    private void RenderHearts(RenderTarget target)
    {
        for (int i = 0; i < maxHealth; i++) {
            sprite.TextureRect = i < currentHealth
                ? new IntRect(72, 36, 18, 18)
                : new IntRect(72, 0, 18, 18);
            sprite.Position = new Vector2f(36 + i * 36, 396);
            base.Render(target);
        }
    }

    public override void Destroy(Scene scene)
    {
        base.Destroy(scene);
        scene.EventHandler.LoseHealth -= OnLoseHealth;
        scene.EventHandler.GainScore -= OnGainedScore;
    }

    public override void Update(Scene scene, float deltaTime)
    {
        if (firstInit && scene.Started)
        {
            Console.WriteLine(currentHealth + " " + scene.Started);
            firstInit = false;
            currentHealth = maxHealth;
            currentScore = 0;
            showHighScore = false;
        }
    }
}