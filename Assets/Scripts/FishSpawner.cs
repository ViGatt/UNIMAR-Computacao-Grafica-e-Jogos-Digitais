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
    [Tooltip("O prefab do peixe que dá tempo.")]
    [SerializeField] private GameObject peixeEspecialPrefab;
    [Tooltip("O tempo (em segundos) entre cada spawn do peixe especial.")]
    [SerializeField] private float intervaloSpawnEspecial = 5f;

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
            yield return new WaitForSeconds(intervaloSpawnEspecial);

            if (GameManager.Instance != null && !GameManager.Instance.JogoTerminou)
            {
                SpawnPeixe(peixeEspecialPrefab);
            }
        }
    }

    public void PeixeFoiPescado()
    {
        StartCoroutine(RespawnPeixeComDelay(5f));
    }

    private IEnumerator RespawnPeixeComDelay(float delay)
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

        float xPos = Random.Range(-areaDeSpawn.x / 2, areaDeSpawn.x / 2);
        float zPos = Random.Range(-areaDeSpawn.y / 2, areaDeSpawn.y / 2);
        Vector3 posicaoDeSpawn = new Vector3(xPos, 0, zPos) + transform.position;

        Instantiate(peixePrefab, posicaoDeSpawn, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(areaDeSpawn.x, 0.1f, areaDeSpawn.y));
    }
}