using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    public GameObject player;
	public AudioClip[] footsounds;
	public Transform eyes;
	public AudioSource growl;
	public GameObject deathCam;
	public Transform camPos;

	private NavMeshAgent nav;
	private AudioSource sound;
	private Animator anim;
	private string state = "idle";
	private bool alive = true;
	private float wait = 0f;
	private bool highAlert = false;
	private float alertness = 20f;

	// Use this for initialization
	void Start () 
	{
		nav = GetComponent<NavMeshAgent>();
		sound = GetComponent<AudioSource>();
		anim = GetComponent<Animator>();
		nav.speed = 2f;
		anim.speed = 1.2f;
	}

	public void footstep(int _num)
	{
		sound.clip = footsounds[_num];
		sound.Play();
	}

	//check if we can see the player//
	public void checkSight()
	{
		if(alive)
		{
			RaycastHit rayHit;
			if(Physics.Linecast(eyes.position,player.transform.position, out rayHit))
			{
				print("hit "+rayHit.collider.gameObject.name);
				if(rayHit.collider.gameObject.name == "Cylinder")
				{
					if(state != "kill")
					{
						state = "chase";
						nav.speed = 5f;
						anim.speed = 1.2f;
						growl.pitch = 1.2f;
						growl.Play();
					}
				}
			}
		}
	}

	
	// Update is called once per frame
	void Update () 
	{
		Debug.DrawLine(eyes.position,player.transform.position,Color.green);

		if(alive)
		{
			anim.SetFloat("velocity",nav.velocity.magnitude);

			if(state == "idle")
			{
				//pick a random place to walk to//
				Vector3 randomPos = Random.insideUnitSphere*alertness;
				NavMeshHit navHit;
				NavMesh.SamplePosition(transform.position + randomPos, out navHit,20f,NavMesh.AllAreas);

				//go near the player//
				if(highAlert)
				{
					NavMesh.SamplePosition(player.transform.position + randomPos, out navHit,20f,NavMesh.AllAreas);
					//each time, lose awareness of player general position//
					alertness += 5f;

					if(alertness > 20f)
					{
						highAlert = false;
						nav.speed = 1.2f;
						anim.speed = 1.2f;
					}
				}


				nav.SetDestination(navHit.position);
				state = "walk";
			}
			if(state == "walk")
			{
				if(nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending)
				{
					state = "search";
					wait = 5f;
				}
			}
			if(state == "search")
			{
				if(wait > 0f)
				{
					wait -= Time.deltaTime;
					transform.Rotate(0f,120f*Time.deltaTime,0f);
				}
				else
				{
					state = "idle";
				}
			}
			if(state == "chase")
			{
				nav.destination = player.transform.position;

				float distance = Vector3.Distance(transform.position,player.transform.position);
				if(distance > 10f)
				{
					state = "hunt";
				}
				else if(nav.remainingDistance <= nav.stoppingDistance + 1f && !nav.pathPending)
				{
					if(player.GetComponent<Player>().alive)
					{
						state = "kill";
						player.GetComponent<Player>().alive = false;
						player.GetComponent<PlayerMovement>().enabled = false;
						player.GetComponent<CharacterController>().enabled = false;
						deathCam.SetActive(true);
						deathCam.transform.position = Camera.main.transform.position;
						deathCam.transform.rotation = Camera.main.transform.rotation;
						Camera.main.gameObject.SetActive(false);
						growl.pitch = 0.7f;
						growl.Play();
						Invoke("reset",1f);
					}
				}
			}
			if(state == "hunt")
			{
				if(nav.remainingDistance <= nav.stoppingDistance && !nav.pathPending)
				{
					state = "search";
					wait = 5f;
					highAlert = true;
					alertness = 5f;
					checkSight();
				}
			}
			if(state == "kill")
			{
				deathCam.transform.position = Vector3.Slerp(deathCam.transform.position,camPos.position,10f*Time.deltaTime);
				deathCam.transform.rotation = Quaternion.Slerp(deathCam.transform.rotation,camPos.rotation,10f*Time.deltaTime);
				anim.speed = 1f;
				nav.SetDestination(deathCam.transform.position);
			}

			//nav.SetDestination(player.transform.position);
		}
	}

	void reset()
	{
		Cursor.lockState = CursorLockMode.None;
		SceneManager.LoadScene("Menu");
	}
}
