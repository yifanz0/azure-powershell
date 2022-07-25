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

using Microsoft.Azure.Commands.Common.Authentication.Abstractions;
using Microsoft.Azure.Management.Storage;
using Microsoft.Azure.Management.Storage.Models;
using Microsoft.WindowsAzure.Commands.Common.Attributes;
using Microsoft.WindowsAzure.Commands.Common.Storage;
using Microsoft.WindowsAzure.Commands.Storage.Adapters;
using Microsoft.Azure.Storage;
using System;
using System.Collections.Generic;
//using StorageModels = Microsoft.Azure.Management.Storage.Models;
using Track2 = Azure.ResourceManager.Storage;
using Track2Models = Azure.ResourceManager.Storage.Models;

namespace Microsoft.Azure.Commands.Management.Storage.Models
{
    public class PSContainer
    {
        public PSContainer(Track2.BlobContainerResource container)
        {
            this.ResourceGroupName = ParseResourceGroupFromId(container.Id);
            this.StorageAccountName = ParseStorageAccountNameFromId(container.Id);
            this.Id = container.Id;
            this.Name = container.Data.Name;
            this.Type = container.Data.ResourceType;
            this.Metadata = container.Data.Metadata;
            this.Etag = container.Data.ETag?.ToString();
            this.PublicAccess = (PSPublicAccess?)container.Data.PublicAccess;
            this.ImmutabilityPolicy = container.Data.ImmutabilityPolicy == null ? null : new PSImmutabilityPolicyProperties(container.Data.ImmutabilityPolicy);
            this.LegalHold = container.Data.LegalHold == null ? null : new PSLegalHoldProperties(container.Data.LegalHold);
            this.LastModifiedTime = container.Data.LastModifiedOn;
            this.LeaseStatus = container.Data.LeaseStatus.ToString();
            this.LeaseState = container.Data.LeaseState.ToString();
            this.LeaseDuration = container.Data.LeaseDuration.ToString();
            this.HasLegalHold = container.Data.HasLegalHold;
            this.HasImmutabilityPolicy = container.Data.HasImmutabilityPolicy;
            this.DefaultEncryptionScope = container.Data.DefaultEncryptionScope;
            this.DenyEncryptionScopeOverride = container.Data.PreventEncryptionScopeOverride;
            this.Deleted = container.Data.IsDeleted;
            this.ImmutableStorageWithVersioning = container.Data.ImmutableStorageWithVersioning is null ? null : new PSImmutableStorageWithVersioning(container.Data.ImmutableStorageWithVersioning);
            this.RemainingRetentionDays = container.Data.RemainingRetentionDays;
            this.DeletedTime = container.Data.DeletedOn;
            this.Version = container.Data.Version;
            this.EnableNfsV3AllSquash = container.Data.EnableNfsV3AllSquash;
            this.EnableNfsV3RootSquash = container.Data.EnableNfsV3RootSquash;
        }

        [Ps1Xml(Label = "ResourceGroupName", Target = ViewControl.List, Position = 0)]
        public string ResourceGroupName { get; set; }

        [Ps1Xml(Label = "StorageAccountName", Target = ViewControl.List, Position = 1)]
        public string StorageAccountName { get; set; }

        public string Id { get; set; }

        [Ps1Xml(Label = "Name", Target = ViewControl.List, Position = 2)]
        public string Name { get; set; }

        public string Type { get; set; }

        public string Etag { get; set; }

        public IDictionary<string, string> Metadata { get; set; }

        [Ps1Xml(Label = "PublicAccess", Target = ViewControl.List, Position = 3)]
        public PSPublicAccess? PublicAccess { get; set; }

        public PSImmutabilityPolicyProperties ImmutabilityPolicy { get; set; }

        public PSLegalHoldProperties LegalHold { get; set; }

        [Ps1Xml(Label = "LastModifiedTime", Target = ViewControl.List, Position = 4)]
        public DateTimeOffset? LastModifiedTime { get; set; }

        public string LeaseStatus { get; set; }

        public string LeaseState { get; set; }

        public string LeaseDuration { get; set; }

        [Ps1Xml(Label = "HasLegalHold", Target = ViewControl.List, Position = 5)]
        public bool? HasLegalHold { get; set; }

        [Ps1Xml(Label = "HasImmutabilityPolicy", Target = ViewControl.List, Position = 6)]
        public bool? HasImmutabilityPolicy { get; set; }

        public string DefaultEncryptionScope { get; set; }
        
        public bool? DenyEncryptionScopeOverride { get; set; }

        public int? RemainingRetentionDays { get; }

        public DateTimeOffset? DeletedTime { get; }

        public bool? Deleted { get; }

        public string Version { get; }

        public bool? EnableNfsV3RootSquash { get; set; }

        public bool? EnableNfsV3AllSquash { get; set; }

        public PSImmutableStorageWithVersioning ImmutableStorageWithVersioning { get; set; }

        public static string ParseResourceGroupFromId(string idFromServer)
        {
            if (!string.IsNullOrEmpty(idFromServer))
            {
                string[] tokens = idFromServer.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                return tokens[3];
            }

            return null;
        }

        public static string ParseStorageAccountNameFromId(string idFromServer)
        {
            if (!string.IsNullOrEmpty(idFromServer))
            {
                string[] tokens = idFromServer.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                return tokens[7];
            }

            return null;
        }

        public static string ParseStorageContainerNameFromId(string idFromServer)
        {
            if (!string.IsNullOrEmpty(idFromServer))
            {
                string[] tokens = idFromServer.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

                return tokens[11];
            }

            return null;
        }

        /// <summary>
        /// Return a string representation of this storage account
        /// </summary>
        /// <returns>null</returns>
        public override string ToString()
        {
            // Allow listing storage contents through piping
            return null;
        }
    }

    public class PSLegalHold
    {

        public PSLegalHold(Track2Models.LegalHold legalHold)
        {
            if (legalHold != null)
            {
                this.HasLegalHold = legalHold.HasLegalHold;
                this.Tags = null;
                if (!(legalHold.Tags == null))
                {
                    Tags = ((List<string>)legalHold.Tags).ToArray();
                }
                this.AllowProtectedAppendWritesAll = legalHold.AllowProtectedAppendWritesAll;
            }
        }

        public bool? HasLegalHold { get; set; }
        public string[] Tags { get; set; }
        public bool? AllowProtectedAppendWritesAll { get; set; }

    }

    public class PSLegalHoldProperties
    {
        //public PSLegalHoldProperties(StorageModels.LegalHoldProperties legalHoldProperty)
        //{
        //    this.HasLegalHold = legalHoldProperty.HasLegalHold;
        //    this.Tags = null;

        //    List<PSTagProperty> tagList = new List<PSTagProperty>();
        //    if (legalHoldProperty.Tags != null && legalHoldProperty.Tags.Count != 0)
        //    {
        //        foreach (StorageModels.TagProperty tagProperty in legalHoldProperty.Tags)
        //        {
        //            tagList.Add(new PSTagProperty(tagProperty));
        //        }
        //    }
        //    this.Tags = tagList.ToArray();
        //    this.ProtectedAppendWritesHistory = legalHoldProperty.ProtectedAppendWritesHistory is null ? null : new PSProtectedAppendWritesHistory(legalHoldProperty.ProtectedAppendWritesHistory);
        //}

        public PSLegalHoldProperties(Track2Models.LegalHoldProperties legalHoldProperty)
        {
            this.HasLegalHold = legalHoldProperty.HasLegalHold;
            this.Tags = null;

            List<PSTagProperty> tagList = new List<PSTagProperty>();
            if (legalHoldProperty.Tags != null && legalHoldProperty.Tags.Count != 0)
            {
                foreach (Track2Models.LegalHoldTag tagProperty in legalHoldProperty.Tags)
                {
                    tagList.Add(new PSTagProperty(tagProperty));
                }
            }
            this.Tags = tagList.ToArray();
            this.ProtectedAppendWritesHistory = legalHoldProperty.ProtectedAppendWritesHistory is null ? null : new PSProtectedAppendWritesHistory(legalHoldProperty.ProtectedAppendWritesHistory);
        }

        public bool? HasLegalHold { get; set; }
        public PSTagProperty[] Tags { get; set; }
        public PSProtectedAppendWritesHistory ProtectedAppendWritesHistory { get; set; }
    }

    public class PSProtectedAppendWritesHistory
    {
        public PSProtectedAppendWritesHistory(ProtectedAppendWritesHistory history)
        {
            this.AllowProtectedAppendWritesAll = history.AllowProtectedAppendWritesAll;
            this.Timestamp = history.Timestamp;
        }

        public PSProtectedAppendWritesHistory(Track2Models.ProtectedAppendWritesHistory history)
        {
            this.AllowProtectedAppendWritesAll = history.AllowProtectedAppendWritesAll;
            this.Timestamp = history.Timestamp;
        }

        public bool? AllowProtectedAppendWritesAll { get; set; }
        public DateTimeOffset? Timestamp { get; }
    }

    public class PSTagProperty
    {
        //public PSTagProperty(StorageModels.TagProperty tagsProperty)
        //{
        //    this.Tag = tagsProperty.Tag;
        //    this.Timestamp = tagsProperty.Timestamp;
        //    this.ObjectIdentifier = tagsProperty.ObjectIdentifier;
        //    this.TenantId = tagsProperty.TenantId;
        //    this.Upn = tagsProperty.Upn;
        //}

        public PSTagProperty(Track2Models.LegalHoldTag tagsProperty)
        {
            this.Tag = tagsProperty.Tag;
            this.Timestamp = tagsProperty.Timestamp;
            this.ObjectIdentifier = tagsProperty.ObjectIdentifier;
            this.TenantId = tagsProperty.TenantId?.ToString();
            this.Upn = tagsProperty.Upn;
        }

        public string Tag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public string ObjectIdentifier { get; set; }
        public string TenantId { get; set; }
        public string Upn { get; set; }
    }

    public class PSImmutabilityPolicy
    {
        public PSImmutabilityPolicy(Track2.ImmutabilityPolicyResource policy)
        {
            this.ImmutabilityPeriodSinceCreationInDays = policy.Data.ImmutabilityPeriodSinceCreationInDays;
            this.State = policy.Data.State?.ToString();
            this.Etag = policy.Data.ETag?.ToString();
            this.Name = policy.Data.Name;
            this.Type = policy.Data.ResourceType.ToString();
            this.Id = policy.Id;
            this.AllowProtectedAppendWrites = policy.Data.AllowProtectedAppendWrites;
            this.AllowProtectedAppendWritesAll = policy.Data.AllowProtectedAppendWritesAll;
        }

        public int? ImmutabilityPeriodSinceCreationInDays { get; set; }
        public string State { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Etag { get; set; }
        public bool? AllowProtectedAppendWrites { get; set; }
        public bool? AllowProtectedAppendWritesAll { get; set; }
    }

    public class PSImmutabilityPolicyProperties
    {
        public PSImmutabilityPolicyProperties(Track2Models.BlobContainerImmutabilityPolicy policy)
        {
            this.ImmutabilityPeriodSinceCreationInDays = policy.ImmutabilityPeriodSinceCreationInDays;
            this.State = policy.State?.ToString();
            this.Etag = policy.ETag?.ToString();
            this.AllowProtectedAppendWrites = policy.AllowProtectedAppendWrites;
            this.AllowProtectedAppendWritesAll = policy.AllowProtectedAppendWritesAll;

            List<PSUpdateHistoryProperty> updateHistoryList = new List<PSUpdateHistoryProperty>();
            if (policy.UpdateHistory != null && policy.UpdateHistory.Count != 0)
            {
                foreach (Track2Models.UpdateHistoryEntry updateHistoryItem in policy.UpdateHistory)
                {
                    updateHistoryList.Add(new PSUpdateHistoryProperty(updateHistoryItem));
                }
            }
            this.UpdateHistory = updateHistoryList.ToArray();
        }

        public int? ImmutabilityPeriodSinceCreationInDays { get; set; }
        public string State { get; set; }
        public string Etag { get; set; }
        public PSUpdateHistoryProperty[] UpdateHistory { get; set; }
        public bool? AllowProtectedAppendWrites { get; set; }
        public bool? AllowProtectedAppendWritesAll { get; set; }
    }

    public enum PSPublicAccess
    {
        None = 0,
        Container = 1,
        Blob = 2
    }

    public class PSUpdateHistoryProperty
    {
        public PSUpdateHistoryProperty(Track2Models.UpdateHistoryEntry updateHistory)
        {
            this.Update = updateHistory.UpdateType.ToString();
            this.ImmutabilityPeriodSinceCreationInDays = updateHistory.ImmutabilityPeriodSinceCreationInDays;
            this.Timestamp = updateHistory.Timestamp;
            this.ObjectIdentifier = updateHistory.ObjectIdentifier;
            this.TenantId = updateHistory.TenantId?.ToString();
            this.Upn = updateHistory.Upn;
            this.AllowProtectedAppendWrites = updateHistory.AllowProtectedAppendWrites;
            this.AllowProtectedAppendWritesAll = updateHistory.AllowProtectedAppendWritesAll;
        }

        public string Update { get; set; }
        public int? ImmutabilityPeriodSinceCreationInDays { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public string ObjectIdentifier { get; set; }
        public string TenantId { get; set; }
        public string Upn { get; set; }
        public bool? AllowProtectedAppendWrites { get; set; }
        public bool? AllowProtectedAppendWritesAll { get; set; }
    }

    public class PSImmutableStorageWithVersioning
    {
        public PSImmutableStorageWithVersioning(ImmutableStorageWithVersioning inputValue)
        {
            this.Enabled = inputValue.Enabled;
            this.TimeStamp = inputValue.TimeStamp;
            this.MigrationState = inputValue.MigrationState;
        }

        public PSImmutableStorageWithVersioning(Track2Models.ImmutableStorageWithVersioning inputValue)
        {
            this.Enabled = inputValue.IsEnabled;
            this.TimeStamp = inputValue.TimeStamp;
            this.MigrationState = inputValue.MigrationState.ToString();
        }

        public bool? Enabled { get; set; }
        public DateTimeOffset? TimeStamp { get; }
        public string MigrationState { get; }
    }
}
