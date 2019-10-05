using System;
using UintaPine.CRM.Model.Shared.Responses;

namespace UintaPine.CRM.App.Services
{
    public class AppState
    {
        private User _user { get; set; }
        public User User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
                IsInitialized = true;
                NotifyStateChanged();
            }
        }

        private bool _isInitialized { get; set; } = false;
        public bool IsInitialized
        {
            get
            {
                return _isInitialized;
            }
            set
            {
                _isInitialized = value;
            }
        }

        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
