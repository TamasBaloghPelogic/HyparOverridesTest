using Elements;
using System.Collections.Generic;
using System;
using System.Linq;

namespace OverrideTraining
{
	/// <summary>
	/// Override metadata for OverrideCoresOverrideRemoval
	/// </summary>
	public partial class OverrideCoresOverrideRemoval : IOverride
	{
        public static string Name = "OverrideCores Removal";
        public static string Dependency = null;
        public static string Context = "[*discriminator=Elements.ServiceCore]";
		public static string Paradigm = "Edit";

        /// <summary>
        /// Get the override name for this override.
        /// </summary>
        public string GetName() {
			return Name;
		}

		public object GetIdentity() {

			return Identity;
		}

	}

}