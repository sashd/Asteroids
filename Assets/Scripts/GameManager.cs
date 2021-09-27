using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int SmallAsteroidScore = 0;
    [SerializeField] private int MediumAsteroidScore = 0;
    [SerializeField] private int LargeAsteroidScore = 0;
    [SerializeField] private int UFOScore = 0;

    [SerializeField] private GUI gui;

    private int score = 0;
    private int lives = 3;

    private void Awake()
    {
        Asteroid.OnAsteroidDestroy += OnAsteroidDestroy;
        UFO.OnUFODestroy           += OnUFODestroy;
        Player.OnPlayerDead        += OnPlayerDead;
    }

    private void Start()
    {
        gui.UpdateLives(lives);
    }

    private void OnAsteroidDestroy(Asteroid.AsteroidType type)
    {
        switch (type)
        {
            case Asteroid.AsteroidType.SmallAsteroid  : score += SmallAsteroidScore;  break;
            case Asteroid.AsteroidType.MediumAsteroid : score += MediumAsteroidScore; break;
            case Asteroid.AsteroidType.LargeAsteroid  : score += LargeAsteroidScore;  break;
        }
        gui.UpdateScore(score);
    }

    private void OnUFODestroy()
    {
        score += UFOScore;
        gui.UpdateScore(score);
    }

    private void OnPlayerDead()
    {
        lives--;
        gui.UpdateLives(lives);

        if (lives <= 0)
        {
            Restart();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDestroy()
    {
        Asteroid.OnAsteroidDestroy -= OnAsteroidDestroy;
        UFO.OnUFODestroy           -= OnUFODestroy;
        Player.OnPlayerDead        -= OnPlayerDead;      
    }

}
