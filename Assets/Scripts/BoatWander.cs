using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoatWander : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    [Tooltip("A força de aceleração do motor do barco.")]
    [SerializeField] private float forcaDoMotor = 5f;

    [Tooltip("A velocidade com que o barco vira (quão rápido é o leme).")]
    [SerializeField] private float velocidadeDeViragem = 1f;

    [Tooltip("A distância que o barco 'olha' à frente para escolher um novo destino.")]
    [SerializeField] private float raioDePasseio = 20f;

    [Tooltip("A distância mínima do destino para o barco escolher um novo.")]
    [SerializeField] private float distanciaDeChegada = 3.0f;

    private Rigidbody rb;
    private Vector3 destinoAtual;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        EscolherNovoDestino();
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, destinoAtual) < distanciaDeChegada)
        {
            EscolherNovoDestino();
        }

        Vector3 direcaoDoDestino = (destinoAtual - transform.position).normalized;

        direcaoDoDestino.y = 0;

        if (direcaoDoDestino != Vector3.zero)
        {
            Quaternion rotacaoAlvo = Quaternion.LookRotation(direcaoDoDestino);

            rb.rotation = Quaternion.Slerp(transform.rotation, rotacaoAlvo, velocidadeDeViragem * Time.fixedDeltaTime);
        }

        rb.AddForce(transform.forward * forcaDoMotor, ForceMode.Acceleration);
    }

    void EscolherNovoDestino()
    {
        Vector3 pontoAleatorio = Random.insideUnitSphere * raioDePasseio;
        destinoAtual = transform.position + pontoAleatorio;

        destinoAtual.y = transform.position.y;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, raioDePasseio);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, destinoAtual);
    }
}