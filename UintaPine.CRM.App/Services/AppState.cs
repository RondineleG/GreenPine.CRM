using System;
using System.Collections.Generic;
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
                NotifyStateChanged();
            }
        }

        private List<Company> _companies { get; set; } = new List<Company>();
        public List<Company> Companies
        {
            get
            {
                return _companies;
            }
            set
            {
                _companies = value;
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
