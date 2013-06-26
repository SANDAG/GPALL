/* Filename:    GPAllUtils.cs
 * Program:     GPAll
 * Version:    7 - Series 13
 * Author:      Terry Beckhelm & Daniel Flyte (C# revision)
 * Date:        7/2012
 * Description: Member class of GPAll.  Includes methods to assign new land use 
 *              densities, handle special scenarios, and perfom vacant flag 
 *              processing.
 */

// includes procedures

//	setInfillDensities()
//  setSFDefaultDensity()
//	vacantFilter()
//	
using System;
using System.Windows.Forms;
using System.IO;


namespace Sandag.TechSvcs.RegionalModels
{
  public class gpallUtils
  {

    /*************************************************************************/
      
    /* method setInfillDensities() */
    /// <summary>
    /// Method to assign new densities to infill land uses.
    /// </summary>

    /* Revision History
     * 
     * STR             Date       By    Description
     * ------------------------------------------------------------------------
     *                 05/09/97   tbe   Initial coding
     *                 06/20/03   dfl   C# revision
     * ------------------------------------------------------------------------
     */
    public static void setInfillDensities(lcpolygon lcp, Density[] infillDen,int infillDenCount)
    {
        for (int i = 0; i < infillDenCount; i++)
        {
            if (infillDen[i].sphere == lcp.sphere)
            {
                lcp.lowDensity = infillDen[i].lowDensity;
                lcp.highDensity = infillDen[i].highDensity;
                break;
            }     // end if
        }     // end for
    }     // end method setInfillDensities()

    /*************************************************************************/

    /* method setSFDefaultDensity() */
    /// <summary>
    /// Method to assign new densities to infill land uses.
    /// </summary>

    /* Revision History
     * 
     * STR             Date       By    Description
     * ------------------------------------------------------------------------
     *                 05/13/09   tbe   Initial coding
     * ------------------------------------------------------------------------
     */
    public static void setSFDefaultDensity(lcpolygon lcp, Density[] infillDen,int infillDenCount)
    {
        for (int i = 0; i < infillDenCount; i++)
        {
            if (infillDen[i].sphere == lcp.sphere)
            {
                lcp.lowDensity = infillDen[i].sfovr;
                lcp.highDensity = infillDen[i].sfovr;
                break;
            }     // end if
        }     // end for
    }     // end method setSFDefaultDensity()

    /*************************************************************************/

    /* method vacantFilter() */
    /// <summary>
    /// Method to perform vacant flag processing.
    /// </summary>

    /* Revision History
     * 
     * STR             Date       By    Description
     * ------------------------------------------------------------------------
     *                 08/15/96   tbe  Initial coding
     *                 02/10/00   tbe  Added 9102, 9504 and 9505 to vacant
     *                                 check per coach
     *                 06/20/03   dfl  C# revision
     *                 01/30/05   dfl  Updated for SR11 forecast with landcore.
     * ------------------------------------------------------------------------
     */
    public static bool vacantFilter(lcpolygon lcp)
    {
        bool inVacant = false;

        if (GPAllChecks.inVacant(lcp.lu))
            inVacant = true;

        // Under construction
        else if (GPAllChecks.inUnderConstruction(lcp.lu))
            inVacant = true;

        // LDSF, golf, extractive, parking lots
        else if (GPAllChecks.inQuasiVacant(lcp.lu))
        {
            if (lcp.plu != lcp.lu)
                inVacant = true;
        }  // end else if
        return inVacant;
    }     // end method vacantFilter()

    //**********************************************************************************
  }     // End class gpallUtils
}     // End namespace 