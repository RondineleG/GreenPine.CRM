using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.UintaPine.Services
{
    //https://chrissainty.com/3-ways-to-communicate-between-components-in-blazor/
    public class AppState
    {
        private string _email { get; set; }
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                NotifyStateChanged();
            }
        }

        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
