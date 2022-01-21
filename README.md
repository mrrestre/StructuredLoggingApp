# Structured Logging App
## _Bachelor Thesis project_

![.NET Build](https://github.com/mrrestre/StructuredLoggingApp/actions/workflows/dotnet.yml/badge.svg)


The goal of this project is to test out some different technologies (Basically Frameworks) which enable the use of semantic logging on .NET applications.  
The application is beeing implemented and the result is going to be used to compare the performance and the features from two of  the most used Logging-Frameworks in .NET applications: 
- [Serilog](https://github.com/serilog/serilog)
- [NLog](https://github.com/NLog/NLog)

## Facts
- The application works a CLI and it parses arguments
- All the configurations are done through `appsettings.json` (Multiple configurations available)
- The application send the generated Logs to 3 different servers (Elastic Slack, Graylog and Seq)
- The application should work as an example for a complete Logging Eviroment and best practice

## Tech
Like mentioned before the used technologies are:
- .NET 5.0 for the application
- All libraries are compatible with .NET standard 2.0
- [Command Line Parser](https://github.com/commandlineparser/commandline)
- Serilog with multiple sinks
- NLog with multiple targets

## Installation
The project was developed in Visual Studio 2019. You have mainly two options to run the application.
1) Run `TestAppWithSerilog.exe` or `TestAppWithNLog.exe` in a shell. The diferent commands and options can be accesed though the shell with option `--help`.
2) Open the project with Visual Studio. A couple of different run where defined in the `launchSettings.json` of the application. Only choose the wanted function and click "Play". (Note: with this variant changing the parameter and options for the application involves changing the mentioned JSON file).

Either way the application is going going to generate a log-file locally on `C:\Temp\TestDocuments\log.json`.

If the functionality with the "Log-Server" wants to be checked out, you are going to need Docker. The installation of the different enviroments with Docker / Docker-compose can be found on the folder `DockerContainer`. The right sinks should be manually configured. To do this, search for the right configuration on the Folder `TestSettings` and copy the content from the wanted Json to `appsetting.json`.

## License

MIT
