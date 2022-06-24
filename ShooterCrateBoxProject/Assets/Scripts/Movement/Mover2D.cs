using UnityEngine;

/// <summary>
/// Acts upon an assigned RigidBody2D. Allows the Rigidbody2D's attached
/// GameObject to move by manipulating the X component of the Rigidbody2D's
/// velocity vector.
/// </summary>
public class Mover2D : MonoBehaviour
{
    /// <summary>
    /// Rate at which x velocity moves toward maxVelocityX.
    /// </summary>
    [SerializeField] private float acceleration = 10.0f;

    /// <summary>
    /// Current velocity of the Mover2D.
    /// </summary>
    [SerializeField] private float currentVelocityX = 0;

    /// <summary>
    /// Rate at which x velocity moves toward 0.0.
    /// </summary>
    [SerializeField] private float deceleration = 10.0f;

    /// <summary>
    /// Maximum X velocity of this Mover2D.
    /// </summary>
    [SerializeField] private float maxVelocityX = 5.0f;

    /// <summary>
    /// Rigidbody2D component to be affected by this Mover2D.
    /// </summary>
    [SerializeField] private Rigidbody2D rb2D;

    /// <summary>
    /// Does this Mover2D accelerate to approach maxVelocityX?
    /// </summary>
    [SerializeField] private bool useAcceleration;

    /// <summary>
    /// Does this Mover2D decelerate to approach 0.0 velocity X?
    /// </summary>
    [SerializeField] private bool useDeceleration;

    /// <summary>
    /// Sensor2D responsible for checking if the Mover2D is against a wall to
    /// its left.
    /// </summary>
    [SerializeField] private Sensor2D wallSensorLeft;

    /// <summary>
    /// Sensor2D responsible for checking if the Mover2D is against a wall to
    /// its right.
    /// </summary>
    [SerializeField] private Sensor2D wallSensorRight;

    /// <summary>
    /// Movement direction based on controller input.
    /// </summary>
    private float moveInput;

    /// <summary>
    /// Movement direction based on controller input.
    /// </summary>
    public float MoveInput
    {
        get
        {
            return moveInput;
        }
        set
        {
            moveInput = value;
            currentVelocityX = rb2D.velocity.x;
        }
    }

    #region Monobehaviour Methods
    private void FixedUpdate()
    {
        CalculateNewXVelocity();
        rb2D.velocity = new Vector2(currentVelocityX, rb2D.velocity.y);
    }
    #endregion

    /// <summary>
    /// Calculates current velocity based on MoveInput and whether the Mover2D 
    /// uses acceleration and/or deceleration.
    /// </summary>
    private void CalculateNewXVelocity()
    {
        if (MoveInput != 0)
        {
            // If the Mover2D is moving into a wall, set its velocity to 0. This
            // prevents unintended wall sticking.
            if ((MoveInput == -1 && wallSensorLeft.Active) ||
                (MoveInput == 1 && wallSensorRight.Active))
            {
                currentVelocityX = 0;
                return;
            }

            if (useDeceleration)
            {
                if (MoveInput == 1 && currentVelocityX < 0.0f)
                {
                    currentVelocityX += deceleration * Time.fixedDeltaTime;
                }
                else if (MoveInput == -1 && currentVelocityX > 0.0f)
                {
                    currentVelocityX -= deceleration * Time.fixedDeltaTime;
                }
            }

            if (useAcceleration)
            {
                if (MoveInput == 1 && currentVelocityX >= 0.0f && currentVelocityX < maxVelocityX)
                {
                    currentVelocityX += acceleration * Time.fixedDeltaTime;
                }
                else if (MoveInput == -1 && currentVelocityX <= 0.0f && currentVelocityX > -maxVelocityX)
                {
                    currentVelocityX -= acceleration * Time.fixedDeltaTime;
                }
            }
            else
            {
                currentVelocityX = maxVelocityX * MoveInput;
            }

        }
        else
        {
            if (useDeceleration)
            {
                if (currentVelocityX > deceleration * Time.fixedDeltaTime)
                {
                    currentVelocityX -= deceleration * Time.fixedDeltaTime;
                }
                else if (currentVelocityX < -deceleration * Time.fixedDeltaTime)
                {
                    currentVelocityX += deceleration * Time.fixedDeltaTime;
                }
                else
                {
                    currentVelocityX = 0;
                }
            }
            else
            {
                currentVelocityX = 0;
            }
        }
    }
}
