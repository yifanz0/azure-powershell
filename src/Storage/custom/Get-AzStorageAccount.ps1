
# ----------------------------------------------------------------------------------
#
# Copyright Microsoft Corporation
# Licensed under the Apache License, Version 2.0 (the \"License\");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
# http://www.apache.org/licenses/LICENSE-2.0
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an \"AS IS\" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
# ----------------------------------------------------------------------------------

<#
.Synopsis
Get-AzStorageAccount
.Description
Get-AzStorageAccount
#>
class PSSku {
    [string]$Name
    [string]$Tier
}

class PSPrimaryEndpoints {
    [string]$Blob
    [string]$Queue
    [string]$Table
    [string]$File
    [string]$Web
    [string]$Dfs
    [string]$MicrosoftEndpoints
    [string]$InternetEndpoints
}

class PSStorageAccount {
    [string]$ResourceGroupName
    [string]$StorageAccountName
    [string]$Id
    [string]$Location
    [PSSku]$Sku
    [PSPrimaryEndpoints]$PrimaryEndpoints
}


function Get-AzStorageAccount {
    [OutputType('Microsoft.Azure.PowerShell.Cmdlets.Storage.Models.Api20210801.StorageAccount')]
    #[CmdletBinding(DefaultParameterSetName='ResourceGroupParameterSet')]
    Param(
        [Parameter(Mandatory = $false,
            ParameterSetName = 'ResourceGroupParameterSet',
            HelpMessage = 'Resource Group Name.',
            Position = 0)]
        [Parameter(Mandatory = $true,
            ParameterSetName = 'AccountNameParameterSet',
            HelpMessage = 'Resource Group Name.',
            Position = 0)]
        [Parameter(Mandatory = $true,
            ParameterSetName = 'BlobRestoreStatusParameterSet',
            HelpMessage = 'Resource Group Name.',
            Position = 0)]
 
        [System.String]
        $ResourceGroupName,

        [Parameter(
            Position = 1,
            Mandatory = $true,
            ParameterSetName = 'AccountNameParameterSet',
            HelpMessage = 'Storage Account Name.')]
        [Parameter(
            Position = 1,
            Mandatory = $true,
            ParameterSetName = 'BlobRestoreStatusParameterSet',
            HelpMessage = 'Storage Account Name.')]
        [System.String]
        $Name,

        [Parameter(
            Mandatory = $false,
            ParameterSetName = 'AccountNameParameterSet',
            HelpMessage = 'Get the GeoReplicationStats of the Storage account.')]
        [System.Management.Automation.SwitchParameter]
        $IncludeGeoReplicationStats,

        [Parameter(
            Mandatory = $true,
            ParameterSetName = 'BlobRestoreStatusParameterSet',
            HelpMessage = 'Get the GeoReplicationStats of the Storage account.')]
        [System.Management.Automation.SwitchParameter]
        $IncludeBlobRestoreStatus
    )

    process {
        $accounts = [System.Collections.ArrayList]::new()
        if ($PSBoundParameters.ContainsKey('ResourceGroupName')) {
            if ($PSBoundParameters.ContainsKey('Name')) {

                $account = Get-AzStorageACcountProperty -ResourceGroupName $ResourceGroupName -AccountName $Name
                return toPSStorageAccount -StorageAccount $account
            } else {
                $account_list = Get-AzStorageAccountInternal -ResourceGroupName $ResourceGroupName
                foreach ($account in $account_list) {
   
                    $PSAccount = toPSStorageAccount -StorageAccount $account

                    $accounts.add($PSAccount) 
                }
                return $accounts

            }
        } else {
            $account_list = Get-AzStorageAccountInternal
            foreach ($account in $account_list) {
                $PSAccount = toPSStorageAccount -StorageAccount $account

                $accounts.add($PSAccount)
            }
            return $accounts
        }

    }
}

function toPSStorageAccount {
    [OutputType('PSStorageAccount')]
    [Microsoft.Azure.PowerShell.Cmdlets.Storage.DoNotExportAttribute()]
    Param(
        [Parameter(Mandatory = $true,
            HelpMessage = 'Storage account',
            Position = 0)]
        [Microsoft.Azure.PowerShell.Cmdlets.Storage.Models.Api20210801.StorageAccount]
        $StorageAccount
    )

    $PSAccount = [PSStorageAccount]::new()

    $PSAccount.ResourceGroupName = $account.ResourceGroupName
    $PSAccount.StorageAccountName = $account.Name
    $PSAccount.Id = $account.Id
    $PSAccount.Location = $account.Location

    $Sku = [PSSku]::new()
    $Sku.Name = $account.SkuName
    $Sku.Tier = $account.SkuTier
    $PSAccount.Sku = $Sku

    $PrimaryEndpoints = [PSPrimaryEndpoints]::new()
    $PrimaryEndpoints.Blob = $account.PrimaryEndpointBlob
    $PrimaryEndpoints.Queue = $account.PrimaryEndpointQueue
    $PrimaryEndpoints.Table = $account.PrimaryEndpointTable
    $PrimaryEndpoints.File = $account.PrimaryEndpointFile
    $PSAccount.PrimaryEndpoints = $PrimaryEndpoints

    return $PSAccount


}



