using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : MonoBehaviour
{
    [SerializeField] private Transform start;
    [SerializeField] private Transform end;

    public KnifeControl knife;
    Animator anim;
    CapsuleCollider2D cc;
    public CapsuleCollider2D sight;
    float timeLeft = 5;
    public float speed = .1f;
    int timesHit = 0;
    int playerSeen2;
    int light;
    public Transform Character;
    public bool dead;
    public bool playerSeen;
    bool timeStart;
    public bool playerDead;
    bool startPath;
    bool endPath;
    bool hit;
    public bool facingRight = true;
    Vector3 localScale;
    private Vector3 directionOfCharacter;
    // Start is called before the first frame update
    void Start()
    {
        startPath = true;
        anim = GetComponent<Animator>();
        cc = GetComponent<CapsuleCollider2D>();
        localScale = transform.localScale;
        transform.position = start.position;
        PlayerPrefs.SetInt("PlayerSeen2", 0);

    }

    // Update is called once per frame
    void Update()
    {
        Prefs();
        //if no player then doesnt do anythinf
        if (Character != null)
        {


            if (dead)
            {
                anim.SetTrigger("Dead");
                Destroy(gameObject, 1.5f);
                playerSeen = false;
            }
            else if (dead == false && playerSeen == false)
            {
                Path();
            }

            if (timeStart && timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
            }

            if (timeLeft < 0)
            {
                timeStart = false;
                timeLeft = 5;
                endPath = !endPath;
                startPath = !startPath;
            }
            if (playerSeen)
            {

                directionOfCharacter = Character.transform.position - transform.position;
                directionOfCharacter = directionOfCharacter.normalized;    // Get Direction to Move Towards
                transform.Translate(directionOfCharacter * speed, Space.World);
                anim.SetTrigger("Attack");
                knife.IsFiring = true;
                if (Character.transform.position.x < transform.position.x)
                {
                    facingRight = false;
                }
                else
                {
                    facingRight = true;
                }
            }
            else
            {
                knife.IsFiring = false;
            
            }

            if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
            {
                localScale.x *= -1;
            }
            transform.localScale = localScale;

            light = PlayerPrefs.GetInt("Lights");

            if (light == 0)
            {
                playerSeen = false;
                sight.enabled = false;
            }
            else
            {
                sight.enabled = true;
            }
        }
        else
        {
            playerDead = true;
            
        }
    }

    void Prefs()
    {
        playerSeen2 = PlayerPrefs.GetInt("PlayerSeen2");

        if(playerSeen2 == 0)
        {
            playerSeen = false;
        }
        else
        {
            playerSeen = true;
        } 
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Weapon"))
        {
            dead = true;
            sight.enabled = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(start.position, Vector3.one * 0.1f);
        Gizmos.DrawCube(end.position, Vector3.one * 0.1f);
    }

    void Path()
    {
        if (startPath)
        {
            transform.position = Vector2.Lerp(transform.position, end.position, Time.deltaTime * .35f);
            facingRight = false;
            timeStart = true;
        }
        if (endPath)
        {
            transform.position = Vector2.Lerp(transform.position, start.position, Time.deltaTime * .35f);
            facingRight = true;
            timeStart = true;

        }
    }
}
