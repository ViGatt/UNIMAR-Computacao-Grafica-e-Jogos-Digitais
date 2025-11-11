using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Configurações do Jogo")]
    [SerializeField] private float tempoDeJogo = 60f; 

    [Header("Referências da UI")]
    [SerializeField] private TextMeshProUGUI textoDoTimer;
    [SerializeField] private GameObject painelFimDeJogo;
    [SerializeField] private TextMeshProUGUI textoPontuacaoFinal;

    private float tempoRestante;
    public bool JogoTerminou { get; private set; } 

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        JogoTerminou = false;
        tempoRestante = tempoDeJogo;
        painelFimDeJogo.SetActive(false); 
        Time.timeScale = 1f; 
    }

    void Update()
    {
        if (JogoTerminou) return;

        tempoRestante -= Time.deltaTime;

        if (tempoRestante <= 0)
        {
            tempoRestante = 0;
            FimDeJogo();
        }

        AtualizarTextoTimer();
    }

    private void FimDeJogo()
    {
        JogoTerminou = true;
        painelFimDeJogo.SetActive(true); 

        if (Score.Instance != null)
        {
            textoPontuacaoFinal.text = "Pontuação Final: " + Score.Instance.GetScore();
        }

        Debug.Log("O JOGO TERMINOU!");
    }

    private void AtualizarTextoTimer()
    {
        int minutos = Mathf.FloorToInt(tempoRestante / 60);
        int segundos = Mathf.FloorToInt(tempoRestante % 60);
        textoDoTimer.text = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    public void ReiniciarJogo()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}