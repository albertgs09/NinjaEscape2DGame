using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region
    [SerializeField]
    private InputActionReference movementControl;
    [SerializeField]
    private InputActionReference jumpControl; 
    [SerializeField]
    private InputActionReference attack1Control;
    [SerializeField]
    private InputActionReference attack2Control; 
    [SerializeField]
    private InputActionReference rollControl;
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 4;
    [SerializeField]
    private float rollSpeed = 1;
    int lives = 3;



    public float timeSinceAttack = 0f;
    public float rollTime = 0f;
    public float punchTime;
    private CharacterController controller;
    private Animator anim;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private bool punching;
    private bool kick = false;
    private bool jumping;
    private bool dead;
    private bool moving;
    public bool rolling;
    public bool punch;
    private Transform cameraMainTransform;
    public Transform rollPoint;

    public GameObject allHealth, health1, health2, health3;

    public BoxCollider punchCollider;
    public SphereCollider kickCollider;
    AudioSource audio;
    public AudioClip[] clips;
    #endregion

    private void OnEnable()
    {
        movementControl.action.Enable();
        jumpControl.action.Enable();
        attack1Control.action.Enable();
        attack2Control.action.Enable();
        rollControl.action.Enable();
    }
    private void OnDisable()
    {
        movementControl.action.Disable();
        jumpControl.action.Disable();
        attack1Control.action.Disable();
        attack2Control.action.Disable();
        rollControl.action.Disable();
    }

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        cameraMainTransform = Camera.main.transform;
        anim = gameObject.GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (dead == false)
        {
            timeSinceAttack += Time.deltaTime;
            //player is grounded
            groundedPlayer = controller.isGrounded;
            if (groundedPlayer)
            {
                punching = false;
                if (!punching)
                {
                    Attack();
                }

            }
            if (groundedPlayer && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
                kick = false;
                jumping = false;
            }

            Vector2 movement = movementControl.action.ReadValue<Vector2>();

            Vector3 move = new Vector3(movement.x, 0, movement.y);
            move = cameraMainTransform.forward * move.z + cameraMainTransform.right * move.x;
            move.y = 0;

            //moves player
            controller.Move(move * Time.deltaTime * playerSpeed);

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
            anim.SetFloat("Speed", move.magnitude);
           
           

            if (movement != Vector2.zero)
            {
                float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
                Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);

            }
           
            // Changes the height position of the player..
            if (jumpControl.action.triggered && groundedPlayer && timeSinceAttack > 1)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                anim.SetTrigger("Jump");
                jumping = true;
            }
            if (jumping)
            {
                Kick();
            }

            anim.SetBool("IsGrounded", groundedPlayer);
            anim.SetBool("Kicking", kick);

            if (timeSinceAttack > .7f)
            {
                punchCollider.enabled = false;
                kickCollider.enabled = false;
            }

            if(lives == 2)
            {
                health3.SetActive(false);
            }
            
            if(lives == 1)
            {
                health2.SetActive(false);
            }

           
            Roll();
        }
        if (rolling == true)
        {
            transform.Translate(Vector3.forward * rollSpeed * Time.deltaTime);
            rollTime += Time.deltaTime;

            Debug.Log("Rolling");

        }
        if (rollTime > .8f)
        {
            rolling = false;
            //rollTime = 0;
        }

        if (punch == true)
        {
            transform.Translate(Vector3.forward * 20 * Time.deltaTime);
            punchTime += Time.deltaTime;
        }

        if(punchTime > .35f)
        {
            punch = false;
        }
    }

    private void FixedUpdate()
    {
        //Roll();

    }

    void Kick()
    {
        if (Input.GetKeyDown(KeyCode.F) || attack2Control.action.triggered && timeSinceAttack > 1f)
        {
            timeSinceAttack = 0f;
            kick = true;
            kickCollider.enabled = true;
            anim.SetTrigger("Kick");
        }
    }
    void Attack()
    {
        if (Input.GetKeyDown(KeyCode.E) || attack1Control.action.triggered && timeSinceAttack > 1f)
        {
            
            anim.SetTrigger("Punch");
            timeSinceAttack = 0f;
            punchCollider.enabled = true;
            punchTime = 0;
            punch= true;
        }  
       
    }

    void Roll()
    {
        
        if (Input.GetKeyDown(KeyCode.R) || rollControl.action.triggered && timeSinceAttack > 1f)
        {
            anim.SetTrigger("Roll");
            rolling = true;
            rollTime = 0;
            timeSinceAttack = 0f;

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EPunchBox"))
        {
            lives--;
            if (lives > 0)
            {
                anim.SetTrigger("Hurt");
                audio.clip = clips[1];
            }
            if (lives <= 0)
            {
                health1.SetActive(false);
                anim.SetBool("Dead", true);
                dead = true;
                Destroy(gameObject, 3f);
            }
        }
        if (other.gameObject.CompareTag("Ship"))
        {
            Debug.Log("Ship Found");
        }
    }



}
