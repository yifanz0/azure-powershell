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

using System.Management.Automation;
using Microsoft.Azure.Commands.Management.Storage.Models;
using Microsoft.Azure.Commands.ResourceManager.Common.ArgumentCompleters;
using Track2 = Azure.ResourceManager.Storage;

namespace Microsoft.Azure.Commands.Management.Storage
{
    /// <summary>
    /// Lists all storage services underneath the subscription.
    /// </summary>
    [Cmdlet("Invoke", ResourceManager.Common.AzureRMConstants.AzureRMPrefix + StorageAccountHierarchicalNamespaceUpgradeNounStr, SupportsShouldProcess = true, DefaultParameterSetName = AccountNameParameterSet), OutputType(typeof(PSStorageAccount))]    
    public class InvokeAzureStorageAccountHierarchicalNamespaceUpgradeCommand : StorageAccountBaseCmdlet
    {
        /// <summary>
        /// AccountName Parameter Set
        /// </summary>
        private const string AccountNameParameterSet = "AccountName";

        /// <summary>
        /// Account object parameter set 
        /// </summary>
        private const string AccountObjectParameterSet = "AccountObject";
        [Parameter(
            Position = 0,
            Mandatory = true,
            HelpMessage = "Resource Group Name.",
            ParameterSetName = AccountNameParameterSet)]
        [ResourceGroupCompleter]
        [ValidateNotNullOrEmpty]
        public string ResourceGroupName { get; set; }

        [Parameter(
            Position = 1,
            Mandatory = true,
            HelpMessage = "Storage Account Name.",
            ParameterSetName = AccountNameParameterSet)]
        [Alias(StorageAccountNameAlias, AccountNameAlias)]
        [ValidateNotNullOrEmpty]
        public string Name { get; set; }

        [Parameter(Mandatory = true,
            HelpMessage = "Storage account object",
            ValueFromPipeline = true,
            ParameterSetName = AccountObjectParameterSet)]
        [ValidateNotNullOrEmpty]
        public PSStorageAccount InputObject { get; set; }

        [Parameter(
            Mandatory = true,
            HelpMessage = "The HierarchicalNamespaceUpgrade requestType  to run: Validation: Validate if the account can be upgrade to enable HierarchicalNamespace. Upgrade: Upgrade the storage account to enable HierarchicalNamespace.")]
        [ValidateNotNullOrEmpty]
        [ValidateSet(HierarchicalNamespaceUpgradeAction.Validation,
            HierarchicalNamespaceUpgradeAction.Upgrade,
            IgnoreCase = true)]
        public string RequestType { get; set; }

        [Parameter(HelpMessage = "Force to upgrade the Account")]
        public SwitchParameter Force
        {
            get { return force; }
            set { force = value; }
        }
        private bool force = false;      

        [Parameter(Mandatory = false, HelpMessage = "Run cmdlet in the background")]
        public SwitchParameter AsJob { get; set; }

        public override void ExecuteCmdlet()
        {
            base.ExecuteCmdlet();

            if (ShouldProcess(this.Name, "Invoke HierarchicalNamespace Upgrade of Storage Account"))
            {
                if (ParameterSetName == AccountObjectParameterSet)
                {
                    this.ResourceGroupName = InputObject.ResourceGroupName;
                    this.Name = InputObject.StorageAccountName;
                }

                if (this.RequestType.ToLower() == HierarchicalNamespaceUpgradeAction.Validation.ToLower() || this.force || ShouldContinue(string.Format("Invoke HierarchicalNamespace Upgrade of Storage Account '{0}'.", this.Name), ""))
                {
                    string requestType;
                    if (this.RequestType.ToLower() == HierarchicalNamespaceUpgradeAction.Validation.ToLower())
                    {
                        requestType = "HnsOnValidationRequest";
                    }
                    else
                    {
                        requestType = "HnsOnHydrationRequest";
                    }
                     
                    this.StorageClientTrack2.GetStorageAccount(this.ResourceGroupName, this.Name).HierarchicalNamespaceMigration(global::Azure.WaitUntil.Completed, requestType);


                    if (this.RequestType.ToLower() == HierarchicalNamespaceUpgradeAction.Validation.ToLower())
                    {
                        WriteObject(true);
                    }
                    else
                    {
                        Track2.StorageAccountResource storageAccount = this.StorageClientTrack2
                            .GetStorageAccount(this.ResourceGroupName, this.Name).Get();
                        WriteStorageAccount(storageAccount);
                    }
                }
            }
        }

        protected struct HierarchicalNamespaceUpgradeAction
        {
            public const string Validation = "Validation";
            public const string Upgrade = "Upgrade";
        }
    }
}
