using UnityEngine;

/// <summary>
/// ScriptableObject representation of an integer.
/// </summary>
[CreateAssetMenu (menuName = "Variable.../Int Variable")]
public class IntVariable : ScriptableObject
{   
    /// <summary>
    /// Integer value of the variable.
    /// </summary>
    public int Value;
}
