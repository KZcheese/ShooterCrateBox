using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageColorAnimation : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> spriteRenderers;
    [SerializeField] private Color damageColor = Color.white;
    [SerializeField] private float colorFadeTime = 0.1f;
    private List<Color> baseColors;
    private float colorFadeTimer;

    #region MonoBehaviour Methods
    private void Awake()
    {
        baseColors = new List<Color>();
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            baseColors.Add(spriteRenderer.color);
        }
    }
    private void Update()
    {
        if (colorFadeTimer > 0.0f)
        {
            colorFadeTimer -= Time.deltaTime;
            for (int i = 0; i < spriteRenderers.Count; i++)
            {
                spriteRenderers[i].color =
                    Color.Lerp(damageColor, baseColors[i],
                    1 - (colorFadeTimer / colorFadeTime));
            }
        }
    }
    #endregion

    /// <summary>
    /// Sets the color of the sprite renderer to the damage color, and sets
    /// a timer for how long it should take to return to the base color.
    /// </summary>
    public void StartAnimation()
    {
        foreach (SpriteRenderer spriteRenderer in spriteRenderers)
        {
            spriteRenderer.color = damageColor;
        }
        colorFadeTimer = colorFadeTime;
    }
}
