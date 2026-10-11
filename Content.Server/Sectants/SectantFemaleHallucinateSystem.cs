using Content.Shared.Sectants;
using Robust.Shared.Map;
using Robust.Shared.Random;

namespace Content.Server.Sectants;

public sealed class SectantFemaleHallucinateSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly SharedTransformSystem _xform = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<SectantFemaleHallucinateActionEvent>(OnHallucinate);
    }

    private void OnHallucinate(SectantFemaleHallucinateActionEvent args)
    {
        var uid = args.Performer;
        var xform = Transform(uid);
        var origin = _xform.GetWorldPosition(uid);

        for (var i = 0; i < 3; i++)
        {
            var angle = _random.NextAngle();
            var dist = _random.NextFloat(1f, 10f);
            var pos = origin + angle.ToVec() * dist;
            var spawn = Spawn("MobSectantHallucination", new MapCoordinates(pos, xform.MapID));
            EnsureComp<SectantHallucinationAuraComponent>(spawn);
            Spawn("SectantHallucinationSpawnEffect", Transform(spawn).Coordinates);
        }
        args.Handled = true;
    }
}
