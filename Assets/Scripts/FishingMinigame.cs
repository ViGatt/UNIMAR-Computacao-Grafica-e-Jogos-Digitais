using UnityEngine;
using UnityEngine.UI;
using System;

public class FishingMinigame : MonoBehaviour
{
    public static FishingMinigame Instance { get; private set; }

    [Header("Elementos da UI")]
    public GameObject painelMinigame; 
    public RectTransform barraPrincipal;
    public RectTransform barraJogador;
    public RectTransform zonaDeCaptura;

    [Header("Configurações do Jogo")]
    private float velocidadeBarraJogador = 250f;
    private float velocidadeZonaDeCaptura;
    private float duracaoDoMinigame = 10f;

    private float tempoRestante;
    private Action<bool> onMinigameComplete;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        if (painelMinigame != null)
        {
            painelMinigame.SetActive(false); 
        }
    }

    void Update()
    {
        if (painelMinigame == null || !painelMinigame.activeSelf)
        {
            return;
        }

        tempoRestante -= Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            barraJogador.anchoredPosition += new Vector2(0, velocidadeBarraJogador * Time.deltaTime);
        }
        else
        {
            barraJogador.anchoredPosition -= new Vector2(0, velocidadeBarraJogador * Time.deltaTime);
        }

        float alturaMetadeBarra = barraPrincipal.rect.height / 2;
        float novaPosicaoY = Mathf.Clamp(barraJogador.anchoredPosition.y, -alturaMetadeBarra, alturaMetadeBarra);
        barraJogador.anchoredPosition = new Vector2(barraJogador.anchoredPosition.x, novaPosicaoY);

        float novaPosicaoAlvoY = Mathf.PingPong(Time.time * velocidadeZonaDeCaptura, barraPrincipal.rect.height) - alturaMetadeBarra;
        zonaDeCaptura.anchoredPosition = new Vector2(zonaDeCaptura.anchoredPosition.x, novaPosicaoAlvoY);

        if (tempoRestante <= 0)
        {
            FinalizarMinigame(EstaDentroDaZona());
        }
    }

    public void IniciarMinigame(float velocidadePeixe, float tamanhoZona, Action<bool> callback)
    {
        this.velocidadeZonaDeCaptura = velocidadePeixe;
        zonaDeCaptura.sizeDelta = new Vector2(zonaDeCaptura.sizeDelta.x, tamanhoZona);
        this.onMinigameComplete = callback;
        tempoRestante = duracaoDoMinigame;
        barraJogador.anchoredPosition = Vector2.zero;

        painelMinigame.SetActive(true);
    }

    private void FinalizarMinigame(bool sucesso)
    {
        Debug.Log(sucesso ? "Você pescou o peixe!" : "O peixe escapou!");
        painelMinigame.SetActive(false);

        onMinigameComplete?.Invoke(sucesso);
    }

    private bool EstaDentroDaZona()
    {
        float jogadorMinY = barraJogador.anchoredPosition.y - barraJogador.rect.height / 2;
        float jogadorMaxY = barraJogador.anchoredPosition.y + barraJogador.rect.height / 2;
        float zonaMinY = zonaDeCaptura.anchoredPosition.y - zonaDeCaptura.rect.height / 2;
        float zonaMaxY = zonaDeCaptura.anchoredPosition.y + zonaDeCaptura.rect.height / 2;
        return jogadorMaxY > zonaMinY && jogadorMinY < zonaMaxY;
    }
}