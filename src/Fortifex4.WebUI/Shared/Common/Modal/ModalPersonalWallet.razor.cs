using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Fortifex4.Shared.Blockchains.Queries.GetAllBlockchains;
using Fortifex4.Shared.Wallets.Commands.CreatePersonalWallet;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalPersonalWallet
    {
        public string Title { get; set; } = "New Wallet";

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
            set => Input.BlockchainID = int.Parse(value);
        }

        public IList<BlockchainDTO> Blockchains { get; set; } = new List<BlockchainDTO>();

        protected async override Task OnInitializedAsync()
        {
            IsLoading = true;

            User = Task.FromResult(await AuthenticationStateTask).Result.User;

            await LoadDataAsync();

            IsLoading = false;

            System.Console.WriteLine($"BlockchainID: {Input.BlockchainID}");

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