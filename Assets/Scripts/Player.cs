using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Unity.Collections;

public class Player : MonoBehaviour
{
    public const int DEFAULT_MAX_JUMPS = 1;

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
    
    Rigidbody2D physicsController;
    private BoxCollider2D collider;

    public bool isGrounded = false;


    public float totalJumpTimeHeld = 0.0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        physicsController = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
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

    // Update is called once per frame
    void Update()
    {

        if (IsGrounded())
        {
            jumps = maxJumps;
        }
        
        // W
        if (mIsJumping && totalJumpTimeHeld <= 1.5f)
        {
            
            //Increment based on :sparkles: math :sparkles:

            physicsController.linearVelocityY += 1.0f / (6.0f * (totalJumpTimeHeld + 0.1f));

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
        return Physics2D.BoxCast(transform.position, collider.size, 0.0f, Vector3.down, 0.05f);
    }

    // private void OnCollisionStay(Collision other)
    // {
    //     if (other.gameObject.CompareTag("Ground"))
    //         isGrounded = true;
    //
    // }
}
