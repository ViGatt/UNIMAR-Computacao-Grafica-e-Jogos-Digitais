using UnityEngine;

public class WaterBuoyancy : MonoBehaviour
{
    // For�a para flutuar (ajuste este valor no Inspector)
    [SerializeField] private float buoyancyForce = 9.81f;
    // Resist�ncia da �gua (ajuste este valor no Inspector)
    [SerializeField] private float waterDrag = 1f;

    // Detecta um objeto entrando no volume de �gua
    private void OnTriggerEnter(Collider other)
    {
        // Pega o Rigidbody do objeto que entrou
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Aplica o drag (resist�ncia) da �gua
            rb.linearDamping = waterDrag;
            rb.angularDamping = waterDrag;
        }
    }

    // Detecta um objeto que est� dentro do volume de �gua
    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Aplica a for�a de flutua��o para cima
            rb.AddForce(Vector3.up * buoyancyForce, ForceMode.Acceleration);
        }
    }

    // Detecta um objeto saindo do volume de �gua
    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Reseta o drag do objeto para o valor padr�o
            rb.linearDamping = 0f;
            rb.angularDamping = 0.05f;
        }
    }
}