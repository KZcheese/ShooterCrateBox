using UnityEngine;
using Cinemachine;

/// <summary>
/// Controls the noise of the CinemachineVirtualCamera component to allow for
/// screen shake in response to events. Reads shake intensity and time from 
/// ScriptableObject Variables.
/// </summary>
public class CinemachineShake : MonoBehaviour
{
    /// <summary>
    /// Camera to control.
    /// </summary>
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    /// <summary>
    /// Intensity (amplitude) of the camera shake. Intensity will fade out over
    /// the duration of the shake.
    /// </summary>
    [SerializeField] private FloatVariable intensity;

    /// <summary>
    /// Shake duration.
    /// </summary>
    [SerializeField] private FloatVariable time;
    
    /// <summary>
    /// Basic Perlin noise component reference.
    /// </summary>
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;

    /// <summary>
    /// Tracks how long the camera has been shaking.
    /// </summary>
    private float timer = 0.0f;

    #region MonoBehavior Methods
    private void Awake()
    {
        cinemachineBasicMultiChannelPerlin =
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =
                Mathf.Lerp(intensity.Value, 0.0f, 1 - (timer / time.Value));
        }
    }
    #endregion

    /// <summary>
    /// Begins shaking the camera at the intensity stored in the intensity 
    /// float variable by setting the shake timer.
    /// </summary>
    public void Shake()
    {
        timer = time.Value;
    }
}
