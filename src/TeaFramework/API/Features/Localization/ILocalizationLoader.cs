using System.Collections.Generic;

namespace TeaFramework.API.Features.Localization
{
    public interface ILocalizationLoader
    {
        Dictionary<string, ILocalizationFileParser> Parsers { get; }

        void ParseFilesFromMod(ITeaMod teaMod);
    }
}