using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour 
{
    [SerializeField] private float destroyTimer;

    void Start()
    {
        //transform.SetPositionAndRotation(transform.position, new Quaternion(0f, 0f, 90f, 90f));
    }

    void FixedUpdate()
    {
        if (gameObject != null)
        {
            destroyTimer -= Time.deltaTime;

            if (destroyTimer <= 0f)
            {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Player"))
        {
            
            Destroy(gameObject);
        }
    }
}
