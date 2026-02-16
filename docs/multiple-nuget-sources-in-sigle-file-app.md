# Using Multiple NuGet Sources in Single-File C# Apps (.NET 10+)

When running single-file C# applications with `dotnet run file.cs`, you may need packages from multiple NuGet sources (e.g., nuget.org and a private Azure DevOps feed).

## Solution

### Step 1: Add Additional NuGet Source in the .cs File

Use the `#:property` directive to add extra NuGet sources via `RestoreAdditionalProjectSources`:

```csharp
#:sdk Microsoft.NET.Sdk.Web
#:property TargetFramework=net10.0
#:property RestoreAdditionalProjectSources=https://pkgs.dev.azure.com/YOUR_ORG/_packaging/YOUR_FEED/nuget/v3/index.json
#:package SomePublicPackage@1.*
#:package YourPrivatePackage@*
```

This adds the Azure DevOps feed **in addition** to the default nuget.org source.

### Step 2: Configure Authentication for Private Feeds

Private feeds (like Azure DevOps Artifacts) require authentication. Add credentials to your global NuGet config:

```powershell
# Add a new source with credentials
dotnet nuget add source "https://pkgs.dev.azure.com/YOUR_ORG/_packaging/YOUR_FEED/nuget/v3/index.json" `
    --name "YourFeedName" `
    --username "any" `
    --password "YOUR_PERSONAL_ACCESS_TOKEN" `
    --store-password-in-clear-text

# Or update an existing source
dotnet nuget update source "YourFeedName" `
    --username "any" `
    --password "YOUR_PERSONAL_ACCESS_TOKEN" `
    --store-password-in-clear-text
```

> **Note:** For Azure DevOps, the username can be any non-empty string. The PAT (Personal Access Token) must have **Packaging > Read** scope.

### Step 3: Create a Personal Access Token (PAT)

1. Go to your Azure DevOps organization (e.g., `https://dev.azure.com/YOUR_ORG`)
2. Click your profile icon → **Personal access tokens**
3. Click **New Token**
4. Set scope: **Packaging → Read**
5. Copy the generated token

### Step 4: Run the Application

```powershell
dotnet run YourFile.cs
```

## Alternative: Using nuget.config (for project-based apps)

For traditional `.csproj` projects, you can use a `nuget.config` file:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />
    <add key="YourFeed" value="https://pkgs.dev.azure.com/YOUR_ORG/_packaging/YOUR_FEED/nuget/v3/index.json" />
  </packageSources>
  <packageSourceCredentials>
    <YourFeed>
      <add key="Username" value="any" />
      <add key="ClearTextPassword" value="YOUR_PAT" />
    </YourFeed>
  </packageSourceCredentials>
</configuration>
```

> **Note:** Single-file apps (`dotnet run file.cs`) may not pick up `nuget.config` automatically, so the `#:property RestoreAdditionalProjectSources` approach is preferred.

## Troubleshooting

### Error: 401 Unauthorized
- Your PAT is invalid, expired, or missing the **Packaging > Read** scope
- Update the source with a new PAT:
  ```powershell
  dotnet nuget update source "YourFeedName" --password "NEW_PAT" --store-password-in-clear-text
  ```

### Error: Unable to find package
- Verify the package exists in the feed
- Check that the source URL is correct
- Ensure both sources are configured:
  ```powershell
  dotnet nuget list source
  ```

### Verify Credentials Are Stored
Credentials are stored in:
- **Windows:** `%APPDATA%\NuGet\NuGet.Config`
- **Linux/macOS:** `~/.nuget/NuGet/NuGet.Config`
