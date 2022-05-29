using TeaFramework.API.Features.Events;

namespace TeaExampleMod.Events
{
    public class VersionDrawEvent : TeaEvent
    {
        public string VersionText { get; set; } = "";
    }
}