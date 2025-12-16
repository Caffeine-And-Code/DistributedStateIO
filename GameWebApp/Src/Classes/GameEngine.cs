using GameWebApp.Classes.Models;
using GameWebApp.Classes.Utilities;
using GameWebApp.Src.Pages;
using Microsoft.JSInterop;
using ClockTimer = System.Timers.Timer;

namespace GameWebApp.Classes;

public class GameEngine
{
    private const int Fps = 60;
    private const int MsPerFrame = 1000 / Fps;
    private ClockTimer? _attackTimer;

    public static async Task Init(IJSRuntime jsRuntime, DotNetObjectReference<Home> dotNetRef)
    {
        GameData.InitializeShapes();
        await JsFunctionProvider.InitializeUi(jsRuntime, dotNetRef);
    }

    public async Task StartGame(IJSRuntime jsRuntime)
    {
        while (true)
        {
            var start = DateTime.Now.Microsecond;

            await JsFunctionProvider.RenderUi(jsRuntime);
            
            Thread.Sleep(start + MsPerFrame - DateTime.Now.Millisecond);
            if (_attackTimer is not null)
            {
                continue;
            }

            // === TIMER DELL’ANIMAZIONE ===
            _attackTimer = new ClockTimer(50);
            _attackTimer.Elapsed += async (_, __) => await UpdateAttacks(jsRuntime);
            _attackTimer.Start();
        }
    }

    private async Task UpdateAttacks(IJSRuntime jsRuntime)
    {
        bool needsRefresh = false;

        foreach (var attack in GameData.Attacks.ToList())
        {
            attack.Progress += 0.015;
            await JsFunctionProvider.ConsoleLog(jsRuntime, $"Update Attacks {attack.Progress}");

            if (attack.Progress >= 1)
            {
                // TODO: risolvi battaglia come nel tuo engine
                GameData.Attacks.Remove(attack);
            }

            needsRefresh = true;
        }

        if (needsRefresh)
            await jsRuntime.InvokeVoidAsync("warboard.drawAttacks", "map-root", GameData.Attacks);
    }
}