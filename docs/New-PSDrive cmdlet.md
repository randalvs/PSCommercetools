# New-PSDrive cmdlet

Adds a new drive connected to the API of a Commercetools project.

## Syntax

```powershell
New-PSDrive -PSProvider PSCommercetools
            -Name <string>
            -Root <string>
            -ProjectKey <string>
            -ClientId <string>
            -ClientSecret <string>
            -Scopes <string>
            [-ApiBaseAddress <string>]
            [-AuthorizationBaseAddress <string>]
```

### Parameters

- **PSProvider** (mandatory)

  Value must be set to 'PSCommercetools' in order to use Powershell provider for Commercetools.

- **Name** (mandatory)

  Name of the drive. Used to reference the drive in paths.

- **Root** (mandatory)

  The root of the drive to create. This must be a concatenation of the drive name, followed by ':\' and optionally a sub path. The sub path can only exists of entity containers.

- **ProjectKey** (mandatory)

  Commercetools Api project key.

- **ClientId** (mandatory)

  Commercetools Api client id

- **ClientSecret** (mandatory)

  Commercetools Api client secret

- **Scopes** (mandatory)

  Commercetools Api scopes

- **ApiBaseAddress** (optional)

  Commercetools Api base url (defaults to: https://api.europe-west1.gcp.commercetools.com/).

- **AuthorizationBaseAddress** (optional)

  Commercetools Api authorization base url (defaults to: https://auth.europe-west1.gcp.commercetools.com/).

### Samples

```powershell
New-PSDrive -PSProvider PSCommercetools `
            -Name ct-abc `
            -Root abc-company:\ `
            -ProjectKey abc-company `
            -ClientId abcdef1234abcdesfd `
            -ClientSecret Abac123avd343dfas3 `
            -Scopes manage_project:abc-company `
```

```powershell
New-PSDrive -PSProvider PSCommercetools `
            -Name ct-abc-prdct `
            -Root abc-company:\products `
            -ProjectKey abc-company `
            -ClientId abcdef1234abcdesfd `
            -ClientSecret Abac123avd343dfas3 `
            -Scopes manage_project:abc-company `
```

### Default drive

The provider supports initialization of a default drive. This is a drive (named `ct-default`) that is configured and connected automatically whenever the provider module is loaded. In order to use the default drive, an .env file needs to be placed in the [PowerShell profile folder](https://learn.microsoft.com/en-us/powershell/module/microsoft.powershell.core/about/about_profiles?view=powershell-7.5).

> **_!_** The profile folder can be found by executing $profile at a PowerShell prompt.

The contents of the .env file should look like this:

```
CTP_PROJECT_KEY=abc-company
CTP_CLIENT_SECRET=Abac123avd343dfas3
CTP_CLIENT_ID=abcdef1234abcdesfd
CTP_AUTH_URL=https://auth.europe-west1.gcp.commercetools.com
CTP_API_URL=https://api.europe-west1.gcp.commercetools.com
CTP_SCOPES=manage_project:abc-company
```

or with default url's:

```
CTP_PROJECT_KEY=abc-company
CTP_CLIENT_SECRET=Abac123avd343dfas3
CTP_CLIENT_ID=abcdef1234abcdesfd
CTP_SCOPES=manage_project:abc-company
```

![initialize default drive](images\initialize_default_drive.jpg)

> **_!_** If you want to temporarly skip initializing the default drive you can remove the .env file or you can set the variable `$SkipInitializeDefaultCommercetoolsDrive` to true before loading the provider module. This will skip initializing the default drive while an .env file is still be present in the profile folder.
