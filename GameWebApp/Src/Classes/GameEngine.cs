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
    private readonly GameSettings _gameSettings = gameSettings;

    public static async Task Init(IJSRuntime jsRuntime, DotNetObjectReference<Home> dotNetRef, GameState gameState)
    {
        await JsFunctionProvider.InitializeUi(jsRuntime, dotNetRef, gameState);
    }

    public async Task StartGame(IJSRuntime jsRuntime, GameState gameState)
    {
        _fpsHelper.Initialize(_gameSettings.Fps);
        TerritoryShapeProvider.Instance.Initialize(Factory.GetUiTerritories(gameState.Players, gameState.Territories));

        while (true)
        {
            gameState = UpdateGame(gameState, _fpsHelper.GetElapsedTime());
            Console.WriteLine(_fpsHelper.GetElapsedTime());
            await JsFunctionProvider.RenderUi(jsRuntime, gameState);

            await _fpsHelper.WaitForNextFrame();
            _fpsHelper.Initialize(_gameSettings.Fps);

            if (_attackTimer is not null)
            {
                continue;
            }

            // === TIMER DELL’ANIMAZIONE ===
            _attackTimer = new ClockTimer(50);
            var state = gameState;
            _attackTimer.Elapsed += async (_, __) => await UpdateAttacks(jsRuntime, state);
            _attackTimer.Start();
        }
        // ReSharper disable once FunctionNeverReturns
    }

    private async Task UpdateAttacks(IJSRuntime jsRuntime, GameState gameState)
    {
        await JsFunctionProvider.RenderAttacks(jsRuntime, gameState);
    }
}