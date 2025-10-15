using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Configurações do Jogo")]
    [SerializeField] private float tempoDeJogo = 60f; // Duração da partida em segundos

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
        // Inicia o jogo
        JogoTerminou = false;
        tempoRestante = tempoDeJogo;
        painelFimDeJogo.SetActive(false); // Esconde a tela de fim de jogo
        Time.timeScale = 1f; // Garante que o tempo está a correr normalmente
    }

    void Update()
    {
        // Se o jogo terminou, não faz mais nada
        if (JogoTerminou) return;

        // Diminui o tempo restante
        tempoRestante -= Time.deltaTime;

        // Se o tempo acabar, termina o jogo
        if (tempoRestante <= 0)
        {
            tempoRestante = 0;
            FimDeJogo();
        }

        // Atualiza o texto do timer na tela
        AtualizarTextoTimer();
    }

    private void FimDeJogo()
    {
        JogoTerminou = true;
        painelFimDeJogo.SetActive(true); // Mostra a tela de fim de jogo

        // Mostra a pontuação final (assumindo que o seu script de score se chama 'Score')
        if (Score.Instance != null)
        {
            textoPontuacaoFinal.text = "Pontuação Final: " + Score.Instance.GetScore();
        }

        Debug.Log("O JOGO TERMINOU!");
    }

    private void AtualizarTextoTimer()
    {
        // Formata o tempo para minutos e segundos
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