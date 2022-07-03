using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StringVariableObserver : MonoBehaviour
{
    [SerializeField] private StringVariable observedVariable;
    [SerializeField] private TextMeshProUGUI textToUpdate;

    #region MonoBehaviour Methods
    private void OnEnable()
    {
        observedVariable.variableUpdated += UpdateText;
    }
    private void OnDisable()
    {
        observedVariable.variableUpdated -= UpdateText;
    }
    #endregion

    private void UpdateText()
    {
        textToUpdate.text = observedVariable.Value;
    }
}
