using Domain.Game;
using Domain.Game.Commands;
using GameEngine;

namespace GameWebApp.Classes;

//TODO: modulize this class, the dispatch methods should not be here
public class HeadlessGameEngine : IGameEngine
{
    private readonly List<AttackEvent> _attackQueue = [];
    private readonly List<PlayerDisconnectedEvent> _disconnectedEvents = [];

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
        gameState = DispatchDisconnectedEvents(gameState, _disconnectedEvents);

        return DispatchAttackEvents(gameState, _attackQueue);
    }

    private static GameState DispatchAttackEvents(GameState gameState, List<AttackEvent> attackEvents)
    {
        foreach (var attackEvent in attackEvents)
        {
            var attackerTerritory = gameState.Territories.FirstOrDefault(t => t.Id == attackEvent.AttackerTerritoryId);
            var defenderTerritory = gameState.Territories.FirstOrDefault(t => t.Id == attackEvent.DefenderTerritoryId);

            if (attackerTerritory == null || defenderTerritory == null)
            {
                continue;
            }

            gameState.Attacks.Add(new Attack
            {
                AttackerTerritoryId = attackEvent.AttackerTerritoryId,
                DefenderTerritoryId = attackEvent.DefenderTerritoryId,
                Troops = attackEvent.Troops,
                Progress = 0
            });
        }

        attackEvents.Clear();

        return gameState;
    }

    private static GameState DispatchDisconnectedEvents(GameState gameState,
        List<PlayerDisconnectedEvent> disconnectedEvents)
    {
        foreach (var disconnectedEvent in disconnectedEvents)
        {
            var user = gameState.Players.FirstOrDefault(p => p.Id == disconnectedEvent.PlayerId);
            if (user == null)
            {
                continue;
            }

            gameState.Players.Remove(user);
            user.IsDisconnected = true;
            //TODO: the value should be got from the appsettings.json 
            user.RemainingMillisecondsForReconnection = 60000;
            gameState.Players.Add(user);
        }

        disconnectedEvents.Clear();

        return gameState;
    }
}