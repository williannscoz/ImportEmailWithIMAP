using MailKit;
using MailKit.Security;
using static ImportEmail.AccountToImport.AccountToImport;

namespace ImportEmail.FeatureUtil
{
    public static class FeatureUtil
    {
        public static void ConnectAccount(IMailService imapClient, Account account)
        {
            imapClient.Connect(account.Host, account.Port, SecureSocketOptions.SslOnConnect);

            var saslMechanism = CreateSaslMechanism(account.User, account.OAuthToken);

            imapClient.Authenticate(saslMechanism);
        }

        private static SaslMechanism CreateSaslMechanism(string userName, string token) => new SaslMechanismOAuth2(userName, token);
    }
}