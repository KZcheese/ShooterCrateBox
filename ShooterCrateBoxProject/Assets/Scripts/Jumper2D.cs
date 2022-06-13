using UnityEngine;

/// <summary>
/// Allows an object to jump in 2D space.
/// </summary>
public class Jumper2D : MonoBehaviour
{
    /// <summary>
    /// Can the object jump once it is in the air?
    /// </summary>
    [Tooltip("Can the object jump once it is in the air?")]
    public bool MultiJump = false;

    /// <summary>
    /// If true, final jump height is higher the longer the player holds down 
    /// jump input, clamped at maxJumpHeight. If false, player always jumps to
    /// maxJumpHeight when jump input is pressed.
    /// </summary>
    [Tooltip("If true, final jump height is higher the longer the player " +
        "holds down jump input, clamped at maxJumpHeight. If false, player " +
        "always jumps to maxJumpHeight when jump input is pressed.")]
    public bool VariableJump = false;

    /// <summary>
    /// Force applied each frame to cancel a jump with variable height.
    /// </summary>
    [Tooltip("Force applied each frame to cancel a jump with variable height.")]
    public float CancelRate = 100.0f;

    /// <summary>
    /// Gravity scale applied to the Rigidbody2D component. Use this parameter
    /// instead of editing the Rigidbody2D's gravity scale property directly.
    /// </summary>
    [Tooltip("Gravity scale applied to the Rigidbody2D component. " +
        "Use this parameter instead of editing the Rigidbody2D's gravity " +
        "scale property directly.")]
    public float GravityScale = 1.0f;

    /// <summary>
    /// Maximum time button can be held for variable jump height.
    /// </summary>
    [Tooltip("Maximum time button can be held for variable jump height.")]
    public float JumpWindowTime = 0.3f;

    /// <summary>
    /// Maximum height to which the object can jump.
    /// </summary>
    [Tooltip("Maximum height to which the object can jump.")]
    public float MaxJumpHeight = 5.0f;

    /// <summary>
    /// How many times can the object jump without grounding?
    /// </summary>
    [Tooltip("How many times can the object jump without grounding?")]
    public int TotalJumps = 1;

    /// <summary>
    /// Sensor2D responsible for checking the grounded state of the object.
    /// </summary>
    [Tooltip("Sensor2D responsible for checking the grounded state of the " +
        "object.")]
    [SerializeField] private Sensor2D groundSensor;

    /// <summary>
    /// Sensor2D responsible for checking the grounded state of the object.
    /// </summary>
    [Tooltip("Sensor2D responsible for checking the grounded state of the " +
        "object.")]
    [SerializeField] private Sensor2D headSensor;

    /// <summary>
    /// Rigidbody2D component to be affected by jump.
    /// </summary>
    [Tooltip("Rigidbody2D component to be affected by jump.")]
    [SerializeField] private Rigidbody2D rb2D;

    /// <summary>
    /// Determines if this object is currently jumping.
    /// </summary>
    private bool isJumping = false;

    /// <summary>
    /// Determines if this object has recieved a signal to cancel a jump.
    /// </summary>
    private bool isJumpCancelled = false;

    /// <summary>
    /// Tracks how many jumps the object has performed without grounding.
    /// </summary>
    private int jumpCount = 0;

    /// <summary>
    /// Force required to reach maximumJumpHeight given a gravitational
    /// constant and gravityScale.
    /// </summary>
    private float jumpForce = 0.0f;

    /// <summary>
    /// Time elapsed since jump has started.
    /// </summary>
    private float jumpTime = 0.0f;

    #region MonoBehaviour Methods
    private void Awake()
    {
        rb2D.gravityScale = GravityScale;
    }

    private void OnEnable()
    {
        groundSensor.sensorStateChanged += ResetJumpCount;
        headSensor.sensorStateChanged += StopJump;
    }

    private void Update()
    {
        if (VariableJump && isJumping)
        {
            jumpTime += Time.deltaTime;
            if (jumpTime > JumpWindowTime)
            {
                isJumping = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (isJumpCancelled && isJumping && rb2D.velocity.y > 0)
        {
            rb2D.AddForce(Vector2.down * CancelRate);
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
        if (groundSensor.Active || (MultiJump && jumpCount < TotalJumps))
        {
            if (!groundSensor.Active)
            {
                rb2D.velocity = new Vector2(rb2D.velocity.x, 0.0f);
            }

            rb2D.gravityScale = GravityScale;
            float gravity = Physics2D.gravity.y * GravityScale;
            jumpForce = Mathf.Sqrt(-2 * MaxJumpHeight * gravity);
            rb2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);

            if (MultiJump)
            {
                jumpCount += 1;
            }

            if (VariableJump)
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
        if (VariableJump)
        {
            StopJump();
        }
    }

    /// <summary>
    /// Resets the number of jumps the character has performed.
    /// </summary>
    private void ResetJumpCount()
    {
        if (groundSensor.Active && MultiJump)
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
