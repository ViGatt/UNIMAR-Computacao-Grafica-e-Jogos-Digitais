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
    [Tooltip("O espaço vazio necessário ao redor do peixe.")]
    [SerializeField] private float raioDeEspaco = 1f;
    [Tooltip("Quantas vezes tentar achar um lugar livre antes de desistir.")]
    [SerializeField] private int maxTentativas = 10;

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
            StartCoroutine(SpawnEspecialLoop());
        }
    }

    private IEnumerator SpawnEspecialLoop()
    {
        while (true)
        {
            if (GameManager.Instance == null || !GameManager.Instance.JogoTerminou)
            {
                SpawnPeixe(peixeEspecialPrefab);
            }
            yield return new WaitForSeconds(intervaloSpawnEspecial);
        }
    }

    public void PeixeEspecialFoiPescado()
    {
        StartCoroutine(RespawnPeixeEspecialComDelay());
    }

    private IEnumerator RespawnPeixeEspecialComDelay()
    {
        yield return new WaitForSeconds(intervaloSpawnEspecial);

        if (GameManager.Instance != null && !GameManager.Instance.JogoTerminou)
        {
            SpawnPeixe(peixeEspecialPrefab);
        }
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
        SpawnPeixe(peixeAleatorio);
    }

    private void SpawnPeixe(GameObject peixePrefab)
    {
        if (peixePrefab == null) return;

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
            Instantiate(peixePrefab, melhorPosicao, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("O Spawner não encontrou espaço livre para criar um peixe! Tente diminuir os obstáculos ou aumentar a área.");
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