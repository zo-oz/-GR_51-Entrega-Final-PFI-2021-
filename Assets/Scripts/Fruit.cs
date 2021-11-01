using UnityEngine;

public class Fruit : MonoBehaviour
{
    [SerializeField]
    private GameObject star;
    [SerializeField]
    private GameObject explosion;
    private int bounce;

    void Start()
    {
        bounce = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Collision con jugador, se consume
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            star.transform.localScale.Set(10, 10, 1);
            GameObject g = Instantiate(star, gameObject.transform.position, Quaternion.identity);
            Destroy(g, 1f);
        }

        //Collision con piso, rebota 1 vez
        if (collision.gameObject.CompareTag("Ground"))
        {
            bounce += 1;
            if (bounce > 1) 
            {
                Destroy(gameObject);
                explosion.transform.localScale.Set(10, 10, 1);
                GameObject g = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
                Destroy(g, 1f);
            }
        }
    }
}
