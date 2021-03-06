﻿using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Common;

namespace Fortifex4.Shared.Wallets.Queries.GetWallet
{
    public class GetWalletResponse : GeneralResponse
    {
        public int WalletID { get; set; }
        public int OwnerID { get; set; }
        public int BlockchainID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public ProviderType ProviderType { get; set; }
        public bool IsSynchronized { get; set; }

        public string BlockchainName { get; set; }
        public string OwnerProviderName { get; set; }
        public PocketDTO MainPocket { get; set; }

        public IList<PocketDTO> TokenPockets { get; set; }

        public string ProviderNameWithWalletName
        {
            get => $"{this.OwnerProviderName} - {this.Name}";
            set
            {
                return;
            }
        }

        public GetWalletResponse()
        {
            this.MainPocket = new PocketDTO();

            this.TokenPockets = new List<PocketDTO>();
        }
    }
}