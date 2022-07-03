using UnityEngine;

/// <summary>
/// ScriptableObject representation of an integer.
/// </summary>
[CreateAssetMenu(menuName = "Variable.../Int Variable")]
public class IntVariable : ScriptableObject
{
    public VariableUpdated variableUpdated;

    [SerializeField] private int intValue;

    /// <summary>
    /// Integer value of the variable.
    /// </summary>
    public int Value
    {
        get
        {
            return intValue;
        }
        set
        {
            intValue = value;
            variableUpdated?.Invoke();
        }
    }

    public void Reset()
    {
        Value = 0;
    }
}
