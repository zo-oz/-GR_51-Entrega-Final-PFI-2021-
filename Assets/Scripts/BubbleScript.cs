using UnityEngine;

public class BubbleScript : MonoBehaviour
{
    [SerializeField]
    private GameObject pop;

    //Burbuja explota en contacto con jugador
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            pop.transform.localScale.Set(10, 10, 1);
            GameObject g = Instantiate(pop, gameObject.transform.position, Quaternion.identity);
            Destroy(g, 0.3f);
        }
    }
}
