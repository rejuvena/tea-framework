using System.Collections.Generic;

namespace TeaFramework.API.Features.Patching
{
    /// <summary>
    ///     Represents an object that may handle IL edits and method detours.
    /// </summary>
    public interface IPatchRepository
    {
        List<IMonoModPatch> Patches { get; }
    }
}