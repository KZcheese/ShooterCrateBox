public class JumpCommand : ICommand
{
    public void Execute(Player player)
    {
        player.Jumper2D.Jump();
    }
}
