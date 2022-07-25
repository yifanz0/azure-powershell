// ----------------------------------------------------------------------------------
//
// Copyright Microsoft Corporation
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// ----------------------------------------------------------------------------------

using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using System.Management.Automation;
using Track2Models = Azure.ResourceManager.Storage.Models;

namespace Microsoft.Azure.Commands.Management.Storage
{
    [Cmdlet("Get", ResourceManager.Common.AzureRMConstants.AzureRMPrefix + "StorageAccountKey"), OutputType(typeof(Track2Models.StorageAccountKey))]
    public class GetAzureStorageAccountKeyCommand : StorageAccountBaseCmdlet
    {
        [Parameter(
            Position = 0,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Resource Group Name.")]
        [ResourceGroupCompleter]
        [ValidateNotNullOrEmpty]
        public string ResourceGroupName { get; set; }

        [Parameter(
            Position = 1,
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "Storage Account Name.")]
        [Alias(StorageAccountNameAlias, AccountNameAlias)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(
            HelpMessage = "Lists the Kerberos keys (if active directory enabled) for the specified storage account.")]
        public SwitchParameter ListKerbKey { get; set; }

        public override void ExecuteCmdlet()
        {
            base.ExecuteCmdlet();

            Track2Models.StorageListKeyExpand? expand = null;
            if (ListKerbKey.IsPresent)
            {
                expand = Track2Models.StorageListKeyExpand.Kerb;
            }

            var storageKeys = this.StorageClientTrack2.GetStorageAccount(this.ResourceGroupName, this.Name)
                .GetKeys(expand).Value.Keys;

            WriteObject(storageKeys, true);
        }
    }
}
