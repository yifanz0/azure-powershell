
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
function Get-AzStorageAccountNew {
    [OutputType('Microsoft.Azure.PowerShell.Cmdlets.Storage.Models.Api20210801.StorageAccount')]
    [CmdletBinding(DefaultParameterSetName='ResourceGroupParameterSet')]
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
        [ValidateNotNullorEmpty()]
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
        $IncludeBlobRestoreStatus,

        [Parameter(
            Mandatory = $false,
            HelpMessage = 'Get the GeoReplicationStats of the Storage account.')]
        [System.Management.Automation.SwitchParameter]
        $AsJob
    )

    process {
        #$accounts = [Microsoft.Azure.PowerShell.Cmdlets.Storage.Models.Api20210801.StorageAccount]::New()
#        $accounts = Az.Storage.internal\Get-AzStorageACcount
        $demoarrayList = [System.Collections.ArrayList]::new()
        if ($AsJob) {
            Start-Job -ScriptBlock {
                if ($ResourceGroupName) {
                    $accounts = Get-AzStorageAccount -ResourceGroupName $ResourceGroupName
                    if ($Name) {
                        foreach ($account in $accounts) {
                            if ($account.Name -eq $Name) {
                                $res_account = [Microsoft.Azure.PowerShell.Cmdlets.Storage.Models.Api20210801.StorageAccount]::New()
                                $account.PSObject.Properties | ForEach-Object {
                                    $res_account.$_.Name = $_.Value
                                    $c = createContextForAccount -Name $Name
                                    $res_account.context = $c
                                }
                                return $res_account
                            }
                        }
                    } else {
                        return $accounts
                    }
                } else {
                    return Get-AzStorageAccount
                } 
            }
        } else {
            if ($ResourceGroupName) {
                $accounts = Get-AzStorageAccount -ResourceGroupName $ResourceGroupName
                if ($Name) {
                    foreach ($account in $accounts) {
                        if ($account.Name -eq $Name) {
                            $res_account = [Microsoft.Azure.PowerShell.Cmdlets.Storage.Models.Api20210801.StorageAccount]::New()
                            foreach($object_properties in $account.PSObject.Properties) {
                                $property_name = $object_properties.Name
                                $property_val = $object_properties.Value
                                $res_account.$property_name = $property_val
                            }
                            return $res_account
                        }
                    }
                } else {
                    return $accounts
                }
            } else {
                return Get-AzStorageAccount
            }
        }
    }
}

function createContextForAccount {
    Param(
        [Parameter(Mandatory = $true,
            HelpMessage = 'Storage account name',
            Position = 0)]
        [System.String]
        $Name
    )
    

    $Context = [Microsoft.WindowsAzure.Commands.Common.Storage.AzureStorageContext]::New()
    $Context.BlobEndPoint = 'https://example.blob.core.windows.net/'
    # TODO: Fill in context information
    return $Context
}

function createPrimaryEndpointsForAccount {
    
}

