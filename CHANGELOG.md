# Changelog

All notable changes to this project will be documented in this file.  
The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/).

## 0.0.1-preview.4 - 2020-01-12 <a name="0.0.1-preview.4"> </a>

Added

* *Module ASP.NET Core:* Creates the __ASP.NET Core MVC Model Binder__ and __Model Binder Provider__ for the semantic type, only if the project has a reference to the assembly `Microsoft.AspNetCore.Mvc`
  * The __Model Binder Provider__ has to be registered in the ``StartUp``.
