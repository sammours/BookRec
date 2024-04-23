# BookRec
The aim of this study is to explain recommendation systems and the three major approaches: Content-Based, Collaborative and Hybrid filtering, and to develop a project to predict books based on them. The study explains the main differences between the three approaches and points out some of the issues that each one of them is facing. It also analyses some training sets to provide a reasonable solution for each issue. 

# Run the app
1. Clone the repository
2. Open the solution using [Visual Studio](https://visualstudio.microsoft.com/downloads/) (Visual Studio 2019 is recommended)
      
3. Create a new database using [Sql Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15) (SSMS version 18 is recommended)
    and add the connection string to the [environment variables](https://www.onmsft.com/how-to/how-to-set-an-environment-variable-in-windows-10) with name: 
      ```
      BookRec:DatabaseConnectionString
      ```

4. Restart visual studio
5. Open Package Manager Console and set the default project to src/Infrastructure.EntityFramework and then run the following command:
      > update-database
6. Add the data to your database by running the data [script.sql](https://github.com/samysammour/BookRec/tree/master/docs)
7. Set BookRec project as a [startup project](https://blogs.msdn.microsoft.com/zainnab/2010/05/09/choosing-the-startup-project/)
8. Run the application by pressing F5

# Reuse
This project is marked as an MIT. Please, feel free to use, modify and republish the project.

# Documentation
The complete documentation can be found under the link:
      [Documentation.pdf](https://github.com/samysammour/BookRec/tree/master/docs)
