using UnityEngine;

/// <summary>
/// Describes the behaviour of a Crate Pickup, a randomly-spawning entity that
/// changes the weapon an actor is using.
/// </summary>
public class CratePickup : MonoBehaviour
{
    /// <summary>
    /// The firearm this crate contains.
    /// </summary>
    public Firearm Content { get; set; }

    /// <summary>
    /// Event to be raised when a crate is picked up.
    /// </summary>
    [SerializeField] private GameEvent cratePickedUpEvent;

    #region MonoBehaviour Methods
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            player.WeaponHandler.CurrentFirearm = Content;
            gameObject.SetActive(false);
            cratePickedUpEvent.Raise();
        }
    }
    #endregion
}
