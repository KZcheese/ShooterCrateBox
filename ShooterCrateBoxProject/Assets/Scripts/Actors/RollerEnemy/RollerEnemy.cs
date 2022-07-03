using UnityEngine;
using UnityEngine.Pool;

public class RollerEnemy : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private Transform graphicsTransform;
    [SerializeField] private Mover2D mover2D;
    [SerializeField] private Rigidbody2D rb2D;

    [Header("Sensors")]
    [SerializeField] private Sensor2D groundSensor;
    [SerializeField] private Sensor2D wallSensorLeft;
    [SerializeField] private Sensor2D wallSensorRight;

    private bool hasLanded = false;
    private int goalsReached = 0;
    private int direction;

    public IObjectPool<GameObject> OriginPool { get; set; }

    #region MonoBehaviour Methods
    private void OnEnable()
    {
        groundSensor.sensorStateChanged += OnFirstGroundContact;
        wallSensorRight.sensorStateChanged += ChangeDirection;
        wallSensorLeft.sensorStateChanged += ChangeDirection;
    }
    private void OnDisable()
    {
        groundSensor.sensorStateChanged -= OnFirstGroundContact;
        wallSensorRight.sensorStateChanged -= ChangeDirection;
        wallSensorLeft.sensorStateChanged -= ChangeDirection;
    }
    #endregion

    public void OnDefeat()
    {
        // Release the enemy back to it's origin pool. This automatically
        // deactivates it's game object.
        OriginPool.Release(gameObject);
    }

    public void OnReachedGoal(Transform spawnPoint)
    {
        transform.position = spawnPoint.position;
        hasLanded = false;
        mover2D.MoveInput = 0;
        rb2D.velocity = Vector2.zero;
        goalsReached += 1;
    }

    private void ChangeDirection()
    {
        if (!wallSensorLeft.Active && !wallSensorRight.Active)
        {
            return;
        }

        if (wallSensorLeft.Active && direction == -1)
        {
            direction = 1;
        }
        if (wallSensorRight.Active && direction == 1)
        {
            direction = -1;
        }

        mover2D.MoveInput = direction;
        UpdateGraphicsXScale();
    }

    private void OnFirstGroundContact()
    {
        if (!hasLanded && groundSensor.Active)
        {
            hasLanded = true;
            SelectRandomDirection();
            mover2D.MoveInput = direction;
        }
    }

    private void SelectRandomDirection()
    {
        direction = Random.Range(0.0f, 1.0f) > 0.5f ? 1 : -1;
        UpdateGraphicsXScale();
    }

    private void UpdateGraphicsXScale()
    {
        float xScale = direction == 1 ? Mathf.Abs(graphicsTransform.localScale.x) :
            -Mathf.Abs(graphicsTransform.localScale.x);

        graphicsTransform.localScale =
            new Vector3(xScale, graphicsTransform.localScale.y,
            graphicsTransform.localScale.z);
    }
}
