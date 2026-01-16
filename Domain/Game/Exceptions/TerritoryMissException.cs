namespace Domain.Game.Exceptions;

public class TerritoryMissException(Guid territoryId) : Exception("Cannot find territory with id " + territoryId);