/* Filename:    GPAll.cs
 * Program:     GPAll
 * Version:    
 *              7   - GPALL for Series 13
 *              6.2 - GPALL for Series 13 final with new server name for modeling (PILA)
 *                    added code for handling mil sitespec
 *              6.1 - GPALL for Series 13 final with new server name for GIS
 *              6.0 - GPALL for Series 13
 *              5.5 - Consolidation and cleanup of 5.0 for Landcore changes for Series 11
 *              5.0 - C# revision of version 4.0              
 * 
 *              4.0 - gpall changes for series 10 final
 *                1:  Added ep lookup table to set densities
 * 
 *              3.0 - gpall changes for series 10 small area transportation 
 *                    modeling using new mgras and database sr10_satm
 *              2.0 - gpall changes for series 10
 *                1:  We got rid of nhd, vpa and palus and went back to wet 
 *                    contraint
 *                2:  Flood constraint was returned to an integer
 *                3:  Added habpres (habitat preservation) to structure
 *                4:  Got rid of rdphase and cp
 *                5:  Added phasing lookup for TP
 * 
 *              1.5 - gpall changes for next round of series 9
 *                1:  I removed almost all processing for redev and infill 
 *                    records this has been moved to landcore processing - this 
 *                    means a new gpall field redevinf carrying the devcode
 *                2:  Infill records processed to assign new densities
 *                3:  Roads processing and checks removed for most modules
 *                4:  Steep slope in county with codes 1,17,18,19,23,24
 *                    assigned new densities
 *                5:  some new land use codes added to processing
 *                6:  gpall structure modified - some new variables
 *                7:  ownership checks removed
 *                8:  wet contraint has been changed to three new vars
 *                    nhd, vpc and palus
 *                9:  flood plain check values changed to 'fw100' and 'fp100'
 *             
 *              1.4 - Added land use checks previously done by GIS stuff
 * 
 *              1.3 - Handles the sr9 technical update forecast increments 
 *                    (see gpall_tu directory)
 *                1:  Changes for spring 2000 technical update
 *                2:  Uses sr9 technical update database sr9tu
 *                3:  Mixed use and spcode processing is unnecessary, however 
 *                    the codes still exist in the structure and the table. 
 *                    we'll zero these at load time.
 *                4:  Also, the old 98 spheres have been combined into 8 large 
 *                    areas (groups)
 *                5:  Changes to the checks arrays for employment, infill and 
 *                    rural uses with a new generic path.
 *                6: 1500 added to emp densities and employment check 9102, 
 *                   9404, and 9405 added to vacant filter
 * 
 *             1.2 - Handles the final sr9 forecast increments
 *               1:  Added code to process sga records
 *  
 * Programmers: Terry Beckhelm & Daniel Flyte (C# revision)
 * Description: This program assigns a devCode to a record from the SQL 
 *              database gpAll.  It includes methods and utilities to build 
 *              then bulk-load the Capacity table.
 * 
 * Database:    SQL Server Database 
 *              pila\sdgintdb.Forecast   new server name with instance
 *                    gis.lcGPALL
 *                    dbo.lcGPALLCopy is actually used for processing - direct copy of LCGPALL
 *              pila\sdgintdb.SR13
 *                   Capacity_init:: this is indexed by scenario and increment
 *                   default_sphere_parms
 *                   lu_check_messages                  
 */

using System;
using System.Data;
using System.Data.SqlTypes;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using System.Configuration;


namespace Sandag.TechSvcs.RegionalModels
{
    public class GPAll : System.Windows.Forms.Form
    {
      private delegate void WriteDelegate(string str);
      private Configuration config;
      private KeyValueConfigurationCollection appSettings;
      private ConnectionStringSettingsCollection connectionStrings;
      #region Fields

      #region Global Variables

      public string CAPACITY_TABLE;
      public string GPALL_TABLE;
      public string GPALL_OUTPUT_FILE;
      public string VERIFICATION_ERROR_OUTPUT_FILE;
      public string SFInfillInclusionsTable;
      public string SITESPEC;
      public string EMPDENSITIES;
      private string networkPath;

      #endregion

      private int devCode;
      private int recordsIn;
      private int scenarioID;
      private int debuglckey;
      public int infillDenCount;
      public int SFInfillInclusionsCount = 0;
      public bool useSFInfillInclusions;
      private bool debugstop;
      private int count5 = 0;
      private static int numVerificationErrors = 0;

      public int[] SFInfillInclusions = new int[47000];

      // Private Structure class instance variables.
      
      private Density[] newDensityMaster;
      private Density[] infillDensity;
      private lcpolygon lcp;
      private StreamWriter gpAllOutput;
      private StreamWriter verErrorsOutput;
      public static string[] luCheckMessages;
      private IContainer components;
      private System.Windows.Forms.ComboBox cboScenarioID;
      private System.Windows.Forms.Label lblScenarioIDSelect;
      private System.Windows.Forms.Button btnExit;
      private System.Data.SqlClient.SqlCommand sqlCommand;
      private System.Data.SqlClient.SqlConnection sqlConnection;
    
      private System.Windows.Forms.Button btnRun;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.MainMenu mainMenu1;
      private System.Windows.Forms.TextBox txtStatus;
      private Label label3;
      private CheckBox chkUseSFInclusions;
      private TextBox txtLckeyStop;
      #endregion Instance Variables

      #region Form Stuff
      protected override void Dispose(bool disposing)
      {
        if (disposing)
        {
          if (components != null)
          {
              components.Dispose();
          }
        }
        base.Dispose(disposing);
      }

      #region Windows Form Designer generated code
      private void InitializeComponent()
      {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GPAll));
            this.btnRun = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblScenarioIDSelect = new System.Windows.Forms.Label();
            this.sqlCommand = new System.Data.SqlClient.SqlCommand();
            this.sqlConnection = new System.Data.SqlClient.SqlConnection();
            this.cboScenarioID = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLckeyStop = new System.Windows.Forms.TextBox();
            this.chkUseSFInclusions = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnRun
            // 
            this.btnRun.BackColor = System.Drawing.Color.PaleGreen;
            this.btnRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRun.Location = new System.Drawing.Point(72, 324);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(72, 32);
            this.btnRun.TabIndex = 0;
            this.btnRun.Text = "Run";
            this.btnRun.UseVisualStyleBackColor = false;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.Crimson;
            this.btnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(160, 324);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(64, 32);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.exit_Click);
            // 
            // lblScenarioIDSelect
            // 
            this.lblScenarioIDSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScenarioIDSelect.Location = new System.Drawing.Point(24, 72);
            this.lblScenarioIDSelect.Name = "lblScenarioIDSelect";
            this.lblScenarioIDSelect.Size = new System.Drawing.Size(112, 16);
            this.lblScenarioIDSelect.TabIndex = 3;
            this.lblScenarioIDSelect.Text = "Scenario ID:";
            // 
            // sqlCommand
            // 
            this.sqlCommand.Connection = this.sqlConnection;
            // 
            // sqlConnection
            // 
            this.sqlConnection.FireInfoMessageEventOnUserErrors = false;
            // 
            // cboScenarioID
            // 
            this.cboScenarioID.Items.AddRange(new object[] {
            "0 - EP",
            "1 - SG",
            "2 - 02",
            "3 - 03",
            "4 - 04",
            "5 - 05"});
            this.cboScenarioID.Location = new System.Drawing.Point(144, 72);
            this.cboScenarioID.Name = "cboScenarioID";
            this.cboScenarioID.Size = new System.Drawing.Size(72, 21);
            this.cboScenarioID.TabIndex = 100;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Garamond", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Navy;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 32);
            this.label1.TabIndex = 104;
            this.label1.Text = "GPAll ";
            // 
            // txtStatus
            // 
            this.txtStatus.Location = new System.Drawing.Point(16, 228);
            this.txtStatus.Multiline = true;
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStatus.Size = new System.Drawing.Size(272, 80);
            this.txtStatus.TabIndex = 105;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(24, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(192, 24);
            this.label3.TabIndex = 8805;
            this.label3.Text = "Debug Stop on LCKEY";
            // 
            // txtLckeyStop
            // 
            this.txtLckeyStop.Location = new System.Drawing.Point(232, 106);
            this.txtLckeyStop.Name = "txtLckeyStop";
            this.txtLckeyStop.Size = new System.Drawing.Size(56, 20);
            this.txtLckeyStop.TabIndex = 8804;
            this.txtLckeyStop.Text = "0";
            this.txtLckeyStop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // chkUseSFInclusions
            // 
            this.chkUseSFInclusions.AutoSize = true;
            this.chkUseSFInclusions.Checked = true;
            this.chkUseSFInclusions.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseSFInclusions.Location = new System.Drawing.Point(28, 155);
            this.chkUseSFInclusions.Name = "chkUseSFInclusions";
            this.chkUseSFInclusions.Size = new System.Drawing.Size(150, 17);
            this.chkUseSFInclusions.TabIndex = 8806;
            this.chkUseSFInclusions.Text = "Use SD SF Infill Inclusions";
            this.chkUseSFInclusions.UseVisualStyleBackColor = true;
            // 
            // GPAll
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(370, 376);
            this.Controls.Add(this.chkUseSFInclusions);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtLckeyStop);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboScenarioID);
            this.Controls.Add(this.lblScenarioIDSelect);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnRun);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Menu = this.mainMenu1;
            this.Name = "GPAll";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GPAll Version 7 SR13 ";
            this.Load += new System.EventHandler(this.gpAll_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

      }
      #endregion
      [STAThread]
      private void gpAll_Load(object sender, System.EventArgs e)
      {
      }
      #endregion Form Stuff

      public GPAll()
      {
          InitializeComponent();
          this.cboScenarioID.SelectedIndex = 0;
          writeToStatus("Awaiting user input...");
      }

      /// <summary>
      /// The main entry point for the application
      /// </summary>
      public static void Main()
      {
        Application.Run(new GPAll());
      }

      /// <summary>
      /// Method processes input and begins extracting data from SQL table.
      /// </summary>

      /* Revision History
       * 
       * Date       By    Description
       * ------------------------------------------------------------------------
       * 08/15/96   tbe  Initial coding
       * 02/10/97   tbe  Rewritten for new psData ideas
       * 01/13/00   tbe  Changes for technical update
       * 10/26/00   tbe  Added calls for devCode assignment
       *                 checks.  MovedwriteCapacityInit() to main
       * 12/13/02   tbe  Changes for version 3 - satm
       * 03/07/03   tbe  Removed checking for sgaId > 0 to get to
       *                 sgaCheck
       * 05/03/03   tbe  Added checking for sgaId = pid for ep 
       *                 final runs
       * 06/17/03   dfl  C# revision of previous main()
       * 01/30/05   dfl  Changed for SR11 with Landcore
       * 02/03/09   tbe  Changes for Series 13 - rewrite gui
       * 11/30/10   tbe  changes for adding place holder for mil sitespec
       * ------------------------------------------------------------------------
       */
      private void btnRun_Click(object sender, System.EventArgs e)
      {
        // Process the input parameters.
        debuglckey = int.Parse(this.txtLckeyStop.Text.ToString());
        useSFInfillInclusions = chkUseSFInclusions.Checked;
        if (debuglckey > 0)
            debugstop = true;
   
        btnRun.Hide();
        btnRun.Enabled = false;
        try
        {
            config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None); 
            appSettings = config.AppSettings.Settings;
            connectionStrings = config.ConnectionStrings.ConnectionStrings;
        }
        catch (ConfigurationErrorsException c)
        {
            throw c;
        }
       
        try
        {
            networkPath= String.Format(appSettings["networkPath"].Value);
            CAPACITY_TABLE = String.Format(appSettings["capacityInitTableName"].Value);
            GPALL_TABLE = String.Format(appSettings["gpallTableName"].Value);
            SFInfillInclusionsTable = String.Format(appSettings["SFInfillInclusions"].Value);
            sqlConnection.ConnectionString = connectionStrings["forecastDBConnectionString"].ConnectionString;
            SITESPEC = String.Format(appSettings["sitespecRaw"].Value);
            EMPDENSITIES = String.Format(appSettings["empDensityDetail"].Value);
           
            GPALL_OUTPUT_FILE = networkPath + String.Format(appSettings["gpallOutputFileName"].Value);
            VERIFICATION_ERROR_OUTPUT_FILE = networkPath + String.Format(appSettings["verificationErrorsFileName"].Value);
        }
        catch (NullReferenceException n)
        {
            throw n;
        }
        
        //sqlConnection.ConnectionString = conn"data source=pila\\sdgintdb;initial catalog=forecast;integrated security=SSPI;persist security info=False;" +
         //                               "workstation id=tbe;packet size=4096";

        if (!processParams())
            return;
        MethodInvoker mi = new MethodInvoker(doGPAll);
        mi.BeginInvoke(null, null);
      }  // end btnRun_Click()

      //******************************************************************************

      /// <summary>
      /// Method processes input and begins extracting data from SQL table.
      /// </summary>
      private void doGPAll()
      {
        try
        {
            writeToStatus("updating SITESPEC for EMPDEN");
            UpdateSITESPECforEMPDEN();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.ToString());
        }
        try
        {
          writeToStatus("Extracting data...");
          extractAndProcessData(debugstop,debuglckey);
        }
        catch (Exception e)
        {
          MessageBox.Show(e.ToString());
        }
        finally
        {
          sqlConnection.Close();
        }

        // Bulk load gpall_update table with new values.
        try
        {
          gpAllOutput.Close();
          verErrorsOutput.Close();
          bulkLoadCapacityInit(scenarioID);
          File.Delete(GPALL_OUTPUT_FILE);   // Delete the ASCII file
          writeToStatus("Adjusting site spec table...");
          adjustSiteSpecTable();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.ToString());
        }
        finally
        {
          sqlConnection.Close();
        }

        writeToStatus("GPAll Finished Successfully!");
        
      }  // end doGPAll()

      //*********************************************************************************

      /// <summary>
      /// Method to extract records from GPALL table in order to begin processing 
      /// them.
      /// </summary>
      /* Revision History
       * 
       * STR             Date       By    Description
       * ------------------------------------------------------------------------
       *                 06/17/03   dfl   C# revision of prior C functions.
       *                 01/30/05   dfl   Updated for SR11 forecast with
       *                                  landcore.
       * ------------------------------------------------------------------------
       */
      private void extractAndProcessData(bool debugstop,int debuglckey)
      {
          int counter = 0;
        System.Data.SqlClient.SqlDataReader rdr;
        sqlCommand.CommandTimeout = 600;
        sqlConnection.ConnectionString = connectionStrings["forecastDBconnectionString"].ConnectionString;
        sqlCommand.CommandText= String.Format(appSettings["extractAndProcessDataQuery"].Value, GPALL_TABLE);
        //sqlCommand.CommandText = "SELECT LCKey, baselu, plu, planid, redevinf,siteid, phase, hs, mgra, pctConstrained, sphere, " +
        //                        "loden, hiden, acres, parcelAcres,empCiv, empMil FROM " + GPALL_TABLE;

        try
        {
          sqlConnection.Open();
          rdr = sqlCommand.ExecuteReader();
          int i = 0;
          while (rdr.Read())
          {
            lcp = new lcpolygon();
            lcp.LCKey = rdr.GetInt32(i++);
            if (!rdr.IsDBNull(i++))
              lcp.lu = rdr.GetInt32(i - 1);
            if (!rdr.IsDBNull(i++))
              lcp.plu = rdr.GetInt32(i - 1);
            if (!rdr.IsDBNull(i++))
              lcp.planid = (double)rdr.GetDecimal(i - 1);
            if (!rdr.IsDBNull(i++))
              lcp.redevInfill = rdr.GetInt32(i - 1);
            if (!rdr.IsDBNull(i++))
              lcp.siteID = rdr.GetInt16(i - 1);
            if (!rdr.IsDBNull(i++))
              lcp.phase = rdr.GetInt32(i - 1);

            if (lcp.phase != 2012)
                lcp.phase = 2012;

            if (!rdr.IsDBNull(i++))
              lcp.hs = rdr.GetInt32(i - 1);
            if (!rdr.IsDBNull(i++))
              lcp.mgra = (short)rdr.GetInt32(i - 1);
            if (!rdr.IsDBNull(i++))
              lcp.pctConstrained = rdr.GetInt32(i - 1);
            if (!rdr.IsDBNull(i++))
              lcp.sphere = rdr.GetInt32(i - 1);
            if (!rdr.IsDBNull(i++))
              lcp.lowDensity = (double)rdr.GetDecimal(i - 1);
            if (!rdr.IsDBNull(i++))
              lcp.highDensity = (double)rdr.GetDecimal(i - 1);
            if (!rdr.IsDBNull(i++))
              lcp.acres = (double)rdr.GetDecimal(i - 1);
            if (!rdr.IsDBNull(i++))
              lcp.parcel_acres = (double)rdr.GetDecimal(i - 1);
            if (!rdr.IsDBNull(i++))
              lcp.empCiv = (int)rdr.GetInt32(i - 1);
            if (!rdr.IsDBNull(i++))
              lcp.empMil = (int)rdr.GetInt32(i - 1);
            i = 0;

            if (++recordsIn % 25000 == 0)
              writeToStatus("Processed " + recordsIn + " records...");
            if (debugstop && debuglckey == lcp.LCKey)
              debugstop = false;
             
            processRecord(ref counter);
          }  // end while
          rdr.Close();
        }
        catch (Exception e)
        {
          MessageBox.Show(e.ToString(), "Runtime Exception");
          Close();
        }
        finally
        {
          sqlConnection.Close();
        } 
        
        recordsIn = 0;
      }  // end extractAndProcessData()

      //**************************************************************************************

      /// <summary>
      /// Method to adjust the siteSpecTable, decrementing the number of site
      /// spec units due to site spec parcels with units already on the ground.
      /// Creates a new copy of the site spec table to be used in Capacity.
      /// </summary>
      /* Revision History
       *
       * Date       By    Description
       * ------------------------------------------------------------------------
       * 08/09/05   dfl   Initial coding
       * ------------------------------------------------------------------------
       */
      private void adjustSiteSpecTable()
      {
        sqlConnection.ConnectionString = connectionStrings["SR13DBconnectionString"].ConnectionString;
          
        sqlCommand.CommandText = "execute createSiteSpecAdjusted"; 
        /* Execute the stored procedure that creates a copy of the sitespec table and updates the sfu and mfu to adjust for siteSpec'd
            locations with units that are already on the ground (built). */
        try
        {
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
        }     // end try

        catch (Exception e)
        {
            MessageBox.Show(e.ToString());
        }
        finally
        {
            sqlConnection.Close();
        }
      }  // end adjustSiteSpecTable()

      //**************************************************************************************
      // bulkLoadCapacityInit()

      /// <summary>
      /// Method to bulk load the gpAll table from sphere data.
      /// </summary>

      /* Revision History
       *
       * STR             Date       By    Description
       * ------------------------------------------------------------------------
       *                 02/10/97   tbe   Initial coding
       *                 11/13/01   tbe   New vars for version 2.0
       *                 12/13/02   tbe   Changes for version 3.00 - satm
       *                                  New directories for gpAll data
       *                 06/17/03   dfl   C# revision
       * ------------------------------------------------------------------------
      */
      private void bulkLoadCapacityInit(int scenarioID)
      {
        sqlConnection.ConnectionString = connectionStrings["SR13DBconnectionString"].ConnectionString;
        sqlCommand.Connection = sqlConnection;
        sqlCommand.CommandTimeout = 600;
        writeToStatus("Bulk loading " + CAPACITY_TABLE + "...");


        deleteCapacityInit(scenarioID);

        //sqlCommand.CommandText = "BULK INSERT " + CAPACITY_TABLE + " FROM '" + GPALL_OUTPUT_FILE + "' WITH " + "(fieldterminator = ',', firstrow = 1)";
        sqlCommand.CommandText = String.Format(appSettings["bulkInsertCapacityInitTable"].Value, CAPACITY_TABLE,GPALL_OUTPUT_FILE);
        try
        {
            sqlConnection.Open();
            sqlCommand.ExecuteNonQuery();
        
        }
        catch (Exception e)
        {
            MessageBox.Show(e.ToString());
        }
        finally
        {
            sqlConnection.Close();
        }

        sqlCommand.CommandText = String.Format(appSettings["updateCapacityInitTable"].Value, CAPACITY_TABLE);
        //sqlCommand.CommandText = "update " + CAPACITY_TABLE + " Set luz = x.luz from xref_mgra_SR13 x," + CAPACITY_TABLE + " c where x.mgra = c.mgra";
        try
        {
          sqlConnection.Open();
          sqlCommand.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            MessageBox.Show(e.ToString());
        }
        finally
        {
          sqlConnection.Close();
        }
      }  // end bulkLoadCapacityInit()


      //*********************************************************************************************

      // deleteCapacityInit()
      /* Revision History
       *
       * Date       By    Description
       * ------------------------------------------------------------------------
       * 08/09/05   dfl   Initial coding
       * ------------------------------------------------------------------------
       */

      private void deleteCapacityInit(int scenarioID)
      {
          sqlConnection.ConnectionString = connectionStrings["SR13DBconnectionString"].ConnectionString;
          sqlCommand.Connection = sqlConnection;
          sqlCommand.CommandTimeout = 600;
         
          sqlCommand.CommandText = String.Format(appSettings["deleteCapacityInitTable"].Value, CAPACITY_TABLE, scenarioID);
          
          try
          {
              sqlConnection.Open();
              sqlCommand.ExecuteNonQuery();
          }

          catch (Exception e)
          {
              MessageBox.Show(e.ToString());
          }
          finally
          {
              sqlConnection.Close();
          }
      }
      //**********************************************************************************************

      // getMiscParms()

      /// <summary>
      /// Method to load infill density lookup tables.
      /// </summary>

      /* Revision History
       * 
       * STR             Date       By    Description
       * ------------------------------------------------------------------------
       *                 08/15/96   tbe   Initial coding
       *                 11/13/01   tbe   Scrapped mixed use table and switch for 
       *                                   SGA
       *                 07/23/02   tbe   Added SGA phasing for scenario 4 (TP)
       *                 06/20/03   dfl   C# revision
       * ------------------------------------------------------------------------
       */
      private void getMiscParams(int scenarioID,ref int infillDenCount, ref int SFInfillInclusionsCount)
      {
        sqlConnection.ConnectionString = connectionStrings["SR13DBconnectionString"].ConnectionString;
       
        System.Data.SqlClient.SqlDataReader rdr;
        
        try
        {
          // Infill densities.
          sqlCommand.CommandText = String.Format(appSettings["extractDefaultSphereParmsQuery"].Value, scenarioID);
       
          sqlConnection.Open();
          rdr = sqlCommand.ExecuteReader();
          while (rdr.Read())
          {
            infillDensity[infillDenCount] = new Density();
            infillDensity[infillDenCount].sphere = rdr.GetInt16(0);
            infillDensity[infillDenCount].lowDensity = rdr.GetDouble(1);
            infillDensity[infillDenCount].highDensity = rdr.GetDouble(2);
            infillDensity[infillDenCount].sfovr = rdr.GetDouble(3);
            infillDenCount++;
          }
          rdr.Close();
        }     // end try
        catch (Exception e)
        {
          throw e;
        }

        finally
        {
            sqlConnection.Close();
        }

        sqlCommand.CommandText = String.Format(appSettings["extractLUCheckMessagesCountQuery"].Value);
        try
        {
          sqlConnection.Open();
          
          int numMessages = (int)sqlCommand.ExecuteScalar();
          luCheckMessages = new string[numMessages];

          numMessages = 0;
          sqlCommand.CommandText = String.Format(appSettings["extractLUCheckMessagesQuery"].Value);
      
          rdr = sqlCommand.ExecuteReader();
          while (rdr.Read())
            luCheckMessages[numMessages++] = rdr.GetString(1);
          rdr.Close();
        }
        catch (Exception e)
        {
            throw e;
        }
        finally
        {
            sqlConnection.Close();
        }

        try
        {
            // SF Infill Exclusions.
            sqlCommand.CommandText = String.Format(appSettings["selectAll"].Value, SFInfillInclusionsTable);

            sqlConnection.Open();
            rdr = sqlCommand.ExecuteReader();
            while (rdr.Read())
            {
                SFInfillInclusions[SFInfillInclusionsCount++] = rdr.GetInt32(0);
            }
            rdr.Close();
        }     // end try
        catch (Exception e)
        {
            throw e;
        }

        finally
        {
            sqlConnection.Close();
        }
      }  // end getMiscParams()

      //**********************************************************************************
      
      /// <summary>
      /// Method to process a record to determine its appropriate devCode.
      /// </summary>

      /* Revision History
       * 
       * Date       By    Description
       * ------------------------------------------------------------------------
       * 06/20/03   dfl   C# revision of prior C procedure(s).
       * 01/30/05   dfl   Updated for SR11 with Landcore. Removed sgaCheck and resetting SGA phase.
       * 10/14/09   tbe   added code to theoretically eliminate dev_code 17s - these are sitespec records with inconsistent data
       *                  such as plu <> lu and redevinf not set for redev or plu = lu and redevinf not set for infil
       *                  we have a list of lu codes that qualify as vacant or quasi vacant; the ones that are not in that list
       *                  have to hae a redevinf code set to avoid getting a code 17
       * 10/09/12   tbe   new devcode assignment logic - redevinf 1 and 2 are set in GIS.  Here, we will set 3 - 16
       *                  1. if redevinf = 1,2  devCode = 1,2 write record
       *                  2. if lu in vacant or quasi vacant list or site > 0 or plu in 4112,4118 set devCode = 3; write record
       *                  3. if lu = plu check infill; if (lu < 1200) devCode = 5; if (1200 <=lu < 1400) devCode = 6; otherwise devCode = 4;
       *                  4. if lu <> plu check for redev; redev varies by lu and plu see logic below           
       * ------------------------------------------------------------------------
       */
      private void processRecord(ref int counter)
      {
          int i;
          bool recordwritten = false;
          devCode = 0;
          try
          {
              if (lcp.redevInfill == 5)  // we don't allow any sf infill to come from landcore.  SF infill is only computed onplaces whre we allow it (inclusions table)
                  lcp.redevInfill = 0;

              // if a sitespec record is constrained - reset the redevinfill and pct constrained
              if (lcp.siteID > 0)
              {
                  if (lcp.redevInfill == 2)
                    lcp.redevInfill = 0;
                  lcp.pctConstrained = 0;
                 
              }   // end if

              if (lcp.pctConstrained == 100)
              {
                  lcp.redevInfill = 2;
                  devCode = 2;
              }  // end if

              if (lcp.redevInfill == 2 || lcp.redevInfill == 1)
              {
                  devCode = lcp.redevInfill;
                  if (devCode == 2)
                      lcp.pctConstrained = 100;
                  writeCapacityInit(gpAllOutput, lcp, devCode);
                  recordwritten = true;
              }  // end if

              else
              {
                  // Fix loDen and hiDen, if necessary.
                  if (lcp.lowDensity == 0 && lcp.highDensity == 0)
                  {
                      if (lcp.plu == 1100 || lcp.plu == 1110 || lcp.plu == 1120 || lcp.plu == 1300)
                      {
                          ++count5;  // keeping track of records with 0 densities
                          gpallUtils.setSFDefaultDensity(lcp, infillDensity, infillDenCount);
                      } // end if
                      else if (lcp.plu == 1000)
                          lcp.lowDensity = lcp.highDensity = 0.03;
                      else if (lcp.plu == 1200 || lcp.plu == 1280 || lcp.plu == 9700)
                          gpallUtils.setInfillDensities(lcp, infillDensity, infillDenCount);
                  }  // end if

                  // check for vacant
                  if (lcp.redevInfill == 3 || gpallUtils.vacantFilter(lcp))
                  {
                      if (lcp.plu == 9700)   // mixed use
                          devCode = 16;
                      else
                          devCode = 3;
                      writeCapacityInit(gpAllOutput, lcp, devCode);
                      recordwritten = true;
                  }  // end if

                  if (devCode == 0)
                  {
                      if (GPAllChecks.inInfill(lcp.lu))  //infill candidates
                      {
                          if (GPAllChecks.inEmployment(lcp.lu) && lcp.lu == lcp.plu)
                              devCode = 4;
                          else if (GPAllChecks.inSF(lcp.lu) && GPAllChecks.inSF(lcp.plu))
                          {
                              // check for SF infill exclusions - mark as developed if it's in the list - only if not sitespec
                              if (lcp.siteID == 0)   
                              {
                                  // see if the use SD SF infill inclusions is turned on - if so , set all included lckeys to have sf infill
                                  if (useSFInfillInclusions)
                                  {
                                      for (i = 0; i < SFInfillInclusionsCount; ++i)
                                      {
                                          if (lcp.LCKey == SFInfillInclusions[i] && lcp.siteID == 0)
                                          {
                                              if ((lcp.lu == 1110 && lcp.plu == 1110) || (lcp.lu == 1110 && lcp.plu == 1120) ||
                                                  (lcp.lu == 1120 && lcp.plu == 1120))
                                              {
                                                  lcp.redevInfill = 5;
                                                  devCode = 5;
                                                  lcp.plu = 1120;
                                                  break;
                                              }   // end if is this sf land 
                                         }  // end if
                                      }  // end for
                                  }  // end if
                              } // end if
                              if (devCode == 5)
                                  lcp.plu = 1120;
                          }  // end else if
                             
                          else if (GPAllChecks.inMF(lcp.lu) && GPAllChecks.inMF(lcp.plu))
                              devCode = 6;
                          if (devCode > 0)
                          {
                              writeCapacityInit(gpAllOutput, lcp, devCode);
                              recordwritten = true;
                          }  // end if
                      }  // end if (infill candidates
                  }  // end if

                  if (devCode == 0 && lcp.plu != lcp.lu)
                  {
                      if (lcp.plu == 9700)    // emp or res to mixed use
                          devCode = 15;
                      else if (GPAllChecks.inEmployment(lcp.lu) && GPAllChecks.inRoadOrFreeway(lcp.plu))  // emp to road
                          devCode = 14;
                      else if (lcp.lu < 1400 && GPAllChecks.inRoadOrFreeway(lcp.plu))  //res to road
                          devCode = 13;
                      else if (GPAllChecks.inEmployment(lcp.lu) && GPAllChecks.inEmployment(lcp.plu))   // emp redev
                          devCode = 12;
                      else if (GPAllChecks.inEmployment(lcp.lu) && lcp.plu < 1400)   // emp to res
                          devCode = 11;
                      else if (GPAllChecks.inAgriculture(lcp.lu))
                          devCode = 10;
                      else if (lcp.lu == 1300 && lcp.plu < 1300)   // MH to other res
                          devCode = 9;
                      else if (lcp.lu < 1200 && (lcp.plu >= 1200 && lcp.plu < 1300))  // SF to MF 
                          devCode = 8;
                      else if (lcp.lu < 1400 && GPAllChecks.inEmployment(lcp.plu))  // res to emp
                          devCode = 7;
                      if (devCode > 0)
                      {
                          writeCapacityInit(gpAllOutput, lcp, devCode);
                          recordwritten = true;
                      }  // end if
                  }  // end if

                  if (!recordwritten)
                  {
                      ++counter;
                      if (lcp.lu == 7603)
                      {
                          devCode = 2;
                          lcp.pctConstrained = 100;
                      }  // end if
                      else
                          devCode = 1;


                      writeCapacityInit(gpAllOutput, lcp, devCode);
                  }  //  end if
              }  // end else
          }  // end try
          catch (Exception e)
          {
              MessageBox.Show(e.ToString());
          }
      }  // end processRecord()

      //*********************************************************************************************************
      
      /// <summary>
      /// Method to process inputs from the form.
      /// </summary>

      /* Revision History
       * 
       * STR             Date       By    Description
       * ------------------------------------------------------------------------
       *                 08/15/96   tbe  Initial coding
       *                 02/10/97   tbe  Rewritten for new psData ideas
       *                 11/13/01   tbe  Changed for version 2.00
       *                 06/17/03   dfl  C# revision
       * ------------------------------------------------------------------------
       */
      private bool processParams()
      {
        if (cboScenarioID.SelectedIndex == -1)
        {
          MessageBox.Show("Invalid scenario! Please try again.");
          return false;
        }  // end if
        scenarioID = cboScenarioID.SelectedIndex;
          
        // Initialize some instance variables      
        infillDensity = new Density[500];
        newDensityMaster = new Density[500];

        // Try opening ASCII file for output.
        try
        {
          gpAllOutput = new StreamWriter(new FileStream(GPALL_OUTPUT_FILE, FileMode.Create));
          gpAllOutput.AutoFlush = true;
          verErrorsOutput = new StreamWriter(new FileStream(VERIFICATION_ERROR_OUTPUT_FILE, FileMode.Create));
          verErrorsOutput.AutoFlush = true;
        }
        catch (Exception e)
        {
          MessageBox.Show(e.ToString());
          return false;
        }
        // Extract miscellaneous params.
        try
        {
          getMiscParams(scenarioID,ref infillDenCount,ref SFInfillInclusionsCount);
        }
        catch (Exception e)
        {
          MessageBox.Show(e.ToString());
          return false;
        }
        return true;
      }  // end processParams()

      //*****************************************************************************************************

      // updateSITESPECforEMPDEN()
      // update the sitespec table for detailed emp densities
      /* Revision History
       * 
       * STR             Date       By    Description
       * ------------------------------------------------------------------------
       *                 05/22/13   tbe   Initial coding
       * ------------------------------------------------------------------------
       */
      private void UpdateSITESPECforEMPDEN()
      {
          int[] sites = new int[200];
          int num_sites = 0;
          int i, j, plu = 0, luz = 0, maxplu = 0;
          int[] dplu = new int[5];
          double empden = 0;
          System.Data.SqlClient.SqlDataReader rdr;

          try
          {
              // get siteID with sq ft and civemp = 0
              sqlCommand.CommandText = String.Format(appSettings["selectSITEID"].Value, SITESPEC);

              sqlConnection.Open();
              rdr = sqlCommand.ExecuteReader();
              while (rdr.Read())
              {
                  sites[num_sites] = rdr.GetInt16(0);
                  num_sites++;
              }
              rdr.Close();
          }     // end try
          catch (Exception e)
          {
              MessageBox.Show(e.ToString());
          }

          finally
          {
              sqlConnection.Close();
          }

          // now for each of these sites with sqft >0 and civemp = 0, we need to get the appropriate plu and then the associated emp den
          for (i = 0; i < num_sites; ++i)
          {
              writeToStatus("Updating SiteID = " + sites[i]);
              // get the list of luz and plu for this siteid
              try
              {
                  // get siteID with sq ft and civemp = 0
                  sqlCommand.CommandText = String.Format(appSettings["selectSiteRecords"].Value, CAPACITY_TABLE,sites[i]);

                  sqlConnection.Open();
                  rdr = sqlCommand.ExecuteReader();
                  maxplu = 0;
                  while (rdr.Read())
                  {
                      j = 0;
                      luz = rdr.GetInt16(0);
                      plu = rdr.GetInt16(1);
                      if (maxplu < plu)
                          maxplu = plu;
                    
                  }
                  rdr.Close();
              }     // end try
              catch (Exception e)
              {
                  MessageBox.Show(e.ToString());
              }

              finally
              {
                  sqlConnection.Close();
              }


              // use max plu to ensure we get mixed use covered if there are more than 1 plu
              // get associated empden from luz and plu table
              try
              {
                  // get siteID with sq ft and civemp = 0
                  sqlCommand.CommandText = String.Format(appSettings["selectEmpDen"].Value, EMPDENSITIES, luz,maxplu);

                  sqlConnection.Open();
                  rdr = sqlCommand.ExecuteReader();
                  empden = 1;  // set default if this guy isn't in table
                  while (rdr.Read())
                  {
                      empden = rdr.GetDouble(0);
                  }
                  rdr.Close();
              }     // end try
              catch (Exception e)
              {
                  MessageBox.Show(e.ToString());
              }

              finally
              {
                  sqlConnection.Close();
              }

              // now update sitespec table with this empden for this site 
              sqlCommand.CommandText = String.Format(appSettings["updateSITESPECForEmpDen"].Value, SITESPEC,empden,sites[i]);
              try
              {
                  sqlConnection.Open();
                  sqlCommand.ExecuteNonQuery();
              }     // end try
              catch (Exception e)
              {
                  MessageBox.Show(e.ToString());
              }

              finally
              {
                  sqlConnection.Close();
              }
          }  // end for

      } // end updateSITESPECforEMPDEN

      //*****************************************************************************************************

      /// <summary>
      /// Method to perform final checks to verify assignment.
      /// </summary>

      /* Revision History
       * 
       * STR             Date       By    Description
       * ------------------------------------------------------------------------
       *                 10/26/00   tbe   Initial coding
       *                 06/19/03   dfl   C# revision
       * ------------------------------------------------------------------------
       */
      private static int verifyChecks(lcpolygon lcp, int devCode)
      {
        int errorCode = 999;
        if (lcp.siteID > 0)
          errorCode = GPAllChecks.verifySiteSpec(lcp, devCode);
        if (errorCode == 999)
          errorCode = GPAllChecks.verifyDevCode(lcp, devCode);
        return errorCode;
      }  // end verifyChecks()

      //******************************************************************************************************
      
      /// <summary>
      /// Method to write gpAll return data to ASCII file.
      /// </summary>
      /* Revision History
        * 
        * STR             Date       By    Description
        * ------------------------------------------------------------------------
        *                 10/27/00   tbe   Initial coding
        *                 06/20/03   dfl   C# revision
        *                 01/30/05   dfl   Updated for SR11- all attributes of
        *                                  polygon now written to ASCII for
        *                                  writing into Capacity table. 
        * ------------------------------------------------------------------------
        */
      public void writeCapacityInit(StreamWriter sw, lcpolygon lcp, int devCode)
      {
        
        string line = "";
        // fix any bad sitespec records
        // Add devCode 17 for siteSpec records that are getting developed.
        if (devCode == 1 && lcp.siteID > 0)
        {
          if (lcp.redevInfill == 0 && lcp.lu == lcp.plu) // infill candidates
          {
            if (lcp.lu >= 1000 && lcp.lu < 1200) // sf infill
            {
              lcp.redevInfill = 5;
              devCode = 5;
            } // end if
            else if (lcp.lu >= 1200 && lcp.plu <= 1300)  // mf infill
            {
              lcp.redevInfill = 6;
              devCode = 6;
            }  // end else if
            else if (lcp.lu > 1400) // emp infill
            {
              lcp.redevInfill = 4;
              devCode = 4;
            } // end else if

          }   // end if
          else if (lcp.redevInfill == 0 && lcp.lu != lcp.plu)  // redev and other infill candidates
          {
            if (lcp.lu <= 1200 && lcp.plu <= 1200)   // sf infill with different lu
            {
              lcp.redevInfill = 5;
              devCode = 5;
            }  // end if
            else if (lcp.lu <= 1200 && (lcp.plu >= 1200 && lcp.plu <= 1300))  // sf - mf redev
            {
              lcp.redevInfill = 8;
              devCode = 8;
            }  // end if
            else if (lcp.lu == 1300 && (lcp.plu < 1300))  //mh to res redev
            {
              lcp.redevInfill = 9;
              devCode = 9;
            }  // end else if
            else if (lcp.lu <= 1300 && (lcp.plu >= 1500)) // res to emp redev
            {
              lcp.redevInfill = 7;
              devCode = 7;
            }  // end else if
            else if (lcp.lu >= 1500 && (lcp.plu <= 1300))  // emp to res redev
            {
              lcp.redevInfill = 11;
              devCode = 11;
            }  // end else if
            else if (lcp.lu >= 1500 && (lcp.plu >= 1500))  // emp to emp redev
            {
              lcp.redevInfill = 12;
              devCode = 12;
            }  // end else if
          }  // end else if
          else
            devCode = 17;
        }  // end fixing bad sitespec records

        int errorCode = verifyChecks(lcp, devCode);
        if (errorCode != 999)
        {
          writeVerificationError(verErrorsOutput, lcp, devCode, errorCode, luCheckMessages[errorCode]);
        }  // end if

        /* Constrain any parcel that is not developed or already constrained that is less than 0.014 acres (~600SF). */
        if (devCode > 2 && lcp.acres < 0.014)
          devCode = 2;

        if (lcp.owner == 41)
          lcp.military = 1;
        else
          lcp.military = 0;

        if (devCode == 2)
        {
          lcp.pctConstrained = 100;
          lcp.phase = 2012;
        }  // end if

        /* scenario, LCKey, planid,mgra, luz, 
        * sphere, site, 
        * dev_code, lu, plu, 
        * udm_emp_lu,udm_sf_lu, udm_mf_lu, phase, devyear, 
        * loden, hiden, 
        * empden, actden,acres, parcel_acres,effective_acres,percent_constrained, pcap_hs, pcap_emp, 
        * emp_civ, emp_mil, hs, 
        * hs_sf, hs_mf, hs_mh, 
        * gq_civ, gq_mil, 
        * cap_hs, cap_hs_sf, cap_hs_mf,cap_hs_mh, 
        * cap_emp_civ, chg_emp_civ, chg_hs_sf, chg_hs_mf, chg_hs_mh,
        * net_flag, mktstat, 
        * siteLU, siteSF, siteMF, siteMH, siteGQCiv, siteGQMil, siteEmp, siteMil
        */

        line  = scenarioID + "," + lcp.LCKey + "," + lcp.planid + "," + lcp.mgra + ",0," +
          lcp.sphere + "," + lcp.siteID + "," +
          devCode + "," + lcp.lu + "," + lcp.plu +
          ",0,0,0," + lcp.phase + ",0," +
          lcp.lowDensity + "," + lcp.highDensity +
          ",0,0," + lcp.acres + "," +lcp.parcel_acres + ",0," + lcp.pctConstrained + ",0,0," +
          lcp.empCiv + "," + lcp.empMil + "," + lcp.hs + "," +
          "0,0,0," +
          "0,0," +
          "0,0,0,0," +
          "0,0,0,0,0," +
          "0,0,0,0,0,0,0,0,0,0";
          
          try
          { 
              sw.WriteLine(line);
          }
          catch (Exception e)
          {
              MessageBox.Show(e.ToString());
              return;
          }
        
      }  // endwriteCapacityInit()

      //****************************************************************************
      
      /// <summary>
      /// Method to write gpAll return data to ASCII file.
      /// </summary>

      /* Revision History
       * 
       * STR             Date       By    Description
       * ------------------------------------------------------------------------
       *                 10/27/00   tbe   Initial coding
       *                 06/20/03   dfl   C# revision
       *                 01/30/05   dfl   Updated for SR11- all attributes of
       *                                  polygon now written to ASCII for
       *                                  writing into Capacity table. 
       * ------------------------------------------------------------------------
       */
      private void writeVerificationError(StreamWriter sw, lcpolygon lcp,int devCode, int ecode,string errorMessage)
      {
        if (++numVerificationErrors == 1)
        {
          string header = "LCKey,MGRA,sphere,pctConstrained,Site,Phase,LU,PLU, LoDen, HiDen,Acres,DevCode, HS, EMPCiv,Code,Error";
          sw.WriteLine(header);
        }
        sw.WriteLine(lcp.LCKey.ToString() + "," + lcp.mgra.ToString() + "," +lcp.sphere +"," + lcp.pctConstrained + "," +lcp.siteID.ToString() + "," +
            lcp.phase.ToString() + "," + lcp.lu.ToString() + "," + lcp.plu.ToString() + "," +
            lcp.lowDensity.ToString() + "," + lcp.highDensity.ToString() + "," + lcp.acres.ToString() + "," + 
            devCode.ToString() + "," + lcp.hs.ToString() + "," + lcp.empCiv + "," + ecode.ToString() + "," + errorMessage);
      }  // end writeVerificationError()

      //***************************************************************************************

      /// <summary>
      /// Append a string on a new line to the status box.
      /// </summary>
      public void writeToStatus(string status)
      {
        /* If we are running this method from primary thread, no marshalling is
         * needed. */
        if (!txtStatus.InvokeRequired)
        {
          // Append to the string (whose last character is already newLine)
          txtStatus.Text += status + Environment.NewLine;

          // Move the caret to the end of the string, and then scroll to it.
          txtStatus.Select(txtStatus.Text.Length, 0);
          txtStatus.ScrollToCaret();
          Refresh();
        }     // End if

        /* Invoked from another thread.  Show progress asynchronously via a new
         * delegate. */
        else
        {
          WriteDelegate write = new WriteDelegate(writeToStatus);
          Invoke(write, new object[] { status });
        }  // end else
      }  // end writeToStatus()

      //*******************************************************************************************

      private void exit_Click(object sender, System.EventArgs e)
      {
      Application.Exit();
    }

      private void mnuExit_Click(object sender, System.EventArgs e)
      {
          Application.Exit();
      }

  }
}