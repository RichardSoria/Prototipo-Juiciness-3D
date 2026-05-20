using UnityEngine;

public class Coin : MonoBehaviour
{
    public float velocidadGiro = 100f;
    public AudioClip sonidoRecoleccion;
    public GameObject chispasPrefab; // Opcional, si descargas un efecto

    void Update()
    {
        // Hace que la moneda gire sobre sí misma para verse "chévere"
        transform.Rotate(0, velocidadGiro * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si el personaje (que tiene el CharacterController) la toca
        if (other.GetComponent<CharacterController>() != null)
        {
            // Reproduce el sonido en la posición de la moneda
            if (sonidoRecoleccion != null)
                AudioSource.PlayClipAtPoint(sonidoRecoleccion, transform.position);

            // Si tienes un efecto de chispas, lo instancia
            if (chispasPrefab != null)
                Instantiate(chispasPrefab, transform.position, Quaternion.identity);

            // Destruye la moneda
            Destroy(gameObject);
        }
    }
}