using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TeaFramework.API.Features.CustomLoading
{
    /// <summary>
    ///     Represents a collection of <see cref="ILoadStep" />s.
    /// </summary>
    public interface ILoadStepCollection : IEnumerable<ILoadStep>
    {
        /// <summary>
        ///     Shorthand for <see cref="Get" />.
        /// </summary>
        /// <param name="name">The load step to get.</param>
        public ILoadStep this[string name] => Get(name);

        /// <summary>
        ///     Adds a load step to the collection.
        /// </summary>
        /// <param name="step">The step to add.</param>
        void Add(ILoadStep step);

        /// <summary>
        ///     Gets a load step based on its unique name.
        /// </summary>
        /// <param name="name">The load step to get.</param>
        /// <returns>The load step. Throws an exception if not present.</returns>
        /// <remarks>
        ///     If you are not sure a load step is present, use <see cref="TryGet" />.
        /// </remarks>
        /// <exception cref="KeyNotFoundException">Thrown if the name does not point to any load step.</exception>
        ILoadStep Get(string name);

        /// <summary>
        ///     Attempts to retrieve a load step based on name.
        /// </summary>
        /// <param name="name">The key; <see cref="ILoadStep.Name" />.</param>
        /// <param name="step">The step that corresponds to the name (key).</param>
        /// <returns>True if the step was found, otherwise false.</returns>
        bool TryGet(string name, [NotNullWhen(true)] out ILoadStep? step);

        /// <summary>
        ///     Gets a collection with reversed ordering, used for unloading.
        /// </summary>
        /// <returns></returns>
        ILoadStepCollection GetReversed();
    }
}