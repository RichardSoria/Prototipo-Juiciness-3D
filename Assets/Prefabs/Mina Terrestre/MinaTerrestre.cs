using UnityEngine;
using Unity.Cinemachine;

public class MinaTerrestre : MonoBehaviour
{
    [Header("Efectos Visuales")]
    public GameObject explosionPrefab; // Arrastra aquí la explosión de tu carpeta Cartoon FX
    public GameObject boomTextPrefab;  // Arrastra aquí el texto "BOOM"

    [Header("Audio")]
    public AudioClip explosionSound;   // Arrastra aquí el sonido de la explosión

    private CinemachineImpulseSource impulseSource;

    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si lo toca el jugador
        if (other.GetComponent<CharacterController>() != null)
        {
            // 1. Instancia las partículas de explosión
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }

            // 2. Instancia el texto "BOOM" flotando un poco más arriba (+2 en Y)
            if (boomTextPrefab != null)
            {
                Vector3 posicionTexto = transform.position + new Vector3(0, 2f, 0);
                Instantiate(boomTextPrefab, posicionTexto, Quaternion.identity);
            }

            // 3. Reproduce el sonido
            if (explosionSound != null)
            {
                AudioSource.PlayClipAtPoint(explosionSound, transform.position);
            }

            // 4. ¡Hace temblar la pantalla!
            if (impulseSource != null)
            {
                impulseSource.GenerateImpulse();
            }

            // 5. Destruye la mina
            Destroy(gameObject);
        }
    }
}