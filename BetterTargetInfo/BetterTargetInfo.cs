using Dalamud.Game.Addon.Lifecycle;
using Dalamud.Game.Addon.Lifecycle.AddonArgTypes;
using Dalamud.Game.ClientState.Objects;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.IoC;
using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using FFXIVClientStructs.FFXIV.Component.GUI;

namespace BetterTargetInfo;

public sealed class BetterTargetInfo : IDalamudPlugin
{
    [PluginService] internal static IAddonLifecycle AddonLifecycle { get; private set; } = null!;
    [PluginService] internal static ITargetManager TargetManager { get; private set; } = null!;

    public BetterTargetInfo()
    {
        AddonLifecycle.RegisterListener(AddonEvent.PostUpdate, "_TargetInfo", PostTargetInfoUpdate);
        AddonLifecycle.RegisterListener(AddonEvent.PostUpdate, "_FocusTargetInfo", PostFocusTargetInfoUpdate);
    }

    public void Dispose()
    {
        AddonLifecycle.UnregisterListener(PostTargetInfoUpdate);
        AddonLifecycle.UnregisterListener(PostFocusTargetInfoUpdate);
    }

    private unsafe void PostTargetInfoUpdate(AddonEvent type, AddonArgs args)
    {
        var addon = (AtkUnitBase*)args.Addon;
        if (addon == null || !addon->IsVisible) return;

        var t = TargetManager.Target;
        if (t != null)
        {
            var node = addon->GetTextNodeById(16);
            if (node != null && t is IPlayerCharacter p)
            {
                node->SetText($"[{p.ClassJob.Value.Name.ExtractText()}] {p.Name.TextValue}");
            }
        }

        var tt = TargetManager.Target?.TargetObject;
        if (tt != null)
        {
            var node = addon->GetTextNodeById(7);
            if (node != null && tt is IPlayerCharacter p)
            {
                node->SetText($"[{p.ClassJob.Value.Name.ExtractText()}] {p.Name.TextValue}");
            }
        }
    }

    private unsafe void PostFocusTargetInfoUpdate(AddonEvent type, AddonArgs args)
    {
        var addon = (AtkUnitBase*)args.Addon;
        if (addon == null || !addon->IsVisible) return;

        var f = TargetManager.FocusTarget;
        if (f != null)
        {
            var node = addon->GetTextNodeById(10);
            if (node != null && f is IPlayerCharacter p)
            {
                node->SetText($"[{p.ClassJob.Value.Name.ExtractText()}] {p.Name.TextValue}");
            }
        }
    }
}
