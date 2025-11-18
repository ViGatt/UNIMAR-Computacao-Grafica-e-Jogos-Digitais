using System.Collections;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public static FishSpawner Instance { get; private set; }

    [Header("Peixes Normais")]
    [SerializeField] private GameObject[] peixePrefabs;
    [SerializeField] private int quantidadeInicialDePeixes = 15;
    [SerializeField] private Vector2 areaDeSpawn = new Vector2(20f, 15f);

    [Header("Peixe Especial")]
    [SerializeField] private GameObject peixeEspecialPrefab;
    [SerializeField] private float intervaloSpawnEspecial = 5f;

    [Header("Verificação de Obstáculos")]
    [Tooltip("Quais camadas são consideradas obstáculos (pedras, troncos, etc).")]
    [SerializeField] private LayerMask camadaObstaculos;
    [SerializeField] private float raioDeEspaco = 1f;
    [SerializeField] private int maxTentativas = 10;

    private GameObject peixeEspecialAtual;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        if (peixePrefabs.Length > 0)
        {
            for (int i = 0; i < quantidadeInicialDePeixes; i++)
            {
                SpawnPeixeNormal();
            }
        }

        if (peixeEspecialPrefab != null)
        {
            CriarPeixeEspecial();
        }
    }

    public void PeixeEspecialFoiPescado()
    {
        peixeEspecialAtual = null;
        StartCoroutine(RespawnPeixeEspecialComDelay());
    }

    private IEnumerator RespawnPeixeEspecialComDelay()
    {
        yield return new WaitForSeconds(intervaloSpawnEspecial);

        if (GameManager.Instance != null && !GameManager.Instance.JogoTerminou)
        {
            CriarPeixeEspecial();
        }
    }

    private void CriarPeixeEspecial()
    {
        if (peixeEspecialAtual != null) return; 

        if (peixeEspecialPrefab == null)
        {
            Debug.LogError("ERRO: Prefab do Peixe Especial não atribuído no Inspector!");
            return;
        }

        peixeEspecialAtual = SpawnPeixe(peixeEspecialPrefab);
    }

    public void PeixeNormalFoiPescado()
    {
        StartCoroutine(RespawnPeixeNormalComDelay(5f));
    }

    private IEnumerator RespawnPeixeNormalComDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnPeixeNormal();
    }

    private void SpawnPeixeNormal()
    {
        if (peixePrefabs.Length == 0) return;

        GameObject peixeAleatorio = peixePrefabs[Random.Range(0, peixePrefabs.Length)];

        if (peixeAleatorio == null)
        {
            Debug.LogError("ERRO CRÍTICO: Um item na lista 'Peixe Prefabs' está vazio ou foi destruído! Certifique-se de arrastar os arquivos da pasta PROJECT, e não objetos da CENA.");
            return;
        }

        SpawnPeixe(peixeAleatorio);
    }

    private GameObject SpawnPeixe(GameObject peixePrefab)
    {
        if (peixePrefab == null) return null;

        Vector3 melhorPosicao = Vector3.zero;
        bool encontrouLugarLivre = false;

        for (int i = 0; i < maxTentativas; i++)
        {
            float xPos = Random.Range(-areaDeSpawn.x / 2, areaDeSpawn.x / 2);
            float zPos = Random.Range(-areaDeSpawn.y / 2, areaDeSpawn.y / 2);
            Vector3 pontoTeste = new Vector3(xPos, 0, zPos) + transform.position;

            if (!Physics.CheckSphere(pontoTeste, raioDeEspaco, camadaObstaculos))
            {
                melhorPosicao = pontoTeste;
                encontrouLugarLivre = true;
                break;
            }
        }

        if (encontrouLugarLivre)
        {
            return Instantiate(peixePrefab, melhorPosicao, Quaternion.identity);
        }
        else
        {
            return Instantiate(peixePrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(areaDeSpawn.x, 0.1f, areaDeSpawn.y));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, raioDeEspaco);
    }
}