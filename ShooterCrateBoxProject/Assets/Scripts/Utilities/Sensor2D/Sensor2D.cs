using UnityEngine;

public class Sensor2D : MonoBehaviour
{
    /// <summary>
    /// Signals the sensor state has changed.
    /// </summary>
    public SensorStateChanged sensorStateChanged;

    /// <summary>
    /// Primitive shape of the sensor.
    /// </summary>
    [Tooltip("Primitive shape of the sensor.")]
    public SensorShape SensorShape;

    /// <summary>
    /// Should this sensor only sense a specific tag?
    /// </summary>
    [Tooltip("Should this sensor only sense a specific tag?")]
    public bool SenseTag;

    /// <summary>
    /// The sensor is considered active when it overlaps an object on 
    /// layerToSense and with the tagToSense (if SenseTag is true).
    /// </summary>
    [Tooltip("The sensor is considered active when it overlaps an object on" +
        "layerToSense and with the tagToSense (if SenseTag is true).")]
    [SerializeField] private bool active;

    /// <summary>
    /// Which layer(s) should this sensor detect?
    /// </summary>
    [Tooltip("Which layer(s) should this sensor detect?")]
    [SerializeField] private LayerMask layerToSense;

    /// <summary>
    /// Radius of the sensor if it is a circle.
    /// </summary>
    [Tooltip("Radius of the sensor if it is a circle.")]
    [SerializeField] private float radius;

    /// <summary>
    /// Size of the sensor if it is a rectangle.
    /// </summary>
    [Tooltip("Size of the sensor if it is a rectangle.")]
    [SerializeField] private Vector2 rectangleSize;

    /// <summary>
    /// If SenseTag is true, this sensor will only sense objects on 
    /// layerToSense with this tag.
    /// </summary>
    [Tooltip("If SenseTag is true, this sensor will only sense objects on " +
        "layerToSense with this tag.")]
    [SerializeField] private string tagToSense;

    /// <summary>
    /// The sensor is considered active when it overlaps an object on 
    /// layerToSense and with the tagToSense (if SenseTag is true).
    /// </summary>
    public bool Active
    {
        get
        {
            return active && !IsDisabled;
        }
        set
        {
            if (value != active)
            {
                active = value;
                sensorStateChanged?.Invoke();
            }
        }
    }

    /// <summary>
    /// Timer for how long the sensor should be disabled. Disabled sensors
    /// are always considered not active.
    /// </summary>
    public float DisabledTimer { get; set; }

    /// <summary>
    /// Flag for whether this sensor is disabled. Disabled sensors are always
    /// considered not active.
    /// </summary>
    public bool IsDisabled
    {
        get
        {
            return DisabledTimer >= 0.0f;
        }
    }

    /// <summary>
    /// Array of colliders detected.
    /// </summary>
    private Collider2D[] detectedColliders;

    #region MonoBehaviour Methods
    private void FixedUpdate()
    {
        switch (SensorShape)
        {
            case SensorShape.Rectangle:
                detectedColliders = Physics2D.OverlapBoxAll(transform.position,
                    rectangleSize, 0.0f, layerToSense);
                break;
            case SensorShape.Circle:
                detectedColliders = 
                    Physics2D.OverlapCircleAll(transform.position, radius,
                    layerToSense);
                break;
            default:
                break;
        }

        if (detectedColliders.Length > 0)
        {
            if (SenseTag && tagToSense != "")
            {
                bool atLeastOneTagSensed = false;
                foreach (Collider2D collider in detectedColliders)
                {
                    if (collider.CompareTag(tagToSense))
                    {
                        atLeastOneTagSensed = true;
                        break;
                    }
                }
                Active = atLeastOneTagSensed;
            }
            else
            {
                Active = true;
            }
        }
        else
        {
            Active = false;
        }
    }

    private void Update()
    {
        if (DisabledTimer >= 0.0f)
        {
            DisabledTimer -= Time.deltaTime;
        }
    }

    private void OnDrawGizmos()
    {
        switch (SensorShape)
        {
            case SensorShape.Rectangle:
                Gizmos.DrawWireCube(transform.position,
                    new Vector3(rectangleSize.x, rectangleSize.y, 0));
                break;

            case SensorShape.Circle:
                Gizmos.DrawWireSphere(transform.position, radius);
                break;

            default:
                break;
        }
    }
    #endregion
}
