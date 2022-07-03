public class StopFireCommand : ICommand
{
    public void Execute(Player player)
    {
        player.WeaponHandler.OnFireEnd();
    }
}

