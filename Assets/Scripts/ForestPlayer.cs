using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ForestPlayer : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]
    float speed;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private BackgroundManager bgm;
    bool left = false;
    private bool isGrounded;
    private Animator animator;
    private int score;
    private int score_bg;
    private float gravity;
    public TextMesh scoreText;
    public TextMesh gravityText;
    [SerializeField]
    private Joystick joystick;


    void Start()
    {
        //puntaje y gravedad inicial
        score = 0;
        score_bg = 0;
        gravity = -9.81f;
        //ajusto a la dificultad
        if (GameManager.instance.difficulty == -1)
            gravity = -6f;
        gravityText.text = "GRAVEDAD: " + gravity;
        if (animator == null)
            animator = GetComponent<Animator>();
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        //Habilito movimiento con teclado
        //float movement = Input.GetAxis("Horizontal");
        float movement = joystick.Horizontal;
        transform.position += new Vector3(movement, 0f, 0f) * speed * Time.deltaTime;
        if (Mathf.Abs(movement) >= 0.1f)
        animator.SetBool("walking", true);
            else
        animator.SetBool("walking", false);
        if (movement <= -0.1f && !left)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            left = true;
        }
        else if (movement >= 0.1f && left)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            left = false;
        }
        //Habilito movimiento con teclado
        //if (Input.GetAxis("Vertical") != 0 && isGrounded)
        //{
        //    Jump();
        //}
        if (joystick.Vertical >= .5f && isGrounded)
        {
            Jump();
        }
    }

    //Accion de saltar
    private void Jump()
    {
        isGrounded = false;
        rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        SoundManager_Game3.instance.PlayJumpSound();
    }

    //Consumo de distintas frutas + salto (grounded)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Cherry"))
        {
            UpdateScore(5);
            SoundManager_Game3.instance.PlayStarSound();
            UpdateGravity(-2f);
        }

        if (collision.gameObject.CompareTag("Tomato"))
        {
            UpdateScore(10);
            SoundManager_Game3.instance.PlayStarSound();
            UpdateGravity(-1.5f);
        }

        if (collision.gameObject.CompareTag("Lemon"))
        {
            UpdateScore(15);
            SoundManager_Game3.instance.PlayStarSound();
            UpdateGravity(1f);
        }

        if (collision.gameObject.CompareTag("Pear"))
        {
            UpdateScore(20);
            SoundManager_Game3.instance.PlayStarSound();
            UpdateGravity(1.5f);
        }

        if (collision.gameObject.CompareTag("Grapes"))
        {
            UpdateScore(25);
            SoundManager_Game3.instance.PlayStarSound();
            UpdateGravity(2f);
        }

        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    //Colisión con explosion
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bomb"))
        {
            UpdateScore(-10);
        }
    }

    //Actualizo el puntaje
    private void UpdateScore(int s)
    {
        score = score + s;
        score_bg = score_bg + s;
        if (score_bg >= 120)
        {
            score_bg = 0;
            //bgm.ChangeImage();
            StartCoroutine(PauseAndChangeImg());
        }
        scoreText.text = "Puntaje: " + score.ToString();
    }

    //Actualizo la aceleración de la gravedad
    private void UpdateGravity(float g)
    {
        gravity = gravity + g;
        Physics2D.gravity = new Vector2(0, gravity);
        if (gravity <= -36f || gravity >= 0.9f)
        {
            SoundManager_Game3.instance.PlayWaveSound();
            gravity = -9.8f;
            if (GameManager.instance.difficulty == -1)
                gravity = -6f;
            Physics2D.gravity = new Vector2(0, gravity);
            UpdateScore(-100);
        }
        gravityText.text = "GRAVEDAD: "+gravity;
    }

    //Coroutine: cambio de imagen y pauso el juego
    IEnumerator PauseAndChangeImg()
    {
        SoundManager_Game3.instance.ToggleMute();
        bgm.ChangeImage();
        //cullingMask esconde elementos en pantalla
        FindObjectOfType<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("Default");
        FindObjectOfType<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("UI");
        GameObject.Find("HomeBT").GetComponent<Image>().enabled = false;
        GameObject.Find("ResetBT").GetComponent<Image>().enabled = false;
        GameObject.Find("JBackground").GetComponent<Image>().enabled = false;
        GameObject.Find("JHandle").GetComponent<Image>().enabled = false;
        SoundManager_Game3.instance.PlayBreathSound();
        //yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(5f);
        FindObjectOfType<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("Default");
        FindObjectOfType<Camera>().cullingMask ^= 1 << LayerMask.NameToLayer("UI");
        GameObject.Find("HomeBT").GetComponent<Image>().enabled = true;
        GameObject.Find("ResetBT").GetComponent<Image>().enabled = true;
        GameObject.Find("JBackground").GetComponent<Image>().enabled = true;
        GameObject.Find("JHandle").GetComponent<Image>().enabled = true;
        Time.timeScale = 1;
        SoundManager_Game3.instance.ToggleMute();
    }
}
