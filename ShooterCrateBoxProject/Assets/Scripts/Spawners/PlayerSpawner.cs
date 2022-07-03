using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;

    private GameObject playerObject;

    public void SpawnPlayer()
    {
        if (playerObject == null)
        {
            playerObject = Instantiate(playerPrefab,
            playerSpawnPoint.position, Quaternion.identity);
        }
        else
        {
            playerObject.transform.position = playerSpawnPoint.position;
            playerObject.SetActive(true);
        }
    }
}
