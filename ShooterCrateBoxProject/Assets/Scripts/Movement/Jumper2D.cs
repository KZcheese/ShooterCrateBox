using UnityEngine;

/// <summary>
/// Acts upon an assigned RigidBody2D. Allows the Rigidbody2D's attached
/// GameObject to jump by adding an initial upward force and manipulating the
/// gravity scale, allowing for highly customizeable jump behaviour.
/// </summary>
public class Jumper2D : MonoBehaviour
{
    /// <summary>
    /// Force applied each frame to cancel a jump with variable height.
    /// </summary>
    [SerializeField] private float cancelRate = 100.0f;

    /// <summary>
    /// Gravity scale applied to the Rigidbody2D component. Use this parameter
    /// instead of editing the Rigidbody2D's gravity scale property directly.
    /// </summary>
    [SerializeField] private float gravityScale = 1.0f;

    /// <summary>
    /// Sensor2D responsible for checking the grounded state of the object.
    /// </summary>
    [SerializeField] private Sensor2D groundSensor;

    /// <summary>
    /// Sensor2D responsible for checking collisions above the object
    /// </summary>
    [SerializeField] private Sensor2D headSensor;

    /// <summary>
    /// Tracks how many jumps the object has performed without grounding.
    /// </summary>
    [SerializeField] private int jumpCount = 0;

    /// <summary>
    /// Force required to reach maximumJumpHeight given a gravitational
    /// constant and gravityScale.
    /// </summary>
    [SerializeField] private float jumpForce = 0.0f;

    /// <summary>
    /// Maximum time button can be held for variable jump height.
    /// </summary>
    [SerializeField] private float jumpWindowTime = 0.3f;

    /// <summary>
    /// Maximum height to which the object can jump.
    /// </summary>
    [SerializeField] private float maxJumpHeight = 5.0f;

    /// <summary>
    /// Can the object jump once it is in the air?
    /// </summary>
    [SerializeField] private bool multiJump = false;

    /// <summary>
    /// Rigidbody2D component to be affected by this Jumper2D.
    /// </summary>
    [SerializeField] private Rigidbody2D rb2D;

    /// <summary>
    /// How many times can the object jump without grounding?
    /// </summary>
    [SerializeField] private int totalJumps = 1;

    /// <summary>
    /// If true, final jump height is higher the longer the player holds down 
    /// jump input, clamped at maxJumpHeight. If false, player always jumps to
    /// maxJumpHeight when jump input is pressed.
    /// </summary>
    [SerializeField] private bool variableJump = false;

    /// <summary>
    /// Determines if this object is currently jumping.
    /// </summary>
    private bool isJumping = false;

    /// <summary>
    /// Determines if this object has recieved a signal to cancel a jump.
    /// </summary>
    private bool isJumpCancelled = false;

    /// <summary>
    /// Time elapsed since jump began.
    /// </summary>
    private float jumpTime = 0.0f;

    #region MonoBehaviour Methods
    private void Awake()
    {
        rb2D.gravityScale = gravityScale;
    }

    private void OnEnable()
    {
        groundSensor.sensorStateChanged += ResetJumpCount;
        headSensor.sensorStateChanged += StopJump;
    }

    private void Update()
    {
        if (variableJump && isJumping)
        {
            jumpTime += Time.deltaTime;
            if (jumpTime > jumpWindowTime)
            {
                isJumping = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isJumpCancelled && isJumping && rb2D.velocity.y > 0)
        {
            if (cancelRate != 0)
            {
                rb2D.AddForce(Vector2.down * cancelRate);
            }
            else
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, 0.0f);
            }
        }
    }

    private void OnDisable()
    {
        groundSensor.sensorStateChanged -= ResetJumpCount;
        headSensor.sensorStateChanged -= StopJump;
    }
    #endregion

    /// <summary>
    /// Makes the object jump.
    /// </summary>
    public void Jump()
    {
        if (groundSensor.Active || (multiJump && jumpCount < totalJumps))
        {
            if (!groundSensor.Active)
            {
                // For a multi jump, reset y velocity before mid air jump.
                rb2D.velocity = new Vector2(rb2D.velocity.x, 0.0f);
            }

            rb2D.gravityScale = gravityScale;
            float gravity = Physics2D.gravity.y * gravityScale;
            jumpForce = Mathf.Sqrt(-2 * maxJumpHeight * gravity);
            rb2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

            if (multiJump)
            {
                jumpCount += 1;
            }

            if (variableJump)
            {
                isJumping = true;
                isJumpCancelled = false;
                jumpTime = 0.0f;
            }
        }
    }

    /// <summary>
    /// "Cancels" a variable jump, best utilized when the player releases the
    /// jump input.
    /// </summary>
    public void StopVariableJump()
    {
        if (variableJump)
        {
            StopJump();
        }
    }

    /// <summary>
    /// Resets the number of jumps the character has performed.
    /// </summary>
    private void ResetJumpCount()
    {
        if (groundSensor.Active && multiJump)
        {
            jumpCount = 0;
        }
    }

    /// <summary>
    /// Stops a jump.
    /// </summary>
    private void StopJump()
    {
        isJumpCancelled = true;
    }
}
