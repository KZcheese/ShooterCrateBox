/// <summary>
/// Implementing classes can be enqueued in a CommandRelay command stream and
/// used to control a GameObject.
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Method called when the command is dequeued from a stream and executed.
    /// </summary>
    /// <param name="commandRelay">Relay containing references to all 
    /// controllable components.</param>
    void Execute(Player player);
}