Mauro Torres	2024/04/24

I wrote this program to save disk space. You pass it a comma delimited list of directories and it just compresses each file individually. 
This can then be called from a scheduled job. This is meant for a team that works with a lot of files that need to be compressed. 

It also keeps a log of everything it did. The log is configured in the app.config file.

I first had the configuration of directories configured in a table named Master.Appsettings, which is what the class ConfigurationRepository is for, 
but I now call the class "ConfigurationWrapper" which just uses the -d command line option passed to it. I will later finish supporting either just the "-d" option or configuration in a table.

The purpose of using the library StructureMap was for me to learn dependency injection. 

Example Usage:
FileCompressionUtility.exe -d "C:\Users\torre\source\CompressFiles"
