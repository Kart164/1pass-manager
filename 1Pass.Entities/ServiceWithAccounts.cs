using System.Collections.Generic;

namespace _1Pass.Entities
{
    public class ServiceWithAccounts : Service
    {
        public List<Account> Accounts { get; set; }

        public ServiceWithAccounts()
        {
            Accounts = new List<Account>();
        }

        public ServiceWithAccounts(Service service)
        {
            Id = service.Id;
            Name = service.Name;
            LastUpdate = service.LastUpdate;
            Accounts = new List<Account>();
        }

        public ServiceWithAccounts(Service service, List<Account> accounts)
        {
            Id = service.Id;
            Name = service.Name;
            LastUpdate = service.LastUpdate;
            Accounts = accounts;
        }
    }
}
