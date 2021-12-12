#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using log4net;
using Terraria.ModLoader;

namespace TeaFramework.Common.Utilities.Extensions
{
    /// <summary>
    ///     Numerous methods to extend <see cref="ILog"/> functionality.
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        ///     Logs an IL patch failure.
        /// </summary>
        /// <param name="log">The <see cref="ILog"/> instance provided by a <see cref="Mod"/></param>
        /// <param name="type">The patched type.</param>
        /// <param name="method">The patched method.</param>
        /// <param name="opCode">The opcode that failed.</param>
        /// <param name="value">The value of the opcode, if applicable.</param>
        public static void PatchFailure(this ILog log, string type, string method, string opCode, string? value = null)
        {
            string logText = $"[Tea Framework/IL] Patch failure: {type}::{method} -> {opCode}";

            if (!string.IsNullOrEmpty(value))
                logText += $" {value}";
            
            log.Warn(logText);
        }
    }
}