using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController3D : MonoBehaviour
{
    
    public float moveSpeed = 5f;
    public Text distanceText;
    
    
    public float distance;
    
    
    public List<GameObject> cars;
    
    public GameObject carPrefab;
    public float radius = 50f;
    private void Update()
    {
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
        
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Car"))
            {
                cars.Add(hit.gameObject);
            }
        }

        if (cars.Count > 0)
        {
            float minDistance = Mathf.Infinity;
            
            foreach (GameObject car in cars)
            {
                float currentDistance = Vector3.Distance(transform.position, car.transform.position);
                if (currentDistance < minDistance)
                {
                    minDistance = currentDistance;
                }
            }
            
            distance = minDistance;
        }

        if(cars.Count > 0) distanceText.text = "Distance: " + distance.ToString("F0");
        else distanceText.text = "Cars Not Found";
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Car"))
        {
            Destroy(other.gameObject);
            
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
