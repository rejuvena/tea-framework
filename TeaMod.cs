#region License
// Copyright (C) 2021 Tomat and Contributors
// GNU General Public License Version 3, 29 June 2007
#endregion

using System;
using System.Reflection;
using Terraria.ModLoader;

namespace TeaFramework
{
	/// <summary>
	///		The <see cref="Mod"/> instance used by tModLoader for Tea Framework. Your mods will typically inherit this.
	/// </summary>
	/// <remarks>
	///		If you do not want to inherit this class, you can implement functionality directly with <see cref="ITeaMod"/>.
	/// </remarks>
	public class TeaMod : Mod, ITeaMod
	{
		Mod ITeaMod.ModInstance => this;

		/// <summary>
		///		Executes a tasks only intended to be done by Tea Framework. Used as a workaround for a tModLoader issue (TML-2332).
		/// </summary>
		private bool ExecutePrivately(Action task)
		{
			if (!GetType().Assembly.FullName?.StartsWith("TeaFramework, ") ?? false)
				return false;

			task();
			return true;
		}
	}
}