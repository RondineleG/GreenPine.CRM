using app.UintaPine.Pages;
using app.UintaPine.Services;
using Microsoft.AspNetCore.Components;
using model.UintaPine.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.UintaPine.Shared
{
    public class MainLayoutBase : LayoutComponentBase
    {
        [Inject]
        public AppState AppState { get; set; }

        private API _api { get; set; }

        public MainLayoutBase()
        {
            _api = new API();
        }

        protected override void OnInitialized()
        {
            AppState.OnChange += StateHasChanged;

        }

        async protected override Task OnInitializedAsync()
        {
            UserSlim user = await _api.GetUserCurrent();
            if (user?.Success == true)
            {
                AppState.User = user;
            }
        }

        async public Task SignOut()
        {
            await _api.Logout();
            AppState.User = null;
        }
    }
}
