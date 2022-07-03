using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A ScriptableObject representation of a Queue of ICommands used to control a
/// GameObject.
/// </summary>
[CreateAssetMenu]
public class CommandStream : ScriptableObject
{
    /// <summary>
    /// Queue to store commands for execution by a CommandRelay
    /// </summary>
    private Queue<ICommand> stream = new Queue<ICommand>();

    /// <summary>
    /// Adds a command to the stream for execution by a CommandRelay
    /// </summary>
    /// <param name="command">Command to be enqueued.</param>
    public void Enqueue(ICommand command)
    {
        stream.Enqueue(command);
    }

    /// <summary>
    /// Retrieves a command from the stream for execution by a CommandRelay
    /// </summary>
    /// <returns>First command in the stream's queue.</returns>
    public ICommand Dequeue()
    {
        return stream.Dequeue();
    }

    /// <summary>
    /// Returns the current count of commands in the stream.
    /// </summary>
    /// <returns>Current count of commands in the stream.</returns>
    public int Count()
    {
        return stream.Count;
    }
}
