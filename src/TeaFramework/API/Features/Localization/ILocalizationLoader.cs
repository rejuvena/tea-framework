using System.Collections.Generic;
using TeaFramework.API.DependencyInjection;

namespace TeaFramework.API.Features.Localization
{
    public interface ILocalizationLoader : IService
    {
        Dictionary<string, ILocalizationFileParser> Parsers { get; }

        void ParseFilesFromMod(ITeaMod teaMod);
    }
}