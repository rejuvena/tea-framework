using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria.GameContent;
using Terraria.UI.Chat;

namespace TeaFramework.Content.ChatTags.Markdown
{
    public class ItalicChatTag : TeaChatTag
    {
        public class ItalicTextSnippet : TeaTextSnippet
        {
            public ItalicTextSnippet(string text, Color color, float scale = 1) : base(text, color, scale)
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
                if (justCheckingString)
                {
                    size = FontAssets.MouseText.Value.MeasureString(Text);
                    return true;
                }

                Vector2 realScale = new(scale);
                realScale.X -= 0.05f;
                size = FontAssets.MouseText.Value.MeasureString(Text);

                foreach (char c in Text)
                {
                    Vector2 charSize = FontAssets.MouseText.Value.MeasureString(c.ToString());

                    spriteBatch.DrawString(
                        FontAssets.MouseText.Value,
                        c.ToString(),
                        position + (charSize / 2f),
                        color,
                        MathHelper.ToRadians(8f),
                        charSize / 2f,
                        realScale,
                        SpriteEffects.None,
                        0f
                    );

                    position.X += charSize.X;
                }

                return true;
            }
        }

        public override IEnumerable<string> Aliases => new[]
        {
            "italics",
            "italic"
            // no "i" as it interferes with the item tag
        };

        public override TextSnippet Parse(string text, Color baseColor = new(), string? options = null) =>
            new ItalicTextSnippet(text, baseColor);
    }
}