using Content.Shared.Sectants;
using Content.Shared.StatusEffect;
using Robust.Shared.Prototypes;
using Content.Shared.Bed.Sleep;
using Content.Shared.Mobs.Components;

namespace Content.Server.Sectants;

public sealed class SectantSleepSystem : EntitySystem
{
    [Dependency] private readonly EntityLookupSystem _lookup = default!;

    private static readonly EntProtoId SleepEffect = "StatusEffectForcedSleeping";

    private readonly HashSet<EntityUid> _entitySet = new();

    public override void Initialize()
    {
        SubscribeLocalEvent<SectantSleepActionEvent>(OnSleep);
    }

    private void OnSleep(SectantSleepActionEvent args)
    {
        var coords = Transform(args.Performer).Coordinates;
        _entitySet.Clear();
        _lookup.GetEntitiesInRange(coords, 5f, _entitySet);

        Logger.GetSawmill("sectant").Info($"Sleep action: found {_entitySet.Count} entities");

        foreach (var ent in _entitySet)
        {
            if (ent == args.Performer) continue;
            if (!HasComp<MobStateComponent>(ent)) continue;
            if (!HasComp<StatusEffectsComponent>(ent)) continue;

            EnsureComp<SleepingComponent>(ent);
        }
        args.Handled = true;
    }
}
