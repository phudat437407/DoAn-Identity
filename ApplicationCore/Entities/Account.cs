using ApplicationCore.Interfaces;
namespace ApplicationCore.Entities
{
    public class Account : IAggregateRoot
    {
        public Account() { }

        public Account(string Username, string Password, string Roles)
        {
            this.Username = Username;
            this.Password = Password;
            this.Roles = Roles;
        }

        public Account(Account acc)
        {
            this.Username = acc.Username;
            this.Password = acc.Password;
            this.Roles = acc.Roles;
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Roles { get; set; }
    }
}