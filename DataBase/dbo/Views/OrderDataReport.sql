CREATE VIEW [dbo].[OrderDataReport]
AS 

SELECT		dbo.SYS_TBL_Item.Name, 
			dbo.SYS_TBL_Item.Cost,			
			dbo.SYS_TBL_Members.FirstName, 
			dbo.SYS_TBL_Members.LastName,
			dbo.SYS_TBL_LendingItem.OrderDate, 
			dbo.SYS_TBL_OrderStatus.StatusName,
			dbo.SYS_TBL_OrderStatus.ID AS StatusID,  
			dbo.SYS_TBL_LendingItem.RatingRequestUnit, 
			dbo.SYS_TBL_LendingItem.RatingLendingUnit, 
			dbo.SYS_TBL_DistanceUnit.Distance,
			dbo.SYS_TBL_LendingItem.ProblemDescriptionRequestUnit, 
			dbo.SYS_TBL_LendingItem.ProblemDescriptionLendingUnit,
			
			RequstingUnit.RequestingUnitCurrentWheelQuantity,
			RequstingUnit.RequestingUnitName,

			LendingUnit.LendingUnitCurrentWheelQuantity,
			LendingUnit.LendingUnitName,

			dbo.SYS_TBL_LendingItem.UnitRequestsID,
			dbo.SYS_TBL_LendingItem.UnitLendingID,
			dbo.SYS_TBL_LendingItem.UpdatedOn,
			dbo.SYS_TBL_LendingItem.UpdatedBy,
			dbo.SYS_TBL_LendingItem.CreatedOn,
			dbo.SYS_TBL_LendingItem.CreatedBy,
			dbo.SYS_TBL_LendingItem.Disable

FROM        dbo.SYS_TBL_LendingItem 

INNER JOIN  dbo.SYS_TBL_Item			ON dbo.SYS_TBL_Item.ID = dbo.SYS_TBL_LendingItem.ItemID 
INNER JOIN	dbo.SYS_TBL_Members			ON dbo.SYS_TBL_LendingItem.MemberID = dbo.SYS_TBL_Members.ID 
INNER JOIN  dbo.SYS_TBL_OrderStatus		ON dbo.SYS_TBL_LendingItem.OrderStatusID = dbo.SYS_TBL_OrderStatus.ID 
INNER JOIN  dbo.SYS_TBL_DistanceUnit	ON dbo.SYS_TBL_DistanceUnit.FirstUnitID = dbo.SYS_TBL_LendingItem.UnitRequestsID and dbo.SYS_TBL_DistanceUnit.SecondUnitID = dbo.SYS_TBL_LendingItem.UnitLendingID

LEFT JOIN (SELECT  SYS_TBL_Unit.ID,
					SYS_TBL_Unit.UnitName as RequestingUnitName, 
					SYS_TBL_Unit.CurrentWheelQuantity AS RequestingUnitCurrentWheelQuantity
			FROM dbo.SYS_TBL_Unit Where Disable=0 ) AS RequstingUnit
			on RequstingUnit.ID = dbo.SYS_TBL_LendingItem.UnitRequestsID

LEFT JOIN (SELECT	SYS_TBL_Unit.ID,SYS_TBL_Unit.UnitName as LendingUnitName, 
					SYS_TBL_Unit.CurrentWheelQuantity AS LendingUnitCurrentWheelQuantity
			FROM dbo.SYS_TBL_Unit Where Disable=0) AS LendingUnit
			ON LendingUnit.ID = dbo.SYS_TBL_LendingItem.UnitLendingID

where dbo.SYS_TBL_LendingItem.Disable = 0
