USE [VPBank_Test]
GO
/****** Object:  UserDefinedFunction [dbo].[splitstring]    Script Date: 15/08/2020 11:05:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[splitstring] ( @stringToSplit VARCHAR(MAX) )
RETURNS
 @returnList TABLE ([Name] [nvarchar] (500))
AS
BEGIN

 DECLARE @name NVARCHAR(255)
 DECLARE @pos INT

 WHILE CHARINDEX(',', @stringToSplit) > 0
 BEGIN
  SELECT @pos  = CHARINDEX(',', @stringToSplit)  
  SELECT @name = SUBSTRING(@stringToSplit, 1, @pos-1)

  INSERT INTO @returnList 
  SELECT @name

  SELECT @stringToSplit = SUBSTRING(@stringToSplit, @pos+1, LEN(@stringToSplit)-@pos)
 END

 INSERT INTO @returnList
 SELECT @stringToSplit

 RETURN
END



GO
/****** Object:  StoredProcedure [dbo].[proc_CountItemInCart]    Script Date: 15/08/2020 11:05:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[proc_CountItemInCart]
	-- Add the parameters for the stored procedure here
	@p_return int output 
AS
BEGIN
	SELECT @p_return= COUNT(*) from ItemInfo where IsInCart = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[proc_ItemInCart]    Script Date: 15/08/2020 11:05:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[proc_ItemInCart]
	-- Add the parameters for the stored procedure here
	@p_Item_Id int, 
	@p_IsInCart int,
	@p_return int output 
AS
BEGIN
	BEGIN
		UPDATE ItemInfo set
			IsInCart = @p_IsInCart
		Where Item_ID = @p_Item_Id;
		SELECT @p_return = @p_Item_Id;
	END
END
GO
/****** Object:  StoredProcedure [dbo].[proc_ItemInCartIdLst]    Script Date: 15/08/2020 11:05:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		ThangNT
-- Create date: 14/08/2020
-- Description:	Load Item List ID item in cart
-- =============================================
CREATE PROCEDURE [dbo].[proc_ItemInCartIdLst]
	
AS
BEGIN
	SELECT Item_ID from ItemInfo where IsInCart = 1;
END
GO
/****** Object:  StoredProcedure [dbo].[proc_ItemLoad]    Script Date: 15/08/2020 11:05:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		ThangNT
-- Create date: 14/08/2020
-- Description:	Load Item List From @p_begin To @p_end 
-- =============================================
CREATE PROCEDURE [dbo].[proc_ItemLoad]
	-- Add the parameters for the stored procedure here
	@p_itemStatus int,
	@p_begin int,
	@p_end int
AS
	Declare @v_sql nvarchar(MAX);
	Declare @v_condition nvarchar(MAX) ;
BEGIN
	SET @v_condition = '';
	IF @p_itemStatus > 0
		BEGIN
			SET @v_condition =' And IsInCart = ' + CAST(@p_itemStatus as nvarchar);
		END
	SET @v_sql = 'SELECT x.* FROM (SELECT ROW_NUMBER() OVER(order by a.Item_ID) AS STT,a.* FROM  (select * from ItemInfo) a ) x
				  WHERE x.STT >=' + CAST(@p_begin as nvarchar) + 'and x.STT <=' + CAST(@p_end as nvarchar) + @v_condition;
	EXEC (@v_sql);

END
GO
