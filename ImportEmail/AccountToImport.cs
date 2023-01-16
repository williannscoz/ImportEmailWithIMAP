namespace ImportEmail.AccountToImport
{
    public class AccountToImport
    {
        public class Account
        {
            public string User { get; set; }
            public string Password { get; set; }
            public string Host { get; set; }
            public int Port { get; set; }
            public string OAuthToken { get; set; }
        }

        public static List<Account> AddAccountsToImport()
        {
            var clientsToImport = new List<Account>();

            clientsToImport.Add(new Account
            {
                User = "",
                Host = "imap.gmail.com",
                Port = 993,
                OAuthToken = ""
            });

            clientsToImport.Add(new Account
            {
                User = "",
                Host = "outlook.office365.com",
                Port = 993,
                OAuthToken = ""
            });

            return clientsToImport;
        }
    }
}