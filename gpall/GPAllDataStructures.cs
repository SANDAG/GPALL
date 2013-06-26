/* Filename:   GPAllDataStructures.cs
 * Program:    gpAll
 * Version:    7.0 - Series 13
 * Author:     Terry Beckhelm
 *             Daniel Flyte (C# revision)
 * Date:       02/03/2009 - Series 13
 */

using System;

namespace Sandag.TechSvcs.RegionalModels
{
	public class Density
	{
		public int sphere;
		public double highDensity;
		public double lowDensity;
        public double sfovr;
	}

	public class lcpolygon
	{
		// Class Variables
		public double acres;
        public double parcel_acres;
		public int baseOwner;
		public int empCiv;
		public int empMil;
		public int lu;
		public double highDensity;
		public int hs;
		public byte military;
		public int mrktstat;
		public double lowDensity;
		public int LCKey;
		public int owner;
		public int pctConstrained;
		public int phase;
		public double planid;
		public int plu;
		public int redevInfill;
		public short mgra;
		public int siteID;
		public int sphere;

  }     // end class lcpolygon
}     // end namespace