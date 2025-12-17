using Domain.Game;
using Domain.Game.Events;
using GameEngine;

namespace GameWebApp.Classes;

public class EventDispatcher : IEventDispatcher
{
    public GameState DispatchAttacks(GameState gameState,ref List<AttackEvent> attacksQueue)
    {
        foreach (var attackEvent in attacksQueue)
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

        attacksQueue.Clear();

        return gameState;
    }

    public GameState DispatchPlayersDisconnected(GameState gameState,
        ref List<PlayerDisconnectedEvent> playerDisconnectedQueue, int disconnectedTimerStartingPoint)
    {
        foreach (var disconnectedEvent in playerDisconnectedQueue)
        {
            var user = gameState.Players.FirstOrDefault(p => p.Id == disconnectedEvent.PlayerId);
            if (user == null)
            {
                continue;
            }

            gameState.Players.Remove(user);
            user.IsDisconnected = true;
            user.RemainingMillisecondsForReconnection = disconnectedTimerStartingPoint;
            gameState.Players.Add(user);
        }

        playerDisconnectedQueue.Clear();

        return gameState;
    }
}