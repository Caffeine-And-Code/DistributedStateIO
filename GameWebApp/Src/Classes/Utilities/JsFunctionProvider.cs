using Domain.Game;
using GameWebApp.Classes.Models;
using Microsoft.JSInterop;

namespace GameWebApp.Classes.Utilities;

public static class JsFunctionProvider
{
    public const string ContainerId = "map-root";

    public static async Task InitializeUi<T>(IJSRuntime jsRuntime, DotNetObjectReference<T> dotNetRef,
        GameState gameState) where T : class
    {
        await jsRuntime.InvokeVoidAsync(
            "warboard.init",
            ContainerId,
            dotNetRef,
            new
            {
                playerColors = new[]
                    { "#3b82f6", "#ef4444", "#22c55e", "#f59e0b", "#8b5cf6", "#ec4899", "#06b6d4", "#eab308" },
                neutralColor = "#94a3b8",
                playerIds = gameState.Players.Select(p => p.Id).Order()
            }
        );
    }

    public static async Task ConsoleLog(IJSRuntime jsRuntime, string message)
    {
        await jsRuntime.InvokeVoidAsync(
            "logJS", $"[C#] says: {message}"
        );
    }

    public static async Task RenderUi(IJSRuntime jsRuntime, GameState gameState)
    {
        await jsRuntime.InvokeVoidAsync(
            "warboard.render",
            ContainerId,
            TerritoryShapeProvider.Instance.GetTerritories(gameState.Territories),
            gameState.Attacks,
            null // territorio selezionato
        );
    }
}