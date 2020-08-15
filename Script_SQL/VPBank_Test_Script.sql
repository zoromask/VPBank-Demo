
-- Create Database
Create database VPBank_Test;

---- Create table 
CREATE TABLE [dbo].[ItemInfo](
    [Item_ID] int IDENTITY(1,1),  
    [Item_Code] [nvarchar] (50) ,
    [Item_Name] [nvarchar](150) NOT NULL,
    [Description] [nvarchar](Max) NOT NULL,
	[Deleted] [int]
 CONSTRAINT [PK_History] PRIMARY KEY CLUSTERED 
(
    [Item_ID] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]


---- Insert Item  

DECLARE @Count int = 1;
DECLARE @Count_Item int = 1;
DECLARE @Count_Max int = 1000;
WHILE @Count < 200
BEGIN
	BEGIN TRAN
		
		WHILE @Count_Item < @Count_Max
		BEGIN
			INSERT INTO ItemInfo(Item_Code,Item_Name,Description,Deleted)
			SELECT N'Item_'+ CAST(@Count_Item as nvarchar), N'Item No '+ CAST(@Count_Item as nvarchar),N'Đây là sản phẩm số ' + CAST(@Count_Item as nvarchar), 0;
			
			SET @Count_Item = @Count_Item + 1;
		END
	COMMIT;
	SET @Count_Max = @Count_Max + 1000;
	SET @Count = @Count + 1;
END
