using UnityEngine;
using UnityEngine.UI;


public class GUI : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;
    
    public void UpdateScore(int score)
    {
        scoreText.text = score.ToString();
    }

    public void UpdateLives(int lives)
    {
        livesText.text = lives.ToString();
    }
}
