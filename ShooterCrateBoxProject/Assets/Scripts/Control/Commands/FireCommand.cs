public class FireCommand : ICommand
{
    public void Execute(Player player)
    {
        player.WeaponHandler.OnFireStart();
    }
}
