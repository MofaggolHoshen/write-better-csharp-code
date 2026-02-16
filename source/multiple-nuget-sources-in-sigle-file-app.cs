// #:sdk Microsoft.NET.Sdk.Web
// #:property TargetFramework=net10.0
// #:property RestoreAdditionalProjectSources=https://pkgs.dev.azure.com/YOUR_ORG/_packaging/YOUR_FEED/nuget/v3/index.json
// #:package SomePublicPackage@1.*
// #:package YourPrivatePackage@*

using System;

Console.WriteLine("Single-file app example with multiple NuGet sources.");
Console.WriteLine("Replace the feed URL and package names with your actual values.");

