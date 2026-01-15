using Domain.Game;
using Domain.Game.Events;
using GameEngine;

namespace GameWebApp.Classes;

public class HeadlessGameEngine(GameSettings gameSettings) : IGameEngine
{
    private List<AttackEvent> _attackQueue = [];
    private List<PlayerDisconnectedEvent> _disconnectedEvents = [];
    private readonly EventDispatcher _dispatcher = new();

    private const int MaxAttackProgress = 10000;
    private const int AttackPercentageOfMovement = 100;

    public void AddAttackEvent(AttackEvent attackEvent) => _attackQueue.Add(attackEvent);

    public void AddPlayerDisconnectedEvent(PlayerDisconnectedEvent playerDisconnectedEvent) =>
        _disconnectedEvents.Add(playerDisconnectedEvent);

    public GameState UpdateGame(GameState gameState, long elapsed)
    {
        gameState = DispatchEvents(gameState);

        return UpdateAttacks(ref gameState);
    }

    private GameState DispatchEvents(GameState gameState)
    {
        gameState = _dispatcher.DispatchPlayersDisconnected(gameState, ref _disconnectedEvents,
            gameSettings.DisconnectedTimerStartingPoint);

        return _dispatcher.DispatchAttacks(gameState, ref _attackQueue);
    }

    /**
     * This method handles the logic of the UpdateAttacks process.
     * So here the attacks are updated, destroyed when at max progress or when a counter-attack is present.
     */
    private GameState UpdateAttacks(ref GameState gameState)
    {
        foreach (var attack in gameState.Attacks.ToList())
        {
            // checks for invalid attacks such as attacks that have been countered
            if (attack.Troops <= 0)
            {
                gameState.Attacks.Remove(attack);
                continue;
            }

            var counterAttack = gameState.Attacks.FirstOrDefault(attack.IsCounterAttack);

            if (counterAttack is not null)
            {
                HandleCounterAttack(attack, counterAttack);
            }

            attack.Progress += AttackPercentageOfMovement;

            if (attack.Progress < MaxAttackProgress)
            {
                continue;
            }

            // Handle the end of track of the attack
            var defender = gameState.Territories.First(territory => territory.Id == attack.DefenderTerritoryId);
            defender.Troops -= attack.Troops;

            if (defender.Troops <= 0)
            {
                defender.Troops = 0;
                defender.OwnerId = null;
            }

            gameState.Attacks.Remove(attack);
        }

        return gameState;
    }

    private void HandleCounterAttack(Attack currentAttack, Attack detectedCounterAttack)
    {
        if (currentAttack.Troops <= 0 || detectedCounterAttack.Troops <= 0)
        {
            return;
        }

        // precision for the collision check
        const int slack = 500;
        var attacksHaveMeet = currentAttack.Progress + detectedCounterAttack.Progress > MaxAttackProgress - slack;

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