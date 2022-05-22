using System.Collections.Generic;

namespace _1Pass.Entities
{
    public class ServiceWithAccounts : Service
    {
        public List<Account> Accounts { get; set; }
    }
}
