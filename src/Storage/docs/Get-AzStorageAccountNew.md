---
external help file:
Module Name: Az.Storage
online version: https://docs.microsoft.com/powershell/module/az.storage/get-azstorageaccountnew
schema: 2.0.0
---

# Get-AzStorageAccountNew

## SYNOPSIS
Get-AzStorageAccount

## SYNTAX

### ResourceGroupParameterSet (Default)
```
Get-AzStorageAccountNew [[-ResourceGroupName] <String>] [-AsJob] [<CommonParameters>]
```

### AccountNameParameterSet
```
Get-AzStorageAccountNew [-ResourceGroupName] <String> [-Name] <String> [-AsJob] [-IncludeGeoReplicationStats]
 [<CommonParameters>]
```

### BlobRestoreStatusParameterSet
```
Get-AzStorageAccountNew [-ResourceGroupName] <String> [-Name] <String> -IncludeBlobRestoreStatus [-AsJob]
 [<CommonParameters>]
```

## DESCRIPTION
Get-AzStorageAccount

## EXAMPLES

### Example 1: {{ Add title here }}
```powershell
{{ Add code here }}
```

```output
{{ Add output here }}
```

{{ Add description here }}

### Example 2: {{ Add title here }}
```powershell
{{ Add code here }}
```

```output
{{ Add output here }}
```

{{ Add description here }}

## PARAMETERS

### -AsJob
Get the GeoReplicationStats of the Storage account.

```yaml
Type: System.Management.Automation.SwitchParameter
Parameter Sets: (All)
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -IncludeBlobRestoreStatus
Get the GeoReplicationStats of the Storage account.

```yaml
Type: System.Management.Automation.SwitchParameter
Parameter Sets: BlobRestoreStatusParameterSet
Aliases:

Required: True
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -IncludeGeoReplicationStats
Get the GeoReplicationStats of the Storage account.

```yaml
Type: System.Management.Automation.SwitchParameter
Parameter Sets: AccountNameParameterSet
Aliases:

Required: False
Position: Named
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -Name
Storage Account Name.

```yaml
Type: System.String
Parameter Sets: AccountNameParameterSet, BlobRestoreStatusParameterSet
Aliases:

Required: True
Position: 1
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### -ResourceGroupName
Resource Group Name.

```yaml
Type: System.String
Parameter Sets: (All)
Aliases:

Required: True
Position: 0
Default value: None
Accept pipeline input: False
Accept wildcard characters: False
```

### CommonParameters
This cmdlet supports the common parameters: -Debug, -ErrorAction, -ErrorVariable, -InformationAction, -InformationVariable, -OutVariable, -OutBuffer, -PipelineVariable, -Verbose, -WarningAction, and -WarningVariable. For more information, see [about_CommonParameters](http://go.microsoft.com/fwlink/?LinkID=113216).

## INPUTS

## OUTPUTS

### Microsoft.Azure.PowerShell.Cmdlets.Storage.Models.Api20210801.StorageAccount

## NOTES

ALIASES

## RELATED LINKS

