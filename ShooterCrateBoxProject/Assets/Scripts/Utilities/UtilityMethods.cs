using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A collection of miscellaneous, static helper functions.
/// </summary>
public static class UtilityMethods
{
    #region Frame Count Methods
    /// <summary>
    /// Returns a float representing the frame count standardized to the 
    /// referenceFPS. Assumes the raw frame count is derived from the project's
    /// fixedDeltaTime parameter.
    /// </summary>
    /// <param name="frameCount">Frame count to standardize to the 
    /// referenceFPS.</param>
    /// <param name="referenceFPS">The frames-per-second to standardize the
    /// frame count to.</param>
    /// <returns> A float representing the frame count standardized to the 
    /// referenceFPS.</returns>
    public static float StandardizeFrameCount(float frameCount, int referenceFPS)
    {
        return frameCount / ((1 / Time.fixedDeltaTime) / referenceFPS);
    }

    /// <summary>
    /// Returns a float representing the frame count standardized to the 
    /// referenceFPS.
    /// </summary>
    /// <param name="frameCount">Frame count to standardize to the 
    /// referenceFPS.</param>
    /// <param name="rawFPS">The frames-per-second at which frameCount was
    /// calculated.</param>
    /// <param name="referenceFPS">The frames-per-second to standardize the
    /// frame count to.</param>
    /// <returns> A float representing the frame count standardized to the 
    /// referenceFPS.</returns>
    public static float StandardizeFrameCount(float frameCount, int rawFPS, int referenceFPS)
    {
        return frameCount / ((1 / (1 / rawFPS)) / referenceFPS);
    }
    #endregion
}
