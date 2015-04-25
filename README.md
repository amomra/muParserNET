# muParserNET library #

This library is a .NET wrapper for the math parser library `muParser` (http://muparser.beltoforion.de/). It was written in C# for Windows 32 or 64 bits and tested in those systems. It wasn't tested with Mono.

This project has no dependences with other libraries except `muParser`, which is included in the solution. The `muParser` is compiled as a dynamic library and linked with this library at runtime according the platform (32 or 64 bits). The library targeted framework is the .NET 4.0.

This project uses the `muParser` version 2.2.4 and its DLL interface. This interface was modified to include some functions that are available by the `Parser` class but not by the DLL interface. The `muParserNET` is compatible with the original API but some functions are not supported.

## Installation ##

The package is available to download at NuGet repository.

- https://www.nuget.org/packages/muParserNET.Win/

The solution can be also downloaded and compiled with Visual Studio 2013.

## Documentation ##

The library implements most functionalities of `muParser` class library with little changes in the API.

The library lacks supports for the following functionalities:

- Implicit variable creation.

There may have more features that are not supported.

The documentation is available at the project [wiki](https://github.com/amomra/muParserNET/wiki).