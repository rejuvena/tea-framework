#region License

// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007

#endregion

using System;

namespace TeaFramework
{
    partial class TeaMod
    {
        /// <summary>
        ///     An internal instance of the real <see cref="TeaMod"/> instance.
        /// </summary>
        private static TeaMod? TeaInstance;

        /// <summary>
        ///     Retrieves the actual <see cref="TeaMod"/> instance.
        /// </summary>
        /// <returns>The loaded <see cref="TeaMod"/> instance, without a content look-up.</returns>
        /// <exception cref="InvalidOperationException">Thrown if this method is somehow invoked prior to <see cref="TeaMod"/> being initialized.</exception>
        public static TeaMod GetTea() => TeaInstance ?? throw new InvalidOperationException("Tea not yet loaded.");
    }
}