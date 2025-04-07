USE [GardenStore]
GO
UPDATE Sales
SET StatusId = 4
WHERE StatusId = 3
AND DATEDIFF(DAY, SaleDate, GETDATE()) > 14;