using UnityEngine;

/// <summary>
/// Contains references to all controllable components of the player character.
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField] private CommandStream playerCommandStream;
    
    public Jumper2D Jumper2D;
    public Mover2D Mover2D;
    public WeaponHandler WeaponHandler;

    /// <summary>
    /// Queue to hold commands.
    /// </summary>
    public CommandStream PlayerCommandStream
    {
        get
        {
            return playerCommandStream;
        }
        set
        {
            playerCommandStream = value;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!System.Object.ReferenceEquals(PlayerCommandStream, null) &&
                PlayerCommandStream.Count() > 0)
        {
            PlayerCommandStream.Dequeue().Execute(this);
        }
    }
}
