using UnityEngine;
using System.Collections;

public class TrampolinSlime : MonoBehaviour
{
    private Vector3 escalaOriginal;
    private bool estaAnimando = false;

    [Header("Configuración del Salto")]
    public float fuerzaDelRebote = 4f;

    [Header("Efectos Visuales y Sonoros")]
    public AudioClip sonidoRebote;
    public GameObject boingPrefab; // <-- NUEVO: Aquí irá tu prefab CFXR_BOING_

    void Start()
    {
        escalaOriginal = transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null && !estaAnimando)
        {
            // 1. Sonido
            if (sonidoRebote != null)
            {
                AudioSource.PlayClipAtPoint(sonidoRebote, transform.position);
            }

            // 2. NUEVO: Instanciar el texto "BOING!"
            // Le sumamos +2 en Y (hacia arriba) para que flote por encima del cubo y del personaje
            if (boingPrefab != null)
            {
                Vector3 posicionTexto = transform.position + new Vector3(0, 2f, 0);
                Instantiate(boingPrefab, posicionTexto, Quaternion.identity);
            }

            // 3. Ejecutar animaciones y físicas
            StartCoroutine(EfectoSquashStretch());

            ThirdPersonController scriptJugador = other.GetComponent<ThirdPersonController>();
            if (scriptJugador != null)
            {
                scriptJugador.AplicarRebote(fuerzaDelRebote);
            }
        }
    }

    private IEnumerator EfectoSquashStretch()
    {
        estaAnimando = true;

        float tiempoSquash = 0.15f;
        float tiempoStretch = 0.20f;
        float tiempoRecuperacion = 0.15f;

        Vector3 escalaSquash = new Vector3(escalaOriginal.x * 1.5f, escalaOriginal.y * 0.4f, escalaOriginal.z * 1.5f);
        Vector3 escalaStretch = new Vector3(escalaOriginal.x * 0.7f, escalaOriginal.y * 1.6f, escalaOriginal.z * 0.7f);

        float tiempoPasado = 0f;

        while (tiempoPasado < tiempoSquash)
        {
            transform.localScale = Vector3.Lerp(escalaOriginal, escalaSquash, tiempoPasado / tiempoSquash);
            tiempoPasado += Time.deltaTime;
            yield return null;
        }

        tiempoPasado = 0f;

        while (tiempoPasado < tiempoStretch)
        {
            transform.localScale = Vector3.Lerp(escalaSquash, escalaStretch, tiempoPasado / tiempoStretch);
            tiempoPasado += Time.deltaTime;
            yield return null;
        }

        tiempoPasado = 0f;

        while (tiempoPasado < tiempoRecuperacion)
        {
            transform.localScale = Vector3.Lerp(escalaStretch, escalaOriginal, tiempoPasado / tiempoRecuperacion);
            tiempoPasado += Time.deltaTime;
            yield return null;
        }

        transform.localScale = escalaOriginal;
        estaAnimando = false;
    }
}