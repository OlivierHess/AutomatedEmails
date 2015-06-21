using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainNameRenewal
{
    [Table(Name = "ESC_DOMAIN")]
    public class Domains
    {
        private int _DomainID = 0;
        private string _DomainName;
        private int _DomainClientID;
        private DateTime _Expiry;
        private string _Registrar;
        private EntityRef<Client> _Client;

        public Domains()
        {
            this._Client = new EntityRef<Client>();
        }


        [Column(Storage = "_DomainID", DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int ID
        {
            get
            {
                return this._DomainID;
            }
            //set
            //{
            //    this._DomainID = value;
            //}
        }

        [Column(Storage = "_DomainClientID")]
        public int ClientID
        {
            get
            {
                return this._DomainClientID;
            }

            set
            {
                this._DomainClientID = value;
            }
        }






        [Column(Storage = "_DomainName")]
        public string Domain
        {
            get
            {
                return this._DomainName;
            }
            set
            {
                this._DomainName = value;
            }
        }



        [Column(Storage = "_Expiry")]
        public DateTime Expiry
        {
            get
            {
                return this._Expiry;
            }
            set
            {
                this._Expiry = value;
            }
        }

        [Column(Storage = "_Registrar")]
        public String Registrar 
        { 
            get
            {
                return this._Registrar;
            } 
            
            set
            {
                this._Registrar = value;
            } 
        }

        [Association(Storage = "_Client", ThisKey = "ClientID")]
        public Client Client
        {
            get { return this._Client.Entity; }
            set { this._Client.Entity = value; }
        }

    }
}
