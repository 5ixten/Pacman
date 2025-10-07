namespace Pacman;

public delegate void ValueChangedEvent(Scene scene, int value);

public class EventHandler
{
    public event ValueChangedEvent GainScore;
    public event ValueChangedEvent LoseHealth;
    public event ValueChangedEvent CandyEaten;
    
    public void PublishGainScore(int amount) => _scoreGained += amount;
    public void PublishLoseHealth(int amount) => _healthLost += amount;
    public void PublishCandyEaten(int amount) =>_candyEaten += amount;
    
    private int _scoreGained;
    private int _healthLost;
    private int _candyEaten;

    public void Update(Scene scene)
    {
        if (_scoreGained != 0) {
            GainScore?.Invoke(scene, _scoreGained);
            _scoreGained = 0;
        }
        
        if (_healthLost != 0) {
            LoseHealth?.Invoke(scene, _healthLost);
            _healthLost = 0;
        }
        
        if (_candyEaten != 0) {
            CandyEaten?.Invoke(scene, _candyEaten);
            _candyEaten = 0;
        }
    }
}