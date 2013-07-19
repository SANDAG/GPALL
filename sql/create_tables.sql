CREATE TABLE [LCGPALLCOPY](
	[OBJECTID] [int] NOT NULL,
	[LCKey] [int] NOT NULL,
	[baselu] [int] NULL,
	[plu] [int] NULL,
	[planId] [numeric](38, 8) NULL,
	[reDevInf] [int] NULL,
	[siteid] [smallint] NULL,
	[phase] [int] NULL,
	[baseown] [int] NULL,
	[hs] [int] NULL,
	[mgra] [int] NULL,
	[parcelId] [int] NULL,
	[scenid] [smallint] NULL,
	[sphere] [int] NULL,
	[Loden] [numeric](38, 8) NULL,
	[Hiden] [numeric](38, 8) NULL,
	[pctConstrained] [int] NULL,
	[acres] [numeric](38, 8) NULL,
	[parcelAcres] [numeric](38, 8) NULL,
	[empCiv] [int] NULL,
	[empMil] [int] NULL,
 CONSTRAINT [PK_LCGPALLCOPY_4] PRIMARY KEY CLUSTERED 
(
	[LCKey] ASC
))
GO

CREATE TABLE [SITESPEC](
	[OBJECTID] [int] NOT NULL,
	[SITEID] [smallint] NOT NULL,
	[SITENAME] [varchar](200) NULL,
	[SQFT] [int] NULL,
	[EMPDEN] [decimal](5, 1) NULL,
	[CIVEMP] [int] NULL,
	[MILEMP] [int] NULL,
	[SFU] [smallint] NULL,
	[MFU] [smallint] NULL,
	[MHU] [smallint] NULL,
	[CIVGQ] [int] NULL,
	[MILGQ] [int] NULL,
	[SOURCE] [varchar](200) NULL,
	[INFODATE] [datetime] NULL,
	[phase] [smallint] NULL,
 CONSTRAINT [PK_SITESPEC] PRIMARY KEY CLUSTERED 
(
	[SITEID] ASC
))
GO

CREATE TABLE [SFInfill_inclusions](
	[lckey] [int] NOT NULL,
	[zone_code] [varchar](30) NULL,
	[lu] [smallint] NULL,
	[plu] [smallint] NULL,
 CONSTRAINT [PK_SFInfill_inclusions] PRIMARY KEY CLUSTERED 
(
	[lckey] ASC
))
GO

CREATE TABLE [lu_check_messages](
	[msg_id] [tinyint] NULL,
	[msg_text] [varchar](75) NULL
)
GO
SET ANSI_PADDING OFF

CREATE TABLE [emp_densities_detail](
	[LUZ] [int] NOT NULL,
	[plu] [int] NOT NULL,
	[emp] [int] NULL,
	[floorspace] [int] NULL,
	[acres] [int] NULL,
	[sfPerEmp] [float] NULL,
	[density] [float] NULL,
	[empPer1000SqFt] [float] NULL
)
GO

CREATE TABLE [default_sphere_parms](
	[scenario] [smallint] NOT NULL,
	[sphere] [smallint] NOT NULL,
	[dpointvl] [float] NULL,
	[dpointrd] [float] NULL,
	[net_flag] [tinyint] NULL,
	[rdloden] [float] NULL,
	[rdhiden] [float] NULL,
	[sfovr] [float] NULL,
 CONSTRAINT [PK_default_sphere_parms] PRIMARY KEY CLUSTERED 
(
	[scenario] ASC,
	[sphere] ASC
))
GO

CREATE TABLE [capacity_init](
	[scenario] [tinyint] NULL,
	[LCKey] [int] NOT NULL,
	[planid] [float] NULL,
	[mgra] [smallint] NOT NULL,
	[luz] [smallint] NOT NULL,
	[sphere] [smallint] NOT NULL,
	[site] [smallint] NOT NULL,
	[dev_code] [tinyint] NOT NULL,
	[lu] [smallint] NOT NULL,
	[plu] [smallint] NOT NULL,
	[udm_emp_lu] [tinyint] NOT NULL,
	[udm_sf_lu] [tinyint] NOT NULL,
	[udm_mf_lu] [tinyint] NOT NULL,
	[phase] [smallint] NOT NULL,
	[devyear] [smallint] NOT NULL,
	[loden] [float] NOT NULL,
	[hiden] [float] NOT NULL,
	[empden] [float] NOT NULL,
	[actden] [float] NOT NULL,
	[acres] [float] NOT NULL,
	[parcel_acres] [float] NULL,
	[effective_acres] [float] NULL,
	[percent_constrained] [float] NULL,
	[pcap_hs] [float] NOT NULL,
	[pcap_emp] [float] NULL,
	[emp_civ] [int] NOT NULL,
	[emp_mil] [int] NULL,
	[hs] [int] NOT NULL,
	[hs_sf] [int] NOT NULL,
	[hs_mf] [int] NOT NULL,
	[hs_mh] [int] NOT NULL,
	[gq_civ] [int] NOT NULL,
	[gq_mil] [int] NOT NULL,
	[cap_hs] [int] NOT NULL,
	[cap_hs_sf] [int] NOT NULL,
	[cap_hs_mf] [int] NOT NULL,
	[cap_hs_mh] [int] NOT NULL,
	[cap_emp_civ] [int] NOT NULL,
	[chg_emp_civ] [int] NOT NULL,
	[chg_hs_sf] [int] NOT NULL,
	[chg_hs_mf] [int] NOT NULL,
	[chg_hs_mh] [int] NOT NULL,
	[net_flag] [tinyint] NULL,
	[mktstat] [int] NULL,
	[siteLU] [smallint] NULL,
	[siteSF] [int] NULL,
	[siteMF] [int] NULL,
	[siteMH] [int] NULL,
	[siteGQCiv] [int] NULL,
	[siteGQMil] [int] NULL,
	[siteEmp] [int] NULL,
	[siteMil] [int] NULL,
 CONSTRAINT [PK_capacity_init] PRIMARY KEY CLUSTERED 
(
	[LCKey] ASC
))
GO