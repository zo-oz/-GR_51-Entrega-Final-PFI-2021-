using UnityEngine;

public class FruitBomb : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;
    
    //Collision con jugador/piso, explota
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            //explosion.transform.localScale.Set(10, 10, 1);
            GameObject g = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            SoundManager_Game3.instance.PlayBombSound();
            Destroy(g, 0.5f);
        }
    }
}
