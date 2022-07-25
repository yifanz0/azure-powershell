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

using Microsoft.Azure.Management.Internal.Resources.Utilities.Models;
using Microsoft.Azure.Management.Storage.Models;
using Microsoft.WindowsAzure.Commands.Common.Attributes;
using System;
using System.Collections.Generic;
using Track2 = Azure.ResourceManager.Storage;
using Track2Models = Azure.ResourceManager.Storage.Models;

namespace Microsoft.Azure.Commands.Management.Storage.Models
{
    /// <summary>
    /// Wrapper of SDK type FileServiceProperties
    /// </summary>
    public class PSFileServiceProperties
    {
        [Ps1Xml(Label = "ResourceGroupName", Target = ViewControl.Table, Position = 0)]
        public string ResourceGroupName { get; set; }
        [Ps1Xml(Label = "StorageAccountName", Target = ViewControl.Table, Position = 1)]
        public string StorageAccountName { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public string Type { get; set; }
        [Ps1Xml(Label = "DeleteRetentionPolicy.Enabled", Target = ViewControl.Table, ScriptBlock = "$_.DeleteRetentionPolicy.Enabled", Position = 2)]
        [Ps1Xml(Label = "DeleteRetentionPolicy.Days", Target = ViewControl.Table, ScriptBlock = "$_.DeleteRetentionPolicy.Days", Position = 3)]
        public PSDeleteRetentionPolicy ShareDeleteRetentionPolicy { get; set; }
        public PSCorsRules Cors { get; set; }
        public PSProtocolSettings ProtocolSettings { get; set; }

        public PSFileServiceProperties()
        { }

        public PSFileServiceProperties(Track2.FileServiceResource policy)
        {
            this.ResourceGroupName = (new ResourceIdentifier(policy.Id)).ResourceGroupName;
            this.StorageAccountName = PSBlobServiceProperties.GetStorageAccountNameFromResourceId(policy.Id);
            this.Id = policy.Id;
            this.Name = policy.Data.Name;
            this.Type = policy.Data.ResourceType;
            this.Cors = policy.Data.CorsRules is null ? null : new PSCorsRules(policy.Data.CorsRules);
            this.ShareDeleteRetentionPolicy = policy.Data.ShareDeleteRetentionPolicy is null ? null : new PSDeleteRetentionPolicy(policy.Data.ShareDeleteRetentionPolicy);
            this.ProtocolSettings = policy.Data.ProtocolSmbSetting is null ? null : new PSProtocolSettings(policy.Data.ProtocolSmbSetting);
        }
    }
    public class PSProtocolSettings
    {
        public PSSmbSetting Smb { get; set; }

        public PSProtocolSettings(Track2Models.SmbSetting smbSetting)
        {
            this.Smb = smbSetting is null ? null : new PSSmbSetting(smbSetting);
        }
    }

    public class PSSmbSetting
    {
        public string[] Versions { get; set; }
        public string[] AuthenticationMethods { get; set; }
        public string[] KerberosTicketEncryption { get; set; }
        public string[] ChannelEncryption { get; set; }
        public PSMultichannel Multichannel { get; set; }

        public PSSmbSetting(Track2Models.SmbSetting smbSetting)
        {
            this.Versions = smbSetting.Versions?.Split(new char[] { ';' });
            this.AuthenticationMethods = smbSetting.AuthenticationMethods?.Split(new char[] { ';' });
            this.KerberosTicketEncryption = smbSetting.KerberosTicketEncryption?.Split(new char[] { ';' });
            this.ChannelEncryption = smbSetting.ChannelEncryption?.Split(new char[] { ';' });
            this.Multichannel = smbSetting.IsMultiChannelEnabled is null ? null : new PSMultichannel(smbSetting.IsMultiChannelEnabled);
        }
    }

    public class PSMultichannel
    {
        public bool? Enabled { get; set; }

        public PSMultichannel(Multichannel multichannel)
        {
            this.Enabled = multichannel.Enabled;
        }

        public PSMultichannel(bool? isMultiChannelEnabled)
        {
            this.Enabled = isMultiChannelEnabled;
        }
    }
}

