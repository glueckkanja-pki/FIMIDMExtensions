# FIMIDMExtensions
A Rules Extension library for Microsoft Forefront Identity Manager (FIM) 2010 R2 -- Identity Management, allowing different filters and transformation on values

## GKFIMIDMExtensions ##

A rules extension used in the MIIS program of FIM. It can be used as an MV extension as well as a rules extension for a Management Agent (MA). It depends on Microsoft.MetadirectoryServicesEx.dll included in FIM and is tested with version 4.1.3508.0 of the DLL.

There are multiple configuration files to set up the behaviour of the rules extension. These files must be copied next to the dll in the extension directory of FIM. If a specific MA or the MV use another configuration, it has to be put in the MA- or MV-specific directory.

* **gkfimexteions.log4net.config**: A standard [log4net](https://logging.apache.org/log4net/) configuration file, used for logging.
* **GKMVExtension.xml**: Determines which objects are provisioned to which MAs. Includes value transformations.
* **GKFIMIDMExtensions.xml**: First, determines which object types are deleted and which are disconnected. Second, defines converters that can be accessed in the FlowRuleName of an import-mapping of the MA the configuration is used for.

## GKFIMIDMExtensionsTest ##

This is an NUnit v2 test project. 

## Tools ##

[clearFimRunLog.ps1](tools/clearFimRunLog.ps1) is a Powershell script that clears the log of runs in the MIIS part of FIM. It exports the logs to disk before clearing.

## Licenses
FIMIDMExtensions is available under the [AGPL](LICENSE). 

FIMIDMExtensions depends on [log4net](https://logging.apache.org/log4net/), which is available under the [Apache License, Version 2.0](https://logging.apache.org/log4net/license.html).

The tests in the subproject GKFIMIDMExtensionsTest use [NUnit](http://www.nunit.org/), which is released under the [Expat license](http://www.nunit.org/nuget/nunit3-license.txt).