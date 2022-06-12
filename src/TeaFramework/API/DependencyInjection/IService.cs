namespace TeaFramework.API.DependencyInjection
{
    /// <summary>
    ///     An object that provides overridable logic for a library.
    /// </summary>
    public interface IService
    {
        /// <summary>
        ///     Called when the service is added to a <see cref="IApiServiceProvider"/>, whether it's newly added or replacing an old implementation.
        /// </summary>
        void OnAdded() {
        }

        /// <summary>
        /// Called when the service is removed from a <see cref="IApiServiceProvider"/>, whether it's set to null or replaced with a new implementation.
        /// </summary>
        void OnRemoved() {
        }
    }
}
