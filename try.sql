
ALTER PROCEDURE [dbo].[DATA_MASKING_COLUMN_LEVEL]
(
	@database_name NVARCHAR(MAX),
    @table_name NVARCHAR(MAX),
    @json_input NVARCHAR(MAX)
)
	
AS
BEGIN
	
    DECLARE @sql NVARCHAR(MAX)
    DECLARE @column_name NVARCHAR(MAX)
    DECLARE @masking_function NVARCHAR(MAX)
    DECLARE @is_nullable BIT


	CREATE TABLE #TempTable (
    column_name NVARCHAR(MAX),
    masking_function NVARCHAR(MAX),
    is_nullable BIT
	)

    -- Parse the JSON input
    INSERT INTO #TempTable (column_name, masking_function, is_nullable)
    SELECT column_name, masking_function, is_nullable
    FROM OPENJSON(@json_input)
    WITH (
        column_name NVARCHAR(MAX) '$.column_name',
        masking_function NVARCHAR(MAX) '$.masking_function',
        is_nullable BIT '$.is_nullable'
    );

    -- Cursor to iterate through columns
    DECLARE @column_cursor CURSOR
    SET @column_cursor = CURSOR FOR
    SELECT column_name, masking_function, is_nullable
    FROM #TempTable

    OPEN @column_cursor

    FETCH NEXT FROM @column_cursor INTO @column_name, @masking_function, @is_nullable

    -- Loop through columns
    WHILE @@FETCH_STATUS = 0
    BEGIN
        -- Construct the dynamic SQL statement based on parameters
        SET @sql = 'ALTER TABLE ' + @database_name + '.' + @table_name +
                   ' ALTER COLUMN ' + @column_name + ' ADD MASKED WITH (FUNCTION = ' + @masking_function;

        -- Check if column is nullable and adjust the SQL query accordingly
        IF @is_nullable = 1
			BEGIN
				SET @sql = @sql  + ') NULL';
			END
		ELSE
			BEGIN
				SET @sql = @sql  + ') NOT NULL';
			END


        -- Execute the dynamic SQL
        EXEC sp_executesql @sql

        FETCH NEXT FROM @column_cursor INTO @column_name, @masking_function, @is_nullable
    END

    CLOSE @column_cursor
    DEALLOCATE @column_cursor
END


CREATE USER TestUser WITHOUT LOGIN;

EXECUTE AS USER = 'TestUser';

GRANT SELECT TO TestUser;

SELECT * FROM TBLEMPMST;

USE [try_me]
GO

-- tried

DECLARE	@return_value int

EXEC	@return_value = [dbo].[DATA_MASKING_COLUMN_LEVEL]
		@database_name = N'try_me',
		@table_name = N'TBLEMPMST',
		@json_input = N'[{"column_name": "Nam", "masking_function": "default", "is_nullable": 1}, {"column_name": "Age", "masking_function": "default", "is_nullable": 0}]'

SELECT	'Return Value' = @return_value

GO

ALTER DATABASE try_me SET COMPATIBILITY_LEVEL = 140;


