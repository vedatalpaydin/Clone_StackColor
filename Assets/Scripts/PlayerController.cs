using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    private List<GameObject> collects = new List<GameObject>();
    private Renderer rend;
    private AudioSource au;
    private Transform cameraTransform;
    private bool gameIsStart;
    private Vector2 lastMousePos;
    private Animator anim;
    private int pickColor;
    private Color playerColor;
    private Rigidbody rb;
    private bool gameIsFinish;
    private bool powerUp;
    private bool cameraSetTarget;
    private bool processFinish;
    private float kickPower, maxDistance, scoreMultiply, finishTimer;
    private int score;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float maxSpeed = 30f;
    [SerializeField] private float xSpeed = 0.5f;
    [SerializeField] private float maxPower = 10f;
    [SerializeField] private PowerController _powerController;
    [SerializeField] private GameObject powerBar;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Transform finishLine;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private AudioClip collectSFX;
    [SerializeField] private AudioClip failSFX;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject startGame;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rend = GetComponentInChildren<Renderer>();
        anim = GetComponentInChildren<Animator>();
        au = GetComponent<AudioSource>();

        maxDistance = GetDistance();
        _powerController.SetMaxPower(maxPower);

        pickColor = Random.Range(0, 4);
        SetColor(pickColor);


        scoreText.text = 0 + "";
    }


    void Update()
    {
        if (transform.position.z <= maxDistance && transform.position.z <= finishLine.position.z)
        {
            float distance = 1 - (GetDistance() / maxDistance);
            progressSlider.value = distance;
        }

        scoreText.text = score + "";
        if (cameraSetTarget)
        {
            if (processFinish)
            {
                finishTimer += Time.deltaTime;
                if (finishTimer > 5f)
                {
                    score = (int) (score * scoreMultiply);
                    cameraTransform.GetComponent<MeshRenderer>().material.color = Color.yellow;
                    gameOver.SetActive(true);
                    cameraSetTarget = false;
                }
            }
            else
            {
                finishTimer = 0;
                processFinish = true;
            }
        }

        playerColor = rend.material.color;
        if (gameIsFinish)
        {
            anim.SetTrigger("Kick");
            powerBar.SetActive(false);
            powerUp = false;
        }
        else
        {
            if (powerUp)
            {
                powerBar.SetActive(true);
                transform.position = new Vector3(0, 0, transform.position.z);
                if (Input.GetMouseButtonDown(0))
                    kickPower++;
                if (kickPower > 0)
                    kickPower -= 0.1f;
                _powerController.SetPower(kickPower);
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    lastMousePos = Input.mousePosition;
                    gameIsStart = true;
                }

                if (gameIsStart)
                {
                    startGame.SetActive(false);
                    anim.SetTrigger("Run");
                    if (rb.velocity.z < maxSpeed)
                        rb.AddForce(Vector3.forward * Time.deltaTime * moveSpeed * Time.time);
                    rb.drag = Mathf.Clamp(rb.drag - 0.1f, 0, 7);
                    if (Input.GetMouseButton(0))
                    {
                        Vector2 currentMousePos = Input.mousePosition;
                        Vector2 delta = currentMousePos - lastMousePos;
                        lastMousePos = currentMousePos;
                        transform.position =
                            new Vector3(Mathf.Clamp(transform.position.x + delta.x * Time.deltaTime * xSpeed, -3, 3),
                                transform.position.y, transform.position.z);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Pickup")
        {
            if (playerColor.Equals(other.GetComponent<MeshRenderer>().material.color))
            {
                if(processFinish) return;
                foreach (var collect in collects)
                {
                    float height = collect.transform.position.y;
                    height += other.transform.lossyScale.y + 0.01f;
                    collect.transform.position = new Vector3(transform.position.x, height, transform.position.z);
                }

                score++;
                au.PlayOneShot(collectSFX);
                other.transform.position = new Vector3(transform.position.x,
                    other.transform.localScale.y / 2 - transform.localScale.y / 4,
                    transform.position.z);
                other.transform.SetParent(transform);
                Destroy(other.GetComponent<BoxCollider>());
                collects.Add(other.gameObject);
            }
            else
            {
                if (collects.Count > 0)
                {
                    rb.drag = Mathf.Clamp(rb.drag + 1, 0, 7);
                    var item = collects[collects.Count - 1];
                    foreach (var collect in collects)
                    {
                        float height = collect.transform.position.y;
                        height -= item.transform.lossyScale.y + 0.01f;
                        collect.transform.position = new Vector3(transform.position.x, height, transform.position.z);
                    }

                    au.PlayOneShot(failSFX);
                    collects.Remove(item);
                    Destroy(item);
                    if (collects.Count <=0)
                    {
                        GameOver();
                    }
                }
                else
                {
                    GameOver();
                }
            }
        }

        if (other.tag == "obstacle")
            GameOver();
        if (other.tag == "Power")
            powerUp = true;
        if (other.tag == "Finish")
        {
            gameIsFinish = true;
            cameraSetTarget = true;
        }

        if (other.tag == "FinishWall")
        {
            GetComponent<BoxCollider>().isTrigger = false;
            rb.velocity = Vector3.zero;
        }
    }

    void GameOver()
    {
        if (!powerUp)
            anim.SetTrigger("Fail");

        Destroy(GetComponent<BoxCollider>());
        rb.velocity = Vector3.zero;
        foreach (var c in collects)
        {
            c.AddComponent<BoxCollider>();
            c.AddComponent<Rigidbody>();
        }
        gameIsStart = false;
        gameOver.SetActive(true);
    }


    public void SetColor(int amount)
    {
        Color[] colors = {new Color(1, 0, 0, 1), new Color(0, 1, 0, 1), new Color(1, 0, 1, 1), new Color(0, 1, 1, 1)};
        foreach (var c in transform.GetComponentsInChildren<Renderer>())
        {
            c.material.color = colors[amount];
        }
    }

    public int GetCollectsCount()
    {
        return collects.Count;
    }

    public bool GetPowerUp()
    {
        return powerUp;
    }

    public bool GetFinish()
    {
        return cameraSetTarget;
    }

    public Transform GetLastCollect()
    {
        return cameraTransform;
    }

    private float GetDistance()
    {
        return Vector3.Distance(transform.position, finishLine.position);
    }

    public void SetScoreMultiply(float value, Transform camera)
    {
        if (value > scoreMultiply && value <= 5f)
        {
            processFinish = false;
            scoreMultiply = value;
            cameraTransform = camera;
        }
    }

    public void SetScoreText(int value)
    {
        score += value;
    }

    public void GetKick()
    {
        foreach (var collect in collects)
        {
            collect.AddComponent<Rigidbody>();
            collect.AddComponent<BoxCollider>();
            collect.GetComponent<Rigidbody>().AddForce(Vector3.forward * ((kickPower+1) * 500));
            collect.transform.parent = null;
        }
        gameIsFinish = false;
    }

    public void LoadNewScene()
    {
        SceneManager.LoadScene("Level1");
    }
}