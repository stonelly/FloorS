
/****** Object:  UserDefinedFunction [dbo].[UFN_DOT_GetAllSummary]    Script Date: 23/6/2021 11:50:21 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Amir
-- Create date: 23/06/2021
-- Description:	select all record from summary tables
-- =============================================
ALTER FUNCTION [dbo].[UFN_DOT_GetAllSummary]
(	
)
RETURNS TABLE 
AS
RETURN
SELECT * FROM (

SELECT 
      [CreationTime] as ConsolidationDateAndTime
	  ,PostingDateTime
      ,[Id]
      ,[ItemNumber]
      ,ItemSize as Configuration
      ,null as BatchWeight
      ,null as QCType
      ,null as [RAFGoodQty]
      --,null as Shift
      ,null as [Warehouse]
      ,null as [Resource]
      ,null as [RejectedQuantity]
      ,null as [SecondGradeQuantity]
      ,null as [RejectedSampleQuantity]
      ,Null as HBBatchNumber
      ,Null as RAFHBSample
      ,Null as RAFVTSample
      ,Null as RAFWTSample
      ,Null as SampleWarehouse
      ,Null as VTBatchNumber
      ,Null as WTBatchNumber
      ,[PickListJournalId]
      ,[RAFJournalId]
      ,[RouteCardJournalId]
      ,[BatchOrderNumber]
      ,[MovementJournalId]
      ,[Location]
      ,[Quantity]
      ,[BaseQuantity]
      ,null as [RouteCategory]
      ,[OperationNo]
      ,null as [SecondGradeWarehouse]
      ,[CustomerReference]
	  ,D365BatchNumber
	  ,D365Parameter
      ,[ModuleSequence]
      ,[SubSequence]
      ,[ErrorMessage]
      ,[FSIdentifier]
      ,[FunctionIdentifier]
      ,[ProcessingStatus]
      ,Null as ResourceGroup
      ,[SalesOrderNumber]
      --,Null as InternalReferenceNumber
      ,Null as TenPcsWt
      --,Null as OldBatchQty
      --,Null as ReferenceItemNumber
      --,Null as RefNumberOfPieces1
      --,Null as RefNumberOfPieces2
      --,Null as RefNumberOfPieces3
      --,Null as RWKReturnMsg
	  ,Null as isRWKDeleted
      ,Null as OriRWKNum
      ,Null as DeliveryDate
      ,Null as Pool
      --,Null as ReworkOrder
      ,Null as Formula
      ,Null as ScanInDateTime
      ,Null as ScanOutDateTime
      ,Null as TransferJournalId
      ,Null as IsOrignalTemppack
      ,Null as FGBatchOrderNumber
      ,[GloveSampleQuantity]
      ,[BatchNumber]
      ,[GloveSize]
      ,[ItemSize]
      ,[Preshipment]
      ,[PreshipmentCases]
	  ,[OriginalPlantNo]
      ,[PlantNo]
	  ,null as SampleCPDConsumptionWeight
	  ,RecordCount
	  ,0 as ParentSrc
  FROM [DOT_FGSumTable] with (nolock) WHERE IsDeleted = 0

UNION

Select
	CreationTime as ConsolidationDateAndTime,
	PostingDateTime,
	Id,
	ItemNumber,
	Configuration,
	Null as BatchWeight,
	Null as QCType,
	Null as RAFGoodQty,
	--Shift,
	Warehouse,
	Null as Resource,
	Null as RejectedQuantity,
	Null as SecondGradeQuantity,
	Null as RejectedSampleQuantity,
	Null as HBBatchNumber,
	Null as RAFHBSample,
	Null as RAFVTSample,
	Null as RAFWTSample,
	Null as SampleWarehouse,
	Null as VTBatchNumber,
	Null as WTBatchNumber,
	Null as PickListJournalId,
	Null as RAFJournalId,
	Null as RouteCardJournalId,
	Null as BatchOrderNumber,
	MovementJournalId,
	Location,
	Quantity,		
    Null as BaseQuantity,
	Null as RouteCategory,
	Null as OperationNo,
	Null as SecondGradeWarehouse,
	Null as CustomerReference,
	D365BatchNumber,
	D365Parameter,
	ModuleSequence,
	SubSequence,
	ErrorMessage,
	FSIdentifier,
	FunctionIdentifier,
	ProcessingStatus, 
	ResourceGroup,
	Null as SalesOrderNumber,
	--Null as InternalReferenceNumber,
	Null as TenPcsWt,
	--Null as OldBatchQty,
	--Null as ReferenceItemNumber,
	--Null as RefNumberOfPieces1,
	--Null as RefNumberOfPieces2,
	--Null as RefNumberOfPieces3,
	--Null as RWKReturnMsg,
	Null as isRWKDeleted,
	Null as OriRWKNum,
	Null as DeliveryDate,
	Null as Pool,
	--Null as ReworkOrder,
	Null as Formula,
	Null as ScanInDateTime,
	Null as ScanOutDateTime,
	Null as TransferJournalId,
	Null as IsOrignalTemppack,
	Null as FGBatchOrderNumber   
	,null as GloveSampleQuantity
    ,null as BatchNumber
    ,null as GloveSize
    ,null as ItemSize
    ,null as Preshipment
    ,null as PreshipmentCases
	,[OriginalPlantNo]
    ,[PlantNo]
	,null as SampleCPDConsumptionWeight
	,null as RecordCount
	,1 as ParentSrc
from DOT_MovementSumTable  with (nolock) WHERE IsDeleted = 0
UNION
SELECT
	CreationTime as ConsolidationDateAndTime,
	PostingDateTime,
	Id,
	ItemNumber,
	Configuration,
	Null as BatchWeight,
	QCType,
	Null as RAFGoodQty,
	--Null as  Shift,
	Warehouse,
	Null as Resource,
	Null as RejectedQuantity,
	Null as SecondGradeQuantity,
	Null as RejectedSampleQuantity,
	Null as HBBatchNumber,
	Null as RAFHBSample,
	Null as RAFVTSample,
	Null as RAFWTSample,
	Null as SampleWarehouse,
	Null as VTBatchNumber,
	Null as WTBatchNumber,
	PickListJournalId,
	Null as RAFJournalId,
	Null as RouteCardJournalId,
	PSIReworkOrderNo as BatchOrderNumber,
	Null as  MovementJournalId,
	Location,
	Null as Quantity,
    Null as BaseQuantity,
	Null as RouteCategory,
	Null as OperationNo,
	Null as SecondGradeWarehouse,
	Null as CustomerReference,
	D365BatchNumber,
	D365Parameter,
	ModuleSequence,
	SubSequence,
	ErrorMessage,
	FSIdentifier,
	FunctionIdentifier,
	ProcessingStatus,
	Null as ResourceGroup,
	SalesOrderNumber,
	--InternalReferenceNumber,
	TenPcsWt,
	--OldBatchQty,
	--ReferenceItemNumber,
	--RefNumberOfPieces1,
	--RefNumberOfPieces2,
	--RefNumberOfPieces3,
	--Null as RWKReturnMsg,
	Null as isRWKDeleted,
	Null as OriRWKNum,
	Null as DeliveryDate,
	Null as Pool,
	--Null as ReworkOrder,
	Null as Formula,
	Null as ScanInDateTime,
	Null as ScanOutDateTime,
	Null as TransferJournalId,
	Null as IsOrignalTemppack,
	Null as FGBatchOrderNumber	
	,null as GloveSampleQuantity
    ,null as BatchNumber
    ,null as GloveSize
    ,null as ItemSize
    ,null as Preshipment
    ,null as PreshipmentCases
	,[OriginalPlantNo]
    ,[PlantNo]
	,null as SampleCPDConsumptionWeight
	,null as RecordCount
	,2 as ParentSrc
FROM DOT_PickingSumTable with (nolock) WHERE IsDeleted = 0
UNION
SELECT
	CreationTime as ConsolidationDateAndTime,
	PostingDateTime,
	Id,
	ItemNumber,
	ItemSize as Configuration,
	BatchWeight,
	Null as QCType,
	RAFGoodQty,
	--Null as  Shift,
	Warehouse,
	Null as Resource,
	Null as RejectedQuantity,
	Null as SecondGradeQuantity,
	Null as RejectedSampleQuantity,
	Null as HBBatchNumber,
	RAFHBSample,
	RAFVTSample,
	RAFWTSample,
	SampleWarehouse,
	VTBatchNumber,
	WTBatchNumber,
	PickListJournalId,
	RAFJournalId,
	RouteCardJournalId,
	BatchOrderNumber,
	MovementJournalId,
	Location,
	Null as Quantity,
    Null as BaseQuantity,
	Null as RouteCategory,
	Null as OperationNo,
	Null as SecondGradeWarehouse,
	Null as CustomerReference,
	D365BatchNumber,
	D365Parameter,
	ModuleSequence,
	SubSequence,
	ErrorMessage,
	FSIdentifier,
	FunctionIdentifier,
	ProcessingStatus,
	Null as ResourceGroup,
	Null as SalesOrderNumber,
	--Null as InternalReferenceNumber,
	Null as TenPcsWt,
	--Null as OldBatchQty,
	--Null as ReferenceItemNumber,
	--Null as RefNumberOfPieces1,
	--Null as RefNumberOfPieces2,
	--Null as RefNumberOfPieces3,
	--Null as RWKReturnMsg,
	Null as isRWKDeleted,
	Null as OriRWKNum,
	Null as DeliveryDate,
	Null as Pool,
	--Null as ReworkOrder,
	Null as Formula,
	Null as ScanInDateTime,
	Null as ScanOutDateTime,
	Null as TransferJournalId,
	Null as IsOrignalTemppack,
	Null as FGBatchOrderNumber
	,null as GloveSampleQuantity
    ,null as BatchNumber
    ,null as GloveSize
    ,ItemSize
    ,null as Preshipment
    ,null as PreshipmentCases
	,[OriginalPlantNo]
    ,[PlantNo]
	,SampleCPDConsumptionWeight
	,RecordCount
	,3 as ParentSrc
FROM DOT_RafSumTable with (nolock) WHERE IsDeleted = 0
UNION
SELECT
	CreationTime as ConsolidationDateAndTime,
	PostingDateTime,
	Id,
	ItemNumber,
	Configuration,
	BatchWeight,
	QCType,
	RAFGoodQty,
	--Null as  Shift,
	Warehouse,
	Resource,
	RejectedQuantity,
	SecondGradeQuantity,
	RejectedSampleQuantity,
	HBBatchNumber,
	RAFHBSample,
	RAFVTSample,
	RAFWTSample,
	SampleWarehouse,
	VTBatchNumber,
	WTBatchNumber,
	PickListJournalId,
	RAFJournalId,
	RouteCardJournalId,
	ReworkOrder as BatchOrderNumber,
	MovementJournalId,
	Location,
	Quantity,
    Null as BaseQuantity,
	RouteCategory,
	OperationNo,
	SecondGradeWarehouse,
	BatchNumber as CustomerReference,
	D365BatchNumber,
	D365Parameter,
	ModuleSequence,
	SubSequence,
	ErrorMessage,
	FSIdentifier,
	FunctionIdentifier,
	ProcessingStatus,
	Null as ResourceGroup,
	Null as SalesOrderNumber,
	--Null as InternalReferenceNumber,
	Weightof10Pcs as TenPcsWt,
	--Null as OldBatchQty,
	--Null as ReferenceItemNumber,
	--Null as RefNumberOfPieces1,
	--Null as RefNumberOfPieces2,
	--Null as RefNumberOfPieces3,
	--RWKReturnMsg, -- 
	isRWKDeleted, --
	OriRWKNum,
	DeliveryDate,
	Pool,
	Null as Formula,
	Null as ScanInDateTime,
	Null as ScanOutDateTime,
	Null as TransferJournalId,
	Null as IsOrignalTemppack,
	Null as FGBatchOrderNumber,
	null as GloveSampleQuantity,
    BatchNumber
    ,null as GloveSize
    ,null as ItemSize
    ,null as Preshipment
    ,null as PreshipmentCases
	,[OriginalPlantNo]
    ,[PlantNo]
	,null as SampleCPDConsumptionWeight
	,null as RecordCount
	,4 as ParentSrc
FROM DOT_RwkcrSumTable with (nolock) WHERE IsDeleted = 0
UNION
SELECT
	CreationTime as ConsolidationDateAndTime,
	PostingDateTime,
	Id,
	ItemNumber,
	Configuration,
	Null as BatchWeight,
	Null as QCType,
	Null as RAFGoodQty,
	--Null as  Shift,
	Warehouse,
	Null as Resource,
	Null as RejectedQuantity,
	Null as SecondGradeQuantity,
	Null as RejectedSampleQuantity,
	Null as HBBatchNumber,
	Null as RAFHBSample,
	Null as RAFVTSample,
	Null as RAFWTSample,
	Null as SampleWarehouse,
	Null as VTBatchNumber,
	Null as WTBatchNumber,
	Null as PickListJournalId,
	RAFJournalId,
	Null as RouteCardJournalId,
	BatchOrderNumber,
	MovementJournalId,
	Location,
	Quantity,
    Null as BaseQuantity,
	Null as RouteCategory,
	Null as OperationNo,
	Null as SecondGradeWarehouse,
	BatchNumber as CustomerReference,
	D365BatchNumber,
	D365Parameter,
	ModuleSequence,
	SubSequence,
	ErrorMessage,
	FSIdentifier,
	FunctionIdentifier,
	ProcessingStatus,
	Null as ResourceGroup,
	Null as SalesOrderNumber,
	--Null as InternalReferenceNumber,
	Null as TenPcsWt,
	--Null as OldBatchQty,
	--ReferenceItemNumber,
	--Null as RefNumberOfPieces1,
	--Null as RefNumberOfPieces2,
	--Null as RefNumberOfPieces3,
	--Null as RWKReturnMsg,
	Null as isRWKDeleted,
	Null as OriRWKNum,
	Null as DeliveryDate,
	Null as Pool,
	--Null as ReworkOrder,
	Formula,
	PostingDateTime as ScanInDateTime,
	ScanOutDateTime,
	TransferJournalId,
	IsOrignalTemppack,
	FGBatchOrderNumber
	,null as GloveSampleQuantity
    ,BatchNumber
    ,null as GloveSize
    ,null as ItemSize
    ,null as Preshipment
    ,null as PreshipmentCases
	,[OriginalPlantNo]
    ,[PlantNo]
	,null as SampleCPDConsumptionWeight
	,null as RecordCount
	,5 as ParentSrc
FROM DOT_TransferSumTable with (nolock) WHERE IsDeleted = 0
UNION
SELECT
	CreationTime as ConsolidationDateAndTime,
	PostingDateTime,
	Id,
	ChangedItemNumber as ItemNumber,
	ItemSize as Configuration,
	BatchWeight,
	Null as QCType,
	RAFGoodQty,
	--Null as  Shift,
	Warehouse,
	Null as Resource,
	Null as RejectedQuantity,
	Null as SecondGradeQuantity,
	Null as RejectedSampleQuantity,
	Null as HBBatchNumber,
	RAFHBSample,
	RAFVTSample,
	RAFWTSample,
	SampleWarehouse,
	VTBatchNumber,
	WTBatchNumber,
	PickListJournalId,
	RAFJournalId,
	RouteCardJournalId,
	BatchOrderNumber,
	MovementJournalId,
	Location,
	Null as Quantity,
    Null as BaseQuantity,
	Null as RouteCategory,
	Null as OperationNo,
	Null as SecondGradeWarehouse,
	BatchNumber as CustomerReference,
	D365BatchNumber,
	D365Parameter,
	ModuleSequence,
	SubSequence,
	ErrorMessage,
	FSIdentifier,
	FunctionIdentifier,
	ProcessingStatus,
	Null as ResourceGroup,
	Null as SalesOrderNumber,
	--Null as InternalReferenceNumber,
	Null as TenPcsWt,
	--Null as OldBatchQty,
	--Null as ReferenceItemNumber,
	--Null as RefNumberOfPieces1,
	--Null as RefNumberOfPieces2,
	--Null as RefNumberOfPieces3,
	--Null as RWKReturnMsg,
	Null as isRWKDeleted,
	Null as OriRWKNum,
	Null as DeliveryDate,
	Null as Pool,
	--Null as ReworkOrder,
	Null as Formula,
	Null as ScanInDateTime,
	Null as ScanOutDateTime,
	Null as TransferJournalId,
	Null as IsOrignalTemppack,
	Null as FGBatchOrderNumber
	,null as GloveSampleQuantity
    ,BatchNumber
    ,null as GloveSize
    ,ItemSize
    ,null as Preshipment
    ,null as PreshipmentCases
	,[OriginalPlantNo]
    ,[PlantNo]
	,0 as SampleCPDConsumptionWeight
	,RecordCount
	,6 as ParentSrc
FROM DOT_ChangeGloveSumTable with (nolock) WHERE IsDeleted = 0
) a
