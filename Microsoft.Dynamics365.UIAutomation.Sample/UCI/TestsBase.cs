﻿using System;
using System.Configuration;
using System.Security;
using Microsoft.Dynamics365.UIAutomation.Api.UCI;
using Microsoft.Dynamics365.UIAutomation.Browser;

namespace Microsoft.Dynamics365.UIAutomation.Sample.UCI
{
    public class TestsBase
    {
        protected readonly Uri _xrmUri = new Uri(ConfigurationManager.AppSettings["OnlineCrmUrl"]);
        protected readonly SecureString _username = ConfigurationManager.AppSettings["OnlineUsername"]?.ToSecureString();
        protected readonly SecureString _password = ConfigurationManager.AppSettings["OnlinePassword"]?.ToSecureString();
        protected readonly SecureString _mfaSecretKey = ConfigurationManager.AppSettings["MfaSecretKey"]?.ToSecureString();
  
        protected XrmApp _xrmApp;
        protected WebClient _client;
      
        public virtual void InitTest()
        {
            try
            {  
                CreateApp();
                NavigateToHomePage();
            }
            catch
            {
                CloseApp();
                throw;
            }
        }

        public virtual void FinishTest()
        {
            CloseApp();
        }

        public XrmApp CreateApp(BrowserOptions options = null)
        {
            options = options ?? TestSettings.Options;
            SetOptions(options);

            _client = new WebClient(options);
            _xrmApp = new XrmApp(_client);

            _xrmApp.OnlineLogin.Login(_xrmUri, _username, _password, _mfaSecretKey);

            return _xrmApp;
        }

        public void CloseApp()
        {
            _xrmApp?.Dispose();
            _xrmApp = null;
            _client = null;
        }
        
        public virtual void SetOptions(BrowserOptions options) { }

        public virtual void NavigateToHomePage() {}
        
        public virtual void NavigateTo(string appName, string area = null, string subarea = null)
        {
            _xrmApp.Navigation.OpenApp(appName);

            var hasArea = !string.IsNullOrWhiteSpace(area);
            var hasSubArea = !string.IsNullOrWhiteSpace(subarea);
            if(hasArea && hasSubArea)
                _xrmApp.Navigation.OpenSubArea(area, subarea);
            else if(hasArea)
                _xrmApp.Navigation.OpenArea(area);
            else if(hasSubArea)
                _xrmApp.Navigation.OpenSubArea(subarea);
        }
    }
}