[![Build Status](https://myopensource.visualstudio.com/_apis/public/build/definitions/8ccd03bc-2148-4a6a-ade6-0beda21510d0/1/badge)](https://github.com/moriazat/BloomSales)

# BloomSales Inc.

BloomSales Inc. is a fictive retail company which also has stores. 
This project consists of its underlying services and its online store. 

The whole system is written as a *showcase only and nothing more*. 

You can learn more about the project, its design and architecture [here](https://moriazat.wordpress.com/bloomsales).

## Getting Started

These instructions will get you a copy of the project up and running on your local machine.

### Prerequisities

The project is implemented using Microsoft .NET Framework 4.5.
Therefore, the following items are at least needed for the system to work:
* .NET Framework 4.5
* SQL Server LocalDB 

### Compilation

The project is comprised of the following three separte categories of projects:
* BloomSales Services
* BloomSales Services Hosts
* BloomSales Web

Services Hosts and Web projects depend on BloomSales Services. Thus, in order to resolve 
their dependencies, Services projects should be compiled in *Release* mode first, which copies the 
output to *Libraries* folder for the others to use. 

## Running the System

After compiling the projects successfully, in order to run the system or integration tests, 
you need to provide a *"connections.config"* file along with the App.config. This file contains
all the connection strings needed for the databases used by the services. You can write the 
settings yourself or you can use the default connections.config. In order to use the default
connections.config file, run *connections.bat* script residing in the root director. This script
will generate the default connections.config file and copies that to the appropriate folders 
for integration tests and service hosts projects.

The next step is to run the desktop app which hosts the services. The project resides in the following folder:
```
\Service Hosts\Source\BloomSales.Hosts.Windows
```

Run the application and start the services. 

The final step is to run the ASP.NET project. 

## Running the tests

The system has both unit tests and integration tests.

### Unit Tests

There are over a hundred unit tests written for Services. These tests can be run independently, or all together.

### Integration Test

In addition to unit tests, Services also have integration tests. Theses tests should be run
independently one at a time for each service, and not all together for all services.

## Authors

* **Mohammad Riazat** - [moriazat](https://github.com/moriazat)

See also the list of [contributors](https://github.com/your/project/contributors) who participated in this project.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Acknowledgments

[Bootswatch Spacelab](http://bootswatch.com/spacelab/) is used as the base bootstrap theme for the 
BloomSales Web Store project. 
