#region License

// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007

#endregion

using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using TeaFramework.Core.Reflection;

namespace TeaFramework.Core.Drawing.Batching
{
    /// <summary>
    ///     Disposable class that starts and ends <see cref="SpriteBatch"/>es with ease. <br />
    ///     Provides safety with beginning and ending batches as well.
    /// </summary>
    public sealed class DisposableSpriteBatch : IDisposable
    {
        public readonly SpriteBatch SpriteBatch;

        public SpriteBatchSnapshot CachedSnapshot { get; }

        public bool BeganPrior { get; }

        public DisposableSpriteBatch(SpriteBatch spriteBatch, SpriteBatchSnapshot snapshot, bool? began = null)
        {
            bool realBegan = began ?? spriteBatch.GetFieldValue<SpriteBatch, bool>("beginCalled");

            if (realBegan)
                spriteBatch.End();

            snapshot.BeginSpriteBatch(spriteBatch);

            CachedSnapshot = new SpriteBatchSnapshot(spriteBatch);
            SpriteBatch = spriteBatch;
            BeganPrior = realBegan;
        }

        public void Draw(Texture2D texture, Vector2 position, Color color) =>
            SpriteBatch.Draw(texture, position, color);

        public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color) =>
            SpriteBatch.Draw(texture, position, sourceRectangle, color);

        public void Draw(
            Texture2D texture,
            Vector2 position,
            Rectangle? sourceRectangle,
            Color color,
            float rotation,
            Vector2 origin,
            float scale,
            SpriteEffects effects,
            float layerDepth) => SpriteBatch.Draw(
            texture,
            position,
            sourceRectangle,
            color, rotation,
            origin,
            scale,
            effects,
            layerDepth
        );

        public void Draw(
            Texture2D texture,
            Vector2 position,
            Rectangle? sourceRectangle,
            Color color,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            SpriteEffects effects,
            float layerDepth) => SpriteBatch.Draw(
            texture,
            position,
            sourceRectangle,
            color,
            rotation,
            origin,
            scale,
            effects,
            layerDepth
        );

        public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color) =>
            SpriteBatch.Draw(texture, destinationRectangle, color);

        public void Draw(
            Texture2D texture,
            Rectangle destinationRectangle,
            Rectangle? sourceRectangle,
            Color color) => SpriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);

        public void Draw(
            Texture2D texture,
            Rectangle destinationRectangle,
            Rectangle? sourceRectangle,
            Color color,
            float rotation,
            Vector2 origin,
            SpriteEffects effects,
            float layerDepth) => SpriteBatch.Draw(
            texture,
            destinationRectangle,
            sourceRectangle,
            color,
            rotation,
            origin,
            effects,
            layerDepth
        );

        public void DrawString(
            SpriteFont spriteFont,
            StringBuilder text,
            Vector2 position,
            Color color) => SpriteBatch.DrawString(spriteFont, text, position, color);

        public void DrawString(
            SpriteFont spriteFont,
            StringBuilder text,
            Vector2 position,
            Color color,
            float rotation,
            Vector2 origin,
            float scale,
            SpriteEffects effects,
            float layerDepth) => SpriteBatch.DrawString(
            spriteFont,
            text,
            position,
            color,
            rotation,
            origin,
            scale,
            effects,
            layerDepth
        );

        public void DrawString(
            SpriteFont spriteFont,
            StringBuilder text,
            Vector2 position,
            Color color,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            SpriteEffects effects,
            float layerDepth) => SpriteBatch.DrawString(
            spriteFont,
            text,
            position,
            color,
            rotation,
            origin,
            scale,
            effects,
            layerDepth
        );

        public void DrawString(SpriteFont spriteFont, string text, Vector2 position, Color color) =>
            SpriteBatch.DrawString(spriteFont, text, position, color);

        public void DrawString(
            SpriteFont spriteFont,
            string text,
            Vector2 position,
            Color color,
            float rotation,
            Vector2 origin,
            float scale,
            SpriteEffects effects,
            float layerDepth) => SpriteBatch.DrawString(
            spriteFont,
            text,
            position,
            color,
            rotation,
            origin,
            scale,
            effects,
            layerDepth
        );

        public void DrawString(
            SpriteFont spriteFont,
            string text,
            Vector2 position,
            Color color,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            SpriteEffects effects,
            float layerDepth) => SpriteBatch.DrawString(
            spriteFont,
            text,
            position,
            color,
            rotation,
            origin,
            scale,
            effects,
            layerDepth
        );

        public void DrawString(
            DynamicSpriteFont spriteFont,
            string text,
            Vector2 position,
            Color color) => SpriteBatch.DrawString(spriteFont, text, position, color);

        public void DrawString(
            DynamicSpriteFont spriteFont,
            StringBuilder text,
            Vector2 position,
            Color color) => SpriteBatch.DrawString(spriteFont, text, position, color);

        public void DrawString(
            DynamicSpriteFont spriteFont,
            string text,
            Vector2 position,
            Color color,
            float rotation,
            Vector2 origin,
            float scale,
            SpriteEffects effects,
            float layerDepth) => SpriteBatch.DrawString(
            spriteFont,
            text,
            position,
            color,
            rotation,
            origin,
            scale,
            effects,
            layerDepth
        );

        public void DrawString(
            DynamicSpriteFont spriteFont,
            StringBuilder text,
            Vector2 position,
            Color color,
            float rotation,
            Vector2 origin,
            float scale,
            SpriteEffects effects,
            float layerDepth) => SpriteBatch.DrawString(
            spriteFont,
            text,
            position,
            color,
            rotation,
            origin,
            scale,
            effects,
            layerDepth
        );

        public void DrawString(
            DynamicSpriteFont spriteFont,
            string text,
            Vector2 position,
            Color color,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            SpriteEffects effects,
            float layerDepth) => SpriteBatch.DrawString(
            spriteFont,
            text,
            position,
            color,
            rotation,
            origin,
            scale,
            effects,
            layerDepth
        );

        public void DrawString(
            DynamicSpriteFont spriteFont,
            StringBuilder text,
            Vector2 position,
            Color color,
            float rotation,
            Vector2 origin,
            Vector2 scale,
            SpriteEffects effects,
            float layerDepth) => SpriteBatch.DrawString(
            spriteFont,
            text,
            position,
            color,
            rotation,
            origin,
            scale,
            effects,
            layerDepth
        );

        public void Dispose()
        {
            SpriteBatch.End();

            if (BeganPrior)
                CachedSnapshot.BeginSpriteBatch(SpriteBatch);
        }
    }
}