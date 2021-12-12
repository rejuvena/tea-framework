#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using TeaFramework.Common.Utilities.Extensions;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace TeaFramework.Content.Components.ItemComponents
{
    /// <summary>
    ///     Handles applying glowmasks to <see cref="IGlowmaskedItem"/>s.
    /// </summary>
    public class GlowmaskedItemHandler : GlobalItem
    {
        private Dictionary<string, short> GlowmaskCollection = new();

        public override void Unload()
        {
            base.Unload();

            TextureAssets.GlowMask = TextureAssets.GlowMask.Where(x => !x?.Name.StartsWith('$') ?? true).ToArray();

            GlowmaskCollection.Clear();
        }

        public short GetGlowmask(IGlowmaskedItem glowmaskedItem)
        {
            if (GlowmaskCollection.ContainsKey(glowmaskedItem.GlowmaskPath))
                return GlowmaskCollection[glowmaskedItem.GlowmaskPath];
            
            short count = (short) TextureAssets.GlowMask.Length;
            Asset<Texture2D> texture = ModContent.Request<Texture2D>(
                glowmaskedItem.GlowmaskPath,
                AssetRequestMode.ImmediateLoad
            );
            
            texture.Value.Name = '$' + texture.Value.Name;

            TextureAssets.GlowMask.Add(texture);
            GlowmaskCollection.Add(glowmaskedItem.GlowmaskPath, count);
            return count;
        }
        
        public override void SetDefaults(Item item)
        {
            base.SetDefaults(item);

            if (item is not IGlowmaskedItem glowmaskedItem)
                return;

            item.glowMask = GetGlowmask(glowmaskedItem);
        }
    }
}