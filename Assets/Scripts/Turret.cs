using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour 
{
    [SerializeField] private GameObject target;
    [SerializeField] private float speed;
    [SerializeField] private float turningRate;
    [SerializeField] private GameObject projectile;
	[SerializeField] private GameObject barrelTip;
    
	private Vector3 barrelForward;    
	private TagComponent tagComp;

    void OnTriggerStay(Collider other)
    {
		// Set the target to the gameObjects within the "turret range"
		target = other.gameObject;

		// Set the tagComp to the TagComponent of the gameObjects within the "turret range"
		tagComp = target.GetComponent<TagComponent> ();

		// If the tag component exists
		if (tagComp != null) 
		{
			// ... get what the secondary tag type is
			if (tagComp.GetTagType() == SecTag.Enemy) 
			{
				// .. if it's enemy, look at it and shoot it
				transform.LookAt (target.transform);
				barrelForward = barrelTip.transform.up;
				StartCoroutine ("FireDelay");
				FireTurret ();
			}
			// ... if it's friendly, just look at it.
			else
			{
				transform.LookAt(target.transform);
			}
		}        
    }

    void FireTurret()
    {
		// Instantiate the bullets as gameObjects
        var bullet = Instantiate(projectile, barrelTip.transform.position, new Quaternion(90f, 90f, 0f, 10f)) as GameObject;
		// Add the impulse force to make the bullets move
        bullet.GetComponent<Rigidbody>().AddForce(barrelForward * 100f, ForceMode.Impulse);
    }

	// So that each bullet has a delay between it being fired
    IEnumerator FireDelay()
    {
        yield return new WaitForSeconds(.5f);
    }
}
