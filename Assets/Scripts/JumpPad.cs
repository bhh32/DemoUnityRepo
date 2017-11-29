using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour 
{
	[SerializeField] float jumpHeight;
	[SerializeField] GameObject player;

	private Rigidbody rb;

	// Use this for initialization
	void Start () 
	{
		// Get the player's rigid body component
		rb = player.GetComponent<Rigidbody> ();
	}

	void OnTriggerEnter(Collider other)
	{
		// Ensure that only the player is affected by the jump pad... for now.
		if (other.CompareTag ("Player"))
			rb.AddForce (Vector3.up * jumpHeight, ForceMode.Impulse);
	}
}
