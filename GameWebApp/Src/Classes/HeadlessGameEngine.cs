using Domain.Game;
using Domain.Game.Events;
using GameEngine;
using Microsoft.Extensions.Options;

namespace GameWebApp.Classes;

public class HeadlessGameEngine(GameSettings gameSettings) : IGameEngine
{
    private List<AttackEvent> _attackQueue = [];
    private List<PlayerDisconnectedEvent> _disconnectedEvents = [];
    private readonly EventDispatcher _dispatcher = new();

    public void AddAttackEvent(AttackEvent attackEvent) => _attackQueue.Add(attackEvent);

    public void AddPlayerDisconnectedEvent(PlayerDisconnectedEvent playerDisconnectedEvent) =>
        _disconnectedEvents.Add(playerDisconnectedEvent);

    public GameState UpdateGame(GameState gameState, long elapsed)
    {
        gameState = DispatchEvents(gameState);
        //TODO: add the update of the game state (such as update the attack progress and update the disconnected timer of the users)

        return gameState;
    }

    private GameState DispatchEvents(GameState gameState)
    {
        gameState = _dispatcher.DispatchPlayersDisconnected(gameState, ref _disconnectedEvents,
            gameSettings.DisconnectedTimerStartingPoint);

        return _dispatcher.DispatchAttacks(gameState, ref _attackQueue);
    }
}