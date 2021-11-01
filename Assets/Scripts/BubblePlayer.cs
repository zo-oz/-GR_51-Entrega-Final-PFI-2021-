using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BubblePlayer : MonoBehaviour {

    [SerializeField]
    private float speed;
    [SerializeField]
    private BackgroundManager bgm;
    private Rigidbody2D rb;
    [SerializeField] 
    private Camera _camera;
    [SerializeField]
    private TextMesh scoreText;
    private int score;
    private int score_chg;
    private ParticleSystem ps;
    private ParticleSystem.MainModule pma;
    private Color lastColor;
    private Color lastColor2;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private SoundManager_Game2 soundManager;

    private void Awake()
    {
        //selecciono ParticleSystem del personaje para cambio de color
        ps = GetComponent<ParticleSystem>();
        pma = ps.main;
        // cache de camara
        if (!_camera) _camera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        // inicializa puntaje
        score = 0;
        score_chg = 0;
    }

    //Agrego el audioclip
    AudioSource AddAudio(AudioClip audioClip)
    {
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = audioClip;
        return audioSource;
    }

    private void Update()
    {
        //movimiento con el touch
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                Vector3 touch_Pos = _camera.ScreenToWorldPoint(touch.position);
                //Debug.Log("La posicion touch es  "+touch.position.ToString());
                transform.position = Vector2.MoveTowards(transform.position, touch_Pos, speed * Time.deltaTime);
            }
        }
        else
        {
            //prueba con el mouse
            if (Input.GetMouseButton(0))
            {
                var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                targetPos.z = transform.position.z;
                transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            }
        }
    }

    //Hacer contacto con una burbuja
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bubble")) 
        {
            //camba con el color de la burbuja
            GetComponent<TrailRenderer>().endColor = GetComponent<TrailRenderer>().startColor;
            GetComponent<TrailRenderer>().startColor = collision.gameObject.GetComponent<SpriteRenderer>().color;
            pma.startColor = collision.gameObject.GetComponent<SpriteRenderer>().color;
            var col = ps.colorOverLifetime;
            col.color = collision.gameObject.GetComponent<SpriteRenderer>().color;
            soundManager.PlayPopSound();
            //Bonus: Tres burbujas consecutivas del mismo color
            if (lastColor2 == collision.gameObject.GetComponent<SpriteRenderer>().color)
            {
                ps.Stop();
                transform.GetChild(0).gameObject.SetActive(true);
                soundManager.PlayElectroSound();
                UpdateScore(30);
                lastColor2 = Color.black;
                lastColor = Color.black;
            }
            else
            {
                //Bonus: dos burbujas consecutivas del mismo color
                if (lastColor == collision.gameObject.GetComponent<SpriteRenderer>().color)
                {
                    UpdateScore(20);
                    lastColor2 = lastColor = collision.gameObject.GetComponent<SpriteRenderer>().color;
                }
                else
                {
                    UpdateScore(10);
                    lastColor = collision.gameObject.GetComponent<SpriteRenderer>().color;
                    lastColor2 = Color.black;
                }
            }
        }

        //Colisión con Magnet con Bonus x3
        if (collision.gameObject.CompareTag("Magnet")) 
        {
            if (transform.GetChild(0).gameObject.activeInHierarchy)
            {
                var pos = collision.gameObject.transform.position;
                Destroy(collision.gameObject);
                explosion.transform.position = pos;
                GameObject g = Instantiate(explosion, pos, Quaternion.identity);
                soundManager.PlayBoomSound();
                Destroy(g, 1.2f);
                transform.GetChild(0).gameObject.SetActive(false);
                ps.Play();

            }

        }

    }

    //Fuera de area
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Finish")) 
        {
            transform.position = new Vector2(0, 0);
            UpdateScore(-50);
            soundManager.PlayExitSound();
        }
    }

    //Actualizo el puntaje y cambio de imagen cada 100 puntos
    private void UpdateScore(int s)
    {
        score = score + s;
        score_chg = score_chg + s;
        if (score_chg >= 100) 
        {
            StartCoroutine(PauseAndChangeImg());
            bgm.ChangeImage();
            score_chg = 0;
        }
        scoreText.text = "Puntaje: " + score.ToString();
    }

    //Coroutine: cambio de imagen y pauso el juego
    IEnumerator PauseAndChangeImg()
    {
        soundManager.ToggleMute();
        bgm.ChangeImage();
        //cullingMask esconde los elementos del juego
        FindObjectOfType<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("Player");
        FindObjectOfType<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("Magnet");
        FindObjectOfType<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("UI");
        FindObjectOfType<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("Default");
        GameObject.Find("HomeBT").GetComponent<Image>().enabled = false;
        GameObject.Find("ResetBT").GetComponent<Image>().enabled = false;
        soundManager.PlayBreathSound();
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(2.2f);
        FindObjectOfType<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("Player");
        FindObjectOfType<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("Magnet");
        FindObjectOfType<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("UI");
        FindObjectOfType<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("Default");
        GameObject.Find("HomeBT").GetComponent<Image>().enabled = true;
        GameObject.Find("ResetBT").GetComponent<Image>().enabled = true;
        Time.timeScale = 1;
        soundManager.ToggleMute();
    }
}
