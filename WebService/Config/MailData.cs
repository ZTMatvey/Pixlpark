namespace WebService.Config
{
    public sealed class MailData
    {
        private static MailData _instance;

        public static MailData Instance
        {
            get
            {
                _instance ??= new MailData();
                return _instance;
            }
        }

        public string SMTP { get; set; }
        public int Port { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
    }
}
