using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using static ImportEmail.AccountToImport.AccountToImport;
using static ImportEmail.FeatureUtil.FeatureUtil;

namespace ImportEmail.EmailImporter
{
    public static class EmailImporter
    {
        public static void ImportEmail()
        {
            var accountsToImport = AddAccountsToImport();

            foreach (var account in accountsToImport)
            {
                Console.WriteLine($"Iniciado importação da conta {account.User}");

                using (var imapClient = new ImapClient())
                {
                    ConnectAccount(imapClient, account);

                    Console.WriteLine($"Conectado na conta {account.User}");

                    var inbox = imapClient.Inbox;
                    inbox.Open(FolderAccess.ReadWrite);
                    var query = SearchQuery.All;
                    var importEmailReceivedFromDate = DateTime.Now.AddDays(-1);
                    query = query.And(SearchQuery.DeliveredAfter((DateTime)importEmailReceivedFromDate));
                    var uids = inbox.Search(query).Distinct();

                    Console.WriteLine($"Encontrou {uids.Count()} emails para importar");

                    foreach (var uid in uids)
                    {
                        var filePath = Path.Combine(@"D:\ImportEmailIMAP\", $"{ uid}.eml");
                        var message = inbox.GetMessage(uid);

                        if (!File.Exists(filePath))
                        {
                            Console.WriteLine($"Importando email {message.Subject} da caixa {account.User}");

                            message.WriteTo(filePath);

                            Console.WriteLine($"Email {message.Subject} importado da caixa {account.User}");
                        }
                        else
                            Console.WriteLine($"Email {message.Subject} já foi importado da caixa {account.User}");
                    }

                    imapClient.Disconnect(true);
                }
            }
        }
    }
}