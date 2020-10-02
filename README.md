# Casbin.AspNetCore

[![Actions Status](https://github.com/casbin-net/casbin-aspnetcore/workflows/Build/badge.svg)](https://github.com/casbin-net/casbin-aspnetcore/actions)
[![Coverage Status](https://coveralls.io/repos/github/casbin-net/casbin-aspnetcore/badge.svg?branch=master)](https://coveralls.io/github/casbin-net/casbin-aspnetcore?branch=master)
[![License](https://img.shields.io/github/license/casbin-net/casbin-aspnetcore)](https://github.com/casbin-net/casbin-aspnetcore/blob/master/LICENSE)
[![Build Version](https://img.shields.io/casbin-net.myget/casbin-net/v/Casbin.AspNetCore?label=Casbin.AspNetCore)](https://www.myget.org/feed/casbin-net/package/nuget/Casbin.AspNetCore)

Casbin.AspNetCore is a [Casbin.NET](https://github.com/casbin/Casbin.NET) integration and extension for [ASP.NET Core](https://asp.net).

## Installation

This project is on developing, You can install the build version to try it.

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
You should add the service at `ConfigureServices` method and add MiddleWare at `Configure` method like this:

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Other codes...

    //Add Casbin Authorization
    services.AddCasbinAuthorization(options =>
    {
        options.DefaultModelPath = "<model path>";
        options.DefaultPolicyPath = "<policy path>";
    });
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Other codes...

    app.UseCasbinAuthorization();

    // You can add this to support offical authorization too.
    app.UseAuthorization();

    // Other codes...
}
```
Now you can use the attribute like offical authorization, If you use the Basic Model, It will like this:

```csharp
[CasbinAuthorize("<obj>", "<act>")]
public IActionResult Index()
{
    return View();
}
```

## Usages
Head over to the [usages](https://github.com/casbin-net/casbin-aspnetcore/wiki/Usages) on the wiki to know all the features.

## How It Works
Head over to the [how it works](https://github.com/casbin-net/casbin-aspnetcore/wiki/How-it-works) on the wiki for user guidance on how it works.

## Samples
Sample applications using `Casbin.AspNetCore` can be found at [sample directory](https://github.com/casbin-net/casbin-aspnetcore/tree/master/samples).

## Getting Help
- [Casbin.NET](https://github.com/casbin/Casbin.NET)

## License
This project is under Apache 2.0 License. See the [LICENSE](LICENSE) file for the full license text.
