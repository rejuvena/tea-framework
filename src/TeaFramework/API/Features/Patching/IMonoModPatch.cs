namespace TeaFramework.API.Features.Patching
{
    /// <summary>
    ///     A MonoMod patch.
    /// </summary>
    public interface IMonoModPatch
    {
        void Apply();

        void Unapply();
    }
}