using UnityEngine;

public class PeixeMovimento : MonoBehaviour
{
    [SerializeField] private float velocidade = 1.5f;
    [SerializeField] private float areaDeMovimento = 5f; 

    private Vector3 pontoInicial;
    private Vector3 novoDestino;

    void Start()
    {
        
        pontoInicial = transform.position;
        
        EscolherNovoDestino();
    }

    void Update()
    {
        
        transform.position = Vector3.MoveTowards(transform.position, novoDestino, velocidade * Time.deltaTime);

        
        if (novoDestino - transform.position != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(novoDestino - transform.position);
        }

        
        if (Vector3.Distance(transform.position, novoDestino) < 0.1f)
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
}