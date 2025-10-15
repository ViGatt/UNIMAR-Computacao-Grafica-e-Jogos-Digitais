// FishSpawner.cs (VERSÃO DE DIAGNÓSTICO)
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
            Debug.LogError("ERRO NO SPAWNER: Nenhum prefab de peixe foi atribuído no Inspector!", this);
            return;
        }

        for (int i = 0; i < quantidadeInicialDePeixes; i++)
        {
            SpawnPeixe();
        }
    }

    public void PeixeFoiPescado()
    {
        // LOG 1: Confirma que o Spawner recebeu a chamada
        Debug.Log("<color=yellow>SPAWNER:</color> Recebi o aviso de que um peixe foi pescado. A iniciar contagem para criar um novo.", this);

        // Espera 5 segundos e cria um novo peixe para repor o que foi pescado
        StartCoroutine(RespawnPeixeComDelay(5f));
    }

    private IEnumerator RespawnPeixeComDelay(float delay)
    {
        // LOG 2: Confirma que a Coroutine começou
        Debug.Log("<color=yellow>SPAWNER:</color> Coroutine iniciada. A esperar " + delay + " segundos...", this);

        yield return new WaitForSeconds(delay);

        // LOG 3: Confirma que a espera terminou e vai chamar o SpawnPeixe
        Debug.Log("<color=yellow>SPAWNER:</color> Tempo de espera terminado. A criar um novo peixe agora.", this);
        SpawnPeixe();
    }

    private void SpawnPeixe()
    {
        // LOG 4: Confirma que o método para criar o peixe foi chamado
        Debug.Log("<color=green>SPAWNER:</color> A executar o método SpawnPeixe().", this);

        if (peixePrefabs.Length == 0) return;

        GameObject peixeAleatorio = peixePrefabs[Random.Range(0, peixePrefabs.Length)];
        float xPos = Random.Range(-areaDeSpawn.x / 2, areaDeSpawn.x / 2);
        float zPos = Random.Range(-areaDeSpawn.y / 2, areaDeSpawn.y / 2);
        Vector3 posicaoDeSpawn = new Vector3(xPos, 0, zPos) + transform.position;

        Instantiate(peixeAleatorio, posicaoDeSpawn, Quaternion.identity);

        // LOG 5: Confirma que o peixe foi instanciado
        Debug.Log("<color=green>SPAWNER:</color> Peixe criado com sucesso na posição " + posicaoDeSpawn, this);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 1, 0.5f);
        Gizmos.DrawCube(transform.position, new Vector3(areaDeSpawn.x, 0.1f, areaDeSpawn.y));
    }
}