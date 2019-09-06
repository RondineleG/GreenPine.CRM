using model.UintaPine.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.UintaPine.Services
{
    public class AppState
    {
        private UserSlim _user { get; set; }
        public UserSlim User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
                NotifyStateChanged();
            }
        }

        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
