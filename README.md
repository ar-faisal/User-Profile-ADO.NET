Tables And Stored Procedures Required For the Project

#######  Query For Creating Tables  #########


1>>>
CREATE TABLE Roles (
    RoleId INT PRIMARY KEY,
    RoleName NVARCHAR(50) NOT NULL
);


2>>>
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Password NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    PhoneNumber NVARCHAR(15) NOT NULL,
    Gender NVARCHAR(10),
    State NVARCHAR(100),
    City NVARCHAR(100),
    ImageUrl NVARCHAR(255),
    ResumeUrl NVARCHAR(255),
    RoleId INT NOT NULL,
    CONSTRAINT FK_Role FOREIGN KEY (RoleId) REFERENCES Roles(RoleId),
    CONSTRAINT UQ_Email UNIQUE (Email)
);



#######  Query For adding stored procedure  #########


1>>>
CREATE PROCEDURE AddUser
    @FirstName NVARCHAR(100),
    @LastName NVARCHAR(100),
    @Password NVARCHAR(100),
    @Email NVARCHAR(255),
    @PhoneNumber NVARCHAR(15),
    @Gender NVARCHAR(10) = NULL,
    @State NVARCHAR(100) = NULL,
    @City NVARCHAR(100) = NULL,
    @ImageUrl NVARCHAR(255) = NULL,
    @ResumeUrl NVARCHAR(255) = NULL,
    @RoleId INT
AS
BEGIN  
    INSERT INTO Users (
        FirstName,
        LastName,
        Password,
        Email,
        PhoneNumber,
        Gender,
        State,
        City,
        ImageUrl,
        ResumeUrl,
        RoleId
    )
    VALUES (
        @FirstName,
        @LastName,
        @Password,
        @Email,
        @PhoneNumber,
        @Gender,
        @State,
        @City,
        @ImageUrl,
        @ResumeUrl,
        @RoleId
    );
END;



2>>>
CREATE PROCEDURE AuthenticateUser
    @Email NVARCHAR(255),
    @Password NVARCHAR(100)
AS
BEGIN
   
    SELECT UserId, FirstName, RoleId
    FROM Users
    WHERE Email = @Email AND Password = @Password;
END;



3>>>
CREATE PROCEDURE UpdateUser
    @userId INT,
    @firstname NVARCHAR(255),
    @lastname NVARCHAR(255),    
    @Phonenumber NVARCHAR(20),
    @gender NVARCHAR(10),
    @state NVARCHAR(255),
    @city NVARCHAR(255),
    @imageUrl NVARCHAR(255),
    @resumeUrl NVARCHAR(255)
AS
BEGIN
    UPDATE Users
    SET
        FirstName = @firstname,
        LastName = @lastname,
        PhoneNumber = @Phonenumber,
        Gender = @gender,
        State = @state,
        City = @city,
        ImageUrl = @imageUrl,
        ResumeUrl = @resumeUrl
    WHERE
        UserId = @userId;
    
END



4>>>
CREATE PROCEDURE DeleteUser
    @userId INT
AS
BEGIN
    DELETE FROM Users
    WHERE UserId = @userId;
END

