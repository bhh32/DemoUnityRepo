using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleporter : MonoBehaviour 
{
    [Header("One-Way Teleporter")]
    [Tooltip("Used to provide a specific destination position")]
	[SerializeField] private Vector3 newPos;
    [Header("Two-Way Teleporter")]
    [Tooltip("Used to teleport to another teleport pad")]
    [SerializeField] private GameObject otherTelePad;
    [Header("General Teleporter Settings")]
	[SerializeField] private GameObject player;
    [SerializeField] private float teleportOffset;
	[SerializeField] private Text teleportText;

	// Use this for initialization
	void Start () 
	{
		teleportText.enabled = false;
	}

	void OnTriggerEnter(Collider other)
	{
		// Enables UI Teleport Text
		if (other.CompareTag ("Player"))
			teleportText.enabled = true;
	}

	void OnTriggerStay(Collider other)
	{
		// Used for one way teleporters to teleport to a specific position in the world
		if (other.CompareTag ("Player") && this.CompareTag ("One-Way Teleporter") && Input.GetKeyDown(KeyCode.E)) 
		{
			player.transform.position = newPos;
			player.GetComponent<Rigidbody> ().velocity = new Vector3 (0f, 0f, 0f);
		}

		// Used for two way teleporters to teleport to another specific teleporter within the same scene
		if (other.CompareTag ("Player") && this.CompareTag ("Two-Way Teleporter") && Input.GetKeyDown(KeyCode.E)) 
		{
			player.transform.position = new Vector3(otherTelePad.transform.position.x, 0f, otherTelePad.transform.position.z);
			player.GetComponent<Rigidbody> ().velocity = new Vector3 (0f, 0f, 0f);
		}
	}

	void OnTriggerExit(Collider other)
	{
		// Disables UI Teleport Text
		if (other.CompareTag ("Player"))
			teleportText.enabled = false;
	}
}
