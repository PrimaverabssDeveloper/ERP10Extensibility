using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Primavera.Platform.Email.DemoProvider
{
    /// <summary>
    /// Demo provider class.
    /// </summary>
    [Export(typeof(EmailProviderBase))]
    public class EmailProvider : EmailProviderBase
    {
        #region Private Properties

        private string configPath = string.Empty;
        private string accountID = string.Empty;
        private string clientSecret = string.Empty;

        #endregion

        public override System.Drawing.Image ProviderImage => Properties.Resources.imgPrimavera;

        public override System.Drawing.Icon ProviderIcon => Properties.Resources.GrupoPrimavera;

        public override string ProviderKey => "DEMOMAIL";

        public override string ProviderName => "DEMO EMAIL PROVIDER";

        public override ProviderConfiguration GetProviderConfigurationItems()
        {
            return new ProviderConfiguration()
            {
                ProviderKey = this.ProviderKey,
                ProviderName = this.ProviderName,
                RemarksText = "provider de email demo",
                RequiresInteractiveInitialization = true,
                Items = new Dictionary<string, ProviderConfigurationItem>()
                {
                    {
                        "CLIENTSECRET",
                        new ProviderConfigurationItem()
                        {
                            ParameterName = "CLIENTSECRET",
                            ParameterDescription = "Segredo do cliente",
                            Visible = true
                        }
                    },
                    {
                        "ACCOUNTID",
                        new ProviderConfigurationItem()
                        {
                            ParameterName = "ACCOUNTID"
                        }
                    }
                }
            };
        }

        public override void Initialize(ProviderConfiguration config, string configurationPath)
        {
            this.configPath = configurationPath;
            this.accountID = config.Items["ACCOUNTID"].ParameterValue;
            this.clientSecret = config.Items["CLIENTSECRET"].ParameterValue;
            return;
        }

        public override async Task<ProviderConfiguration> InitializeInteractive(ProviderConfiguration config)
        {
            try
            {
                // validate config
                if (!config.Items.ContainsKey("CLIENTID") || string.IsNullOrEmpty(config.Items["CLIENTID"].ParameterValue))
                    throw new Exception();

                if (!config.Items.ContainsKey("CLIENTSECRET") || string.IsNullOrEmpty(config.Items["CLIENTSECRET"].ParameterValue))
                    throw new Exception();

                // implement provider authentication flow

                // here you can store de account to be displayed
                config.Items["ACCOUNTID"].ParameterValue = "a@b.com";

                return config;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override Task SignOut()
        {
            // here you can remove cache file, etc.
            // this method is called when user tries to clear email configurations.
            return Task.CompletedTask;
        }

        public override Task SendEmail(IEnumerable<string> to, IEnumerable<string> cc, IEnumerable<string> bcc, string subject, string body, bool isbodyhtml, EmailEnums.Importance importance, IEnumerable<string> attachements)
        {
            return base.SendEmail(to, cc, bcc, subject, body, isbodyhtml, importance, attachements);
        }

        public override Task SendEmail(MailMessage mailMessage)
        {
            return base.SendEmail(mailMessage);
        }
    }
}
