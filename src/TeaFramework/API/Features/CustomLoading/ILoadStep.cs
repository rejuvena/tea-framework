namespace TeaFramework.API.Features.CustomLoading
{
    /// <summary>
    ///     Represents a step of mod loading. This replaces the basic load system used by tModLoader.
    /// </summary>
    public interface ILoadStep
    {
        /// <summary>
        ///     A unique, identifiable name for a load step.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     The step's load weight, which is used for sorting.
        /// </summary>
        float Weight { get; set; }

        /// <summary>
        ///     Invoked once this load step is loaded.
        /// </summary>
        /// <param name="teaMod">The mod this load step belongs to.</param>
        void Load(ITeaMod teaMod);

        /// <summary>
        ///     Invoked once this load step is unloaded.
        /// </summary>
        /// <param name="teaMod">The mod this load step belongs to.</param>
        void Unload(ITeaMod teaMod);
    }
}
