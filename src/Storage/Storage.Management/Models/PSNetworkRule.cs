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

using Microsoft.Azure.Management.Storage.Models;
using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Commands.Common.Attributes;
using Azure.ResourceManager.Storage.Models;

namespace Microsoft.Azure.Commands.Management.Storage.Models
{
    //Wrapper of NetworkRuleSet property Bypass 
    [Flags]
    public enum PSNetWorkRuleBypassEnum
    {
        None = 0,
        Logging = 1,
        Metrics = 2,
        AzureServices = 4
    }

    //Wrapper of NetworkRuleSet property DefaultAction 
    public enum PSNetWorkRuleDefaultActionEnum
    {
        Allow = 0,
        Deny = 1
    }

    //Wrapper of NetworkRule property Action 
    public enum PSNetworkRuleActionEnum
    {
        Allow = 0
    }

    //Wrapper of NetworkRuleSet property IpRule 
    public struct PSIpRule
    {
        public PSNetworkRuleActionEnum? Action;
        public string IPAddressOrRange;
    }

    //Wrapper of NetworkRuleSet property NetworkRule 
    public struct PSVirtualNetworkRule
    {
        public PSNetworkRuleActionEnum? Action;
        public string VirtualNetworkResourceId;
        public string State;
    }

    public struct PSResourceAccessRule
    {
        public string TenantId;
        public string ResourceId;
    }

    //Wrapper of NetworkRuleSet  
    public class PSNetworkRuleSet
    {
        [Ps1Xml(Label = "IpRules", Target = ViewControl.List, ScriptBlock = "if (($_.ipRules -ne $null) -and ($_.ipRules.Count -ne 0)) {\"[\" + $_.ipRules[0].IPAddressOrRange + \",...]\"} else {$null}", Position = 2)]
        public PSIpRule[] IpRules { get; set; }

        [Ps1Xml(Label = "VirtualNetworkRules", Target = ViewControl.List, ScriptBlock = "if ($_.virtualNetworkRules[0] -ne $null) {\"[\" + $_.virtualNetworkRules[0].VirtualNetworkResourceId + \",...]\"} else {$null}", Position = 3)]
        public PSVirtualNetworkRule[] VirtualNetworkRules { get; set; }

        [Ps1Xml(Label = "Bypass", Target = ViewControl.List, Position = 0)]
        public PSNetWorkRuleBypassEnum? Bypass { get; set; }

        [Ps1Xml(Label = "DefaultAction", Target = ViewControl.List, Position = 1)]
        public PSNetWorkRuleDefaultActionEnum DefaultAction { get; set; }

        public PSResourceAccessRule[] ResourceAccessRules { get; set; }


        //Parse NetworkRule property Action in SDK to wrapped property PSNetworkRuleActionEnum
        public static PSNetworkRuleActionEnum? ParsePSNetworkRuleAction(string action)
        {
            if (action == null)
            {
                return null;
            }

            if (string.Equals(action.ToUpper(), "ALLOW"))
            {
                return PSNetworkRuleActionEnum.Allow;
            }

            return PSNetworkRuleActionEnum.Allow;
        }


        //Parse wrapped property PSNetworkRuleActionEnum to NetworkRule rule property Action in SDK 
        public static Microsoft.Azure.Management.Storage.Models.Action? ParseStorageNetworkRuleAction(PSNetworkRuleActionEnum? action)
        {
            if (action == null)
            {
                return null;
            }

            if (action == PSNetworkRuleActionEnum.Allow)
            {
                return Microsoft.Azure.Management.Storage.Models.Action.Allow;
            }
            return Microsoft.Azure.Management.Storage.Models.Action.Allow;
        }

        //Parse NetworkRule property Bypass in SDK to wrapped property PSNetworkRuleBypassEnum
        public static PSNetWorkRuleBypassEnum? ParsePSNetworkRuleBypass(string bypass)
        {
            if (bypass == null)
            {
                return null;
            }

            PSNetWorkRuleBypassEnum returnBypass = PSNetWorkRuleBypassEnum.None;

            if (bypass.ToLower().Contains(PSNetWorkRuleBypassEnum.Logging.ToString().ToLower()))
                returnBypass = returnBypass | PSNetWorkRuleBypassEnum.Logging;
            if (bypass.ToLower().Contains(PSNetWorkRuleBypassEnum.Metrics.ToString().ToLower()))
                returnBypass = returnBypass | PSNetWorkRuleBypassEnum.Metrics;
            if (bypass.ToLower().Contains(PSNetWorkRuleBypassEnum.AzureServices.ToString().ToLower()))
                returnBypass = returnBypass | PSNetWorkRuleBypassEnum.AzureServices;

            return returnBypass;
        }

        //Parse wrapped property PSNetworkRuleBypassEnum to NetworkRule property Bypass in SDK
        public static string ParseStorageNetworkRuleBypass(PSNetWorkRuleBypassEnum? bypass)
        {
            if (bypass == null)
            {
                return null;
            }

            string returnBypass = string.Empty;

            //Build the Bypass string for input of SDK, format "[value1],[value2],...", e.g: "Metrics,AzureServices"
            if ((bypass & PSNetWorkRuleBypassEnum.Logging) == PSNetWorkRuleBypassEnum.Logging)
            {
                returnBypass += PSNetWorkRuleBypassEnum.Logging.ToString() + ",";
            }
            if ((bypass & PSNetWorkRuleBypassEnum.Metrics) == PSNetWorkRuleBypassEnum.Metrics)
            {
                returnBypass += PSNetWorkRuleBypassEnum.Metrics.ToString() + ",";
            }
            if ((bypass & PSNetWorkRuleBypassEnum.AzureServices) == PSNetWorkRuleBypassEnum.AzureServices)
            {
                returnBypass += PSNetWorkRuleBypassEnum.AzureServices.ToString() + ",";
            }

            if (returnBypass == string.Empty)
            {
                returnBypass = PSNetWorkRuleBypassEnum.None.ToString();
            }
            else
            {
                //remove the last ","
                returnBypass = returnBypass.Substring(0, returnBypass.Length - 1);
            }

            return returnBypass;
        }

        //Parse NetworkRule property DefaultAction in SDK to wrapped property PSNetworkRuleDefaultActionEnum
        public static PSNetWorkRuleDefaultActionEnum ParsePSNetworkRuleDefaultAction(global::Azure.ResourceManager.Storage.Models.DefaultAction defaultAction)
        {
            if (defaultAction == global::Azure.ResourceManager.Storage.Models.DefaultAction.Allow)
            {
                return PSNetWorkRuleDefaultActionEnum.Allow;
            }
            else
            {
                return PSNetWorkRuleDefaultActionEnum.Deny;
            }
        }

        //Parse wrapped property PSNetworkRuleDefaultActionEnum to NetworkRule property DefaultAction in SDK
        public static global::Azure.ResourceManager.Storage.Models.DefaultAction ParseStorageNetworkRuleDefaultAction(PSNetWorkRuleDefaultActionEnum defaultAction)
        {
            if (defaultAction == PSNetWorkRuleDefaultActionEnum.Allow)
            {
                return global::Azure.ResourceManager.Storage.Models.DefaultAction.Allow;
            }
            else
            {
                return global::Azure.ResourceManager.Storage.Models.DefaultAction.Deny;
            }
        }

        //Parse single NetworkRule IpRule in SDK to wrapped property PSIpRule
        public static PSIpRule ParsePSNetworkRuleIPRule(global::Azure.ResourceManager.Storage.Models.IPRule ipRule)
        {
            PSIpRule returnRule = new PSIpRule();
            returnRule.Action = ipRule.Action != null ? ParsePSNetworkRuleAction(ipRule.Action) : null;
            //returnRule.Action = ParsePSNetworkRuleAction(ipRule.Action);
            returnRule.IPAddressOrRange = ipRule.IPAddressOrRange;
            return returnRule;
        }

        //Parse wrapped property PSIpRule to single NetworkRule IpRule in SDK
        public static global::Azure.ResourceManager.Storage.Models.IPRule ParseStorageNetworkRuleIPRule(PSIpRule ipRule)
        {
            global::Azure.ResourceManager.Storage.Models.IPRule returnRule = new global::Azure.ResourceManager.Storage.Models.IPRule(ipRule.IPAddressOrRange);
            returnRule.Action = ParseStorageNetworkRuleAction(ipRule.Action).Value.ToString();
            return returnRule;
        }

        //Parse single NetworkRule PSResourceAccessRule in SDK to wrapped property PSPSResourceAccessRule
        public static PSResourceAccessRule ParsePSResourceAccessRule(global::Azure.ResourceManager.Storage.Models.ResourceAccessRule rule)
        {
            PSResourceAccessRule returnRule = new PSResourceAccessRule();
            returnRule.TenantId = rule.TenantId;
            returnRule.ResourceId = rule.ResourceId;
            return returnRule;
        }

        //Parse wrapped property PSPSResourceAccessRule to single NetworkRule PSResourceAccessRule in SDK
        public static global::Azure.ResourceManager.Storage.Models.ResourceAccessRule ParseStorageResourceAccessRule(PSResourceAccessRule rule)
        {
            global::Azure.ResourceManager.Storage.Models.ResourceAccessRule returnRule = 
                new global::Azure.ResourceManager.Storage.Models.ResourceAccessRule();
            returnRule.TenantId = rule.TenantId;
            returnRule.ResourceId = rule.ResourceId;
            return returnRule;
        }

        //Parse single NetworkRule VirtualNetworkRule in SDK to wrapped property PSVirtualNetworkRule
        public static PSVirtualNetworkRule ParsePSNetworkRuleVirtualNetworkRule(global::Azure.ResourceManager.Storage.Models.VirtualNetworkRule virtualNetworkRule)
        {
            PSVirtualNetworkRule returnRule = new PSVirtualNetworkRule();
            returnRule.Action = ParsePSNetworkRuleAction(virtualNetworkRule.Action);
            returnRule.VirtualNetworkResourceId = virtualNetworkRule.VirtualNetworkResourceId;
            returnRule.State = virtualNetworkRule.State.ToString();

            return returnRule;
        }

        //Parse wrapped property PSVirtualNetworkRule to single NetworkRule VirtualNetworkRule in SDK
        public static global::Azure.ResourceManager.Storage.Models.VirtualNetworkRule ParseStorageNetworkRuleVirtualNetworkRule(PSVirtualNetworkRule virtualNetworkRule)
        {
            global::Azure.ResourceManager.Storage.Models.VirtualNetworkRule returnRule = 
                new global::Azure.ResourceManager.Storage.Models.VirtualNetworkRule(virtualNetworkRule.VirtualNetworkResourceId);
            var action = ParseStorageNetworkRuleAction(virtualNetworkRule.Action);
            returnRule.Action = action != null ? action.Value.ToString() : null;

            return returnRule;
        }

        //Parse Storage NetworkRule object in SDK to wrapped PSNetworkRuleSet
        public static PSNetworkRuleSet ParsePSNetworkRule(global::Azure.ResourceManager.Storage.Models.NetworkRuleSet rules)
        {
            if (rules == null)
            {
                return null;
            }
            PSNetworkRuleSet returnRules = new PSNetworkRuleSet();
            returnRules.Bypass = rules.Bypass != null ? ParsePSNetworkRuleBypass(rules.Bypass.ToString()) : null;
            returnRules.DefaultAction = ParsePSNetworkRuleDefaultAction(rules.DefaultAction);

            List<PSIpRule> ipRuleList = new List<PSIpRule>();
            if (rules.IPRules != null)
            {
                foreach (var ipRule in rules.IPRules)
                {
                    ipRuleList.Add(ParsePSNetworkRuleIPRule(ipRule));
                }
                returnRules.IpRules = ipRuleList.ToArray();
            }

            List<PSVirtualNetworkRule> virtualNetworkList = new List<PSVirtualNetworkRule>();
            if (rules.VirtualNetworkRules != null)
            {
                foreach (var virtualNetworkRule in rules.VirtualNetworkRules)
                {
                    virtualNetworkList.Add(ParsePSNetworkRuleVirtualNetworkRule(virtualNetworkRule));
                }
                returnRules.VirtualNetworkRules = virtualNetworkList.ToArray();
            }

            List<PSResourceAccessRule> resourceAccessRuleList = new List<PSResourceAccessRule>();
            if (rules.ResourceAccessRules != null)
            {
                foreach (var rule in rules.ResourceAccessRules)
                {
                    resourceAccessRuleList.Add(ParsePSResourceAccessRule(rule));
                }
                returnRules.ResourceAccessRules = resourceAccessRuleList.ToArray();
            }

            return returnRules;
        }

        //Parse wrapped PSNetworkRuleSet to storage NetworkRule object in SDK
        public static global::Azure.ResourceManager.Storage.Models.NetworkRuleSet ParseStorageNetworkRule(PSNetworkRuleSet rules)
        {
            if (rules == null)
            {
                return null;
            }
            global::Azure.ResourceManager.Storage.Models.NetworkRuleSet returnRules = 
                new global::Azure.ResourceManager.Storage.Models.NetworkRuleSet(ParseStorageNetworkRuleDefaultAction(rules.DefaultAction));
            returnRules.Bypass = ParseStorageNetworkRuleBypass(rules.Bypass);

            if (rules.IpRules != null)
            {
                foreach (var ipRule in rules.IpRules)
                {
                    returnRules.IPRules.Add(ParseStorageNetworkRuleIPRule(ipRule));
                }
            }

            if (rules.VirtualNetworkRules != null)
            {
                foreach (var virtualNetworkRule in rules.VirtualNetworkRules)
                {
                    returnRules.VirtualNetworkRules.Add(ParseStorageNetworkRuleVirtualNetworkRule(virtualNetworkRule));
                }
            }

            if (rules.ResourceAccessRules != null)
            {
                foreach (var rule in rules.ResourceAccessRules)
                {
                    returnRules.ResourceAccessRules.Add(ParseStorageResourceAccessRule(rule));
                }
            }

            return returnRules;
        }
    }
}
