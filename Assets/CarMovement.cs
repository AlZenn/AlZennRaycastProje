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

    public GameObject EndPanel;
    
    public Text distanceText;
    public Text finalText;

    
    [Header("Başlangıç Yazıları")]
    public GameObject startTextGO;
    public GameObject endTextGO;

    void Awake()
    {
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
    
        if (isEngineOn && !isBrake)
        {
            startTextGO.SetActive(false);
            rb.velocity = transform.forward * speed;
        }
        else if (isBrake)
        {
            rb.velocity = Vector3.zero; // fren basılıysa dur
        }
        

        // brake control & distance text
        if (Input.GetKeyDown(KeyCode.Space))
        { 
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

}