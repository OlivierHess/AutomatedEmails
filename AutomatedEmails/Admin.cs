using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainNameRenewal
{
    public class Admin : DataContext
    {
        public Table<Domains> Domain;
        public Table<ClientContact> ClientContact;
        public Table<Client> Client;

        public Admin(string connection) : base(connection) { }
    }
}
