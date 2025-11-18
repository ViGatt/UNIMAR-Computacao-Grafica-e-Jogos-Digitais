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
    [Tooltip("Marque isto se for um peixe especial (dá tempo, não inicia minigame).")]
    [SerializeField] private bool ehEspecial = false;
    [Tooltip("Segundos para adicionar ao timer (apenas se 'ehEspecial' for true).")]
    [SerializeField] private float tempoParaAdicionar = 15f;

    public void IniciarPesca()
    {
        if (GameManager.Instance != null && GameManager.Instance.JogoTerminou)
        {
            return;
        }

        if (ehEspecial)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AdicionarTempo(tempoParaAdicionar);
            }

            if (efeitoDeCliquePrefab != null)
            {
                Instantiate(efeitoDeCliquePrefab, transform.position, Quaternion.identity);
            }
            if (somDeClique != null)
            {
                AudioSource.PlayClipAtPoint(somDeClique, transform.position);
            }

            Destroy(gameObject);
        }
        else
        {
            if (FishingMinigame.Instance == null)
            {
                Debug.LogError("ERRO: O peixe tentou iniciar o minigame, mas não encontrou uma Instância do FishingMinigame.");
                return;
            }

            if (efeitoDeCliquePrefab != null)
            {
                Instantiate(efeitoDeCliquePrefab, transform.position, Quaternion.identity);
            }
            if (somDeClique != null)
            {
                AudioSource.PlayClipAtPoint(somDeClique, transform.position);
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
                FishSpawner.Instance.PeixeFoiPescado();
            }

            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}