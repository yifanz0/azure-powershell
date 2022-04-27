using Azure.ResourceManager;
using Track2 = Azure.ResourceManager.Storage;
using Track2Model = Azure.ResourceManager.Storage.Models;
using Azure.ResourceManager.Resources;

using Microsoft.Azure.Commands.Common.Authentication;
using Microsoft.Azure.Commands.Common.Authentication.Abstractions;

using System.Collections.Generic;
using Azure.ResourceManager.Storage;
using Azure;
using Azure.Core;
using Microsoft.Azure.Commands.Management.Storage.Models;
using Azure.ResourceManager.Storage.Models;
using Microsoft.Azure.Storage.Blob.Protocol;
using System.Diagnostics.Contracts;

namespace Microsoft.Azure.Commands.Management.Storage
{
    public class Track2StorageManagementClient
    {
        private ArmClient _armClient;
        private string _subscription;
        private IClientFactory _clientFactory;

        public Track2StorageManagementClient(IClientFactory clientFactory, IAzureContext context)
        {
            _clientFactory = clientFactory;
            _armClient = _clientFactory.CreateArmClient(context, AzureEnvironment.Endpoint.ActiveDirectoryServiceEndpointResourceId);
            _subscription = context.Subscription.Id;
        }

        /// <summary>
        /// Get resource group instance
        /// </summary>
        /// <param name="resourcegroup"></param>
        public ResourceGroupResource GetResourceGroup(string resourcegroup) =>
            _armClient.GetResourceGroupResource(new global::Azure.Core.ResourceIdentifier(
                string.Format("/subscriptions/{0}/resourceGroups/{1}", _subscription, resourcegroup)));


        public SubscriptionResource GetSubscription(string subscription) =>
            _armClient.GetSubscriptionResource(new global::Azure.Core.ResourceIdentifier(
                string.Format("/subscriptions/{0}", subscription)));

        /// <summary>
        /// List accounts from subscription
        /// </summary>
        /// 

        public Pageable<StorageAccountResource> ListStorageAccounts() =>
            _armClient.GetSubscriptionResource(new global::Azure.Core.ResourceIdentifier(
                string.Format("/subscriptions/{0}", _subscription)))
            .GetStorageAccounts();

        /// <summary>
        /// List accounts from Resource group
        /// </summary>
        public Pageable<Track2.StorageAccountResource> ListStorageAccounts(string resourcegroup) =>
            GetResourceGroup(resourcegroup).GetStorageAccounts().GetAll();

        /// <summary>
        /// Get single storage account
        /// </summary>
        public Track2.StorageAccountResource GetStorageAccount(string resourcegroup, string storageAccountName) =>
            _armClient.GetStorageAccountResource(Track2.StorageAccountResource.CreateResourceIdentifier(_subscription, resourcegroup, storageAccountName));


        public Track2.StorageAccountResource GetSingleStorageAccount(string resourcegroup, string storageAccountName) =>
            GetResourceGroup(resourcegroup).GetStorageAccounts().Get(storageAccountName);

        //public Track2Model.StorageAccount CreateStorageAccount(string resourcegroup, string storageAccountName, Track2Model.StorageAccountCreateParameters parameters) =>
        //    GetResourceGroup(resourcegroup).GetStorageAccounts().CreateOrUpdate(storageAccountName, parameters).Value;
        

        public Track2.StorageAccountResource GetStorageAccountWithExpand(string resourceGroup, string storageAccountName, StorageAccountExpand? expand = null) =>
            GetResourceGroup(resourceGroup).GetStorageAccounts().Get(storageAccountName, expand);


        public ArmOperation<StorageAccountResource> CreateStorageAccount(string resourceGroup, string storageAccountName, StorageAccountCreateOrUpdateContent content) =>
            GetResourceGroup(resourceGroup).GetStorageAccounts().CreateOrUpdate(WaitUntil.Completed, storageAccountName, content);


        public Track2.StorageAccountResource GetStorageAccountProperties(string resourceGroup, string storageAccountName) =>
            GetResourceGroup(resourceGroup).GetStorageAccounts().Get(storageAccountName);

        public Track2.StorageAccountResource UpdateStorageAccount(string resourcegroup, string storageAccountName, StorageAccountPatch patch) =>
            _armClient.GetStorageAccountResource(Track2.StorageAccountResource.CreateResourceIdentifier(_subscription, resourcegroup, storageAccountName)).Update(patch);


        //public Track2.ManagementPolicyResource GetManagementPolicy() => 

    }
}
