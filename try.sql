USE [try_me]
GO
/****** Object:  StoredProcedure [dbo].[DATA_MASKING_COLUMN_LEVEL]    Script Date: 29/08/2023 23:32:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[DATA_MASKING_COLUMN_LEVEL]
(
    @table_name NVARCHAR(MAX),
    @json_input NVARCHAR(MAX)
)
	
AS
BEGIN
	
    DECLARE @sql NVARCHAR(MAX)
    DECLARE @column_name NVARCHAR(MAX)
    DECLARE @masking_function NVARCHAR(MAX)


	CREATE TABLE #TempTable (
    column_name NVARCHAR(MAX),
    masking_function NVARCHAR(MAX)    
	)

    -- Parse the JSON input
    INSERT INTO #TempTable (column_name, masking_function)
    SELECT column_name, masking_function
    FROM OPENJSON(@json_input)
    WITH (
        column_name NVARCHAR(MAX) '$.column_name',
        masking_function NVARCHAR(MAX) '$.masking_function'     
    );

    -- Cursor to iterate through columns
    DECLARE @column_cursor CURSOR
    SET @column_cursor = CURSOR FOR
    SELECT column_name, masking_function
    FROM #TempTable

    OPEN @column_cursor

    FETCH NEXT FROM @column_cursor INTO @column_name, @masking_function

    -- Loop through columns
    WHILE @@FETCH_STATUS = 0
    BEGIN
				
        -- Construct the dynamic SQL statement based on parameters
        SET @sql = 'ALTER TABLE ' + QUOTENAME(@table_name) +
                   ' ALTER COLUMN ' + QUOTENAME(@column_name) + ' ADD MASKED WITH (FUNCTION = '+ QUOTENAME(@masking_function, '''') + ')'; 

		print(@sql);
				           
        EXEC sp_executesql @sql

        FETCH NEXT FROM @column_cursor INTO @column_name, @masking_function
    END

    CLOSE @column_cursor
    DEALLOCATE @column_cursor
END


EXECUTE	[dbo].[DATA_MASKING_COLUMN_LEVEL]
		@table_name = N'TBLEMPMST',
		@json_input = N'[{"column_name": "Name", "masking_function": "default()"}, {"column_name": "Age", "masking_function": "default()"}]'


ALTER DATABASE try_me SET COMPATIBILITY_LEVEL = 140;
