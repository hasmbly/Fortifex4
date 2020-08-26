using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.Blockchains.Queries.GetAllBlockchains;
using Fortifex4.Shared.Wallets.Commands.CreatePersonalWallet;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalCreatePersonalWallet
    {
        public string Title { get; set; } = "New Personal Wallet";

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        public string MetaMaskMessage { get; set; }

        public bool IsEthereum { get; set; }

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        [Parameter]
        public EventCallback<bool> OnAfterSuccessful { get; set; }

        public BaseModal BaseModal { get; set; }

        public ClaimsPrincipal User { get; set; }

        public bool IsLoading { get; set; } = false;

        public CreatePersonalWalletRequest Input { get; set; } = new CreatePersonalWalletRequest();

        public string SelectedBlockchain
        {
            get => Input.BlockchainID.ToString();
            set
            {
                Input.BlockchainID = int.Parse(value);

                CheckIsEthereum(Input.BlockchainID);
            }
        }

        public IList<BlockchainDTO> Blockchains { get; set; } = new List<BlockchainDTO>();

        private void CheckIsEthereum(int blockchainID)
        {
            var isETH = Blockchains.Where(x => x.BlockchainID == blockchainID).First().Symbol;

            if (isETH == "ETH")
            {
                IsEthereum = true;
            }
            else
            {
                IsEthereum = false;
                MetaMaskMessage = string.Empty;
            }
        }

        private async Task ConnectToMetaMasukAsync()
        {
            IsLoading = true;

            var currentUrl = $"{_navigationManager.BaseUri}wallets";

            var isEthereumWalletInstalled = await JsRuntime.InvokeAsync<bool>("MetaMask.IsEthereumWalletInstalled");

            if (isEthereumWalletInstalled)
            {
                var isMetaMask = await JsRuntime.InvokeAsync<bool>("MetaMask.IsMetaMask");

                if (!isMetaMask)
                {
                    MetaMaskMessage = $"MetaMask not Installed on your Browser, please <a href=\"https://metamask.io/\" target=\"_blank\">Download here</a>, " +
                        $"after MetaMask was ready, <a href=\"{currentUrl}\">refresh this page</a>, then try again.";

                    IsLoading = false;
                }
                else
                {
                    var connectToMetaMask = await JsRuntime.InvokeAsync<List<string>>("MetaMask.ConnectToMetaMask");

                    if (connectToMetaMask.Count > 0)
                    {
                        string accountAddress = connectToMetaMask.First();

                        MetaMaskMessage = $"Succeed, MetaMask Accounts: {accountAddress}";

                        Input.Address = accountAddress;

                        IsLoading = false;
                    }
                }
            }
            else
            {
                MetaMaskMessage = $"MetaMask not Installed on your Browser, please <a href=\"https://metamask.io/\" target=\"_blank\">Download here</a>, " +
                    $"after MetaMask was ready, <a href=\"javascript:history.go(0)\">refresh this page</a>, then try again.";

                IsLoading = false;
            }
        }

        protected async override Task OnInitializedAsync()
        {
            IsLoading = true;

            User = Task.FromResult(await AuthenticationStateTask).Result.User;

            await LoadDataAsync();

            IsLoading = false;

            StateHasChanged();
        }

        private async Task LoadDataAsync()
        {
            var getAllBlockchains = await _blockchainsService.GetAllBlockchains();
            Blockchains = getAllBlockchains.Result.Blockchains.ToList();

            // default value for selecte option if user doesn't change the select option
            Input.BlockchainID = Blockchains.First().BlockchainID;
        }

        private async void OnSubmitPersonalWalletAsync()
        {
            StateHasChanged();

            IsLoading = true;

            Input.MemberUsername = User.Identity.Name;

            var result = await _walletsService.CreatePersonalWallet(Input);

            if (result.Status.IsError)
            {
                System.Console.WriteLine($"IsError: {result.Status.Message}");
            }
            else
            {
                if (result.Result.IsSuccessful)
                {
                    _navigationManager.NavigateTo($"/wallets/details/{result.Result.WalletID}");

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