/* Filename:   GPALLChecks.cs
 * Program:    GPALL
 * Version:     7
 * Author:      Terry Beckhelm
 *              Daniel Flyte (C# revision)
 * Description: Member class of gpAll utility.  Includes methods to properly
 *              assign a devCode based on testing of land use attributes.
 * Date:        7/2012 - modified for Series 13
 */

// includes procedures()
//	inAgriculture()
//  inConstrainedList()
//  inEmployment()
//  inInfill()
//  inLandfillOrExtractive()
//  inParkingLot()
//  inResidential()
//  inSF()
//  inRoadOrFreeway()
//  inUnderConstruction()
//  inVacant()
//  ownerCheck()
//  structCheck()
//  verifyDevCode()
//  verifySiteSpec()
//  verify1 - verify16

using System;


namespace Sandag.TechSvcs.RegionalModels
{
  public class GPAllChecks
  {

	  /* method inAgriculture() */
	  /// <summary>
	  /// Method to check for inclusion in ag category.
	  /// </summary>

	  /* Revision History
	  * 
	  * STR             Date       By    Description
	  * ------------------------------------------------------------------------
	  *                 10/27/00   tbe   Initial coding
	  *                 06/19/03   dfl   C# revision
	  * ------------------------------------------------------------------------
	  */
	  public static bool inAgriculture(int lu)
	  {
		  return lu >= 8000 && lu <= 8003;
	  }     // end method inAgriculture()

	  /*************************************************************************/

	    /* method inEmployment() */
	    /// <summary>
	    /// Method to check for inclusion in employment category.
	    /// </summary>
      
	    /* Revision History
	     * 
	     * STR             Date       By    Description
	     * ------------------------------------------------------------------------
	     *                 08/15/96   tbe   Initial coding
	     *                 02/18/97   tbe   Added 6100 range to emp check
	     *                 10/06/97   tbe   Removed 6701 and 7601 per COACH
	     *                 10/31/97   tbe   Re-added 6701 per COACH
	     *                 02/10/00   tbe   Added 1500 per coach
	     *                 02/24/00   tbe   Added schools to emp list per coach
	     *                 11/16/01   tbe   Added 7210
	     *                 06/19/03   dfl   C# revision
	     *                 01/30/05   dfl   Updated for SR11
         *                 02/11/09   tbe   added 2105; removed 4112, 4117,4118; added 7211 per JHO
         *                 02/19/09   tbe   added 6801 per Beth and Ed
	     * ------------------------------------------------------------------------
	     */
	    public static bool inEmployment(int lu)
	    {
		    return (lu >= 1401 && lu <= 1404) ||
            (lu >= 1409 && lu <= 1503) ||
			(lu >= 2001 && lu <= 2105) ||
            lu == 2201 || lu == 2301 || lu == 4101 ||
            lu == 4103 || lu == 4111 || 
            (lu >= 4113 && lu <= 4117) ||
		    (lu >= 4119 && lu <= 4120) ||
		    (lu >= 5000 && lu <= 6003) ||
		    (lu >= 6100 && lu <= 6200) ||
		    (lu >= 6500 && lu <= 6509) ||
		    (lu >= 6701 && lu <= 6703) ||
		    (lu >= 6801 && lu <= 6809) ||
		    (lu >= 7200 && lu <= 7211) ||
            (lu >= 7601 && lu <= 7609) ||
            lu == 9400 || lu == 9700;
	    }   // end inEmployment()

	    //***************************************************************************
  	
	    /* method inInfill() */
	    /// <summary>
	    /// Method to check for land use included in infill list.
	    /// </summary>
      
	    /* Revision History
	     * 
	     * STR             Date       By    Description
	     * --------------------------------------------------------------------------
	     *                 04/02/98   tbe   Initial coding
	     *                 06/19/03   dfl   C# revision
         *                 02/11/09   tbe   added 1110, 1120,1280,2105 per JHO
	     * --------------------------------------------------------------------------
	     */
	    public static bool inInfill(int lu)
	    {
		    return (lu >= 1000 && lu < 1300) ||
                lu == 1404 || lu == 1409 || lu == 1501 ||
			    (lu >= 2100 && lu <= 2105) || lu == 4113 || 
			    lu == 4119 || (lu >= 5000 && lu <= 6003);
	    }   // end procedure inInfill()

	    /*****************************************************************************/
      
	    /* method inLandfillOrExtractive() */
	    /// <summary>
	    /// Method to check for inclusion in landfill or extractive category.
	    /// </summary>

	    /* Revision History
	    * 
	    * STR             Date       By    Description
	    * --------------------------------------------------------------------------
	    *                 10/26/00   tbe   Initial coding
	    *                 06/19/03   dfl   C# revision
	    * --------------------------------------------------------------------------
	    */
	    public static bool inLandfillOrExtractive(int lu)
	    {
		    return lu == 2201 || lu == 2301;
	    }   // end inLandfillOrExtractive

	    /*****************************************************************************/

        /* method inParkingLot() */
	    /// <summary>
	    /// Method to check for inclusion in parking lots category.
	    /// </summary>

	    /* Revision History
	    * 
	    * STR             Date       By    Description
	    * --------------------------------------------------------------------------
	    *                 10/26/00   tbe   Initial coding
	    *                 06/19/03   dfl   C# revision
	    * --------------------------------------------------------------------------
	    */
	    public static bool inParkingLot(int lu)
	    {
		    return lu == 4114 || lu == 4116;
	    }   // end inParkingLot

	  /*****************************************************************************/ 

	    /* method inResidential() */
	    /// <summary>
	    /// Method to check for inclusion in residential category.
	    /// </summary>
      
	    /* Revision History
	     * 
	     * STR             Date       By    Description
	     * --------------------------------------------------------------------------
	     *                 08/15/96   tbe   Initial coding
	     *                 06/19/03   dfl   C# revision
	     * --------------------------------------------------------------------------
	     */
	    public static bool inResidential(int lu)
	    {
		    return lu >= 1000 && lu <= 1409;
	    }     // end method inResidential()

	    /*****************************************************************************/

      /* method inRoadOrFreeway() */
      /// <summary>
      /// Method to check for inclusion in ag category.
      /// </summary>
      
      /* Revision History
       * 
       * STR             Date       By    Description
       * ------------------------------------------------------------------------
       *                 01/30/05   dfl   Initial coding
       *                 02/11/09   tbe   added 4117 per JHO
       * ------------------------------------------------------------------------
       */
      public static bool inRoadOrFreeway(int lu)
      {
            return lu == 4112 || lu == 4118 || lu == 4117;
      }     // end method inRoadOrFreeway()   

      /*****************************************************************************/

      /* method inMF() */
      /// <summary>
      /// Method to check for inclusion in MF category.
      /// </summary>
      public static bool inMF(int lu)
      {
          return lu >= 1200 && lu < 1300;
      }     // end method inSF()

      //*****************************************************************************

      /* method inSF() */
      /// <summary>
      /// Method to check for inclusion in SF category.
      /// </summary>
      public static bool inSF(int lu)
      {
          return lu >= 1000 && lu < 1200;
      }     // end method inSF()

      /*****************************************************************************/
      
	    /* method inConstructionCheck() */
	    /// <summary>
	    /// Method to check for inclusion in under construction category.
	    /// </summary>

	    /* Revision History
	     * 
	     * STR             Date       By    Description
	     * --------------------------------------------------------------------------
	     *                 08/15/96   tbe   Initial coding
	     *                 10/26/00   tbe   Added 9504, 9505 to under construction
	     *                 11/26/01   tbe   Extended UC to 9509
	     *                 06/19/03   dfl   C# revision
	     * --------------------------------------------------------------------------
	     */
	    public static bool inUnderConstruction(int lu)
	    {
		    return (lu >= 9500 && lu <= 9507);
	    }   // end inUnderConstruction()

	    /*****************************************************************************/

	    /* method inVacant() */
	    /// <summary>
	    /// Method to check for inclusion in vacant category.
	    /// </summary>
      
	    /* Revision History
	     * 
	     * STR             Date       By    Description
	     * --------------------------------------------------------------------------
	     *                 10/26/00   tbe   Initial coding
	     *                 06/19/03   dfl   C# revision
	     * --------------------------------------------------------------------------
	     */
	    public static bool inVacant(int lu)
	    {
		    return lu == 9101;
	    }   // end inVacant

	    /*************************************************************************/

	 
    /* method phaseCheck() */
    /// <summary>
    /// Method to check for proper phase codes.
    /// </summary>
      
    /* Revision History
     * 
     * STR             Date       By    Description
     * --------------------------------------------------------------------------
     *                 10/26/00   tbe   Initial coding
     *                 06/19/03   dfl   C# revision
     *                 01/30/05   dfl   Changed phases for SR11 forecast.
     * --------------------------------------------------------------------------
     */
    public static bool phaseCheck(int phase)
    {
        return phase == 2012 || phase == 2020 || phase == 2025 || phase == 2030
                             || phase == 2035 || phase == 2040 || phase == 2045 || phase == 2050;
    }   // end phaseCheck()

    /*****************************************************************************/

    public static bool inQuasiVacant(int lu)
    {
        return lu == 1000 || lu == 1190 || lu == 2301 ||
               lu == 2201 || lu == 4114 ||
               lu == 4116 || lu == 4118 || lu == 2105 || lu == 7204 || lu == 7606;
    }     // end method inQuasiVacant

    //*****************************************************************************

    /* method structCheck() */
    /// <summary>
    /// Method to check for inclusion in the infrastructure category.
    /// </summary>
    
    /* Revision History
     * 
     * STR             Date       By    Description
     * --------------------------------------------------------------------------
     *                 11/05/96   tbe   Initial coding
     *                 06/19/03   dfl   C# revision
     * --------------------------------------------------------------------------
     */
    public static bool structCheck(int lu)
    {
        return lu == 6003 || (lu >= 6103 && lu <= 6105) || lu == 6109;
    }     // end method structCheck()

    /*****************************************************************************/
   
    /* method verifyDevCode() */
    /// <summary>
    /// Method to perform final checks to verify devCode assignment.
    /// </summary>
      
    /* Revision History
     * 
     * STR             Date       By    Description
     * ------------------------------------------------------------------------
     *                 10/26/00   tbe   Initial coding
     *                 06/19/03   dfl   C# revision
     * ------------------------------------------------------------------------
     */
    public static int verifyDevCode(lcpolygon lcp,int devCode)
    {
      int errorCode = 999;
      switch (devCode)
      {
        case 0:    // devcode = 0
          errorCode = 0;
          break;
        case 1:     // Developed
          errorCode = GPAllChecks.verify1(lcp);
          break;

        case 2:     // Constrained
          errorCode = GPAllChecks.verify2(lcp);
          break;

        case 3:     // Vacant
          errorCode = GPAllChecks.verify3(lcp);
          break;

        case 4:     // Employment infill
          errorCode = GPAllChecks.verify4(lcp);
          break;
          
        case 5:     // Single family infill
          errorCode = GPAllChecks.verify5(lcp);
          break;

        case 6:     // Multi-family infill
          errorCode = GPAllChecks.verify6(lcp);
          break;

        case 7:     // Res to emp redev
          errorCode = GPAllChecks.verify7(lcp);
          break;

        case 8:     // SF to MF redev
          errorCode = GPAllChecks.verify8(lcp);
          break;

        case 9:     // MH to res redev
          errorCode = GPAllChecks.verify9(lcp);
          break;

        case 10:      // AG redev
          errorCode = GPAllChecks.verify10(lcp);
          break;

        case 11:      // Emp to res redev
          errorCode = GPAllChecks.verify11(lcp);
          break;

        case 12:      // Emp to emp redev
          errorCode = GPAllChecks.verify12(lcp);
          break;

        case 13:      // Res to rd or fwy
          errorCode = GPAllChecks.verify13(lcp);
          break;

        case 14:      // Emp to rd or fwy
          errorCode = GPAllChecks.verify14(lcp);
          break;

        case 15:      // Developed to mixed use
          errorCode = GPAllChecks.verify15(lcp);
          break;

        case 16:      // Vacant to mixed use
          errorCode = GPAllChecks.verify16(lcp);
          break;
      }     // end switch
      return errorCode;
    }     // end method verifyDevCode()

    /*****************************************************************************/

    /* method verifySiteSpec() */
    /// <summary>
    /// Method to perform final checks to verify sitespec records.
    /// </summary>
    
    /* Revision History
     * 
     * STR             Date       By    Description
     * --------------------------------------------------------------------------
     *                 10/26/00   tbe   Initial coding
     *                 06/19/03   dfl   C# revision
     * --------------------------------------------------------------------------
     */
    public static int verifySiteSpec(lcpolygon lcp,int devCode)
    {
      int errorCode = 999;
      // Not developed or constained
      if (lcp.siteID != 9999 && devCode <= 2)
        errorCode = 29;
      else if (inRoadOrFreeway(lcp.plu))    // Not marked for freeway
        errorCode = 30;   
      // Correct base year land
      else if (inVacant(lcp.lu) && inUnderConstruction(lcp.lu) &&
               inLandfillOrExtractive(lcp.lu) && inAgriculture(lcp.lu) &&
               lcp.lu != 1000 && lcp.lu != 4118 && lcp.lu != 2105)
        errorCode = 28;
      else if (!phaseCheck(lcp.phase))
        errorCode = 12;
      return errorCode;
    }     // end method verifySiteSpec()

    //*****************************************************************************   

    // Final checks to verify assignment is correct devCode.

    private static int verify1(lcpolygon lcp)
    {
      int errorCode = 999;
      if (inVacant(lcp.lu) || inUnderConstruction(lcp.lu))
        errorCode = 2;
      else if (lcp.siteID > 0)    // No site spec allowed
        errorCode = 4;
      return errorCode;
    }     // end method verify1()


    private static int verify2(lcpolygon lcp)
    {
      int errorCode = 999;
      if (lcp.pctConstrained < 100 && !inVacant(lcp.lu) && !inUnderConstruction(lcp.lu))
        errorCode = 6;
      // Planned land use = freeway
      else if (lcp.phase > 2012 && inRoadOrFreeway(lcp.plu))
        errorCode = 5;
      else if (lcp.siteID > 0)      // No site spec allowed
        errorCode = 4;
      return errorCode;
    }     // end method verify2()

    private static int verify3(lcpolygon lcp)
    {
      int errorCode = 999;

      // Not in vacant, ldsf, construction, land fill or parking lots
      if (!inVacant(lcp.lu) && !inUnderConstruction(lcp.lu) &&
          lcp.lu != 1000 && !inParkingLot(lcp.lu) &&  !inLandfillOrExtractive(lcp.lu) && !inQuasiVacant(lcp.lu))
        errorCode = 28;
      else if (lcp.lu == lcp.plu)
        errorCode = 7;
        
      return errorCode;
    }     // end method verify3()

    private static int verify4(lcpolygon lcp)
    {
      int errorCode = 999;
      if ((lcp.lu == 6001 || lcp.lu == 6002) && (lcp.plu == 6001 || lcp.plu == 6002))
        errorCode = 999;
      else if (lcp.lu != lcp.plu)     // Base lu != planned lu
        errorCode = 1;
      else if (!inEmployment(lcp.lu) || !inEmployment(lcp.plu))
        errorCode = 13;
      return errorCode;
    }     // end method verify4()
      
      
    private static int verify5(lcpolygon lcp)
    {
      int errorCode = 999;
      if ((lcp.lu >= 1000 && lcp.lu <= 1190) && (lcp.plu > 1190))
        errorCode = 1;
      else if (lcp.lu > 1190)
        errorCode = 15;

      return errorCode;
    }     // end method verify5()

        
    private static int verify6(lcpolygon lcp)
    {
      int errorCode = 999;
      if ((lcp.lu == 1200 || lcp.lu == 1280) && (lcp.plu < 1200 || lcp.lu > 1280))    // Base lu != planned lu
        errorCode = 1;

      return errorCode;
    }     // end method verify6()


    private static int verify7(lcpolygon lcp)
    {
      int errorCode = 999;
      if (!inResidential(lcp.lu))
        errorCode = 19;
      else if (lcp.lu == lcp.plu)
        errorCode = 7;
      else if (!inEmployment(lcp.plu))
        errorCode = 25;
      return errorCode;
    }     // end method verify7()


    private static int verify8(lcpolygon lcp)
    {
      int errorCode = 999;
      if (lcp.lu != 1000 && lcp.lu != 1100 && lcp.lu != 1110 && lcp.lu != 1120 && lcp.lu != 1190)
        errorCode = 14;
      else if (lcp.plu != 1200 && lcp.lu != 1280)
        errorCode = 20;

      return errorCode;
    }     // end method verify8()

      
    private static int verify9(lcpolygon lcp)
    {
      int errorCode = 999;
      if (lcp.lu != 1300)
        errorCode = 22;
      else if (lcp.plu != 1100 && lcp.plu != 1200 && lcp.plu != 1110 && lcp.plu != 1120 && lcp.plu != 9700)
        errorCode = 23;
  
      return errorCode;
    }     // end method verify9()


    private static int verify10(lcpolygon lcp)
    {
      int errorCode = 999;
      if (!inAgriculture(lcp.lu))
        errorCode = 24;
      else if (lcp.lu == lcp.plu)
        errorCode = 7;
      return errorCode;
    }     // end method verify10()


    private static int verify11(lcpolygon lcp)
    {
      int errorCode = 999;
      if (!inEmployment(lcp.lu))
        errorCode = 13;
      else if (lcp.plu > 1280)
        errorCode = 23;
      return errorCode;
    }     // end method verify11()


    private static int verify12(lcpolygon lcp)
    {
      int errorCode = 999;
      if (!inEmployment(lcp.lu))
        errorCode = 13;
      else if (!inEmployment(lcp.plu))
        errorCode = 25;
      else if (lcp.lu == lcp.plu)
        errorCode = 7;
      return errorCode;
    }     // end method verify12()


    private static int verify13(lcpolygon lcp)
    {
      int errorCode = 999;
      if (!inResidential(lcp.lu))
        errorCode = 19;
      else if (!inRoadOrFreeway(lcp.plu))
        errorCode = 57;
      else if (lcp.siteID > 0)
        errorCode = 4;
      return errorCode;
    }     // end method verify13()


    private static int verify14(lcpolygon lcp)
    {
      int errorCode = 999;
      if (!GPAllChecks.inEmployment(lcp.lu))
        errorCode = 13;
      else if (!inRoadOrFreeway(lcp.plu))
        errorCode = 57;
      else if (lcp.siteID > 0)
        errorCode = 4;
      return errorCode;
    }     // end method verify14()


    private static int verify15(lcpolygon lcp)
    {
      int errorCode = 999;
      if (inVacant(lcp.lu) ||inUnderConstruction(lcp.lu))
        errorCode = 2;
      return errorCode;
    }     // end method verify15()


    private static int verify16(lcpolygon lcp)
    {
      // Vacant to mixed use exactly like vacant (3), but also check plu.
      int errorCode = verify3(lcp);
      if (lcp.plu != 9700)
        errorCode = 59;
      return errorCode;
    }     // end method verify16()


   

  }     // end class GPAllChecks
}     // end namespace gpAll.checks