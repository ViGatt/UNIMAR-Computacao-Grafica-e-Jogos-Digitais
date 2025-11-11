using System.Collections;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public static FishSpawner Instance { get; private set; }

    [Header("Configurações do Spawner")]
    [SerializeField] private GameObject[] peixePrefabs;
    [SerializeField] private int quantidadeInicialDePeixes = 15;
    [SerializeField] private Vector2 areaDeSpawn = new Vector2(20f, 15f);

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        if (peixePrefabs.Length == 0)
        {
            return;
        }

        for (int i = 0; i < quantidadeInicialDePeixes; i++)
        {
            SpawnPeixe();
        }
    }

    public void PeixeFoiPescado()
    {
        StartCoroutine(RespawnPeixeComDelay(5f));
    }

    private IEnumerator RespawnPeixeComDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnPeixe();
    }

    private void SpawnPeixe()
    {
        if (peixePrefabs.Length == 0) return;

        GameObject peixeAleatorio = peixePrefabs[Random.Range(0, peixePrefabs.Length)];
        float xPos = Random.Range(-areaDeSpawn.x / 2, areaDeSpawn.x / 2);
        float zPos = Random.Range(-areaDeSpawn.y / 2, areaDeSpawn.y / 2);
        Vector3 posicaoDeSpawn = new Vector3(xPos, 0, zPos) + transform.position;

        Instantiate(peixeAleatorio, posicaoDeSpawn, Quaternion.identity);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(areaDeSpawn.x, 0.1f, areaDeSpawn.y));
    }
}