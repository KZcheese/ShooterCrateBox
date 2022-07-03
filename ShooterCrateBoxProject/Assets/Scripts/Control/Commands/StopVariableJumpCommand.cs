public class StopVariableJumpCommand : ICommand
{
    public void Execute(Player player)
    {
        player.Jumper2D.StopVariableJump();
    }
}
