using UnityEngine;

public class FishController : MonoBehaviour
{
    // Variáveis públicas para ajustar a velocidade no Inspector
    [Header("Movement Settings")]
    [SerializeField] private float forwardSpeed = 5f;
    [SerializeField] private float turnSpeed = 100f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        
        rb.useGravity = true;

        
        rb.isKinematic = false;
    }

    void FixedUpdate()
    {
        // 1. Captura a entrada do teclado
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        // 2. Aplica a rotação do peixe
        float turnAmount = turnInput * turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0, turnAmount, 0);
        rb.MoveRotation(rb.rotation * turnRotation);

        // 3. Aplica a força de avanço na direção que o peixe está virado
        Vector3 forwardForce = transform.forward * moveInput * forwardSpeed;
        rb.AddForce(forwardForce, ForceMode.Acceleration);
    }
}