#region License

// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007

#endregion

namespace TeaFramework.Content.Components.ItemComponents
{
    /// <summary>
    ///     Represents an item with a Terraria glowmask texture.
    /// </summary>
    public interface IGlowmaskedItem
    {
        string GlowmaskPath { get; }
    }
}