using CounterStrikeSharp.API.Core;
using CounterStrikeSharp.API.Modules.Memory;
using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;

namespace MultiboostFix;

public class Plugin : BasePlugin
{
    public override string ModuleName => "MultiboostFix";
    public override string ModuleVersion => "1.0.0";
    public override string ModuleAuthor => "xstage";

    public override void Load(bool hotReload) =>
        VirtualFunctions.CCSPlayerPawnBase_PostThinkFunc.Hook(Hook_PostThink, HookMode.Post);

    public override void Unload(bool hotReload) =>
        VirtualFunctions.CCSPlayerPawnBase_PostThinkFunc.Unhook(Hook_PostThink, HookMode.Post);

    private HookResult Hook_PostThink(DynamicHook hook)
    {
        var pawn = hook.GetParam<CCSPlayerPawn>(0);

        if (pawn == null || !pawn.IsValid) return HookResult.Continue;

        var groundEntity = pawn.GroundEntity;
        var nextGroundEntity = groundEntity.Value?.As<CCSPlayerPawn>().GroundEntity;

        if (groundEntity is { IsValid: true } && nextGroundEntity is { IsValid: true })
        {
            groundEntity.Raw = nextGroundEntity.Raw;
        }
        
        return HookResult.Continue;
    }
}
