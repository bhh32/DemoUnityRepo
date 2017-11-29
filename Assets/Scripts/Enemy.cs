using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour 
{
	public NavMeshAgent agent;
	public GameObject Prey;
	public float radius;
	public float Wradius;
	public float speed;
	Rigidbody rbz;
	Vector3 Cforce = Vector3.zero;
	Vector3 Aforce = Vector3.zero;
	Vector3 Sforce = Vector3.zero;
	Vector3 Wander = Vector3.zero;
	public bool IsWander = true;
	public int AIstate = 1; // 0 - flocking 1 - wander 2 - evade
	// public GameObject Lead;

	public float jitter = 1;
	public float distance = 1;
	public float EvadeTime = 3;
	private float EvadeStart;
	public float WanderTime = 5;
	private float WanderStart;
	void Start ()
	{
		WanderStart = WanderTime;
		EvadeStart = EvadeTime;
		rbz = GetComponent<Rigidbody>();	
		agent = GetComponent<NavMeshAgent> ();
	}
	void DoWander()
	{

		Vector3 target = Vector3.zero;
		target = Random.insideUnitCircle.normalized * Wradius;

		target = (Vector2)target + Random.insideUnitCircle * jitter;
		target = target.normalized * Wradius;


		target.z = target.y;
		target.y = 0.0f;
		target += transform.position;
		target += transform.forward * distance;




		Vector3 dir = (target - transform.position).normalized;
		Vector3 desiredVelocity = dir * speed;


		Vector3 steeringForce = desiredVelocity - rbz.velocity;
		Wander = steeringForce;

		//Vector3 head = rbz.velocity;
		//head.y = 0;
		//transform.LookAt(transform.position + head, Vector3.up);
		// transform.forward = new Vector3(rbz.velocity.x, 0, rbz.velocity.z);
	}
	void DoEvade()
	{
		Vector3 target = Prey.transform.position + Prey.GetComponent<Rigidbody>().velocity;
		Vector3 dir = -(target - transform.position).normalized;
		Vector3 desiredVelocity = dir * speed;
		Vector3 steeringForce = desiredVelocity - rbz.velocity;




		rbz.AddForce(steeringForce.normalized * 4);


		//Vector3 head = rbz.velocity;
		//head.y = 0;
		//transform.LookAt(transform.position + head, Vector3.up);

		// if (Vector3.Distance(transform.position, Prey.transform.position) <= radius) { IsEvading = false; }

	}
	public void DoObstacle()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, rbz.velocity, out hit, 3))
		{
			if (hit.collider.tag == "Wall")
			{
				rbz.AddForce(hit.normal * speed * 20);
			}

		}

	}
	void flocking()
	{
		Vector3 Ctarget = Vector3.zero;
		Vector3 aDesire = Vector3.zero;
		Vector3 sSum = Vector3.zero;
		int hood = 0;
		Collider[] Hood = Physics.OverlapSphere(transform.position, radius);

		foreach( Collider T in Hood)
		{
//			var Q = T.GetComponent<SardineController>();
//			if(Q != null)
//			{
//				if (Q.BigFoish == true)
//				{
//					Prey = Q.gameObject;
//					//IsEvading = true;
//					AIstate = 2;
//				}
//
//			}
			if (T.GetComponent<Rigidbody>() == true )
			{
				hood++;
				Rigidbody rb = T.GetComponent<Rigidbody>();
				Ctarget += T.transform.position;
				aDesire += rb.velocity;
				sSum += (transform.position - T.transform.position) / radius;// * (radius - Vector3.Distance(transform.position, T.transform.position)) / radius;
			}

		}
		Ctarget /= hood;
		aDesire /= hood;
		sSum /= hood;



		Cforce = (Ctarget - transform.position).normalized*speed - rbz.velocity;
		Sforce = sSum.normalized * speed - rbz.velocity;        
		Aforce = aDesire.normalized*speed - rbz.velocity;

	}
	void flockingForce()
	{
		// var X = Lead.GetComponent<Rigidbody>().velocity;
		if (AIstate == 1) { rbz.AddForce(Wander); }
		if(AIstate == 0)
		{

			rbz.AddForce((Cforce + Sforce + Aforce).normalized * speed);
			//rbz.AddForce(Cforce);
			//rbz.AddForce(Cforce + Aforce);
			//rbz.AddForce(Sforce);
		}
		DoObstacle();

		Vector3 head = rbz.velocity;
		head.y = 0;
		transform.LookAt(transform.position + head, Vector3.up);
		//transform.LookAt(Lead.transform);
	}

	void Update ()
	{
		agent.destination = Prey.transform.position;
		DoWander();
		rbz.AddForce (Wander.normalized * speed);
//		WanderTime -= Time.deltaTime;
//		if(WanderTime <= 0)
//		{
//			WanderTime = WanderStart;
//			if (AIstate == 0) { AIstate = 1; }
//			else if (AIstate == 1) { AIstate = 0; }
//		}
//
//		if (/*IsEvading == false ||*/ AIstate == 0) { flocking();  }
//
//		if (AIstate == 1) { DoWander();  }
//
//		if (/*IsEvading == true &&*/ Prey != null && AIstate == 2)
//		{
//			DoEvade();
//			DoObstacle();
//			EvadeTime -= Time.deltaTime;
//			if(EvadeTime <= 0)
//			{
//				Prey = null;
//				//IsEvading = false;
//				AIstate = 0;
//				EvadeTime = EvadeStart;
//			}
//
//		}
	}
	void FixedUpdate()
	{
		//flockingForce();

	}






//	[SerializeField] private float speed = 10f;
//	private Rigidbody rb;
//	[SerializeField] private Transform[] wayPoints;
//	[SerializeField] private int currentWaypoint;
//	private Transform targetWaypoint;
//
//	// Use this for initialization
//	void Start () 
//	{
//		rb = gameObject.GetComponent<Rigidbody> ();
//	}
//	
//	// Update is called once per frame
//	void Update () 
//	{
//		MoveTowards ();
//	}
//
//	void MoveTowards()
//	{
//		if (currentWaypoint < this.wayPoints.Length) 
//		{
//			if (targetWaypoint == null)
//				targetWaypoint = wayPoints [currentWaypoint];
//
//			Walk ();
//		}
//	}
//
//	void Walk()
//	{
//		transform.forward = Vector3.RotateTowards (transform.forward, targetWaypoint.position - transform.position, speed * Time.deltaTime, 0f);
//
//		transform.position = Vector3.MoveTowards (transform.position, targetWaypoint.position, speed * Time.deltaTime);
//
//		if (transform.position == targetWaypoint.position) 
//		{
//			currentWaypoint++;
//			targetWaypoint = wayPoints [currentWaypoint];
//		}
//	}
}
