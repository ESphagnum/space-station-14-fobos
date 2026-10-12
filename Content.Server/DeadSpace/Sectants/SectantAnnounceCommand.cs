using Content.Server.Administration;
using Content.Shared.Administration;
using Robust.Shared.Audio;
using Robust.Shared.Console;

namespace Content.Server.Sectants;

[AdminCommand(AdminFlags.Admin)]
public sealed class SectantAnnounceCommand : IConsoleCommand
{
    [Dependency] private readonly IEntitySystemManager _sysMan = default!;

    public string Command => "sectant_announce";
    public string Description => "Запускает анонс Альпа для всех игроков.";
    public string Help => "sectant_announce [текст]";

    public void Execute(IConsoleShell shell, string argStr, string[] args)
    {
        var text = args.Length > 0
            ? string.Join(' ', args)
            : "ТЫ ЧУВСТВУЕШЬ ЕГО. ОНО УЖЕ ЗДЕСЬ.";

        var announce = _sysMan.GetEntitySystem<SectantAnnouncementSystem>();
        announce.Announce(
            "ОНО ПРИБЛИЖАЕТСЯ…",
            new SoundPathSpecifier(
                "/Audio/_DeadSpace/TEMP_FOR_EVENT/little_gabry/laughAl.ogg",
                AudioParams.Default.WithVolume(-8f)),
            Color.FromHex("#8b0000"),
            flashDuration: 1.2f,
            vignetteDuration: 5f,
            textDuration: 4f,
            shakeIntensity: 5f,
            whisper: true);
    }
}
