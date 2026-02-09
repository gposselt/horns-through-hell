using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using EditorAttributes;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using Random = UnityEngine.Random;

[Serializable]
public enum Masks
{
    Lyre,
    Guitar, // Guitar
    Cymbal, // Cymbal
    Tuba, // Tuba
    Panflute // Panflute
}

[Serializable]
public class AmmoHolder
{
    //public int maxGitaurAmmo = 15,
    //    maxCymbalAmmo = 5,
    //    maxTubaAmmo = 7,
    //    maxPanfluteAmmo = 30;

    //public int[] remainingAmmo;

    public SerializedDictionary<Masks, int> remainingAmmo;

    public static readonly SerializedDictionary<Masks, int> maxAmmos = new()
    {
        {Masks.Lyre, -1},
        {Masks.Guitar, 15 },
        {Masks.Cymbal, 5 },
        {Masks.Tuba, 7 },
        {Masks.Panflute, 30 }
    };

    public SerializedDictionary<Masks, Projectile> proj;


    public AmmoHolder()
    {
        //remainingAmmo = new[] { -1, maxGitaurAmmo, maxCymbalAmmo, maxTubaAmmo, maxPanfluteAmmo };

        remainingAmmo = new()
        {
            {Masks.Lyre, -1},
            {Masks.Guitar, 0 },
            {Masks.Cymbal, 0 },
            {Masks.Tuba, 0 },
            {Masks.Panflute, 0  }
        };
        
    }


}

public class Player : MonoBehaviour
{
    public const int DEFAULT_MAX_JUMPS = 1;
    public const float GROUNDING_COOLDOWN = 0.25f;
    public float JUMP_POWER = 5.0f;
    public float START_OF_JUMP_JUMP_POWER = 5.0f;
    public const int DEFAULT_HP = 10;

    public const float jumpGraceTime = 0.1f;
    
    //Parameter Name Constants, used for the animator.
    private static readonly int AnimSpeed = Animator.StringToHash("Speed");
    private static readonly int AnimGrounded = Animator.StringToHash("Grounded");
    private static readonly int AnimAttack = Animator.StringToHash("Attack");

    public enum Inputs
    {
        Left,
        Right,
        Up,
        Down,
        Shoot
    }

    public Projectile[] projPrefab;
    public Projectile[] projPrefab1;

    public Projectile[] projPrefab2;

    public Projectile[] projPrefab3;
    
    public Projectile[] projPrefab4;



    private float jumpBufferDuration = 0.1f;

    public float baseVelocity = 0.0f; // Change if riding a platform, etc.

    public float projectileLifetime = 1.0f;

    public const float SHOOT_COOLDOWN = 1.0f;
    public float shootTimer = 0.0f;


    private InputAction mLeft, mRight, mUp, mDown, mShoot;
    
    
    private InputAction mMaskOne, mMaskTwo, mMaskThree, mMaskFour, mMaskAction;

    //MASK STUFF
    
    
    [SerializeField] 
    public Masks activeMask = Masks.Lyre;

    public AmmoHolder ammo = new AmmoHolder();
    
    //END MASK STUFF

    [SerializeField, ReadOnly] public bool[] inputIsActive = { false, false, false, false, false };
    [SerializeField, ReadOnly] public bool[] moveInDir = { false, false, false, false };
    [SerializeField, ReadOnly] public bool[] nullInputCheckingStorage = { false, false, false, false };

    //right = true, left = false
    private bool lastDirection = true;
    
    

    //JUMPING STUFF
    public int playerHp = DEFAULT_HP;
    public int maxJumps = DEFAULT_MAX_JUMPS;
    public int jumps = DEFAULT_MAX_JUMPS;
    public bool isJumping = false;
    public float totalJumpTimeHeld = 0.0f;

    public float jumpGraceTimer = jumpGraceTime;
    public bool resetJumpsInAir = false;

    public bool bufferedJump = false;
    
    //END JUMPING STUFF

    [SerializeField]
    public GameObject spawnpoint;

    private Rigidbody2D physicsController;
    private BoxCollider2D mCollider;

    public float groundingCooldown = 0.0f;
    public bool isGrounded = false;

    public AudioClip jumpAudio;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Coroutine shooting;

    IEnumerator Shoot()
    {

        if (shootTimer > 0.0f)
        {
            shooting = null;
        }
        else
        {
            animator.SetTrigger(AnimAttack);

            yield return new WaitForSeconds(0.5f);

            TryShootProjectile();

            shooting = null;
        }


    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        physicsController = GetComponent<Rigidbody2D>();
        mCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        physicsController.freezeRotation = true;
        
        

        //If I goof, the system will yell at me before you guys do.
        if (animator == null)
            Debug.LogWarning("[Player] Missing Animator component on Player.");

        if (spriteRenderer == null)
            Debug.LogWarning("[Player] Missing SpriteRenderer component on Player.");



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

            }, true);


        mDown = new InputAction(binding: "<Keyboard>/s");
        

        mDown.Enable();

        mShoot = new InputAction(binding: "<Keyboard>/space");

        SetupInputSystemWithoutStarted(ref mShoot, context =>
        {
            inputIsActive[(int)Inputs.Shoot] = true;
        },
        context =>
        {
            inputIsActive[(int)Inputs.Shoot] = false;
        }, true);


        mMaskOne = new InputAction(binding: "<Keyboard>/1");

        SetupInputSystemWithoutStarted(ref mMaskOne, _ =>
            {
                if (activeMask == Masks.Guitar)
                {
                    activeMask = Masks.Lyre;
                }
                else
                {
                    activeMask = Masks.Guitar;
                }
            }, null
            );

        mMaskTwo = new InputAction(binding: "<Keyboard>/2");
        
        SetupInputSystemWithoutStarted(ref mMaskTwo, _ =>
        {
            if (activeMask == Masks.Cymbal)
            {
                activeMask = Masks.Lyre;
            }
            else
            {
                activeMask = Masks.Cymbal;
            }
        }, null );

        mMaskThree = new InputAction(binding: "<Keyboard>/3");
        
        SetupInputSystemWithoutStarted(ref mMaskThree, _ =>
        {
            if (activeMask == Masks.Tuba)
            {
                activeMask = Masks.Lyre;
            }
            else
            {
                activeMask = Masks.Tuba;
            }
        }, null);

        mMaskFour = new InputAction(binding: "<Keyboard>/4");
        
        SetupInputSystemWithoutStarted(ref mMaskFour, _ =>
        {
            if (activeMask == Masks.Panflute)
            {
                activeMask = Masks.Lyre;
            }
            else
            {
                activeMask = Masks.Panflute;
            }
        }, null);

    }

    private void FixedUpdate()
    {
        // How do we give the check more time to check if grounded?
        isGrounded = IsGrounded();
    }

    void Update()
    {

        if (playerHp <= 0)
        {
            // Kill the player, whatever that logic will be.
            //Debug.Log("Player is dead!");
        }

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
            if (bufferedJump)
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
            // physicsController.linearVelocityY = Math.Min(JUMP_POWER, physicsController.linearVelocityY);

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

        // Space (shoot)

        if (shootTimer > 0.0f)
            shootTimer -= Time.deltaTime;

        if (inputIsActive[(int)Inputs.Shoot])
        {

            if (shooting == null)
                shooting = StartCoroutine(Shoot());

            // if (shootTimer <= 0.0f)
            // {
            //     //Im trying to time it so that the animation plays when the projectile is launched
            //     if (animator != null)
            //         animator.SetTrigger(AnimAttack);
            //     
            //     // Shoot has been charged up.
            //     Vector3 direction;
            //
            //     if (!lastDirection)
            //     {
            //         direction = Vector3.left;
            //     }
            //     else
            //     {
            //         direction = Vector3.right;
            //     }
            //
            //     Projectile proj = Instantiate<Projectile>(projPrefab, transform.position + direction, Quaternion.identity);
            //     proj.LaunchProjectile(direction * 10.0f, projectileLifetime);
            //     shootTimer = SHOOT_COOLDOWN;
            // }
        }

        if (!(inputIsActive[(int)Inputs.Right] || inputIsActive[(int)Inputs.Left]))
        {
            physicsController.linearVelocityX = baseVelocity;
        }

        if (transform.position.y < -10.0f)
        {
            KillPlayer();
        }


        //-------------------
        // ANIMATION! :D
        //-------------------

        if (animator != null)
        {
            animator.SetFloat(AnimSpeed, Mathf.Abs(physicsController.linearVelocityX));
            animator.SetBool(AnimGrounded, isGrounded);
        }

        //Sprites face LEFT by default.
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = lastDirection;
        }
    }

    private void KillPlayer()
    {
        //play sound effect here


        //for right now, move the player back to their spawnpoint\

        if(spawnpoint)
        {
            physicsController.position = spawnpoint.transform.position;
            physicsController.linearVelocity = Vector2.zero;
        }   


        //reset all the pertinent player variables
    }

    void StartMovingLeft()
    {
        inputIsActive[(int)Inputs.Left] = true;
        moveInDir[(int)Inputs.Left] = true;
        lastDirection = false;

        if (moveInDir[(int)Inputs.Right])
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

            if (moveInDir[(int)Inputs.Right])
            {
                lastDirection = true;
            }
        }
    }

    void StartMovingRight()
    {
        inputIsActive[(int)Inputs.Right] = true;
        moveInDir[(int)Inputs.Right] = true;

        lastDirection = true;


        if (moveInDir[(int)Inputs.Left])
        {
            nullInputCheckingStorage[(int)Inputs.Left] = true;
            moveInDir[(int)Inputs.Left] = false;
        }
    }

    void StopMovingRight()
    {
        inputIsActive[(int)Inputs.Right] = false;
        moveInDir[(int)Inputs.Right] = false;

        if (nullInputCheckingStorage[(int)Inputs.Left])
        {
            nullInputCheckingStorage[(int)Inputs.Left] = false;
            moveInDir[(int)Inputs.Left] = inputIsActive[(int)Inputs.Left];

            if (moveInDir[(int)Inputs.Left])
            {
                lastDirection = false;
            }
        }
    }

    void TryShootProjectile()
    {
        if (shootTimer <= 0.0f)
        {
            Masks currentWeapon = activeMask;
            
                
            // Shoot has been charged up.
            //shoot hahs been charged op.
            Vector3 direction;

            if (!lastDirection)
            {
                direction = Vector3.left;
            }
            else
            {
                direction = Vector3.right;
            }


            if (currentWeapon == Masks.Lyre)
            {
                ShootLyreShot(direction);
            }
            else
            {
                //shoot the right shot here

                if (ammo.remainingAmmo[currentWeapon] == 0)
                    return;

                if (currentWeapon == Masks.Tuba)
                {
                    ShootCyclopsShot(direction);
                }

                if (currentWeapon == Masks.Guitar)
                {
                    ShootGorgonFreemanShot(direction);
                }

                if (currentWeapon == Masks.Cymbal)
                {
                    ShootHarpyShot(direction);
                }

                if (currentWeapon == Masks.Panflute)
                {
                    ShootMinotaurShot(direction);
                }

                ammo.remainingAmmo[currentWeapon] -= 1;

                if (ammo.remainingAmmo[currentWeapon] == 0)
                {
                    //change current weapon type to mask

                    activeMask = Masks.Lyre;
                }
            }
            
            
        }
        
    }

    void ShootLyreShot(Vector3 direction)
    {
        Projectile proj;
        if (Random.Range(0.0f, 1.0f) < 0.5f)
        {
            proj = Instantiate(projPrefab[0], transform.position + direction, Quaternion.identity);
        }
        else
        {
            proj = Instantiate(projPrefab[1], transform.position + direction, Quaternion.identity);

        }
        proj.LaunchProjectile(direction * 10.0f, projectileLifetime);
        shootTimer = SHOOT_COOLDOWN;
    }
    
    void ShootGorgonFreemanShot(Vector3 direction)
    {
        Projectile proj;
        if (Random.Range(0.0f, 1.0f) < 0.5f)
        {
            proj = Instantiate(projPrefab1[0], transform.position + direction, Quaternion.identity);
        }
        else
        {
            proj = Instantiate(projPrefab1[1], transform.position + direction, Quaternion.identity);

        }
        proj.LaunchProjectile(direction * 10.0f, projectileLifetime);
        shootTimer = SHOOT_COOLDOWN;
    }
    void ShootHarpyShot(Vector3 direction)
    {
        Projectile proj;
        if (Random.Range(0.0f, 1.0f) < 0.5f)
        {
            proj = Instantiate(projPrefab2[0], transform.position + direction, Quaternion.identity);
        }
        else
        {
            proj = Instantiate(projPrefab2[1], transform.position + direction, Quaternion.identity);

        }
        proj.LaunchProjectile(direction * 10.0f, projectileLifetime);
        shootTimer = SHOOT_COOLDOWN;
    }
    
    
    void ShootCyclopsShot(Vector3 direction)
    {
        Projectile proj;
        if (Random.Range(0.0f, 1.0f) < 0.5f)
        {
            proj = Instantiate(projPrefab3[0], transform.position + direction, Quaternion.identity);
        }
        else
        {
            proj = Instantiate(projPrefab3[1], transform.position + direction, Quaternion.identity);

        }
        proj.LaunchProjectile(direction * 10.0f, projectileLifetime);
        shootTimer = SHOOT_COOLDOWN;
    }
    
    
    void ShootMinotaurShot(Vector3 direction)
    {
        Projectile proj;
        if (Random.Range(0.0f, 1.0f) < 0.5f)
        {
            proj = Instantiate(projPrefab4[0], transform.position + direction, Quaternion.identity);
        }
        else
        {
            proj = Instantiate(projPrefab4[1], transform.position + direction, Quaternion.identity);

        }
        proj.LaunchProjectile(direction * 10.0f, projectileLifetime);
        shootTimer = 0.05f;
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

        if (enableNow)
            action.Enable();
    }


    void StartJump()
    {
        if (jumps > 0)
        {
            physicsController.linearVelocityY = START_OF_JUMP_JUMP_POWER;
            // varJumpSpeed = physicsController.linearVelocityY;


            isJumping = true;

            jumps -= 1;
            groundingCooldown = GROUNDING_COOLDOWN;

            resetJumpsInAir = true;
            
        }
    }

    IEnumerator BufferJump()
    {
        bufferedJump = true;

        yield return new WaitForSeconds(jumpBufferDuration);

        bufferedJump = false;
    }
}
