using UnityEngine;

public class FishController : MonoBehaviour
{
    // Vari�veis p�blicas para ajustar a velocidade no Inspector
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

        // 2. Aplica a rota��o do peixe
        float turnAmount = turnInput * turnSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0, turnAmount, 0);
        rb.MoveRotation(rb.rotation * turnRotation);

        // 3. Aplica a for�a de avan�o na dire��o que o peixe est� virado
        Vector3 forwardForce = transform.forward * moveInput * forwardSpeed;
        rb.AddForce(forwardForce, ForceMode.Acceleration);
    }
}