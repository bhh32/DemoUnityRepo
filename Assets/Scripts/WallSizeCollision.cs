using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSizeCollision : MonoBehaviour 
{
	[SerializeField] private float sizeChange;
	[SerializeField] private GameObject player;
	private Transform playerTrans;
	private float minSize;
	private float maxSize;

	// Use this for initialization
	void Start () 
	{
		playerTrans = player.GetComponent<Transform> ();
		minSize = .5f;
		maxSize = 1.25f;
	}

	void OnCollisionEnter(Collision other)
	{
		if (other.collider.CompareTag ("Player") && playerTrans.localScale.x > minSize || 
			other.collider.CompareTag ("Player") && playerTrans.localScale.x <= minSize && playerTrans.localScale.x < maxSize)
				playerTrans.localScale = new Vector3(playerTrans.localScale.x + sizeChange, playerTrans.localScale.y + sizeChange, playerTrans.localScale.z + sizeChange);

        if (playerTrans.localScale.x < minSize)
            playerTrans.localScale = new Vector3(Mathf.Abs(minSize), Mathf.Abs(minSize), Mathf.Abs(minSize));
        else if (playerTrans.localScale.x > maxSize)
            playerTrans.localScale = new Vector3(maxSize, maxSize, maxSize);

		if (other.collider.CompareTag("Player") && other.collider.GetComponent<TagComponent>().GetTagType() == SecTag.Friendly)
        {
			other.collider.GetComponent<TagComponent>().SetTagType(SecTag.Enemy);
        }
		else if(other.collider.CompareTag("Player") && other.collider.GetComponent<TagComponent>().GetTagType() == SecTag.Enemy)
			player.GetComponent<TagComponent>().SetTagType(SecTag.Friendly);
	}
}
