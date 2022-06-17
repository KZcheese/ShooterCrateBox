using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Reads information from Firearm ScriptableObject, allowing the actor to 
/// use the firearm in the game world.
/// </summary>
public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private Firearm currentFirearm;

    public Firearm CurrentFirearm
    {
        get
        {
            return currentFirearm;
        }
        set
        {
            currentFirearm = value;
        }
    }

    public void Fire()
    {

    }
}
