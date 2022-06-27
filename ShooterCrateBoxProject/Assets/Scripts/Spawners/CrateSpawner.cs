using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Randomly spawns crates one-at-a-time in pre-determined areas on the level.
/// </summary>
public class CrateSpawner : MonoBehaviour
{
    /// <summary>
    /// Prefab representing the crate pickup.
    /// </summary>
    [SerializeField] private GameObject cratePickupPrefab;

    /// <summary>
    /// Array of all possible firearms to be contained in crates.
    /// </summary>
    [SerializeField] private List<Firearm> firearms;

    /// <summary>
    /// Array of all possible spawn points for crates.
    /// </summary>
    [SerializeField] private Transform[] spawnPoints;

    /// <summary>
    /// Reference to the crate pickup gameobject that will be recycled for
    /// spawning.
    /// </summary>
    private GameObject cratePickupObject;

    /// <summary>
    /// Tracks the last firearm loaded into a crate.
    /// </summary>
    private Firearm previousFirearm;

    #region MonoBehaviour Methods
    private void Start()
    {
        SpawnCrate();
    }
    #endregion

    /// <summary>
    /// Spawns a crate with a random weapon contained. Selects random spawn 
    /// location from an array of all possible spawn locations.
    /// </summary>
    public void SpawnCrate()
    {
        // Select spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Create or move cratePickup
        if (cratePickupObject == null)
        {
            cratePickupObject = Instantiate(cratePickupPrefab,
                spawnPoint.position, Quaternion.identity);
        }
        else
        {
            cratePickupObject.transform.position = spawnPoint.position;
        }

        // Fill the crate
        CratePickup cratePickup = cratePickupObject.GetComponent<CratePickup>();
        int randomFirearmIndex = Random.Range(0, firearms.Count);
        cratePickup.Content = firearms[randomFirearmIndex];

        // Add the previous firearm to consideration for the next pickup.
        if (previousFirearm != null)
        {
            firearms.Add(previousFirearm);
        }

        // Remove the assigned firearm from consideration from the next pickup
        previousFirearm = firearms[randomFirearmIndex];
        firearms.Remove(previousFirearm);

        // Activate the crate, as it may be inactive after pickup.
        cratePickupObject.SetActive(true);
    }
}
