using app.UintaPine.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.UintaPine.Shared
{
    public class MainLayoutBase : LayoutComponentBase
    {
        [Inject]
        protected AppState AppState { get; set; }

        public MainLayoutBase()
        {
            int a = 0;
            //AppState.SetEmail("start up email");
        }

        protected override void OnInitialized()
        {
            AppState.OnChange += StateHasChanged;
        }

        public void SignOut()
        {
            AppState.Email = null;
        }
    }
}
