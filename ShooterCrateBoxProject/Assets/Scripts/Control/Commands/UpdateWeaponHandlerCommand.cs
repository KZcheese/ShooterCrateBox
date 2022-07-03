using UnityEngine;

public class UpdateWeaponHandlerCommand : ICommand
{
    public int NewDir = 0;
    public void Execute(Player player)
    {
        player.WeaponHandler.transform.localScale =
                new Vector3(NewDir, player.WeaponHandler.transform.localScale.y,
                player.WeaponHandler.transform.localScale.z);
        player.WeaponHandler.Direction = NewDir;
    }
}
