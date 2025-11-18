using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PeixeMovimento : MonoBehaviour
{
    public enum ComportamentoSePreso { Teleportar, DesaparecerEReaparecer }

    [Header("Configura��es de Movimento")]
    [SerializeField] private float velocidadeBase = 1.5f;
    [SerializeField] private float forcaDeAceleracao = 2f;
    [SerializeField] private float raioDePasseio = 5f;
    [SerializeField] private float velocidadeDeRotacao = 2f;

    [Header("Comportamento")]
    [Range(0, 1)]
    [SerializeField] private float chanceDePausar = 0.2f;
    [SerializeField] private float duracaoMinPausa = 0.5f;
    [SerializeField] private float duracaoMaxPausa = 2.0f;

    [Header("Sistema Anti-Bloqueio")]
    [SerializeField] private ComportamentoSePreso acaoSePreso = ComportamentoSePreso.Teleportar;
    [SerializeField] private float tempoAteSerConsideradoPreso = 3f;

    private Rigidbody rb;
    private Vector3 novoDestino;
    private bool estaPausado = false;
    private float velocidadeAtual;

    private float temporizadorDeBloqueio;
    private Vector3 posicaoAnterior;
    private const float distanciaMinimaParaMover = 0.05f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        posicaoAnterior = transform.position;
        EscolherNovoDestino();
        InvokeRepeating("VerificarSeEstaPreso", 1f, 1f);
    }

    void FixedUpdate()
    {
        if (estaPausado)
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, Time.fixedDeltaTime * 2f);
            return;
        }

        Vector3 direcaoParaDestino = (novoDestino - transform.position).normalized;

        if (direcaoParaDestino != Vector3.zero)
        {
            Quaternion rotacaoAlvo = Quaternion.LookRotation(direcaoParaDestino);
            rb.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, velocidadeDeRotacao * Time.fixedDeltaTime);
        }

        rb.AddForce(transform.forward * forcaDeAceleracao, ForceMode.Acceleration);

        if (rb.linearVelocity.magnitude > velocidadeAtual)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * velocidadeAtual;
        }
    }

    void Update()
    {
        if (estaPausado) return;

        if (Vector3.Distance(transform.position, novoDestino) < 1.5f)
        {
            EscolherNovoDestino();
        }
    }

    void EscolherNovoDestino()
    {
        if (Random.value < chanceDePausar)
        {
            StartCoroutine(PausarPeixe());
            return;
        }

        velocidadeAtual = velocidadeBase * Random.Range(0.8f, 1.2f);
        Vector3 pontoAleatorio = Random.insideUnitSphere * raioDePasseio;
        novoDestino = transform.position + pontoAleatorio;
        novoDestino.y = transform.position.y;
    }

    IEnumerator PausarPeixe()
    {
        estaPausado = true;
        float tempoDePausa = Random.Range(duracaoMinPausa, duracaoMaxPausa);
        yield return new WaitForSeconds(tempoDePausa);
        estaPausado = false;
        EscolherNovoDestino();
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
                    FishSpawner.Instance.PeixeNormalFoiPescado();
                }
                Destroy(gameObject);
                break;
        }
    }
}