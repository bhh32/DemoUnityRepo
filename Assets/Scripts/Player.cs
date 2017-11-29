using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{
	[SerializeField] private float speed = 10f;
	private Rigidbody rb;

	// Use this for initialization
	void Start () 
	{
		rb = gameObject.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		float moveX = Input.GetAxis ("Horizontal");
		float moveZ = Input.GetAxis ("Vertical");

		rb.AddForce (moveX * speed * Time.deltaTime, 0f, moveZ * speed * Time.deltaTime);
	}
}
