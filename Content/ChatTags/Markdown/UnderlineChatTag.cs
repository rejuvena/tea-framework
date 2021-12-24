using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.UI.Chat;

namespace TeaFramework.Content.ChatTags.Markdown
{
    public class UnderlineChatTag : TeaChatTag
    {
        private static Texture2D Underliner;
        
        static UnderlineChatTag()
        {
            Main.QueueMainThreadAction(() =>
            {
                Underliner = new Texture2D(Main.graphics.GraphicsDevice, 1, 2);
                Underliner.SetData(new[]
                {
                    Color.White,
                    Color.White
                });
            });
        }
        
        public class UnderlineTextSnippet : TeaTextSnippet
        {
            public UnderlineTextSnippet(string text, Color color, float scale = 1) : base(text, color, scale)
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
                size = FontAssets.MouseText.Value.MeasureString(Text);
                
                if (justCheckingString)
                    return false;

                spriteBatch.Draw(
                    Underliner,
                    position + (new Vector2(0f, size.Y) * 0.65f),
                    null,
                    color,
                    0f,
                    Vector2.Zero,
                    new Vector2(size.X * scale, scale),
                    SpriteEffects.None,
                    0f
                );
                
                return false;
            }
        }

        public override IEnumerable<string> Aliases => new[]
        {
            "underline",
            "u"
        };

        public override TextSnippet Parse(string text, Color baseColor = new(), string? options = null) =>
            new UnderlineTextSnippet(text, baseColor);
    }
}