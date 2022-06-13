using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor2D : MonoBehaviour
{
    public SensorStateChanged sensorStateChanged;
    public SensorShape SensorShape;

    [Tooltip("Should this sensor only sense a specific tag?")]
    public bool SenseTag;
    
    [SerializeField] private Vector2 rectangleSize;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask layerToSense;
    [SerializeField] private string tagToSense;
    [SerializeField] private bool active;

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

    public float DisabledTimer { get; set; }

    public bool IsDisabled
    {
        get
        {
            return DisabledTimer >= 0.0f;
        }
    }

    private void FixedUpdate()
    {
        switch (SensorShape)
        {
            case SensorShape.Rectangle:
                if (Physics2D.OverlapBox(transform.position, rectangleSize, 0.0f,
                    layerToSense))
                {
                    Active = true;
                }
                else
                {
                    Active = false;
                }
                break;

            case SensorShape.Circle:
                if (Physics2D.OverlapCircle(transform.position, radius, 
                    layerToSense))
                {
                    Active = true;
                }
                else
                {
                    Active = false;
                }
                break;

            default:
                break;
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

}
