using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = System.Random;

public class CarMovement : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float speed = 10f;
    public Rigidbody rb;
    
    [Header("Durum Değişkenleri")]
    public bool isEngineOn = false;
    public bool isBrake = false;
    public bool isEnd = false;

    [Header("UI Elemanları")]
    public GameObject EndPanel;
    public Text distanceText;
    public Text finalText;
    public Text allDistanceText;
    
    [Header("Başlangıç Yazıları")]
    public GameObject startTextGO;
    public GameObject endTextGO;

    [Header("Efektler")]
    public GameObject particle;
    
    private float distance;
    
    void Awake()
    {
        InitializeGame();
    }

    void Start()
    {
        speed = UnityEngine.Random.Range(10f, 20f);
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleInput();
        HandleMovement();
        UpdateDistance();
    }

    void InitializeGame()
    {
        isEnd = false;
        endTextGO.SetActive(false);
        EndPanel.SetActive(false);
    }

    void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.E)) 
            isEngineOn = true;

        if (Input.GetKeyDown(KeyCode.Space) && isEngineOn && !isEnd)
            StopCar();
    }

    void HandleMovement()
    {
        if (isEngineOn && !isBrake && !isEnd)
        {
            startTextGO.SetActive(false);
            rb.velocity = transform.forward * speed;
        }
        else if (isBrake)
        {
            rb.velocity = Vector3.zero;
        }
    }

    void UpdateDistance()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 100f))
        {
            distance = hit.distance;
            // ya da 
            //distance = (transform.position - hit.transform.position).magnitude;
            allDistanceText.text = "Mesafe: " + distance.ToString("F0");
        }
    }

    void StopCar()
    {
        isEnd = true;
        isBrake = true;
        
        endTextGO.SetActive(true);
        ShowEndResult();
    }

    void ShowEndResult()
    {
        distanceText.text = "Duvarla Arandaki Mesafe: " + distance.ToString("F0") + "m";
        
        if (distance <= 5)
            finalText.text = "SÜPER! DURUŞ...";
        else if (distance <= 10)
            finalText.text = "Ortalama ya...";
        else if (distance <= 15)
            finalText.text = "Çok erken durdun...";
        else
            finalText.text = "Dalga geçiyor olmalısın...";
        
        EndPanel.SetActive(true);
    }

    public void restartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Instantiate(particle, transform.position, Quaternion.identity);
            
            isEnd = true;
            endTextGO.SetActive(true);
            distanceText.text = "Duvarla Arandaki Mesafe: 0m";
            finalText.text = "Öldün ho.";
            EndPanel.SetActive(true);
        }
    }
}