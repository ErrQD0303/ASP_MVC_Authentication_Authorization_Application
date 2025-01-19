# Simple Authentication and Authorization App With ASP.NET MVC

## Tech Stack:

- Database: SQLServer or JsonFile
- ASP.NET MVC

## To change Database, go into Program.cs, change `builder.AddUserRepository(UserRepository.Database);` to `builder.AddUserRepository(UserRepository.JsonFile);` to change from default database `SQLServer` to `JsonFile`
