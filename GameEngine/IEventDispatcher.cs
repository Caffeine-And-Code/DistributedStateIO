using Domain.Game;
using Domain.Game.Events;

namespace GameEngine;

public interface IEventDispatcher
{
    public GameState DispatchAttacks(GameState gameState,
        ref List<AttackEvent> attacksQueue);
    
    public GameState DispatchPlayersDisconnected(GameState gameState,
        ref List<PlayerDisconnectedEvent> playerDisconnectedQueue, int disconnectedTimerStartingPoint);
}