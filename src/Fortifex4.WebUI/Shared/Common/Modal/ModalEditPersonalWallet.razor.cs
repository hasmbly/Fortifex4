using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.Blockchains.Queries.GetAllBlockchains;
using Fortifex4.Shared.Wallets.Commands.CreatePersonalWallet;
using Fortifex4.Shared.Wallets.Commands.UpdatePersonalWallet;
using Fortifex4.Shared.Wallets.Queries.GetWallet;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalEditPersonalWallet
    {
        public string Title { get; set; } = "Edit Personal Wallet";

        [Parameter]
        public EventCallback<bool> OnAfterSuccessful { get; set; }

        [Parameter]
        public int WalletID { get; set; }

        public BaseModal BaseModal { get; set; }

        public bool IsLoading { get; set; } = false;

        public UpdatePersonalWalletRequest Input { get; set; } = new UpdatePersonalWalletRequest();

        public GetWalletResponse Wallet { get; set; } = new GetWalletResponse();

        public string SelectedBlockchain
        {
            get => Input.BlockchainID.ToString();
            set => Input.BlockchainID = int.Parse(value);
        }

        public IList<BlockchainDTO> Blockchains { get; set; } = new List<BlockchainDTO>();

        protected async override Task OnInitializedAsync()
        {
            IsLoading = true;

            await LoadDataAsync();

            IsLoading = false;
        }

        private async Task LoadDataAsync()
        {
            await LoadWalletAsync();
            
            LoadExistingData();

            LoadBlockchainsAsync();
        }

        private async Task LoadWalletAsync()
        {
            var result = await _walletsService.GetWallet(WalletID);

            if (result.Result.IsSuccessful)
                Wallet = result.Result;
        }

        private void LoadExistingData()
        {
            Input.WalletID = Wallet.WalletID;
            Input.Name = Wallet.Name;
            Input.Address = Wallet.Address;
            Input.BlockchainID = Wallet.BlockchainID;
        }

        private async void LoadBlockchainsAsync()
        {
            var getAllBlockchains = await _blockchainsService.GetAllBlockchains();
            Blockchains = getAllBlockchains.Result.Blockchains.ToList();
        }

        private async void OnSubmitEditPersonalWalletAsync()
        {
            StateHasChanged();

            IsLoading = true;

            var result = await _walletsService.UpdatePersonalWallet(Input);

            if (result.Status.IsError)
            {
                System.Console.WriteLine($"IsError: {result.Status.Message}");
            }
            else
            {
                if (result.Result.IsSuccessful)
                {
                    IsLoading = false;

                    await OnAfterSuccessful.InvokeAsync(true);

                    BaseModal.Close();
                }
                else
                {
                    System.Console.WriteLine($"ErrorMessage: {result.Result.ErrorMessage}");
                }
            }
        }
    }
}