using System.Collections;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{
    public static FishSpawner Instance { get; private set; }

    [Header("Configurações do Spawner")]
    [SerializeField] private GameObject[] peixePrefabs; // Array para colocar vários tipos de peixes
    [SerializeField] private int quantidadeInicialDePeixes = 15;
    [SerializeField] private Vector2 areaDeSpawn = new Vector2(20f, 15f); // Largura (X) e Comprimento (Z) da área

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        // Cria a população inicial de peixes
        for (int i = 0; i < quantidadeInicialDePeixes; i++)
        {
            SpawnPeixe();
        }
    }

    private void SpawnPeixe()
    {
        if (peixePrefabs.Length == 0)
        {
            Debug.LogError("Nenhum prefab de peixe foi atribuído no FishSpawner!");
            return;
        }

        // 1. Escolhe um tipo de peixe aleatório do array
        GameObject peixeAleatorio = peixePrefabs[Random.Range(0, peixePrefabs.Length)];

        // 2. Calcula uma posição aleatória dentro da área definida
        float xPos = Random.Range(-areaDeSpawn.x / 2, areaDeSpawn.x / 2);
        float zPos = Random.Range(-areaDeSpawn.y / 2, areaDeSpawn.y / 2);
        Vector3 posicaoDeSpawn = new Vector3(xPos, transform.position.y, zPos) + transform.position; // Usa a altura do Spawner

        // 3. Cria o peixe na cena
        Instantiate(peixeAleatorio, posicaoDeSpawn, Quaternion.identity);
    }

    public void PeixeFoiPescado()
    {
        // Espera 5 segundos e cria um novo peixe para repor o que foi pescado
        StartCoroutine(RespawnPeixeComDelay(5f));
    }

    private IEnumerator RespawnPeixeComDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnPeixe();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, 0.5f); // Cor ciano semi-transparente
        Gizmos.DrawCube(transform.position, new Vector3(areaDeSpawn.x, 0.1f, areaDeSpawn.y));
    }
}