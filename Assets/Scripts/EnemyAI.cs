using UnityEngine;
using UnityEngine.Playables;
using static MovingPlatform;

public class EnemyAI : MonoBehaviour
{
    public enum MovementState
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

    private MovementState state;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        state = MovementState.MovingRight;
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
            case MovementState.WaitingAtLeft:
            case MovementState.WaitingAtRight:
                timer -= Time.deltaTime;
                break;
            case MovementState.MovingLeft:
                transform.position += speed * Time.deltaTime * Vector3.left;
                break;
            case MovementState.MovingRight:
                transform.position += speed * Time.deltaTime * Vector3.right;
                break;
        }

        // EE109 State Machines baby!
        // If the platform is done waiting at the left, let it move right.
        if (state == MovementState.WaitingAtLeft && timer <= 0.0f)
        {
            state = MovementState.MovingRight;

        }
        // If the platform arrives at the right, let it wait at the right.
        else if (state == MovementState.MovingRight && transform.position.x >= rightX)
        {
            state = MovementState.WaitingAtRight;
            timer = endWaitTime;
        }
        // If the platform is done waiting at the right, let it move left.
        else if (state == MovementState.WaitingAtRight && timer <= 0.0f)
        {
            state = MovementState.MovingLeft;
        }
        // If the platform arrives at the left, let it wait at the left.
        else if (state == MovementState.MovingLeft && transform.position.x <= leftX)
        {
            state = MovementState.WaitingAtLeft;
            timer = endWaitTime;
        }
    }
}
