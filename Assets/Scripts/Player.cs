using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Player : MonoBehaviour
{
    public const int DEFAULT_MAX_JUMPS = 1;
    public const float GROUNDING_COOLDOWN = 0.25f;
    public const float JUMP_POWER = 5.0f;
    public const float START_OF_JUMP_JUMP_POWER = 5.0f;
    
    public const float jumpGraceTime = 0.1f;


    public enum Inputs
    {
        Left,
        Right,
        Up,
        Down
    }

    private float jumpBufferDuration = 0.1f;

    public float baseVelocity = 0.0f; // Change if riding a platform, etc.
    
    
    private InputAction mLeft, mRight, mUp, mDown;

    
    public bool[] inputIsActive = { false, false, false, false };
    public bool[] moveInDir = { false, false, false, false };
    public bool[] nullInputCheckingStorage = { false, false, false, false };

    
    public int maxJumps = DEFAULT_MAX_JUMPS;
    public int jumps = DEFAULT_MAX_JUMPS;
    public bool isJumping = false;
    public float totalJumpTimeHeld = 0.0f;
    
    public float jumpGraceTimer = jumpGraceTime;
    public bool resetJumpsInAir = false;

    public bool bufferedJump = false;

    [SerializeField]
    private GameObject spawnpoint;
    
    private Rigidbody2D physicsController;
    private BoxCollider2D mCollider;

    public float groundingCooldown = 0.0f;
    public bool isGrounded = false;

    public AudioClip jumpAudio;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        physicsController = GetComponent<Rigidbody2D>();
        mCollider = GetComponent<BoxCollider2D>();
        physicsController.freezeRotation = true;
 

        mLeft = new InputAction(binding: "<Keyboard>/a");
        SetupInputSystemWithoutStarted(
            ref mLeft,
            ct => StartMovingLeft(),
            ct => StopMovingLeft(),
            true, "<Keyboard>/left");
        
        
        mRight = new InputAction(binding: "<Keyboard>/d");
        SetupInputSystemWithoutStarted(
            ref mRight,
            context => StartMovingRight(),
            context => StopMovingRight(),
            true, "<Keyboard>/right");
        
        
        mUp = new InputAction(binding: "<Keyboard>/w");
        // mUp.performed += context =>
        // {
        //     inputIsActive[(int)Inputs.Up] = true;
        //     
        //     if ( jumps > 0 )
        //     {
        //         GetComponent<Rigidbody2D>().linearVelocityY = 2.5f;
        //         isJumping = true;
        //         
        //         jumps -= 1;
        //         groundingCooldown = GROUNDING_COOLDOWN;
        //     }
        //     
        // };
        
        SetupInputSystemWithoutStarted(
            ref mUp,
            context =>
            {
                inputIsActive[(int)Inputs.Up] = true;
            
                // StartJump();
                StartCoroutine(BufferJump());
            },
            context =>
            {
                inputIsActive[(int)Inputs.Up] = false;
                totalJumpTimeHeld = 0.0f;
            
                isJumping = false;
            
            }, true, "<Keyboard>/space");
        
        
        mDown = new InputAction(binding: "<Keyboard>/s");

        mDown.started += context => Debug.Log($"Input Started Recieved {context}");
        mDown.performed += context => Debug.Log($"Performed Phase Input Recieved {context}");
        mDown.canceled += context => Debug.Log($"Cancelled Recieved {context}");
        
        mDown.Enable();
    }

    private void FixedUpdate()
    {
        // How do we give the check more time to check if grounded?
        isGrounded = IsGrounded();
    }

    void Update()
    {

        if (groundingCooldown > Constants.TimeEpsilon)
        {
            groundingCooldown -= Time.deltaTime;
        }
        else
        {
            groundingCooldown = 0.0f;
        }

        if (isGrounded && groundingCooldown < Constants.TimeEpsilon)
        {
            jumps = maxJumps;
            resetJumpsInAir = false;
            jumpGraceTimer = jumpGraceTime;

        }

        if (isGrounded || (!isGrounded && jumpGraceTimer > Constants.TimeEpsilon))
        {
            if(bufferedJump)
                StartJump();
        }

        if (!isGrounded && jumpGraceTimer > Constants.TimeEpsilon && !resetJumpsInAir)
        {
            jumpGraceTimer -= Time.deltaTime;
        }
        
        else if (!isGrounded && jumpGraceTimer < Constants.TimeEpsilon && !resetJumpsInAir)
        {
            jumpGraceTimer = 0.0f;
            jumps -= 1;
            resetJumpsInAir = true;
        }
        
        // W
        //keep the same velocity when jumping for 0.2 seconds
        if (isJumping && totalJumpTimeHeld <= 0.2f)
        {
            physicsController.linearVelocityY = Math.Max(JUMP_POWER, physicsController.linearVelocityY);

            totalJumpTimeHeld += Time.deltaTime;

        }
        
        
        
        
        // A
        if (moveInDir[(int)Inputs.Left])
        {
            // Start moving left
            physicsController.linearVelocityX = baseVelocity - 7.5f;
            
        }
        
        // S
        if (inputIsActive[(int)Inputs.Down])
        {
            // Crouch (will we have fall-through platforms)?
            
        }
        
        // D
        if (moveInDir[(int)Inputs.Right])
        {
            // Start moving right
            physicsController.linearVelocityX = baseVelocity + 7.5f;
        }

        if (!(inputIsActive[(int)Inputs.Right] || inputIsActive[(int)Inputs.Left]))
        {
            physicsController.linearVelocityX = baseVelocity;
        }

        if (transform.position.y < -10.0f)
        {
            KillPlayer();
        }
        
        
    }

    private void KillPlayer()
    {
        //play sound effect here
        
        
        //for right now, move the player back to their spawnpoint\

        physicsController.position = spawnpoint.transform.position;
        physicsController.linearVelocity = Vector2.zero;


        //reset all the pertinent player variables
    }

    void StartMovingLeft()
    {
        inputIsActive[(int)Inputs.Left] = true;
        moveInDir[(int)Inputs.Left] = true;
                
        if(moveInDir[(int)Inputs.Right])
        {
            nullInputCheckingStorage[(int)Inputs.Right] = true;
            moveInDir[(int)Inputs.Right] = false;
        }
    }

    void StopMovingLeft()
    {
        inputIsActive[(int)Inputs.Left] = false;
        moveInDir[(int)Inputs.Left] = false;
        // GetComponent<Rigidbody2D>().linearVelocityX = 0.0f;

        if (nullInputCheckingStorage[(int)Inputs.Right])
        {
            nullInputCheckingStorage[(int)Inputs.Right] = false;
            moveInDir[(int)Inputs.Right] = inputIsActive[(int)Inputs.Right];
        }
    }

    void StartMovingRight()
    {
        inputIsActive[(int)Inputs.Right] = true;
        moveInDir[(int)Inputs.Right] = true;
                
        if(moveInDir[(int)Inputs.Left])
        {
            nullInputCheckingStorage[(int)Inputs.Left] = true;
            moveInDir[(int)Inputs.Left] = false;
        }
    }

    void StopMovingRight()
    {
        inputIsActive[(int)Inputs.Right] = false;
        moveInDir[(int)Inputs.Right] = false;
                
        if(nullInputCheckingStorage[(int)Inputs.Left])
        {
            nullInputCheckingStorage[(int)Inputs.Left] = false;
            moveInDir[(int)Inputs.Left] = inputIsActive[(int)Inputs.Left];
        }
    }
    
    bool IsGrounded()
    {
        if (groundingCooldown > Constants.TimeEpsilon)
            return false;
        
        return Physics2D.BoxCast(transform.position, mCollider.bounds.size, 0.0f, Vector3.down, 0.02f);
    }

    private void SetupInputSystemWithoutStarted(
        ref InputAction action,
        Action<InputAction.CallbackContext> performed,
        Action<InputAction.CallbackContext> cancelled,
        bool enableNow = true,
        params string[] secondaryDefaultBindings)
    {
        SetupInputSystem(ref action, null, performed, cancelled, enableNow, secondaryDefaultBindings);
    }

    private void SetupInputSystem(
        ref InputAction action, 
        Action<InputAction.CallbackContext> started,
        Action<InputAction.CallbackContext> performed, 
        Action<InputAction.CallbackContext> cancelled, 
        bool enableNow = true,
        params string[] secondaryDefaultBindings)
    {
        
        
        if (started != null)
            action.started += started;

        if (performed != null)
            action.performed += performed;

        if (cancelled != null)
            action.canceled += cancelled;
        
        foreach (string s in secondaryDefaultBindings)
        {
            action.AddBinding(s);
        }
        
        if(enableNow)
            action.Enable();
    }


    void StartJump()
    {
        if ( jumps > 0 )
        {
            physicsController.linearVelocityY = START_OF_JUMP_JUMP_POWER;
            // varJumpSpeed = physicsController.linearVelocityY;
            
                
            isJumping = true;
                
            jumps -= 1;
            groundingCooldown = GROUNDING_COOLDOWN;

            resetJumpsInAir = true;
            
            SoundFXManager.Instance.PlaySoundFXClip(jumpAudio, transform, 1.0f);

        }
    }

    IEnumerator BufferJump()
    {
        bufferedJump = true;
        
        yield return new WaitForSeconds(jumpBufferDuration);

        bufferedJump = false;
    }
}
