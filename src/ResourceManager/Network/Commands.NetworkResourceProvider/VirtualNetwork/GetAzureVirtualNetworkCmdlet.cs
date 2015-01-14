﻿// ----------------------------------------------------------------------------------
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

using System.Collections.Generic;
using System.Management.Automation;
using AutoMapper;
using Microsoft.Azure.Management.Network;
using Microsoft.Azure.Commands.NetworkResourceProvider.Models;
using MNM = Microsoft.Azure.Management.Network.Models;

namespace Microsoft.Azure.Commands.NetworkResourceProvider
{
     [Cmdlet(VerbsCommon.Get, "AzureVirtualNetwork"), OutputType(typeof(PSVirtualNetwork))]
    public class GetAzureVirtualNetworkCmdlet : VirtualNetworkBaseClient
    {
        [Alias("ResourceName")]
        [Parameter(
            Mandatory = false,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The resource name.")]
        [ValidateNotNullOrEmpty]
        public virtual string Name { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipelineByPropertyName = true,
            HelpMessage = "The resource group name.")]
        [ValidateNotNullOrEmpty]
        public virtual string ResourceGroupName { get; set; }

        public override void ExecuteCmdlet()
        {
            base.ExecuteCmdlet();
            if (!string.IsNullOrEmpty(this.Name))
            {
                var vnet = this.GetVirtualNetwork(this.ResourceGroupName, this.Name);

                WriteObject(vnet);
            }
            else
            {
                var vnetGetResponse = this.VirtualNetworkClient.List(this.ResourceGroupName);

                var vnets = Mapper.Map<List<PSVirtualNetwork>>(vnetGetResponse.VirtualNetworks);

                // populate the virtualNetworks with the ResourceGroupName
                foreach (var vnet in vnets)
                {
                    vnet.ResourceGroupName = this.ResourceGroupName;
                }

                WriteObject(vnets, true);
            }
        }
    }
}
