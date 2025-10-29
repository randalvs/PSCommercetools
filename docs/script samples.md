### Fetch the most recent cart of a certain user with expanded payment info, convert it to Json and put the result on the clipboard.

```powershell
Get-ChildItem -Path ct-abc:\carts -Filter 'customerEmail="michael-williams@ehlt.com"' -Expands 'paymentInfo.payments[*].paymentStatus.state' -Sort 'lastModifiedAt desc' -Limit 1 | ConvertTo-CtJson | Set-Clipboard
```

### Loop over all products and execute a PowerShell function for each product.

```powershell
function LoopAllProducts {
    param(
        [scriptblock]$callbackFunction
    )

    $lastId = $null
    [long]$counter = 1
    $whereClause = ''

    while ($true) {

        if ($null -ne $lastId) {
            $whereClause = 'id > "' + $lastId + '"'
        }

        $products = Get-ChildItem ct-abc:\products -Sort 'id' -Filter $whereClause -Limit 500 -WithNoTotal
       
        foreach ($product in $products) {
            & $callbackFunction $counter $product
            $counter++
            $lastId = $product.Id
        }

        if ($products.Length -lt 500) {
            break
        }
    }
}

$processProduct = {
    param(
        [long]$index,
        $product
    )

    $red = "`e[91m"
    $yellow = "`e[33m"
    $default = "`e[0m"

    Write-Host $red$index$default '-' $product.Key $product.Version $yellow($product.MasterData.Current.Name.Values | Select-Object -First 1)

}

LoopAllProducts $processProduct
```

### Get a product using a filter and open the JSON in VSCode.

```powershell
function Open-JsonInVSCode {
    [CmdletBinding()]
    param (
        [Parameter(ValueFromPipeline = $true)]
        $json
    )

    $tempFile = [System.IO.Path]::GetTempFileName() -replace '\.tmp$', '.json'
    $json | Out-File -FilePath $tempFile -Encoding utf8
    code --reuse-window "$tempFile"
}


Get-ChildItem ct-abc:\products -Filter 'key="12345"'  | ConvertTo-CtJson | Open-JsonInVSCode
```