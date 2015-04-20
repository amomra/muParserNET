# muParserNET library #

This library is a .NET wrapper for the math parser library `muParser` (http://muparser.beltoforion.de/). It was written in C++ CLI for Windows 32 or 64 bits and tested in those systems. It wasn't tested with Mono.

This project has no dependences with other libraries except `muParser`, which is included in the solution. The `muParser` is compiled as a static library and linked with this library. The library targeted framework is the .NET 4.0. This project uses the `muParser` version 2.2.3 and its class library.

## Installation ##

The package is available to download at NuGet repository. It contains the 32 and 64 bits versions of the package.

- https://www.nuget.org/packages/muParserNET.Win-x86/
- https://www.nuget.org/packages/muParserNET.Win-x64/

The solution can be also downloaded and compiled with Visual Studio 2013.

## Documentation ##

The library implements most functionalities of muParser library without changes in the API.

The library lacks supports for the following functionalities:

- Implicit variable creation.

There may have more features that are not supported.

The documentation is available at the project [wiki](https://github.com/amomra/muParserNET/wiki).