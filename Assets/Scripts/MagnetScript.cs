using UnityEngine;

public class MagnetScript : MonoBehaviour
{
    private float speed = 0.2f;
    private float rotSpeed = 0.5f;
    private bool left;

    void Update()
    {
        //Determino la dirección según su posición inicial
        if (left == true)
        {
            //Debug.Log("Lado Izquierdo");
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + 100, transform.position.y), speed * Time.deltaTime);
            transform.Rotate(0, 0, -rotSpeed);
        }
        else
        {
            //Debug.Log("Lado Derecho");
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x - 100, transform.position.y), speed * Time.deltaTime);
            transform.Rotate(0, 0, rotSpeed);
        }

    }

    //Determino su posición inicial
    private void Awake()
    {
        ParticleSystem ps = transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<ParticleSystem>();
        ParticleSystem.MainModule pma = ps.main;
        if (GameManager.instance.difficulty == -1)
        {
            pma.startSize = 4;
            transform.GetComponent<CircleCollider2D>().radius = 1.15f;
        }
        if (GameManager.instance.difficulty == 1)
        {
            pma.startSize = 7;
            transform.GetComponent<CircleCollider2D>().radius = 1.55f;
        }
        if (transform.position.x > 0)
            left = false;
        else
            left = true;
    }

    //Se elimina al salir del borde de la pantalla
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Finish"))
            Destroy(gameObject);
    }

}
