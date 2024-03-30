using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class addsp2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string SpInsertStudent = @"CREATE or ALTER  PROCEDURE dbo.SpInsertStudent 
    @Name nvarchar(50),
    @Address nvarchar(250),
	@ContactNo nvarchar(max),
	@StudentImage nvarchar(max),
	@Class int,
	@Regular bit
AS
   
INSERT INTO [dbo].[Students]
           ([Name]
           ,[Address]
           ,[ContactNo]
           ,[StudentImage]
           ,[Class]
           ,[Regular])
     VALUES
           (@Name, @Address,  @ContactNo,@StudentImage,	@Class,@Regular)
		   RETURN @@identity 
                            Go ";

            migrationBuilder.Sql(SpInsertStudent);

            string SpInsertStudentMarks = @"CREATE or ALTER  PROCEDURE dbo.SpInsertStudentMarks  
	@SubjectId int,
    @ObtainedNumber int,
    @StartDate datetime2(7),
    @StudentId int

	as

INSERT INTO [dbo].[StudentMarks]
           ([SubjectId]
           ,[ObtainedNumber]
           ,[StartDate]
            ,[StudentId]                      )
     VALUES
           (@SubjectId , @ObtainedNumber, @StartDate,@StudentId )

		   return @@rowcount
GO";

            migrationBuilder.Sql(SpInsertStudentMarks
                );
            string SpUpdateStudent = @"CREATE or ALTER  PROCEDURE dbo.SpUpdateStudent  
    @Name nvarchar(50),
    @Address nvarchar(250),
	@ContactNo nvarchar(max),
	@StudentImage nvarchar(max),
	@Class int,
	@Regular bit,
    @StudentId int
As
UPDATE [dbo].[Students]
       
            SET [Name]= @Name
           ,[Address]=  @Address
           ,[ContactNo]=@ContactNo
           ,[StudentImage]=@StudentImage
           ,[Class]=@Class
           ,[Regular]=@Regular
	  where ID =@StudentId

	  delete from StudentMarks where   StudentId =   @StudentId

	  return @@rowcount
GO";

            migrationBuilder.Sql(SpUpdateStudent);
            string SpDeleteStudent = @"CREATE or ALTER PROCEDURE [dbo].[SpDeleteStudent] 
    @StudentId int
AS
	  delete from StudentMarks where StudentId = @StudentId
	   delete from [Students] where ID = @StudentId
	  return @@rowcount
GO";

            migrationBuilder.Sql(SpDeleteStudent);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("drop proc SpInsertStudent");
            migrationBuilder.Sql("drop proc SpInsertStudentMarks");
            migrationBuilder.Sql("drop proc SpUpdateStudent");
            migrationBuilder.Sql("drop proc SpDeleteStudent");
        }
    }
}
