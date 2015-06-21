using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainNameRenewal
{
    [Table(Name = "ESC_CLIENT_CONTACT")] 
    public class ClientContact
    {
        private int _ID;
        private int _ClientID;
        private string _ClientName;
        private string _ClientEmail;
        private int _IsDomainContact;
        private EntityRef<Client> _Client;

        public ClientContact()
        {
            this._Client = new EntityRef<Client>();
        }

        [Association(Storage = "_Client", ThisKey = "ClientID")]
        public Client Client
        {
            get { return this._Client.Entity; }
            set { this._Client.Entity = value; }
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


        [Column(Storage = "_ClientID")]
        public int ClientID
        {
            get
            {
                return this._ClientID;
            }

            //set
            //{
            //    this._ClientID = value;
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


        [Column(Storage = "_ClientEmail")]
        public string Email
        {
            get
            {
                return this._ClientEmail;
            }
            set
            {
                this._ClientEmail = value;
            }
        }

        [Column(Storage = "_IsDomainContact")]
        public int IsDomainContact
        {
            get
            {
                return this._IsDomainContact;
            }

            set
            {
                this._IsDomainContact = value;
            }
        }
    }
}
