# Casbin.AspNetCore

[![Actions Status](https://github.com/SeventhServices/SeventhServices.Client/workflows/Build/badge.svg)](https://github.com/SeventhServices/SeventhClient/actions)
[![Coverage Status](https://coveralls.io/repos/github/casbin-net/casbin-aspnetcore/badge.svg?branch=master)](https://coveralls.io/github/casbin-net/casbin-aspnetcore?branch=master)
[![License](https://img.shields.io/github/license/casbin-net/casbin-aspnetcore)](https://github.com/casbin-net/casbin-aspnetcore/blob/master/LICENSE)
[![Build Version](https://img.shields.io/casbin-net.myget/casbin-net/v/Casbin.AspNetCore)](https://www.myget.org/feed/casbin-net/package/nuget/Casbin.AspNetCore)

Casbin.AspNetCore is a [Casbin.NET](https://github.com/casbin/Casbin.NET) integration and extension for [ASP.NET Core](https://asp.net).

## Installation

This project is on developping, You can install the build version to try it now.

```csharp
dotnet add package Casbin.AspNetCore --version <build package version> --source https://www.myget.org/F/casbin-net/api/v3/index.json
```

Or you create a `NuGet.config` file on you solution directory like this.

```xml
<configuration>
  <packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <add key="myget.org" value="https://www.myget.org/F/casbin-net/api/v3/index.json" />
  </packageSources>
</configuration>
```

## Quick Start
1. You should add the service at `ConfigureServices` method and add MiddleWare at `Configure` method like this:
```chsrp
public void ConfigureServices(IServiceCollection services)
{
    // Other codes...

    //Add Casbin Authorization
    services.AddCasbinAuthorization(options =>
    {
        options.DefaultModelPath = <model path>;
        options.DefaultPolicyPath = <policy path>;
    });
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Other codes...

    app.UseCasbinAuthorization();

    // If you want support offical authorization, you can add this.
    app.UseAuthorization();

    // Other codes...
}
```
2. Now you can use the attribute like offical authorization, If you use the Basic Model, It will like this:
```csharp
[CasbinAuthorize("<obj>", "<act>")]
public IActionResult Index()
{
    return View();
}
```

## How To Work
Head over to the [wiki](https://github.com/casbin-net/casbin-aspnetcore/wiki) for user guidance on how it works.

## Samples
Sample applications using `Casbin.AspNetCore` can be found at [sample directory](https://github.com/casbin-net/casbin-aspnetcore/tree/master/samples).



