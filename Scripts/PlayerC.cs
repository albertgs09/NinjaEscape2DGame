using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerC : MonoBehaviour
{
    
    public float speed = 4;
    public float jumpSpeed = 0;
    public float timeSinceAttack = 0;
    float timeLeft = 1.5f;
    float inputX;

    public int points;
    //playerPrefs
    int fail;
    int lights;
    int Edead;
    int playerSeen;
    int playerSeen2;
    int loadLvl;
    bool facingRight = true;
    bool attacking;
    bool startTime;
    public bool dead;
    public bool nearEnemy;
    public bool nearEnemy2;
    public BoxCollider2D trigger;
    public CapsuleCollider2D knife;
    public Transform startPosition;
    public GameObject hoverWalls;
    Rigidbody2D rb;
    Animator anim;
    Vector3 localScale;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("Points", 0);
        PlayerPrefs.SetInt("Fail", 0);
        PlayerPrefs.SetInt("PlayerSeen", 0);

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        localScale = transform.localScale;
        knife.enabled = false;
        //transform.position = startPosition.position; // starting position
    }

    
    // Update is called once per frame
    void Update()
    {
        timeSinceAttack += Time.deltaTime;
        float inputX = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
        if (inputX > 0)
        {
            anim.SetBool("Walking", true);
            facingRight = true;
            Right();
        }
        else if (inputX < 0)
        {
            anim.SetBool("Walking", true);
            facingRight = false;
            Left();
        }

        if (((facingRight) && (localScale.x < 0)) || ((!facingRight) && (localScale.x > 0)))
        {
            localScale.x *= -1;
        }
        transform.localScale = localScale;

        if (startTime && timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
        }

        if(timeLeft < 0)
        {
            startTime = false;
            knife.enabled = false ;
            timeLeft = 1.5f;
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
            rb.constraints &= ~RigidbodyConstraints2D.FreezePositionX;
        }
       

        if (inputX == 0)
        {
            anim.SetBool("Walking", false);
        }
        
        if (Input.GetKey(KeyCode.Return) && timeSinceAttack > 1f)
        {
            startTime = true;
            attacking = true;
            knife.enabled = true;
            anim.SetTrigger("Attack2");
            //rb.constraints = RigidbodyConstraints2D.FreezePositionX;

           rb.constraints = RigidbodyConstraints2D.FreezeAll;
            timeSinceAttack = 0;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            anim.SetBool("Run", true);
            anim.SetBool("Walking", false);
            speed = 2;

        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.SetBool("Run", false);
            anim.SetBool("Walking", true);
            speed = 1.5f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && timeSinceAttack > 1f)
        {
           rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            timeSinceAttack = 0f;
        }

        if (Input.GetKey(KeyCode.RightShift))
        {
            PlayerPrefs.SetInt("Lights", 0);
        }
        if (nearEnemy && attacking)
        {
            PlayerPrefs.SetInt("EDead", 1);
        }

        if (nearEnemy ) // seperate nearEnemy bools
        {
            Physics2D.IgnoreLayerCollision(8, 9);
        }
        else if((nearEnemy == false) && attacking)
        {
            Physics2D.IgnoreLayerCollision(8, 9, false);
        } 
        if (nearEnemy2) // seperate nearEnemy bools
        {
            Physics2D.IgnoreLayerCollision(8, 9);
        }
        else if((nearEnemy2 == false) && attacking)
        {
            Physics2D.IgnoreLayerCollision(8, 9, false);
        }


        fail = PlayerPrefs.GetInt("Fail");
        if(fail == 1)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        loadLvl = PlayerPrefs.GetInt("Levels");
    }
   
    public void Right()
    {

    }

    public void Left()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Trigger"))
        {
            Destroy(other.gameObject);
            if(trigger != null)
            {
                trigger.enabled = false;
            } 
            
            if( hoverWalls != null)
            {
                hoverWalls.SetActive(true);
            }

            if(trigger == null && hoverWalls == null)
            {
                transform.position = startPosition.position;
                PlayerPrefs.SetInt("Lights", 0);
            }
        }

        if (other.CompareTag("Enemy"))
        {
            //attacking = true;
            nearEnemy = true;
        }
        
        if (other.CompareTag("Enemy2"))
        {
            //attacking = true;
            nearEnemy2 = true;
        }


        if (other.CompareTag("EyeContact"))
        {
            PlayerPrefs.SetInt("PlayerSeen", 1);
                }
        if (other.CompareTag("EyeContact2"))
        {
            PlayerPrefs.SetInt("PlayerSeen2", 1);
        }

        if (other.CompareTag("Weapon2"))
        {
            anim.SetTrigger("Dead");
            Destroy(gameObject, 1f);
            knife.enabled = false;
            PlayerPrefs.SetInt("Fail", 1);

        }

        if (other.CompareTag("PickUps"))
        {
            ++points;
            Destroy(other.gameObject);
            PlayerPrefs.SetInt("Points", points);
        }

        if (other.CompareTag("Next"))
        {
            loadLvl = PlayerPrefs.GetInt("Levels");
            PlayerPrefs.SetInt("Levels",++loadLvl);
            SceneManager.LoadScene(loadLvl);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            nearEnemy = false;
        }
        
        if (other.CompareTag("Enemy2"))
        {
            nearEnemy2 = false;
        }
    }

 
}
