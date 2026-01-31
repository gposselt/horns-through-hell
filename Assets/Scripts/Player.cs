using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.Collections;

public class Player : MonoBehaviour
{
    public const int DEFAULT_MAX_JUMPS = 1;
    private const float GROUNDING_COOLDOWN = 0.1f;

    public enum Inputs
    {
        Left,
        Right,
        Up,
        Down
    }
    
    
    private InputAction mLeft, mRight, mUp, mDown;

    
    public bool[] inputIsActive = {false, false, false, false};

    
    public int maxJumps = DEFAULT_MAX_JUMPS;
    public int jumps = DEFAULT_MAX_JUMPS;
    public bool mIsJumping = false;
    
    private Rigidbody2D physicsController;
    private BoxCollider2D mCollider;

    public float groundingCooldown = 0.0f;
    public bool isGrounded = false;
    
    public float totalJumpTimeHeld = 0.0f;

    public AudioClip jumpAudio;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        physicsController = GetComponent<Rigidbody2D>();
        mCollider = GetComponent<BoxCollider2D>();
        physicsController.freezeRotation = true;
        // InputSystem.onEvent += (ptr, device) => Debug.Log($"Input For Device: {device}");
        
        // Keyboard.current.onTextInput 

        mLeft = new InputAction(binding: "<Keyboard>/a");

        // mLeft.started += context => Debug.Log($"Input Started Recieved {context}");
        mLeft.performed += context =>
        {
            inputIsActive[(int)Inputs.Left] = true;
        };
        mLeft.canceled += context =>
        {
            inputIsActive[(int)Inputs.Left] = false;
            GetComponent<Rigidbody2D>().linearVelocityX = 0.0f;
        };
        
        mRight = new InputAction(binding: "<Keyboard>/d");

        // mRight.started += context => Debug.Log($"Input Started Recieved {context}");
        mRight.performed += context => 
        {
            inputIsActive[(int)Inputs.Right] = true;
        };
        mRight.canceled += context =>
        {
            inputIsActive[(int)Inputs.Right] = false;
            GetComponent<Rigidbody2D>().linearVelocityX = 0.0f;
        };
        
        mUp = new InputAction(binding: "<Keyboard>/w");

        // mUp.started += context => Debug.Log($"Input Started Recieved {context}");
        mUp.performed += context =>
        {
            inputIsActive[(int)Inputs.Up] = true;
            
            if ( jumps > 0 )
            {
                GetComponent<Rigidbody2D>().linearVelocityY = 2.5f;
                mIsJumping = true;
                
                jumps -= 1;
                groundingCooldown = GROUNDING_COOLDOWN;
                SoundFXManager.Instance.PlaySoundFXClip(jumpAudio, transform, 1.0f);
            }
            
        };
        
        mUp.canceled += context =>
        {
            inputIsActive[(int)Inputs.Up] = false;
            totalJumpTimeHeld = 0.0f;
            
            mIsJumping = false;
            
        };
        
        mDown = new InputAction(binding: "<Keyboard>/s");

        mDown.started += context => Debug.Log($"Input Started Recieved {context}");
        mDown.performed += context => Debug.Log($"Performed Phase Input Recieved {context}");
        mDown.canceled += context => Debug.Log($"Cancelled Recieved {context}");
        
        mLeft.Enable(); 
        mRight.Enable();
        mDown.Enable();
        mUp.Enable();
    }

    private void FixedUpdate()
    {
        // How do we give the check more time to check if grounded?
        isGrounded = IsGrounded();
    }

    // Update is called once per frame
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

        if (isGrounded)
        {
            jumps = maxJumps;
        }
        
        // W
        if (mIsJumping && totalJumpTimeHeld <= 0.25f)
        {
            
            //Increment based on :sparkles: math :sparkles:

            physicsController.linearVelocityY += 1.0f / (20.0f * (totalJumpTimeHeld + 0.1f));

            totalJumpTimeHeld += Time.deltaTime;

        }
        
        
        // A
        if (inputIsActive[(int)Inputs.Left])
        {
            // Start moving left
            physicsController.linearVelocityX = -7.5f;
            
        }
        
        // S
        if (inputIsActive[(int)Inputs.Down])
        {
            // Crouch (will we have fall-through platforms)?
        }
        // D
        if (inputIsActive[(int)Inputs.Right])
        {
            // Start moving right
            physicsController.linearVelocityX = 7.5f;
        }
        
        
    }
    
    bool IsGrounded()
    {
        if (groundingCooldown > Constants.TimeEpsilon)
            return false;
        
        return Physics2D.BoxCast(transform.position, mCollider.bounds.size, 0.0f, Vector3.down, 0.01f);
    }

    // private void OnCollisionStay(Collision other)
    // {
    //     if (other.gameObject.CompareTag("Ground"))
    //         isGrounded = true;
    //
    // }
}
