namespace TeaFramework.API.CustomLoading
{
    /// <summary>
    ///     Represents a step of mod loading.
    /// </summary>
    public interface ILoadStep
    {
        string Name { get; set; }
        float Weight { get; set; }

        void Load(ITeaMod teaMod);
        void Unload(ITeaMod teaMod);
    }
}
