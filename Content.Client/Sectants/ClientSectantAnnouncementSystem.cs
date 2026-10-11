using System.Numerics;
using Content.Client.Camera;
using Content.Shared.Camera;
using Content.Shared.DeadSpace.Camera;
using Content.Shared.Sectants;
using Robust.Client.Graphics;
using Robust.Client.Player;
using Robust.Shared.Random;
using Content.Client.Sectants;

namespace Content.Client.Sectants;

public sealed class ClientSectantAnnouncementSystem : EntitySystem
{
    [Dependency] private readonly IOverlayManager _overlay = default!;
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly IRobustRandom _random = default!;
    [Dependency] private readonly CameraRecoilSystem _recoil = default!;
    [Dependency] private readonly ScreenshakeSystem _screenshake = default!;

    private SectantFlashOverlay? _currentFlash;

    public override void Initialize()
    {
        SubscribeNetworkEvent<SectantAnnouncementEvent>(OnAnnounce);
    }

    private void OnAnnounce(SectantAnnouncementEvent ev)
    {
        if (_currentFlash != null)
        {
            _overlay.RemoveOverlay(_currentFlash);
            _currentFlash = null;
        }

        _currentFlash = new SectantFlashOverlay(ev.Color, ev.FlashDuration, _overlay);
        _overlay.AddOverlay(_currentFlash);

        if (_player.LocalEntity is not { } local)
            return;

        var kick = new Vector2(
            _random.NextFloat(-ev.ShakeIntensity, ev.ShakeIntensity),
            _random.NextFloat(-ev.ShakeIntensity, ev.ShakeIntensity));
        _recoil.KickCamera(local, kick);

        _screenshake.Screenshake(local, null, new ScreenshakeParameters
        {
            Trauma = 0.5f,
            DecayRate = 1.2f,
            Frequency = 0.008f,
        });
    }
}
