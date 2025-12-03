using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController3D : MonoBehaviour
{
    public int car = 0;
    public Text carText;
    
    public float moveSpeed = 5f;
    public Text distanceText;
    
    public float distance;
    
    public GameObject carPrefab;
    public float radius = 50f;
    
    private void Update()
    {
        if(car<10) carText.text = car.ToString() + "/10";
        else carText.text = "Well Done!";
        
        
        #region movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        if (movement != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, 10f * Time.deltaTime);
        }
        #endregion

        Collider[] hits = Physics.OverlapSphere(transform.position, radius);
        
        float minDistance = Mathf.Infinity;
        bool carFound = false;
        
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Car"))
            {
                carFound = true;
                float currentDistance = Vector3.Distance(transform.position, hit.transform.position);
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                }
            }
        }

        if (carFound)
        {
            distance = minDistance;
            distanceText.text = "Distance: " + distance.ToString("F0");
        }
        else
        {
            distanceText.text = "Cars Not Found";
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Car"))
        {
            Destroy(other.gameObject);
            car++;
            #region çakma spawner
            Vector3 randomDirection = UnityEngine.Random.onUnitSphere;
            randomDirection.y = 0f;
            randomDirection.Normalize();
            
            Vector3 spawnPosition = transform.position + randomDirection * 50f;
            
            Instantiate(carPrefab, spawnPosition, Quaternion.identity);
            #endregion
        }
    }
}