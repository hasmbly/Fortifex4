using Fortifex4.Shared.Owners.Commands.DeleteOwner;
using Microsoft.AspNetCore.Components;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalDeleteExchange
    {
        public string Title { get; set; } = "Delete Exchange";

        [Parameter]
        public int OwnerID { get; set; }

        public BaseModal BaseModal { get; set; }

        public bool IsLoading { get; set; } = false;

        private async void OnSubmitAsync()
        {
            IsLoading = true;

            var result = await _ownersService.DeleteOwner(new DeleteOwnerRequest() { OwnerID = OwnerID });

            if (result.Status.IsError)
            {
                System.Console.WriteLine($"IsError: {result.Status.Message}");
            }
            else
            {
                if (result.Result.IsSuccessful)
                {
                    _navigationManager.NavigateTo($"/exchanges");

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