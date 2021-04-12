using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyC : MonoBehaviour
{
    Animator anim;
    public BoxCollider punchCollider;
    public GameObject[] paths;

    public NavMeshAgent enemy;
    public Transform player;
    public BoxCollider punchBox;

    public float MoveSpeed = 4;
    public float rotateSpeed = 3;
    public float maxDist = 10;
    public float minDist = 5;
    public float attackTime = 2f;

    bool startAttack;
    bool dead;


    private int startingPath = 0;
    private int pathLength = 0;
    private int lives = 3;

    bool followPlayer = false;
    bool playerClose = false;

    AudioSource audio;

    // Start is called before the first frame update
    void Start()
    {
        startingPath = 0;
        anim = GetComponent<Animator>();
        enemy = GetComponent<NavMeshAgent>();
        pathLength = paths.Length;
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (followPlayer == false)
        {
            if (startingPath < pathLength)
            {
                if (Vector3.Distance(paths[startingPath].transform.position, gameObject.transform.position) < 1)
                {
                    if (startingPath == pathLength - 1)
                    {
                        startingPath = 0;
                    }
                    else
                    {
                        anim.SetBool("Walk", true);
                        startingPath++;
                    }
                }

            }

            enemy.SetDestination(paths[startingPath].transform.position);

        }
        else
        {

            anim.SetBool("Walk", false);
            enemy.enabled = false;
            if(dead == false)
            {
                Path();
            }
        }

       
    }

    void Path()
    {

        transform.LookAt(player);
        /*
        Vector3 lookVector = player.transform.position - transform.position;
        lookVector.y = transform.position.y;
        Quaternion rot = Quaternion.LookRotation(lookVector);
        transform.rotation = Quaternion.Slerp(transform.rotation, rot, 1);
        */
        if (Vector3.Distance(transform.position, player.position) > maxDist)
        {
            playerClose = false;
        }
        if (Vector3.Distance(transform.position, player.position) >= minDist && playerClose == false)
        {
            transform.position += transform.forward * MoveSpeed * Time.deltaTime;
            anim.SetBool("ReadyAttack", false);
            anim.SetBool("Run", true);

            Debug.Log("far");
        }

        if (Vector3.Distance(transform.position, player.position) <= maxDist)
        {
            anim.SetBool("Run", false);
            anim.SetBool("ReadyAttack", true);
            startAttack = true;
            playerClose = true;
            Debug.Log("Close");
            Attack();
        }

      
    }

    void Attack()
    {
        if(startAttack && attackTime > 0)
        {
            attackTime -= Time.deltaTime;
            punchBox.enabled = false;
        }

        if(attackTime < 0)
        {
            startAttack = false;
            attackTime = 2;
            punchBox.enabled = true;
            if(dead == false)
            {
                anim.SetTrigger("Punch");
            }
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PunchBox"))
        {
            lives--;

            if (lives > 0)
            {
                anim.SetTrigger("Hurt");
                audio.Play(0);
            }
            if (lives <= 0)
            {
                anim.SetBool("Dead", true);
                dead = true;
                Destroy(gameObject, 3f);
            }


        } 
        
        if (other.gameObject.CompareTag("KickBox"))
        {

            anim.SetBool("Dead", true);
            Destroy(gameObject, 3f);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            followPlayer = true;
            this.gameObject.layer = 12;

        }
        if (other.gameObject.CompareTag("DetectPlayer"))
        {

           // punchBox.enabled = true;
            //anim.SetTrigger("Punch");
        }
    }
}
