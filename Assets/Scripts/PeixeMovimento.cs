using UnityEngine;

public class PeixeMovimento : MonoBehaviour
{
    public enum ComportamentoSePreso { Teleportar, DesaparecerEReaparecer }

    [Header("Configurações de Movimento")]
    [SerializeField] private float velocidade = 1.5f;
    [SerializeField] private float areaDeMovimento = 5f;
    [SerializeField] private float velocidadeDeRotacao = 2f;

    [Header("Sistema Anti-Bloqueio")]
    [Tooltip("O que o peixe faz quando fica preso num canto.")]
    [SerializeField] private ComportamentoSePreso acaoSePreso = ComportamentoSePreso.Teleportar;
    [Tooltip("Quantos segundos parado para ser considerado preso.")]
    [SerializeField] private float tempoAteSerConsideradoPreso = 3f;

    private Vector3 pontoInicial;
    private Vector3 novoDestino;
    private Rigidbody rb;

    private float temporizadorDeBloqueio;
    private Vector3 posicaoAnterior;
    private const float distanciaMinimaParaMover = 0.05f;

    void Start()
    {
        pontoInicial = transform.position;
        posicaoAnterior = transform.position;
        rb = GetComponent<Rigidbody>();
        EscolherNovoDestino();

        InvokeRepeating("VerificarSeEstaPreso", 1f, 1f);
    }

    void FixedUpdate()
    {
        Vector3 direcao = novoDestino - transform.position;
        if (direcao != Vector3.zero)
        {
            Quaternion rotacaoAlvo = Quaternion.LookRotation(direcao);
            rb.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, velocidadeDeRotacao * Time.deltaTime);
        }

        Vector3 novaPosicao = Vector3.MoveTowards(rb.position, novoDestino, velocidade * Time.deltaTime);
        rb.MovePosition(novaPosicao);
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, novoDestino) < 0.5f)
        {
            EscolherNovoDestino();
        }
    }

    void EscolherNovoDestino()
    {
        float xAleatorio = Random.Range(-areaDeMovimento, areaDeMovimento);
        float zAleatorio = Random.Range(-areaDeMovimento, areaDeMovimento);
        novoDestino = pontoInicial + new Vector3(xAleatorio, 0, zAleatorio);
    }

    private void VerificarSeEstaPreso()
    {
        float distanciaMovida = Vector3.Distance(transform.position, posicaoAnterior);

        if (distanciaMovida < distanciaMinimaParaMover)
        {
            temporizadorDeBloqueio += 1f;
        }
        else
        {
            temporizadorDeBloqueio = 0f;
        }

        posicaoAnterior = transform.position;

        if (temporizadorDeBloqueio >= tempoAteSerConsideradoPreso)
        {
            Debug.LogWarning("Peixe '" + name + "' está preso! A acionar plano de fuga.", gameObject);
            PlanoDeFuga();
        }
    }

    private void PlanoDeFuga()
    {
        temporizadorDeBloqueio = 0f;

        switch (acaoSePreso)
        {
            case ComportamentoSePreso.Teleportar:
                EscolherNovoDestino();
                break;

            case ComportamentoSePreso.DesaparecerEReaparecer:
                if (FishSpawner.Instance != null)
                {
                    FishSpawner.Instance.PeixeFoiPescado(); 
                }
                Destroy(gameObject);
                break;
        }
    }
}