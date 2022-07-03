using UnityEngine;

/// <summary>
/// Describes the behaviour of a Crate Pickup, a randomly-spawning entity that
/// changes the weapon an actor is using.
/// </summary>
public class CratePickup : MonoBehaviour
{
    /// <summary>
    /// Event to be raised when a crate is picked up.
    /// </summary>
    [SerializeField] private GameEvent cratePickedUpEvent;

    /// <summary>
    /// IntVariable representing the player's score.
    /// </summary>
    [SerializeField] private IntVariable playerScore;

    /// <summary>
    /// The firearm this crate contains.
    /// </summary>
    public Firearm Content { get; set; }

    #region MonoBehaviour Methods
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.WeaponHandler.CurrentFirearm = Content;
            gameObject.SetActive(false);
            playerScore.Value += 1;
            cratePickedUpEvent.Raise();
        }
    }
    #endregion
}
