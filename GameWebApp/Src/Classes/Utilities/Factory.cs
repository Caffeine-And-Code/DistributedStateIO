using Domain.Game;
using GameWebApp.Classes.Models;
using GameWebApp.Classes.Utilities.TerritoriesShape;

namespace GameWebApp.Classes.Utilities;

public abstract class Factory
{
    public static GameState GetFirstGameState()
    {
        var generatedPlayers = CreateGamePlayers();

        return new GameState
        {
            GameId = Guid.Empty,
            Attacks = [],
            IsGameWaitingForReconnection = false,
            Players = generatedPlayers,
            Configuration = CreateConfiguration(),
            Territories = GetUiTerritories(generatedPlayers).Select(t => t.ToTerritory()).ToList()
        };
    }

    private static List<Player> CreateGamePlayers()
    {
        return
        [
            CreatePlayer(),
            CreatePlayer(),
            CreatePlayer(),
            CreatePlayer(),
            CreatePlayer(),
            CreatePlayer(),
            CreatePlayer(),
            CreatePlayer()
        ];
    }

    private static Player CreatePlayer()
    {
        var rand = new Random();

        return new Player
        {
            Id = rand.Next(),
            Username = "User" + rand.Next(),
            IsInGame = true
        };
    }

    private static GameConfiguration CreateConfiguration()
    {
        return new GameConfiguration
        {
            SpawnRate = 1.5,
            MaxTroopForNotOwnedTerritory = 15,
            MaxTroopsForOwnedTerritory = 50,
            DisconnectionSeconds = 90,
            MaxAttackProgress = 10000,
            AttackPercentageOfMovement = 100
        };
    }

    public static ICollection<UiTerritory> GetUiTerritories(ICollection<Player> players,
        ICollection<Territory>? alreadyCreatedTerritories = null)
    {
        var conf = CreateConfiguration();
        const int defaultNumberOfPlayerForOwnedTerritories = 10;

        if (alreadyCreatedTerritories == null)
        {
            return
            [
                CreateTerritory(150, 200, conf.MaxTroopForNotOwnedTerritory),
                CreateTerritory(320, 120, defaultNumberOfPlayerForOwnedTerritories, players.ElementAt(0).Id),
                CreateTerritory(500, 150, conf.MaxTroopForNotOwnedTerritory),
                CreateTerritory(200, 380, defaultNumberOfPlayerForOwnedTerritories, players.ElementAt(6).Id),
                CreateTerritory(500, 320, defaultNumberOfPlayerForOwnedTerritories, players.ElementAt(1).Id),
                CreateTerritory(700, 180, conf.MaxTroopForNotOwnedTerritory),
                CreateTerritory(350, 500, defaultNumberOfPlayerForOwnedTerritories, players.ElementAt(7).Id),
                CreateTerritory(650, 400, conf.MaxTroopForNotOwnedTerritory),
                CreateTerritory(880, 300, defaultNumberOfPlayerForOwnedTerritories, players.ElementAt(2).Id),
                CreateTerritory(1050, 250, conf.MaxTroopForNotOwnedTerritory),
                CreateTerritory(150, 550, defaultNumberOfPlayerForOwnedTerritories, players.ElementAt(4).Id),
                CreateTerritory(550, 550, conf.MaxTroopForNotOwnedTerritory),
                CreateTerritory(800, 520, conf.MaxTroopForNotOwnedTerritory),
                CreateTerritory(1050, 450, defaultNumberOfPlayerForOwnedTerritories, players.ElementAt(3).Id),
                CreateTerritory(850, 100, conf.MaxTroopForNotOwnedTerritory),
                CreateTerritory(1050, 100, defaultNumberOfPlayerForOwnedTerritories, players.ElementAt(5).Id),
            ];
        }
        
        return
        [
            CreateTerritory(150, 200, conf.MaxTroopForNotOwnedTerritory,id:alreadyCreatedTerritories.ElementAt(0).Id),
            CreateTerritory(320, 120, defaultNumberOfPlayerForOwnedTerritories, players.ElementAt(0).Id,alreadyCreatedTerritories.ElementAt(1).Id),
            CreateTerritory(500, 150, conf.MaxTroopForNotOwnedTerritory,id:alreadyCreatedTerritories.ElementAt(2).Id),
            CreateTerritory(200, 380, defaultNumberOfPlayerForOwnedTerritories, players.ElementAt(6).Id,alreadyCreatedTerritories.ElementAt(3).Id),
            CreateTerritory(500, 320, defaultNumberOfPlayerForOwnedTerritories, players.ElementAt(1).Id,alreadyCreatedTerritories.ElementAt(4).Id),
            CreateTerritory(700, 180, conf.MaxTroopForNotOwnedTerritory,id:alreadyCreatedTerritories.ElementAt(5).Id),
            CreateTerritory(350, 500, defaultNumberOfPlayerForOwnedTerritories, players.ElementAt(7).Id,alreadyCreatedTerritories.ElementAt(6).Id),
            CreateTerritory(650, 400, conf.MaxTroopForNotOwnedTerritory,id:alreadyCreatedTerritories.ElementAt(7).Id),
            CreateTerritory(880, 300, defaultNumberOfPlayerForOwnedTerritories, players.ElementAt(2).Id,alreadyCreatedTerritories.ElementAt(8).Id),
            CreateTerritory(1050, 250, conf.MaxTroopForNotOwnedTerritory,id:alreadyCreatedTerritories.ElementAt(9).Id),
            CreateTerritory(150, 550, defaultNumberOfPlayerForOwnedTerritories, players.ElementAt(4).Id,alreadyCreatedTerritories.ElementAt(10).Id),
            CreateTerritory(550, 550, conf.MaxTroopForNotOwnedTerritory,id:alreadyCreatedTerritories.ElementAt(11).Id),
            CreateTerritory(800, 520, conf.MaxTroopForNotOwnedTerritory,id:alreadyCreatedTerritories.ElementAt(12).Id),
            CreateTerritory(1050, 450, defaultNumberOfPlayerForOwnedTerritories, players.ElementAt(3).Id,alreadyCreatedTerritories.ElementAt(13).Id),
            CreateTerritory(850, 100, conf.MaxTroopForNotOwnedTerritory,id:alreadyCreatedTerritories.ElementAt(14).Id),
            CreateTerritory(1050, 100, defaultNumberOfPlayerForOwnedTerritories, players.ElementAt(5).Id,alreadyCreatedTerritories.ElementAt(15).Id),
        ];
    }

    private static UiTerritory CreateTerritory(int x, int y, int troops, int? ownerId = null, Guid? id = null)
    {
        return new UiTerritory
        {
            Id = id ?? Guid.NewGuid(),
            Shape = TerritoryShapeGenerator.Generate(x, y),
            X = x,
            Y = y,
            Troops = troops,
            OwnerId = ownerId,
        };
    }
}