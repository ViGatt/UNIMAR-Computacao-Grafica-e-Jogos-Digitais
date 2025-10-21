using UnityEngine;

public class PontoDePesca : MonoBehaviour
{
    [Header("Configurações do Peixe")]
    [SerializeField] private float velocidadeDoPeixe = 50f;
    [SerializeField] private float tamanhoDaZona = 100f;

    [Tooltip("Quantos pontos este peixe vale.")]
    [SerializeField] private int valorEmPontos = 10;


    public void IniciarPesca()
    {
        // Verifica se o jogo já terminou
        if (GameManager.Instance != null && GameManager.Instance.JogoTerminou)
        {
            Debug.LogWarning("Clique no peixe IGNORADO porque o jogo já terminou.");
            return;
        }

        // A lógica que estava no OnMouseDown vem para aqui
        Debug.Log("Clique detectado (via Raycast) em: " + gameObject.name + ". Chamando o minigame.", gameObject);
        if (FishingMinigame.Instance == null)
        {
            Debug.LogError("ERRO: O peixe tentou iniciar o minigame, mas não encontrou uma Instância do FishingMinigame.");
            return;
        }

        gameObject.SetActive(false);
        FishingMinigame.Instance.IniciarMinigame(velocidadeDoPeixe, tamanhoDaZona, ResultadoDaPesca);
    }

    private void ResultadoDaPesca(bool sucesso)
    {
        if (sucesso)
        {
            Debug.Log("Sucesso! O peixe foi pego, adicionando " + valorEmPontos + " pontos.");
            Score.Instance.AddScore(valorEmPontos);

            if (FishSpawner.Instance != null)
            {
                FishSpawner.Instance.PeixeFoiPescado();
            }

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Falhou! O peixe escapou.");
            gameObject.SetActive(true);
        }
    }
}