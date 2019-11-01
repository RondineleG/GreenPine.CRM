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

        private List<Organization> _companies { get; set; } = new List<Organization>();
        public List<Organization> Companies
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

        private string _lastOrganizationId { get; set; }
        public string LastOrganizationId
        {
            get
            {
                return _lastOrganizationId;
            }
            set
            {
                _lastOrganizationId = value;
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
