using GameWebApp.Classes.Models;
using Microsoft.JSInterop;

namespace GameWebApp.Classes.Utilities;

public static class JsFunctionProvider
{
    public const string ContainerId = "map-root";

    public static async Task InitializeUi<T>(IJSRuntime jsRuntime, DotNetObjectReference<T> dotNetRef) where T : class
    {
        await jsRuntime.InvokeVoidAsync(
            "warboard.init",
            ContainerId,
            dotNetRef,
            new
            {
                playerColors = new[] { "#3b82f6", "#ef4444", "#22c55e" },
                neutralColor = "#94a3b8"
            }
        );
    }

    public static async Task ConsoleLog(IJSRuntime jsRuntime,string message)
    {
        await jsRuntime.InvokeVoidAsync(
            "logJS",
            message
        );
    }

    public static async Task RenderUi(IJSRuntime jsRuntime)
    {
        await jsRuntime.InvokeVoidAsync(
            "warboard.render",
            ContainerId,
            GameData.Territories,
            GameData.Attacks,
            null // territorio selezionato
        );
    }
}