# Update-Item cmdlet

Updates an existing Commercetools entity.

## Syntax

```powershell
Update-Item [-Path <string[]>]
            [-Actions <string>]
            [-CustomObjectDraft <string>]
            [-Expands <string[]>]
```

- **Path** (optional)

  Path to the Commercetools entity to be updated.
  If the path is ommitted the current location will be used or the path value must come from a piped drive entity
  container object (see samples).

- **Actions** (optional)

  Action object or Json to be performed on the Commercetools entity.

- **CustomObjectDraft** (optional)

  Draft of the custom object to update.

- **Expands** (optional)
  Commercetools expression(s) to expand the results. Returned objects will be expanded with additional data of
  referenced entities.
  See: [https://docs.commercetools.com/api/general-concepts#reference-expansion](https://docs.commercetools.com/api/general-concepts#reference-expansion)

> **_!_** When updating a custom object the `CustomObjectDraft` parameter needs to be specified. When updating other
> Commercetools entities the `Actions` parameter needs to be specified. The two parameters cannot be used together in a
> single statement.

> **_!_** Updating an api-client is not possible. You can only create or delete an api-client.

### Samples

```powershell
Update-Item -Path ct-abc:\products\006754dd-c316-4d56-96a0-9761137088b6 -Actions '[{"action": "changeName",  "name": { "en-US": "Updated Product Name" }}]'
```

```powershell
Get-Item -Path ct-abc:\products\006754dd-c316-4d56-96a0-9761137088b6 | Update-Item -Actions '[{"action": "changeName",  "name": { "en-US": "Updated Product Name" }}]'
```

```powershell
Add-Type -Path "c:\libs\commercetools\commercetools.Sdk.Api.dll"

$nameLocalizedString = [commercetools.Sdk.Api.Models.Common.LocalizedString]::new()
$nameLocalizedString.Add("en", "new Channel Name EN")

$actionsList = [System.Collections.Generic.List[commercetools.Sdk.Api.Models.Channels.IChannelUpdateAction]]@(
    [commercetools.Sdk.Api.Models.Channels.ChannelChangeKeyAction]@{ Key = "myNewChannelKey" },
    [commercetools.Sdk.Api.Models.Channels.ChannelChangeNameAction]@{ Name = $nameLocalizedString }
)

Update-Item -Path ct-test:\channels\006754dd-c316-4d56-96a0-9761137088b8 -Actions $actionsList
```

```powershell
Update-Item -Path ct-test:\customobjects\006754dd-c316-4d56-96a0-9761137088b7 -CustomObjectDraft {"container": "myContainer", "key": "myKey", "value": { "customObjectValue": {"field1" : "value1"}}}
```
