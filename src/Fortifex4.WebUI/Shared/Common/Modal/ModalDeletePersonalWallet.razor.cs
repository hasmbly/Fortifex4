using Fortifex4.Shared.Wallets.Commands.DeleteWallet;
using Microsoft.AspNetCore.Components;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalDeletePersonalWallet
    {
        public string Title { get; set; } = "Delete Wallet";

        [Parameter]
        public int WalletID { get; set; }

        public BaseModal BaseModal { get; set; }

        public bool IsLoading { get; set; } = false;

        private async void OnSubmitPersonalWalletAsync()
        {
            IsLoading = true;

            var result = await _walletsService.DeleteWallet(new DeleteWalletRequest() { WalletID = WalletID });

            if (result.Status.IsError)
            {
                System.Console.WriteLine($"IsError: {result.Status.Message}");
            }
            else
            {
                if (result.Result.IsSuccessful)
                {
                    _navigationManager.NavigateTo($"/wallets");

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