using System.Text.Json.Serialization;
using Domain.Game;
using GameWebApp.Classes.Utilities;
using GameWebApp.Src.Pages;
using Microsoft.JSInterop;
using ClockTimer = System.Timers.Timer;

namespace GameWebApp.Classes;

public class GameEngine(GameSettings gameSettings) : HeadlessGameEngine(gameSettings)
{
    private ClockTimer? _attackTimer;
    private readonly FpsHelper _fpsHelper = new();

    public static async Task Init(IJSRuntime jsRuntime, DotNetObjectReference<Home> dotNetRef, GameState gameState)
    {
        await JsFunctionProvider.InitializeUi(jsRuntime, dotNetRef, gameState);
    }

    public async Task StartGame(IJSRuntime jsRuntime, GameState gameState)
    {
        _fpsHelper.Initialize(gameSettings.Fps);
        TerritoryShapeProvider.Instance.Initialize(Factory.GetUiTerritories(gameState.Players, gameState.Territories));

        while (true)
        {
            gameState = UpdateGame(gameState, _fpsHelper.GetElapsedTime());
            await JsFunctionProvider.RenderUi(jsRuntime, gameState);

            await _fpsHelper.WaitForNextFrame();
            _fpsHelper.Initialize(gameSettings.Fps);

            //if (_attackTimer is not null)
            //{
            //    continue;
            //}

            // === TIMER DELL’ANIMAZIONE ===
            //_attackTimer = new ClockTimer(50);
            //_attackTimer.Elapsed += async (_, __) => await UpdateAttacks(jsRuntime);
            //_attackTimer.Start();
        }
    }

    //private async Task UpdateAttacks(IJSRuntime jsRuntime)
    //{
    //    var needsRefresh = false;
//
    //    foreach (var attack in GameData.Attacks.ToList())
    //    {
    //        attack.Progress += 0.015;
    //        await JsFunctionProvider.ConsoleLog(jsRuntime, $"Update Attacks {attack.Progress}");
//
    //        if (attack.Progress >= 1)
    //        {
    //            // TODO: risolvi battaglia come nel tuo engine
    //            GameData.Attacks.Remove(attack);
    //        }
//
    //        needsRefresh = true;
    //    }
//
    //    if (needsRefresh)
    //        await jsRuntime.InvokeVoidAsync("warboard.drawAttacks", "map-root", GameData.Attacks);
    //}
}