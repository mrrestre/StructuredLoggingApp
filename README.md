# Structured Logging App
## _Bachelor Thesis project_

![.NET Build](https://github.com/mrrestre/StructuredLoggingApp/actions/workflows/dotnet.yml/badge.svg)

The goal of this project is to test out different technologies (Basically Frameworks) which enable the use of semantic logging on .NET applications.  
The application is being implemented and the result is going to be used to compare the performance and the features from two of  the most used Logging-Frameworks in .NET applications: 
- [Serilog](https://github.com/serilog/serilog)
- [NLog](https://github.com/NLog/NLog)

Both applications (each one implemented with a different framework) should be able to send log-messages to three different endpoints:
- [Seq](https://datalust.co/seq)
- [Graylog](https://www.graylog.org/products/open-source)
- [Elastic Stack](https://www.elastic.co/elastic-stack/)

## Facts
- The application works a CLI and it parses arguments
- The writing configurations are done through either `appsettings.json` or `nlog.config` (Multiple configurations available)
- The application should work as an example for a complete Logging Environment and show best practices when logging

## Involved technologies
Like mentioned before the used technologies are:
- .NET 5.0 for the application
- All libraries are compatible with .NET standard 2.0
- [Command Line Parser](https://github.com/commandlineparser/commandline)
- Serilog with multiple sinks
- NLog with multiple targets

## How does it work
The project was developed in Visual Studio 2019. You have mainly two options to run both applications.
1) Run `TestAppWithSerilog.exe` or `TestAppWithNLog.exe` in a shell. The diferent commands and options can be accessed though the shell with the option `--help`. Within a given verb (e.g. `single`) the `--help` option may be called as well. 
```sh
PS C:\[someFolder]> ./TestAppWithSerilog.exe --help
PS C:\[someFolder]> ./TestAppWithSerilog.exe single --help
```
2) Open the project with Visual Studio. A couple of different run where defined in the `launchSettings.json` of each application. Choose the wanted application and the wanted function within and click "Play". (Note: with this variant changing the parameter and options for the application involves changing the mentioned JSON file).

Either way the application is going to generate a log-file locally on `C:\Temp\TestDocuments\log.json`.

If the functionality with the "Log-Server" wants to be checked out, you are going to need Docker. The installation of the different environments with Docker / Docker-compose can be found on the folder `DockerContainer`. The right sinks should be manually configured. To do this, search for the right configuration on the Folder `TestSettings` and copy the content from the wanted configuration to either `appsetting.json` (Serilog) or `nlog.config` (NLog).

## License

MIT
