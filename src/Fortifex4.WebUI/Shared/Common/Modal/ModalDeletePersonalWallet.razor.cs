using Fortifex4.Domain.Enums;
using Fortifex4.Shared.Wallets.Commands.DeleteWallet;
using Fortifex4.Shared.Wallets.Queries.GetWallet;
using Microsoft.AspNetCore.Components;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalDeletePersonalWallet
    {
        public string Title { get; set; } = "Delete Wallet";

        [Parameter]
        public GetWalletResponse Wallet { get; set; }

        public BaseModal BaseModal { get; set; }

        public bool IsLoading { get; set; } = false;

        private async void OnSubmitPersonalWalletAsync()
        {
            IsLoading = true;

            var result = await _walletsService.DeleteWallet(new DeleteWalletRequest() { WalletID = Wallet.WalletID });

            if (result.Status.IsError)
            {
                System.Console.WriteLine($"IsError: {result.Status.Message}");
            }
            else
            {
                if (result.Result.IsSuccessful)
                {
                    if (Wallet.ProviderType == ProviderType.Personal)
                        _navigationManager.NavigateTo($"/wallets");
                    else
                        _navigationManager.NavigateTo($"/exchanges/details/{Wallet.OwnerID}");

                    IsLoading = false;

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