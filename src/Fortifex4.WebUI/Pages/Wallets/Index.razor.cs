using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.Wallets.Commands.CreatePersonalWallet;
using Fortifex4.Shared.Wallets.Queries.GetMyPersonalWallets;
using Fortifex4.Shared.Wallets.Queries.GetPersonalWallets;
using Fortifex4.WebUI.Shared.Common.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace Fortifex4.WebUI.Pages.Wallets
{
    public partial class Index
    {
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        public string MetaMaskMessage { get; set; }

        public string MetaMaskAccountAddress { get; set; }

        public CreatePersonalWalletRequest Input { get; set; } = new CreatePersonalWalletRequest();

        private bool _disposed = false;

        [CascadingParameter]
        public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        public ClaimsPrincipal User { get; set; }

        public GetMyPersonalWalletsResponse GetMyPersonalWalletsResponse { get; set; } = new GetMyPersonalWalletsResponse();
        public GetPersonalWalletsResponse GetPersonalWalletsResponse { get; set; } = new GetPersonalWalletsResponse();

        private ModalCreatePersonalWallet ModalCreatePersonalWallet { get; set; }

        public bool IsLoading { get; set; }

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
                        MetaMaskAccountAddress = connectToMetaMask.First();

                        MetaMaskMessage = $"Succeed, MetaMask Accounts: {MetaMaskAccountAddress}";

                        await CreateMetamaskWallet();

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

        private async Task CreateMetamaskWallet()
        {
            Input.Name = "MetaMask Account";
            Input.Address = MetaMaskAccountAddress;

            var ethereumID = Task.FromResult(await _blockchainsService.GetAllBlockchains())
                .Result.Result.Blockchains.ToList()
                .Where(x => x.Symbol == "ETH")
                .First().BlockchainID;

            Input.BlockchainID = ethereumID;
            Input.MemberUsername = User.Identity.Name;

            var createPersonalWallet = await _walletsService.CreatePersonalWallet(Input);

            if (createPersonalWallet.Status.IsError)
            {
                Console.WriteLine($"IsError: {createPersonalWallet.Status.Message}");
            }
            else
            {
                if (createPersonalWallet.Result.IsSuccessful)
                {
                    await _walletsService.SyncPersonalWallet(createPersonalWallet.Result.WalletID);

                    _navigationManager.NavigateTo($"/wallets/details/{createPersonalWallet.Result.WalletID}");
                }
                else
                {
                    Console.WriteLine($"ErrorMessage: {createPersonalWallet.Result.ErrorMessage}");
                }
            }
        }

        protected async override Task OnInitializedAsync()
        {
            globalState.ShouldRender += RefreshMe;

            await InitAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                globalState.ShouldRender -= RefreshMe;
            }

            _disposed = true;
        }

        private async void RefreshMe()
        {
            await InitAsync();
        }

        private async void UpdateStateHasChanged(bool IsSuccessful)
        {
            if (IsSuccessful)
                await InitAsync();
        }

        private async Task InitAsync()
        {
            IsLoading = true;

            User = Task.FromResult(await AuthenticationStateTask).Result.User;

            var result = await _walletsService.GetPersonalWallets(User.Identity.Name);

            if (result.Result.IsSuccessful)
                GetPersonalWalletsResponse = result.Result;
            IsLoading = false;

            StateHasChanged();
        }
    }
}