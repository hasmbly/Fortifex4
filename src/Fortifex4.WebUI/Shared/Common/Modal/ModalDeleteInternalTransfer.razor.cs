using System;
using Fortifex4.Shared.InternalTransfers.Commands.DeleteInternalTransfer;
using Microsoft.AspNetCore.Components;

namespace Fortifex4.WebUI.Shared.Common.Modal
{
    public partial class ModalDeleteInternalTransfer
    {
        public BaseModal BaseModal { get; set; }

        public string Title { get; set; } = "Delete Internal Transfer";

        public string Message { get; set; } = "Internal Transfer";

        public int ID { get; set; }

        [Parameter]
        public EventCallback<bool> OnAfterSuccessful { get; set; }

        public bool IsLoading { get; set; } = false;

        private void SetID(int value) => ID = value;

        private async void OnSubmitAsync()
        {
            IsLoading = true;

            var result = await _internalTransfersService.DeleteInternalTransfer(new DeleteInternalTransferRequest() { InternalTransfersID = ID });

            if (result.Status.IsError)
            {
                Console.WriteLine($"IsError: {result.Status.Message}");
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
                    Console.WriteLine($"ErrorMessage: {result.Result.ErrorMessage}");
                }
            }
        }
    }
}