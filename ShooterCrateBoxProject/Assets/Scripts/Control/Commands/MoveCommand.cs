public class MoveCommand : ICommand
{
    public float MoveInput = 0;

    public void Execute(Player player)
    {
        player.Mover2D.MoveInput = MoveInput;
    }
}
