using UnityEngine;

public class PontoDePesca : MonoBehaviour
{
    [Header("Configurações do Peixe")]
    [SerializeField] private float velocidadeDoPeixe = 50f;
    [SerializeField] private float tamanhoDaZona = 100f;
    [SerializeField] private int valorEmPontos = 10;

    [Header("Efeitos")]
    [SerializeField] private GameObject efeitoDeCliquePrefab;
    [SerializeField] private AudioClip somDeClique;

    [Header("Configurações Especiais")]
    [Tooltip("Marque isto se for um peixe de bónus ou armadilha (clique instantâneo).")]
    [SerializeField] private bool ehEspecial = false;

    [Tooltip("Use valores positivos para BÓNUS (ex: 15) e negativos para DEBUFF (ex: -5).")]
    [SerializeField] private float tempoParaAdicionar = 15f;

    [Tooltip("Se for especial, quantos segundos ele fica na tela antes de sumir.")]
    [SerializeField] private float tempoDeVida = 5f;

    [Tooltip("Marque isto se este peixe especial deve spawnar misturado com os peixes normais.")]
    [SerializeField] private bool fazParteDoSpawnNormal = false;

    private void Start()
    {
        if (ehEspecial)
        {
            float variacao = Random.Range(-1.0f, 2.0f);
            float tempoReal = tempoDeVida + variacao;

            if (tempoReal < 2f) tempoReal = 2f;

            Invoke("SumirSozinho", tempoReal);
        }
    }

    private void SumirSozinho()
    {
        AvisarSpawnerParaRepor();
        Destroy(gameObject);
    }

    private void AvisarSpawnerParaRepor()
    {
        if (FishSpawner.Instance != null)
        {
            if (fazParteDoSpawnNormal)
            {
                FishSpawner.Instance.PeixeNormalFoiPescado();
            }
            else
            {
                FishSpawner.Instance.PeixeEspecialFoiPescado();
            }
        }
    }

    public void IniciarPesca()
    {
        if (GameManager.Instance != null && GameManager.Instance.JogoTerminou) return;

        if (efeitoDeCliquePrefab != null) Instantiate(efeitoDeCliquePrefab, transform.position, Quaternion.identity);
        if (somDeClique != null) AudioSource.PlayClipAtPoint(somDeClique, transform.position);

        if (ehEspecial)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AdicionarTempo(tempoParaAdicionar);
            }

            AvisarSpawnerParaRepor(); 
            Destroy(gameObject);
        }
        else
        {
            if (FishingMinigame.Instance == null)
            {
                Debug.LogError("ERRO: FishingMinigame não encontrado.");
                return;
            }

            gameObject.SetActive(false);
            FishingMinigame.Instance.IniciarMinigame(velocidadeDoPeixe, tamanhoDaZona, ResultadoDaPesca);
        }
    }

    private void ResultadoDaPesca(bool sucesso)
    {
        if (sucesso)
        {
            Score.Instance.AddScore(valorEmPontos);

            if (FishSpawner.Instance != null)
            {
                FishSpawner.Instance.PeixeNormalFoiPescado();
            }
            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}