using System.Threading.Tasks;
using Fortifex4.Shared.Pockets.Queries.GetPocket;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Fortifex4.WebUI.Pages.Tokens
{
    public partial class Details
    {
        [Parameter]
        public int PocketID { get; set; }

        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        public bool FirstStage { get; set; }

        public string TokensTableID { get; set; } = "data-table-tokens";

        public GetPocketResponse Pocket { get; set; } = new GetPocketResponse();

        public bool IsLoading { get; set; }

        protected async override Task OnInitializedAsync() => await InitAsync();

        private async void UpdateStateHasChanged(bool IsSuccessful)
        {
            if (IsSuccessful)
                await InitAsync();
        }

        private async Task InitAsync()
        {
            IsLoading = true;

            var result = await _walletsService.GetPocket(PocketID);

            if (result.Result.IsSuccessful)
                Pocket = result.Result;

            IsLoading = false;

            StateHasChanged();

            FirstStage = true;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                System.Console.WriteLine($"OnAfterRender - This is First Render");
            }
            else if (FirstStage)
            {
                FirstStage = false;

                await JsRuntime.InvokeVoidAsync("DataTable.init", $"#{TokensTableID}");
            }
        }
    }
}