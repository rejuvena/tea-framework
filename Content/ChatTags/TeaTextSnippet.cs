using Microsoft.Xna.Framework;
using Terraria.UI.Chat;

namespace TeaFramework.Content.ChatTags
{
    public abstract class TeaTextSnippet : TextSnippet
    {
        // Redefine here to enforce this constructor elsewhere.
        protected TeaTextSnippet(string text, Color color, float scale = 1f) : base(text, color, scale)
        {
        }
    }
}