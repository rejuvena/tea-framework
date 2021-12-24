#region License

// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007

#endregion

using System;
using Terraria.ModLoader;

namespace TeaFramework
{
    partial class TeaMod
    {
        /// <summary>
        ///     An internal instance of the real <see cref="TeaMod"/> instance.
        /// </summary>
        private static TeaMod? TeaInstance;

        /// <summary>
        ///     Assembly-constant value used for logging.
        /// </summary>
        public const string TeaVersion = "0.1.0";
        
        /// <summary>
        ///     Retrieves the actual <see cref="TeaMod"/> instance.
        /// </summary>
        /// <returns>The loaded <see cref="TeaMod"/> instance, without a content look-up.</returns>
        /// <exception cref="InvalidOperationException">Thrown if this method is somehow invoked prior to <see cref="TeaMod"/> being initialized.</exception>
        public static TeaMod GetTea() => TeaInstance ?? throw new InvalidOperationException("Tea not yet loaded.");

        internal static void LogError(string system, string message)
        {
            // Ensure we always have an instance of TeaMod instead of relying on TeaInstance.
            TeaMod mod = ModContent.GetInstance<TeaMod>();

            mod.Logger.Error(@"  _     ___                         _   ");
            mod.Logger.Error(@" | |   | __| _ _  _ _  ___  _ _    | |  ");
            mod.Logger.Error(@" |_|   | _| | '_|| '_|/ _ \| '_|   |_|  ");
            mod.Logger.Error(@" (_)   |___||_|  |_|  \___/|_|     (_)  ");
            mod.Logger.Error($"[{system}] {message}");
        }
    }
}