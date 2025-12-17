using Domain.Game;
using Domain.Game.Commands;

namespace GameEngine;

public interface IGameEngine
{
    public void AddAttackEvent(AttackEvent attackEvent);
    public void AddPlayerDisconnectedEvent(PlayerDisconnectedEvent playerDisconnectedEvent);
    /// <summary>
    /// Process all the available events in a FIFO order.
    /// Given game state and elapsed time since last update, return the updated game
    /// state at previous state + elapsed time.
    /// </summary>
    /// <param name="gameState"></param>
    /// <param name="elapsed">how much time is passed since last update game in milliseconds</param>
    /// <returns></returns>
    public GameState UpdateGame(GameState gameState, long elapsed);
}