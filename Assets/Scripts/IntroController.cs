using System.Collections;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    [Header("Componentes da Cena")]
    public ParticleSystem chuvaParticulas;
    public AudioSource chuvaSom;
    public Light sol; 
    public GameManager gameManager; 

    [Header("Configuração da Intro")]
    public float duracaoDaChuva = 5f; 
    public float duracaoDoAmanhecer = 3f; 

    [Header("Cores do Céu")]
    public Color corDaNoite = new Color(0.2f, 0.2f, 0.3f);
    public Color corDoDia = new Color(1f, 0.95f, 0.8f);    

    void Start()
    {
        if (gameManager != null)
        {
            gameManager.enabled = false;
        }

        StartCoroutine(SequenciaDeIntro());
    }

    IEnumerator SequenciaDeIntro()
    {

        if (sol != null)
        {
            sol.color = corDaNoite;
            sol.intensity = 0.2f; 
        }

        if (chuvaParticulas != null) chuvaParticulas.Play();
        if (chuvaSom != null)
        {
            chuvaSom.volume = 1f;
            chuvaSom.Play();
        }

        yield return new WaitForSeconds(duracaoDaChuva);


        if (chuvaParticulas != null) chuvaParticulas.Stop();

        float tempoPassado = 0f;
        float volumeInicial = chuvaSom != null ? chuvaSom.volume : 1f;

        while (tempoPassado < duracaoDoAmanhecer)
        {
            tempoPassado += Time.deltaTime;
            float progresso = tempoPassado / duracaoDoAmanhecer;

            if (sol != null)
            {
                sol.color = Color.Lerp(corDaNoite, corDoDia, progresso);
                sol.intensity = Mathf.Lerp(0.2f, 1.0f, progresso);
            }

            if (chuvaSom != null)
            {
                chuvaSom.volume = Mathf.Lerp(volumeInicial, 0f, progresso);
            }

            yield return null; 
        }

        if (chuvaSom != null) chuvaSom.Stop();

        Debug.Log("Intro terminada. Iniciando o jogo!");

        if (gameManager != null)
        {
            gameManager.enabled = true;
        }

        Destroy(gameObject);
    }
}