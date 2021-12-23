#region License

// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007

#endregion

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TeaFramework.Common.Utilities.Extensions;

namespace TeaFramework.Core.Drawing.Batching
{
    /// <summary>
    ///     Records <see cref="SpriteBatch"/> drawing parameters.
    /// </summary>
    public readonly struct SpriteBatchSnapshot
    {
        public SpriteSortMode SortMode { get; }

        public BlendState? BlendState { get; }

        public SamplerState? SamplerState { get; }

        public DepthStencilState? DepthStencilState { get; }

        public RasterizerState? RasterizerState { get; }

        public Effect? Effect { get; }

        public Matrix TransformMatrix { get; }

        public SpriteBatchSnapshot(
            SpriteSortMode sortMode,
            BlendState? blendState,
            SamplerState? samplerState,
            DepthStencilState? depthStencilState,
            RasterizerState? rasterizerState,
            Effect? effect,
            Matrix transformMatrix
        )
        {
            SortMode = sortMode;
            BlendState = blendState;
            SamplerState = samplerState;
            DepthStencilState = depthStencilState;
            RasterizerState = rasterizerState;
            Effect = effect;
            TransformMatrix = transformMatrix;
        }

        public SpriteBatchSnapshot(SpriteBatch spriteBatch) : this(
            spriteBatch.GetFieldValue<SpriteBatch, SpriteSortMode>("sortMode"),
            spriteBatch.GetFieldValue<SpriteBatch, BlendState>("blendState"),
            spriteBatch.GetFieldValue<SpriteBatch, SamplerState>("samplerState"),
            spriteBatch.GetFieldValue<SpriteBatch, DepthStencilState>("depthStencilState"),
            spriteBatch.GetFieldValue<SpriteBatch, RasterizerState>("rasterizerState"),
            spriteBatch.GetFieldValue<SpriteBatch, Effect>("customEffect"),
            spriteBatch.GetFieldValue<SpriteBatch, Matrix>("transformMatrix")
        )
        {
        }

        /// <summary>
        ///     Starts a <see cref="SpriteBatch"/> using the recorded parameters.
        /// </summary>
        public void BeginSpriteBatch(SpriteBatch spriteBatch) => spriteBatch.Begin(
            SortMode,
            BlendState,
            SamplerState,
            DepthStencilState,
            RasterizerState,
            Effect,
            TransformMatrix
        );
    }
}