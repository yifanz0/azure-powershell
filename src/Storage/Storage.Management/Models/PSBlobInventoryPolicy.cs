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

using Track2 = global::Azure.ResourceManager.Storage;
using Azure.ResourceManager.Storage.Models;
using Microsoft.Azure.Management.Storage.Models;
using Microsoft.WindowsAzure.Commands.Common.Attributes;
using System;
using System.Collections.Generic;
using Azure.ResourceManager.Storage;

namespace Microsoft.Azure.Commands.Management.Storage.Models
{
    /// <summary>
    /// Wrapper of SDK type BlobInventoryPolicy 
    /// </summary>
    public class PSBlobInventoryPolicy
    {
        public PSBlobInventoryPolicy()
        { }

        public PSBlobInventoryPolicy(BlobInventoryPolicyResource policy, string resourceGroupName, string storageAccountName)
        {
            this.ResourceGroupName = resourceGroupName;
            this.StorageAccountName = storageAccountName;
            this.Id = policy.Id;
            this.Name = policy.Data.Name;
            this.Type = policy.Data.ResourceType;
            this.LastModifiedTime = policy.Data.LastModifiedOn;

            this.SystemData = policy.Data.SystemData is null ? null : new PSSystemData(policy.Data.SystemData);

            this.Enabled = policy.Data.Policy.Enabled;

            if (policy.Data.Policy.Rules != null)
            {
                List<PSBlobInventoryPolicyRule> psRules = new List<PSBlobInventoryPolicyRule>();
                foreach (Track2.Models.BlobInventoryPolicyRule rule in policy.Data.Policy.Rules)
                {
                    psRules.Add(new PSBlobInventoryPolicyRule(rule));
                }
                this.Rules= psRules.ToArray();
            } else
            {
                this.Rules = null;
            }
        }



        //public BlobInventoryPolicy ParseBlobInventoryPolicy()
        //{
        //    List<Track2.Models.BlobInventoryPolicyRule> invRules = ParseBlobInventoryPolicyRules(this.Rules);

        //    Track2.Models.BlobInventoryPolicySchema policySchema = new Track2.Models.BlobInventoryPolicySchema(
        //        this.Enabled,
        //        // Should Inventory be set here? 
        //        Track2.Models.InventoryRuleType.Inventory,
        //        invRules);


        //    //BlobInventoryPolicySchema policySchema = new BlobInventoryPolicySchema()
        //    //{
        //    //    Enabled = this.Enabled,
        //    //    //Destination = this.Destination,
        //    //    Rules = invRules
        //    //};


        //    return new BlobInventoryPolicy(
        //        policySchema,
        //        this.Id,
        //        this.Name,
        //        this.Type,
        //        this.LastModifiedTime,
        //        this.SystemData is null ? null : this.SystemData.ParseSystemData()
        //    );
        //}

        public static List<Track2.Models.BlobInventoryPolicyRule> ParseBlobInventoryPolicyRules(PSBlobInventoryPolicyRule[] rules)
        {
            List<Track2.Models.BlobInventoryPolicyRule> invRules = null;
            if (rules != null)
            {
                invRules = new List<Track2.Models.BlobInventoryPolicyRule>();
                foreach (PSBlobInventoryPolicyRule rule in rules)
                {
                    invRules.Add(rule.ParseBlobInventoryPolicyRule());
                }
            }
            return invRules;
        }

        [Ps1Xml(Label = "ResourceGroupName", Target = ViewControl.List, Position = 1)]
        public string ResourceGroupName { get; set; }
        [Ps1Xml(Label = "StorageAccountName", Target = ViewControl.List, Position = 0)]
        public string StorageAccountName { get; set; }
        [Ps1Xml(Label = "Id", Target = ViewControl.List, Position = 3)]
        public string Id { get; set; }
        public string Name { get; set; }
        [Ps1Xml(Label = "Type", Target = ViewControl.List, Position = 2)]
        public string Type { get; set; }
        [Ps1Xml(Label = "LastModifiedTime", Target = ViewControl.List, Position = 4)]
        public DateTimeOffset? LastModifiedTime { get; set; }

        //public PSBlobInventoryPolicySchema Policy { get; set; }

        [Ps1Xml(Label = "Enabled", Target = ViewControl.List, Position = 6)]
        public bool Enabled { get; set; }
        [Ps1Xml(Label = "Rules", Target = ViewControl.List, Position = 7)]
        public PSBlobInventoryPolicyRule[] Rules { get; set; }
        public PSSystemData SystemData { get; set; }
    }

    /// <summary>
    /// Wrapper of SDK type BlobInventoryPolicyRule
    /// </summary>
    public class PSBlobInventoryPolicyRule
    {
        public PSBlobInventoryPolicyRule() { }
        public PSBlobInventoryPolicyRule(Track2.Models.BlobInventoryPolicyRule rule)
        {
            this.Enabled = rule.Enabled;
            this.Name = rule.Name;
            this.Destination = rule.Destination;
            this.Definition = rule.Definition is null ? null : new PSBlobInventoryPolicyDefinition(rule.Definition);
        }

        public Track2.Models.BlobInventoryPolicyRule ParseBlobInventoryPolicyRule()
        {
            Track2.Models.BlobInventoryPolicyDefinition policyDefinition = this.Definition is null ? null : this.Definition.parseBlobInventoryPolicyDefinition();

            Track2.Models.BlobInventoryPolicyRule policyRule = new Track2.Models.BlobInventoryPolicyRule(
                this.Enabled,
                this.Name,
                this.Destination,
                policyDefinition);

            return policyRule;

            //return new BlobInventoryPolicyRule()
            //{
            //    Enabled = this.Enabled,
            //    Name = this.Name,
            //    Destination = this.Destination,
            //    Definition = this.Definition is null ? null : this.Definition.parseBlobInventoryPolicyDefinition()
            //};
        }

        public bool Enabled { get; set; }

        public string Name { get; set; }

        public string Destination { get; set; }

        public PSBlobInventoryPolicyDefinition Definition { get; set; }
    }

    /// <summary>
    /// Wrapper of SDK type BlobInventoryPolicyDefinition
    /// </summary>
    public class PSBlobInventoryPolicyDefinition
    {
        public PSBlobInventoryPolicyDefinition() { }

        public PSBlobInventoryPolicyDefinition(Track2.Models.BlobInventoryPolicyDefinition Definition)
        {
            this.Filters = Definition.Filters is null ? null : new PSBlobInventoryPolicyFilter(Definition.Filters);
            this.Format = Definition.Format.ToString();
            this.Schedule = Definition.Schedule.ToString();
            this.ObjectType = Definition.ObjectType.ToString();
            this.SchemaFields = Definition.SchemaFields is null ? null : ((List<string>)Definition.SchemaFields).ToArray();
        }

        public Track2.Models.BlobInventoryPolicyDefinition parseBlobInventoryPolicyDefinition()
        {
            Track2.Models.BlobInventoryPolicyDefinition policyDefinition = new Track2.Models.BlobInventoryPolicyDefinition(
                new Track2.Models.Format(this.Format),
                new Track2.Models.Schedule(this.Schedule),
                new Track2.Models.ObjectType(this.ObjectType),
                this.SchemaFields is null ? null : new List<string>(this.SchemaFields)

                
                );

            if (this.Filters != null)
            {
                policyDefinition.Filters = this.Filters.ParseBlobInventoryPolicyFilter();
            }


            return policyDefinition;


            //return new BlobInventoryPolicyDefinition(this.Format, this.Schedule, this.ObjectType, 
            //    this.SchemaFields is null? null: new List<string>(this.SchemaFields),
            //    this.Filters is null? null : this.Filters.ParseBlobInventoryPolicyFilter());
        }

        public PSBlobInventoryPolicyFilter Filters { get; set; }

        //Possible values include: 'Csv', 'Parquet'
        public string Format { get; set; }

        // Possible values include: 'Daily', 'Weekly'
        public string Schedule { get; set; }

        // Possible values include: 'Blob', 'Container'
        public string ObjectType { get; set; }

        //     Valid values for this field for the blob object type include: Name, Creation-Time, Last-Modified, Content-Length,
        //     Content-MD5, BlobType, AccessTier, AccessTierChangeTime, Expiry-Time, hdi_isfolder,
        //     Owner, Group, Permissions, Acl, Snapshot, VersionId, IsCurrentVersion, Metadata, LastAccessTime, AccessTierInferred, Tags. 
        //     Valid values for container object type include Name, Last-Modified,
        //     Metadata, LeaseStatus, LeaseState, LeaseDuration, PublicAccess, HasImmutabilityPolicy, HasLegalHold.
        public string[] SchemaFields { get; set; }

        //private string[] BlobSchemaField = new string[] {"Name", "Creation-Time", "Last-Modified", "Content-Length", "Content-MD5", "BlobType", "AccessTier", "AccessTierChangeTime",
        //            "Expiry-Time", "hdi_isfolder", "Owner", "Group", "Permissions", "Acl", "Snapshot", "VersionId", "IsCurrentVersion", "Metadata", "LastAccessTime"};
        //private string[] ContainerSchemaField = new string[] { "Name", "Last-Modified", "Metadata", "LeaseStatus", "LeaseState", "LeaseDuration", "PublicAccess", "HasImmutabilityPolicy", "HasLegalHold" };

        //public const string[] BlobInventoryPolicyRuleFormat = {"Csv", "Parquet"};

        //public static string NormalizeString(string input, string[] validValue)
        //{
        //    foreach (string s in validValue)
        //    {
        //        if (input.ToLower() == s.ToLower())
        //        {
        //            return s;
        //        }
        //    }
        //    return input;
        //}

    }

    /// <summary>
    /// Wrapper of SDK type BlobInventoryPolicyFilter
    /// </summary>
    public class PSBlobInventoryPolicyFilter
    {
        public PSBlobInventoryPolicyFilter() { }

        public PSBlobInventoryPolicyFilter(Track2.Models.BlobInventoryPolicyFilter filters)
        {
            this.PrefixMatch = PSManagementPolicyRuleFilter.StringListToArray(filters.PrefixMatch);
            this.BlobTypes = PSManagementPolicyRuleFilter.StringListToArray(filters.BlobTypes);
            this.IncludeBlobVersions = filters.IncludeBlobVersions;
            this.IncludeSnapshots = filters.IncludeSnapshots;
        }

        public Track2.Models.BlobInventoryPolicyFilter ParseBlobInventoryPolicyFilter()
        {
            Track2.Models.BlobInventoryPolicyFilter policyFilter =  new Track2.Models.BlobInventoryPolicyFilter()
            {
                //PrefixMatch = PSManagementPolicyRuleFilter.StringArrayToList(this.PrefixMatch),
                //BlobTypes = PSManagementPolicyRuleFilter.StringArrayToList(this.BlobTypes),
                IncludeSnapshots = this.IncludeSnapshots,
                IncludeBlobVersions = this.IncludeBlobVersions
            };

            if (this.PrefixMatch != null)
            {
                foreach (string prefixMatch in this.PrefixMatch)
                {
                    policyFilter.PrefixMatch.Add(prefixMatch);
                }
            }

            if (this.BlobTypes != null)
            {
                foreach (string blobType in this.BlobTypes)
                {
                    policyFilter.BlobTypes.Add(blobType);
                }
            }
            return policyFilter;

        }

        public string[] PrefixMatch { get; set; }
        public string[] BlobTypes { get; set; }
        public bool? IncludeBlobVersions { get; set; }
        public bool? IncludeSnapshots { get; set; }
    }

    /// <summary>
    /// Wrapper of SDK type SystemData
    /// </summary>
    public class PSSystemData
    {
        public PSSystemData() { }

        public PSSystemData(global::Azure.ResourceManager.Models.SystemData SystemData)
        {
            this.CreatedBy = SystemData.CreatedBy;
            this.CreatedByType = SystemData.CreatedByType.ToString();
            this.CreatedAt = SystemData.CreatedOn;
            this.LastModifiedBy = SystemData.LastModifiedBy;
            this.LastModifiedByType = SystemData.LastModifiedByType.ToString();
            this.LastModifiedAt = SystemData.LastModifiedOn;
        }

        //public global::Azure.ResourceManager.Models.SystemData ParseSystemData()
        //{
        //    return new global::Azure.ResourceManager.Models.SystemData
        //    {
        //        // how to set property values? 
        //    };
            


        //    //return new SystemData(this.CreatedBy, this.CreatedByType, this.CreatedAt, this.LastModifiedBy, this.LastModifiedByType, this.LastModifiedAt);
        //}

        public string CreatedBy { get; set; }
        public string CreatedByType { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
        public string LastModifiedBy { get; set; }
        public string LastModifiedByType { get; set; }
        public DateTimeOffset? LastModifiedAt { get; set; }
    }
}
