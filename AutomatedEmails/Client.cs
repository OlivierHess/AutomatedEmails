using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainNameRenewal
{
    [Table(Name = "ESC_CLIENT")]
    public class Client
    {
        private int _ID;
        private string _ClientName;
        private EntitySet<Domains> _Domains;
        private EntitySet<ClientContact> _ClientContacts;

        public Client()
        {
            this._Domains = new EntitySet<Domains>();

            this._ClientContacts = new EntitySet<ClientContact>();
        }

        [Column(Storage = "_ID", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int ID
        {
            get
            {
                return this._ID;
            }

            //set
            //{
            //    this._ID = value;
            //}
        }

        [Column(Storage = "_ClientName")]
        public string Name
        {
            get
            {
                return this._ClientName;
            }

            set
            {
                this._ClientName = value;
            }
        }

        [Association(Storage = "_Domains", OtherKey = "ID")]
        public EntitySet<Domains> Domains
        {
            get { return this._Domains; }
            set { this._Domains.Assign(value); }
        }

        [Association(Storage = "_ClientContacts", OtherKey = "ClientID")]
        public EntitySet<ClientContact> ClientContacts
        {
            get { return this._ClientContacts; }
            set { this._ClientContacts.Assign(value); }
        }
    }
}
