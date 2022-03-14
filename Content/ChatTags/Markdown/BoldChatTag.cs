using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.UI.Chat;

namespace TeaFramework.Content.ChatTags.Markdown
{
    public class BoldChatTag : TeaChatTag
    {
        public class BoldTextSnippet : TeaTextSnippet
        {
            public BoldTextSnippet(string text, Color color, float scale = 1) : base(text, color, scale)
            {
            }

            public override bool UniqueDraw(
                bool justCheckingString,
                out Vector2 size,
                SpriteBatch spriteBatch,
                Vector2 position = new(),
                Color color = new(),
                float scale = 1
            )
            {
                scale *= 1.15f;
                
                Vector2 realScale = new(scale);
                
                if (justCheckingString)
                {
                    size = FontAssets.MouseText.Value.MeasureString(Text) * realScale;
                    return true;
                }

                size = FontAssets.MouseText.Value.MeasureString(Text) * realScale;
                
                spriteBatch.DrawString(
                    FontAssets.MouseText.Value,
                    Text,
                    position + size / 2f,
                    color,
                    0f,
                    size / 2f,
                    realScale,
                    SpriteEffects.None,
                    0f
                );
                return true;
            }
        }

        public override IEnumerable<string> Aliases => new[]
        {
            "bold",
            "b"
        };

        public override TextSnippet Parse(string text, Color baseColor = new(), string? options = null) =>
            new BoldTextSnippet(text, baseColor);
    }
}