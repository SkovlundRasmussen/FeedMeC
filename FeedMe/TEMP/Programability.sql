CREATE PROCEDURE SP_newUser @firstname VARCHAR(50), @lastname VARCHAR(50), @email VARCHAR(50), @password VARCHAR(100), 
							@city VARCHAR(50), @postal_code VARCHAR(50), @street_name VARCHAR(50), @street_number VARCHAR(50)
AS
  BEGIN TRAN
    IF EXISTS (SELECT * FROM Users WHERE email = @email)
      BEGIN
        RAISERROR('There is already a user with this email', 16, -1);
        ROLLBACK TRAN
        RETURN
      END
    ELSE
      BEGIN
        INSERT INTO Users(firstname, lastname, email, password, role_id)
        VALUES (@firstname, @lastname, @email, @password, 1);
		SELECT SCOPE_IDENTITY();
		INSERT INTO CustomerInfo(street_name, street_number, postal_code, city, user_id)
        VALUES (@street_name, @street_number, @postal_code, @city, @@IDENTITY);
        COMMIT
      END
  GO


CREATE PROCEDURE [dbo].[getRole]
  
  AS
   SELECT * FROM UsersRole 
GO