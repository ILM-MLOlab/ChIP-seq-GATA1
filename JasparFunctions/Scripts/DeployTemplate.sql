-- JASPAR functions
IF EXISTS (SELECT * FROM sys.objects WHERE [name] = 'JASPARScanSequence' AND type_desc = 'CLR_TABLE_VALUED_FUNCTION')
	DROP FUNCTION dbo.[JASPARScanSequence]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE [name] = 'JASPARScanVariant' AND type_desc = 'CLR_TABLE_VALUED_FUNCTION')
	DROP FUNCTION dbo.[JASPARScanVariant]
GO

-- Assembly
IF EXISTS (SELECT * FROM sys.assemblies WHERE [name] = 'Erythrogene')
	DROP ASSEMBLY Erythrogene
GO