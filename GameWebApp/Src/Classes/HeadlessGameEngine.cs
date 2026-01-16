using Domain.Game;
using Domain.Game.Events;
using Domain.Game.Exceptions;
using GameEngine;

namespace GameWebApp.Classes;

public class HeadlessGameEngine(GameSettings gameSettings) : IGameEngine
{
    private List<AttackEvent> _attackQueue = [];
    private List<PlayerDisconnectedEvent> _disconnectedEvents = [];
    private readonly EventDispatcher _dispatcher = new();

    // TODO: consider with mattia how this can be added to the GameConfiguration class
    private const int MaxElapsedTimeToUpdateTroops = 1000;
    private long _lastTroopUpdateElapsedTime = MaxElapsedTimeToUpdateTroops;

    public void AddAttackEvent(AttackEvent attackEvent) => _attackQueue.Add(attackEvent);

    public void AddPlayerDisconnectedEvent(PlayerDisconnectedEvent playerDisconnectedEvent) =>
        _disconnectedEvents.Add(playerDisconnectedEvent);

    /// <summary>
    /// Dispatch all the events, then update all the actors of the game, first the Attacks and
    /// then the Troops if is needed (enough time is passed from last update, check MaxElapsedTimeToUpdateTroops)
    /// </summary>
    /// <param name="gameState">current game state</param>
    /// <param name="elapsed">time elapsed from last frame</param>
    /// <returns>an updated game state</returns>
    public GameState UpdateGame(GameState gameState, long elapsed)
    {
        _lastTroopUpdateElapsedTime += elapsed;

        gameState = DispatchEvents(gameState);
        gameState = UpdateAttacks(ref gameState);

        if (_lastTroopUpdateElapsedTime >= MaxElapsedTimeToUpdateTroops)
        {
            gameState = UpdateTroops(ref gameState);
            _lastTroopUpdateElapsedTime = 0;
        }

        return gameState;
    }

    private GameState DispatchEvents(GameState gameState)
    {
        gameState = _dispatcher.DispatchPlayersDisconnected(gameState, ref _disconnectedEvents,
            gameSettings.DisconnectedTimerStartingPoint);

        return _dispatcher.DispatchAttacks(gameState, ref _attackQueue);
    }

    private GameState UpdateTroops(ref GameState gameState)
    {
        var gameConfigurations = gameState.Configuration;

        foreach (var territory in gameState.Territories)
        {
            if (territory.OwnerId is null && territory.Troops >= gameConfigurations.MaxTroopForNotOwnedTerritory)
            {
                continue;
            }

            if (territory.OwnerId is not null && territory.Troops >= gameConfigurations.MaxTroopsForOwnedTerritory)
            {
                continue;
            }

            territory.Troops += 1;
        }

        return gameState;
    }

    /// <summary>
    /// This method handles the logic of the UpdateAttacks process.
    /// So here the attacks are updated, destroyed when at max progress or when a counter-attack is present.
    /// </summary>
    /// <param name="gameState">Current State of the game</param>
    /// <returns>An updated version of the State of the game</returns>
    private GameState UpdateAttacks(ref GameState gameState)
    {
        var gameConfigurations = gameState.Configuration;
        var maxAttackProgress = gameConfigurations.MaxAttackProgress;

        foreach (var attack in gameState.Attacks.ToList())
        {
            // checks for invalid attacks such as attacks that have been countered
            if (attack.Troops <= 0)
            {
                gameState.Attacks.Remove(attack);
                continue;
            }

            var counterAttack = gameState.GetCounterAttackOfAttack(attack);

            if (counterAttack is not null)
            {
                HandleCounterAttack(attack, counterAttack, maxAttackProgress);
            }

            attack.Progress += gameConfigurations.AttackPercentageOfMovement;

            if (attack.Progress < maxAttackProgress)
            {
                continue;
            }

            // Handle the end of track of the attack
            var defender = gameState.FindTerritoryById(attack.DefenderTerritoryId);
            var attacker = gameState.FindTerritoryById(attack.AttackerTerritoryId);

            if (attacker.IsAlly(defender))
            {
                defender.Troops =
                    Math.Min(gameConfigurations.MaxTroopsForOwnedTerritory, attack.Troops + defender.Troops);
            }
            else
            {
                defender.Troops -= attack.Troops;

                if (defender.Troops <= 0)
                {
                    // the overflown troops of the attack take the territory 
                    defender.Troops = Math.Abs(defender.Troops);
                    defender.OwnerId = attacker.OwnerId;
                }
            }

            gameState.Attacks.Remove(attack);
        }

        return gameState;
    }

    private void HandleCounterAttack(Attack currentAttack, Attack detectedCounterAttack, int maxAttackProgress)
    {
        if (currentAttack.Troops <= 0 || detectedCounterAttack.Troops <= 0)
        {
            return;
        }

        // precision for the collision check
        const int slack = 500;
        var attacksHaveMeet = currentAttack.Progress + detectedCounterAttack.Progress > maxAttackProgress - slack;

        if (!attacksHaveMeet)
        {
            return;
        }

        // Handle the battle between the troops
        var currentAttackTroops = currentAttack.Troops;
        currentAttack.Troops -= detectedCounterAttack.Troops;
        detectedCounterAttack.Troops -= currentAttackTroops;
    }
}