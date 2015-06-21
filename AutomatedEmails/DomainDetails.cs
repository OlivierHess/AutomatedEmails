using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainNameRenewal
{
    public class DomainDetails
    {
        public String checkDomainExtension(String domainName)
        {
            String domainExtension;
            String[] checkDomainExtension = domainName.ToLowerInvariant().Split('.');

            if (checkDomainExtension.Length > 2)
            {
                domainExtension = domainName.Substring(domainName.IndexOf('.') + 1);
            }

            else
            {
                domainExtension = checkDomainExtension.Last();
            }

            domainExtension.Trim();

            return domainExtension;
        }

        public Double? domainCost(String domainExtension)
        {
            Double? domainCost = null;

            switch (domainExtension)
            {
                case "co.uk":
                    domainCost = 25.00;
                    break;
                case "com":
                    domainCost = 40.00;
                    break;

            }

            return domainCost;
        }
    }
}
