using Domain.Game;

namespace GameWebApp.Classes.Utilities;

public class GameConsole(List<Player> players)
{
    public List<string> Actions { get; } = [];

    private string GetPlayerName(int? playerId) =>
        players.Find(p => p.Id == playerId)?.Username ?? "Stato Neutrale";


    public void AddNewAttackAction(Territory attacker, Territory defender, int troopsToSend)
    {
        var attackerName = GetPlayerName(attacker.OwnerId);
        var defenderName = GetPlayerName(defender.OwnerId);

        Actions.Add(
            attacker.IsAlly(defender)
                ? $"{attackerName} invia {troopsToSend} truppe a {defenderName}"
                : $"{attackerName} attacca con {troopsToSend} truppe a {defenderName}"
        );
    }
}