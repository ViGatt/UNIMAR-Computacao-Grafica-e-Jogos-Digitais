using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlayerClickHandler : MonoBehaviour
{
    private Camera mainCamera;
    private int uiLayer;

    void Start()
    {
        mainCamera = GetComponent<Camera>();
        uiLayer = LayerMask.NameToLayer("UI");

        if (mainCamera == null)
        {
            Debug.LogError("PlayerClickHandler: Não há componente 'Camera' neste objeto! Adicione este script à sua Main Camera.", this);
        }
    }

    void Update()
    {
        // VERIFICAÇÃO 0: Se o minigame de pesca está ativo, IGNORA QUALQUER CLIQUE e para aqui.
        if (FishingMinigame.Instance != null && FishingMinigame.Instance.IsMinigameActive)
        {
            return;
        }

        // 1. O clique foi pressionado?
        if (Input.GetMouseButtonDown(0))
        {
            // Verificação 1: O clique foi na UI (botões)?
            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                if (results[0].gameObject.layer == uiLayer)
                {
                    Debug.LogWarning("Clique BLOQUEADO pela UI: <b>" + results[0].gameObject.name + "</b>", results[0].gameObject);
                    return; // É um botão, para aqui.
                }
            }

            // Verificação 2: O clique foi no mundo 3D
            Debug.Log("Clique no Jogo. Disparando Physics.Raycast...");
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                Debug.Log("Physics.Raycast ATINGIU: <b>" + hit.collider.gameObject.name + "</b>", hit.collider.gameObject);
                PontoDePesca peixe = hit.collider.GetComponent<PontoDePesca>();
                if (peixe != null)
                {
                    Debug.Log("<color=green>SUCESSO!</color> Objeto é um peixe. A chamar IniciarPesca().");
                    peixe.IniciarPesca();
                }
                else
                {
                    Debug.LogWarning("O objeto atingido (" + hit.collider.gameObject.name + ") não tem o script 'PontoDePesca'.");
                }
            }
            else
            {
                Debug.Log("Physics.Raycast disparado, mas não atingiu NENHUM collider.");
            }
        }
    }
}