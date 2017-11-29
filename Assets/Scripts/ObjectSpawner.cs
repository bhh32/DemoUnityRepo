using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour 
{
	[SerializeField] private GameObject[] spawnees;
	[SerializeField] private float spawnDelay = 10f;
	[SerializeField] private float spawnDestroyDelay = 5f;
	GameObject spawnBaby;
	[SerializeField] TagComponent tagComp;
	private int goToSpawn;
    private int tagNum = 0;

	void Start()
	{
		tagComp = gameObject.GetComponent<TagComponent> ();

		// Starts the spawn emitter spawning repeatedly
		InvokeRepeating("Spawn", spawnDelay, spawnDelay);
	}

	void Spawn()
	{
		// Random index for spawned object
		int spawnObjectIndex = Random.Range (0, spawnees.Length - 1);
        int tagNum = Random.Range(0, 2);

		// Spawns the gameobject at the specific position
		switch (spawnObjectIndex) 
		{
            case 0:
                spawnBaby = Instantiate(spawnees[spawnObjectIndex], new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
                TagType(tagNum);
			    break;
		    case 1:
			    spawnBaby = Instantiate (spawnees [spawnObjectIndex], new Vector3 (transform.position.x, transform.position.y + .5f, transform.position.z), Quaternion.identity);
                TagType(tagNum);
			    break;
		    case 2:
			    spawnBaby = Instantiate (spawnees [spawnObjectIndex], new Vector3 (transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
                TagType(tagNum);
			    break;
		    case 3:
			    spawnBaby = Instantiate (spawnees [spawnObjectIndex], new Vector3 (transform.position.x, transform.position.y + .5f, transform.position.z), Quaternion.identity);
                TagType(tagNum);
			    break;
		    default:
			    break;
		}

		// Destroys the GameObject after specified time
		StartCoroutine (DestroyDelay(spawnBaby, spawnDestroyDelay));
	}

	IEnumerator DestroyDelay(GameObject obj, float destroyDelay)
	{
		yield return new WaitForSeconds (destroyDelay);
		Destroy(obj);
	}

    public void SetTagType(SecTag newTag)
    {
		tagComp.SetTagType(newTag);
    }

    public SecTag GetTagType()
    {
		return tagComp.GetTagType();
    }

    void TagType(int newNum)
    {
        if (newNum == 0)
        {
            spawnBaby.GetComponent<TagComponent>().SetTagType(SecTag.Friendly);
        }
        else
            spawnBaby.GetComponent<TagComponent>().SetTagType(SecTag.Enemy);
    }
}
