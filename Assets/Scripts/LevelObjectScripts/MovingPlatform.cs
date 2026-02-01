using Unity.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public enum PlatformState
    {
        WaitingAtLeft,
        WaitingAtRight,
        MovingLeft,
        MovingRight,
        None
    };

    // Wherever the moving platform's initial position is will be considered the leftmost point by default.
    // The rightmost point will be determined by a user-specified delta.

    private float leftX;
    private float rightX;

    // The difference between the left and right Xs. The initial position of the object is the leftmost position.
    public float deltaX;

    // How long it takes to go from the leftmost X to the rightmost X and vice versa.
    public float timeBetween;

    // How long the platform waits after reaching one end or the other.
    public float endWaitTime;

    // A timer used to detect how long the platform has been waiting at either end.
    private float timer;

    // The calculated speed based on timeBetween and deltaX.
    private float speed;

    private PlatformState playerState;
    private PlatformState state;

    //private float playerXOffset = 0.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Start moving right immediately.
        state = PlatformState.MovingRight;
        playerState = PlatformState.None;
        leftX = transform.position.x;
        rightX = leftX + deltaX;
        speed = deltaX / timeBetween;
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Track the timer when the platform is waiting.
        switch (state)
        {
            case PlatformState.WaitingAtLeft:
            case PlatformState.WaitingAtRight:
                timer -= Time.deltaTime;
                break;
            case PlatformState.MovingLeft:
                transform.position += speed * Time.deltaTime * Vector3.left;
                break;
            case PlatformState.MovingRight:
                transform.position += speed * Time.deltaTime * Vector3.right;
                break;
        }

        // EE109 State Machines baby!
        // If the platform is done waiting at the left, let it move right.
        if (state == PlatformState.WaitingAtLeft && timer <= 0.0f)
        {
            state = PlatformState.MovingRight;
            
        }
        // If the platform arrives at the right, let it wait at the right.
        else if (state == PlatformState.MovingRight && transform.position.x >= rightX)
        {
            state = PlatformState.WaitingAtRight;
            timer = endWaitTime;
        }
        // If the platform is done waiting at the right, let it move left.
        else if (state == PlatformState.WaitingAtRight && timer <= 0.0f)
        {
            state = PlatformState.MovingLeft;
        }
        // If the platform arrives at the left, let it wait at the left.
        else if (state == PlatformState.MovingLeft && transform.position.x <= leftX)
        {
            state = PlatformState.WaitingAtLeft;
            timer = endWaitTime;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerCollision") && collision.GetContact(0).normal.y < -0.5f)
        {
            // This might not be the most efficient way but it's the best I can do for now...
            // Only change player velocity if the state of the platform changes.
            if (playerState != state)
            {
                Player player = collision.gameObject.GetComponent<Player>();

                player.baseVelocity = state switch
                {
                    PlatformState.MovingRight => speed,
                    PlatformState.MovingLeft => -speed,
                    _ => 0.0f,
                };
                playerState = state;

            }
            
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerCollision"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.baseVelocity = 0.0f;
            playerState = PlatformState.None;
        }
    }
}
