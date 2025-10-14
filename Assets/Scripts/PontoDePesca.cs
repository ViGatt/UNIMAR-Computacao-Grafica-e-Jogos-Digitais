using UnityEngine;
public class PontoDePesca : MonoBehaviour
{
    [Header("Configurações do Peixe")]
    [SerializeField] private float velocidadeDoPeixe = 50f;
    [SerializeField] private float tamanhoDaZona = 100f;

    [Tooltip("Quantos pontos este peixe vale.")]
    [SerializeField] private int valorEmPontos = 10;

    private void OnMouseDown()
    {
        Debug.Log("Clique detectado em: " + gameObject.name + ". Chamando o minigame.", gameObject);
        if (FishingMinigame.Instance == null)
        {
            Debug.LogError("ERRO: O peixe tentou iniciar o minigame, mas não encontrou uma Instância do FishingMinigame. O gerenciador foi destruído ou não existe?");
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

            // Adiciona os pontos (usando o nome do seu script, 'Score')
            Score.Instance.AddScore(valorEmPontos);

            // Avisa o Spawner para criar um novo peixe em algum lugar
            if (FishSpawner.Instance != null)
            {
                FishSpawner.Instance.PeixeFoiPescado();
            }
            else
            {
                Debug.LogWarning("FishSpawner não encontrado na cena. Nenhum peixe novo será criado.");
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