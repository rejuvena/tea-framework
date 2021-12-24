using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TeaFramework.Common.Utilities.Extensions;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace TeaFramework.Content.ChatTags
{
    public abstract class TeaChatTag : ITagHandler, ILoadable
    {
        public abstract IEnumerable<string> Aliases { get; }

        public abstract TextSnippet Parse(string text, Color baseColor = new(), string? options = null);

        public virtual void Load(Mod mod)
        {
            IEnumerable<string> aliases = Aliases;

            ConcurrentDictionary<string, ITagHandler>? handlers = typeof(ChatManager).GetCachedField("_handlers")
                .GetValue<ConcurrentDictionary<string, ITagHandler>>();

            if (handlers is null)
            {
                TeaMod.LogError("ChatTags", "Could not retrieve a dictionary from ChatManager. Aborting.");
                return;
            }

            foreach (string alias in aliases) 
                handlers[alias.ToLower()] = this;
        }

        public virtual void Unload()
        {
            throw new System.NotImplementedException();
        }
    }
}