using UnityEngine;
using TMPro; 

public class Score : MonoBehaviour
{
    public static Score Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI scoreText; 

    private int score = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        UpdateScoreText();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
        Debug.Log("Pontuação atual: " + score);
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            
            scoreText.text = "Pontos: " + score;
        }
    }
}