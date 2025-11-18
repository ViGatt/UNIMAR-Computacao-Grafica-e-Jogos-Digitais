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
    [SerializeField] private bool ehEspecial = false;
    [SerializeField] private float tempoParaAdicionar = 15f;

    [Tooltip("Se for especial, quantos segundos ele fica na tela antes de sumir.")]
    [SerializeField] private float tempoDeVida = 5f;

    private void Start()
    {
        if (ehEspecial)
        {
            Invoke("SumirSozinho", tempoDeVida);
        }
    }

    private void SumirSozinho()
    {
        if (FishSpawner.Instance != null)
        {
            FishSpawner.Instance.PeixeEspecialFoiPescado();
        }


        Destroy(gameObject);
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

            if (FishSpawner.Instance != null)
            {
                FishSpawner.Instance.PeixeEspecialFoiPescado();
            }

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