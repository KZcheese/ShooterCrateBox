using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityXParameterObserver : MonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private Mover2D mover2D;

    private int velocityXID;

    #region MonoBehaviour Methods
    private void Awake()
    {
        velocityXID = Animator.StringToHash("VelocityX");
    }
    private void Update()
    {
        animator.SetFloat(velocityXID, Mathf.Abs(mover2D.CurrentVelocityX));
    }
    #endregion
}
