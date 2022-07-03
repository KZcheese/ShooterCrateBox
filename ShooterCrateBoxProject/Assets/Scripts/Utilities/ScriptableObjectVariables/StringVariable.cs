using UnityEngine;

[CreateAssetMenu(menuName = "Variable.../String Variable")]
public class StringVariable : ScriptableObject
{
    public VariableUpdated variableUpdated;

    [SerializeField] private string stringValue;

    /// <summary>
    /// Integer value of the variable.
    /// </summary>
    public string Value
    {
        get
        {
            return stringValue;
        }
        set
        {
            stringValue = value;
            variableUpdated?.Invoke();
        }
    }

    public void Reset()
    {
        Value = "";
    }
}
