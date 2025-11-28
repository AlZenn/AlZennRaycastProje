using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarMovement : MonoBehaviour
{
    public float speed = 10f;
    public Transform wall;
    public Rigidbody rb;
    
    public bool isEngineOn = false;
    public bool isBrake = false;
    public bool isEnd = false;

    public GameObject EndPanel;
    
    public Text distanceText;
    public Text finalText;
    public Text allDistanceText;
    
    [Header("Başlangıç Yazıları")]
    public GameObject startTextGO;
    public GameObject endTextGO;

    public GameObject particle;
    
    void Awake()
    {
        isEnd = false;
        endTextGO.SetActive(false);
        EndPanel.SetActive(false);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // car movement
        if (Input.GetKeyDown(KeyCode.E)) isEngineOn = true;
    
        if (isEngineOn && !isBrake && !isEnd)
        {
            startTextGO.SetActive(false);
            rb.velocity = transform.forward * speed;
        }
        else if (isBrake)
        {
            rb.velocity = Vector3.zero;
            isEnd = true;
        }
        
        // ray
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f))
        {
            float d = Vector3.Distance(hit.transform.position, this.transform.position);
            allDistanceText.text = "Mesafe: "+ d.ToString("F0");
        }

        // brake control & distance text
        if (Input.GetKeyDown(KeyCode.Space) && isEngineOn && !isEnd)
        { 
            isEnd = true;
            isBrake = true;
            
            float distance = Vector3.Distance(transform.position, wall.position);
            distanceText.text = "Duvarla Arandaki Mesafe: " + distance.ToString("F0") + "m";
            
            endTextGO.SetActive(true);
            
            if (distance <= 5)
            {
                finalText.text = "SÜPER! DURUŞ...";
            } 
            else if (distance <= 10)
            {
                finalText.text = "Ortalama ya...";
            }
            else if (distance <= 15)
            {
                finalText.text = "Çok erken durdun...";
            }
            else
            {
                finalText.text = "Dalga geçiyor olmalısın...";
            }
            EndPanel.SetActive(true);

        }
    }

    public void restartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            //Destroy(this.gameObject);
            Instantiate(particle,this.gameObject.transform.position,Quaternion.identity);

            isEnd = true;
            EndPanel.SetActive(true);
            distanceText.text = "Duvarla Arandaki Mesafe: 0m";
            finalText.text = "Öldün ho.";
            endTextGO.SetActive(true);
        }
    }
}