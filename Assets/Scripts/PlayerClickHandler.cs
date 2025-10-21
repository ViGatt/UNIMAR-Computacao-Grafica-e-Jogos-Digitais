// PlayerClickHandler.cs (A VERSÃO FINAL CORRIGIDA)
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic; // Precisamos disto para usar "List"

public class PlayerClickHandler : MonoBehaviour
{
    private Camera mainCamera;
    private int uiLayer; // Vamos guardar o número da camada "UI"

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
        // 1. O clique foi pressionado?
        if (Input.GetMouseButtonDown(0))
        {

            PointerEventData pointerData = new PointerEventData(EventSystem.current);
            pointerData.position = Input.mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            // 2. A UI foi atingida?
            if (results.Count > 0)
            {
                // 3. O objeto que foi atingido está na camada "UI"?
                if (results[0].gameObject.layer == uiLayer)
                {
                    // 4. SIM, é um botão ou painel. Bloqueia o clique.
                    Debug.LogWarning("Clique BLOQUEADO pela UI: <b>" + results[0].gameObject.name + "</b>", results[0].gameObject);
                    return; 
                }

                Debug.Log("EventSystem atingiu um objeto 3D (" + results[0].gameObject.name + "), mas não é UI. A permitir o clique.");
            }


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