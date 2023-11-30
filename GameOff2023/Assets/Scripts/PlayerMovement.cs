using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;
    [SerializeField] private GameObject exit;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private float minheight;

    [SerializeField] private float jumpForce;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float glidingSpeed;
    [SerializeField] private float gravitySc;
    [SerializeField] public float bulletForce;
    [SerializeField] public float bulletRate;

    [SerializeField] private int shifts;
    [SerializeField] private float[] timeValue = new float[4];
    [SerializeField] private float[] jumpValue = new float[4];
    [SerializeField] private float[] speedValue = new float[4];
    [SerializeField] private float[] glideValue = new float[4];
    [SerializeField] private float[] forceValue = new float[4];
    [SerializeField] private float[] rateValue = new float[4];

    public TMP_Text timeDisplay;
    public TMP_Text speedDisplay;
    public TMP_Text jumpDisplay;
    public TMP_Text glideDisplay;
    public TMP_Text forceDisplay;
    public TMP_Text rateDisplay;

    private Rigidbody2D body;
    private BoxCollider2D box;
    private Transform trans;
    private SpriteRenderer sprite;
    private float horizontalInput;

    public bool timerActive;
    private float timePassed;
    private int iteration;
    private bool isDead;
    private bool coinCollected;
    private bool exitAllowed;

    [SerializeField] private int NOmeters;
    [SerializeField] private GameObject[] meters = new GameObject[6];
    [SerializeField] private int[] statsShown = new int[6];
    [SerializeField] private float[] maxStats = new float[6]; //Zawsze 6 (0-time; 1-speed; 2-jump; 3-glide; 4-force; 5-rate)
    [SerializeField] private float[] minStats = new float[6];
    private const float maxAngle = -180;
    private const float minAngle = 0;
    //K¥TY W UNITY ID¥ W PRZECIWN¥ STRONÊ NI¯ ZEGAR

    [SerializeField] private AudioSource coincol;
    [SerializeField] private AudioSource win;
    [SerializeField] private AudioSource loss;

    public GameObject deathEffect;
    public GameObject winEffect;

    void Awake()
    {
        Instance = this;
        body = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        trans = GetComponent<Transform>();
        sprite = GetComponent<SpriteRenderer>();
        timerActive = false;
        isDead = false;
        coinCollected = false;
        exitAllowed = false;
        timePassed = 0;
        iteration = 0;
        pauseMenu.SetActive(false);
        sprite.enabled = true;
        this.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;
        body.constraints = RigidbodyConstraints2D.FreezeRotation;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if(!isDead)
        {
            if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
            {
                Jump();
            }
            horizontalInput = Input.GetAxis("Horizontal");
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
            if (!IsGrounded() && Input.GetKey(KeyCode.W) && body.velocity.y <= glidingSpeed)
            {
                body.velocity = new Vector2(body.velocity.x, glidingSpeed);
                body.gravityScale = 0;
            }
            else
            {
                body.gravityScale = gravitySc;
            }

            if (!timerActive && Input.anyKey)
            {
                timerActive = true;
            }

            if (timerActive)
            {
                timePassed += Time.deltaTime;
                if (timePassed > timeValue[iteration + 1])
                {
                    iteration++;
                    if (iteration == shifts)
                    {
                        StartCoroutine(Death());
                    }
                }
                
                jumpForce = Mathf.Lerp(jumpValue[iteration], jumpValue[iteration + 1], (timePassed - timeValue[iteration]) / (timeValue[iteration + 1] - timeValue[iteration]));
                speed = Mathf.Lerp(speedValue[iteration], speedValue[iteration + 1], (timePassed - timeValue[iteration]) / (timeValue[iteration + 1] - timeValue[iteration]));
                glidingSpeed = Mathf.Lerp(glideValue[iteration], glideValue[iteration + 1], (timePassed - timeValue[iteration]) / (timeValue[iteration + 1] - timeValue[iteration]));
                bulletForce = Mathf.Lerp(forceValue[iteration], forceValue[iteration + 1], (timePassed - timeValue[iteration]) / (timeValue[iteration + 1] - timeValue[iteration]));
                bulletRate = Mathf.Lerp(rateValue[iteration], rateValue[iteration + 1], (timePassed - timeValue[iteration]) / (timeValue[iteration + 1] - timeValue[iteration]));

                timeDisplay.text = (Math.Round(timePassed*10)/10).ToString();
                speedDisplay.text = (Math.Round(speed * 10) / 10).ToString();
                jumpDisplay.text = (Math.Round(jumpForce * 10) / 10).ToString();
                glideDisplay.text = (Math.Round(Math.Abs(glidingSpeed) * 10) / 10).ToString();
                forceDisplay.text = (Math.Round(bulletForce * 10) / 10).ToString();
                rateDisplay.text = (Math.Round((1/bulletRate) * 10) / 10).ToString();

                for(int i=0; i<NOmeters; i++)
                {
                    switch (statsShown[i])
                    {
                        case 0:
                            meters[i].transform.GetChild(1).gameObject.transform.eulerAngles = new Vector3(0, 0, minAngle - ((timePassed-minStats[statsShown[i]]) * (minAngle - maxAngle) / (maxStats[statsShown[i]]-minStats[statsShown[i]])));
                            break;
                        case 1:
                            meters[i].transform.GetChild(1).gameObject.transform.eulerAngles = new Vector3(0, 0, minAngle - ((speed - minStats[statsShown[i]]) * (minAngle - maxAngle) / (maxStats[statsShown[i]] - minStats[statsShown[i]])));
                            break;
                        case 2:
                            meters[i].transform.GetChild(1).gameObject.transform.eulerAngles = new Vector3(0, 0, minAngle - ((jumpForce - minStats[statsShown[i]]) * (minAngle - maxAngle) / (maxStats[statsShown[i]] - minStats[statsShown[i]])));
                            break;
                        case 3:
                            meters[i].transform.GetChild(1).gameObject.transform.eulerAngles = new Vector3(0, 0, minAngle - ((Math.Abs(glidingSpeed) - minStats[statsShown[i]]) * (minAngle - maxAngle) / (maxStats[statsShown[i]] - minStats[statsShown[i]])));
                            break;
                        case 4:
                            meters[i].transform.GetChild(1).gameObject.transform.eulerAngles = new Vector3(0, 0, minAngle - ((bulletForce - minStats[statsShown[i]]) * (minAngle - maxAngle) / (maxStats[statsShown[i]] - minStats[statsShown[i]])));
                            break;
                        case 5:
                            meters[i].transform.GetChild(1).gameObject.transform.eulerAngles = new Vector3(0, 0, minAngle - (((1/bulletRate) - minStats[statsShown[i]]) * (minAngle - maxAngle) / (maxStats[statsShown[i]] - minStats[statsShown[i]])));
                            break;
                        default:
                            break;
                    }
                }
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(pauseMenu.activeInHierarchy)
                {
                    SceneManager.LoadScene(1);
                }
                else
                {
                    pauseMenu.SetActive(true);
                    Time.timeScale = 0f;
                }
            }

            if(Input.GetKeyDown(KeyCode.Space) && pauseMenu.activeInHierarchy)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1f;
            }

            if(Input.GetKeyDown(KeyCode.S) && exitAllowed)
            {
                StartCoroutine(NextLevel());
            }

            if(Input.GetKey(KeyCode.W) && body.velocity.y < -0.0001f)
            {
                this.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                this.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }

            if(trans.position.y < minheight)
            {
                StartCoroutine(Death());
            }
        }
        else
        {
            body.velocity = Vector2.zero;
            body.gravityScale = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag=="Exit")
        {
            exitAllowed = true;
        }
        if (col.gameObject.tag == "SecretCoin")
        {
            coinCollected = true;
            coincol.Play();
            col.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.tag=="Exit")
        {
            exitAllowed = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag=="Spike" || col.gameObject.tag=="Sawblade" || col.gameObject.tag == "Bullet" || col.gameObject.tag == "Shooter")
        {
            if (!isDead)
            {
                StartCoroutine(Death());
            }
        }
    }

    private void Jump()
    {
        body.velocity = new Vector2(body.velocity.x, jumpForce);
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(box.bounds.center, box.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    public IEnumerator Death()
    {
        isDead = true;
        body.constraints = RigidbodyConstraints2D.FreezeAll;
        sprite.enabled = false;
        this.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        loss.Play();
        PlayerPrefs.SetInt("Deaths", PlayerPrefs.GetInt("Deaths", 0) + 1);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private IEnumerator NextLevel()
    {
        isDead = true;
        sprite.enabled = false;
        this.gameObject.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
        Instantiate(winEffect, transform.position, Quaternion.identity);
        win.Play();
        yield return new WaitForSeconds(1);
        if (PlayerPrefs.GetInt("Levels", 1)==(SceneManager.GetActiveScene().buildIndex)-1)
        {
            PlayerPrefs.SetInt("Levels", PlayerPrefs.GetInt("Levels", 1) + 1);
        }
        if(coinCollected)
        {
            PlayerPrefs.SetInt("Coin" + (SceneManager.GetActiveScene().buildIndex - 1).ToString(), 1);
        }
        if(SceneManager.GetActiveScene().buildIndex != 11)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }
}
