using MailKit;
using MailKit.Net.Imap;
using static ImportEmail.AccountToImport.AccountToImport;
using static ImportEmail.FeatureUtil.FeatureUtil;

namespace ImportEmail.EmailImporter
{
    public static class EmailImporter
    {
        public static void ImportEmail()
        {
            string filePath = @"C:\";

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

                    Console.WriteLine($"Encontrou {inbox.Count} emails para importar");

                    for (int i = 0; i < inbox.Count; i++)
                    {
                        var message = inbox.GetMessage(i);

                        filePath = Path.Combine(filePath, $"{account.User}-{message.Subject}_{i}.eml");

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