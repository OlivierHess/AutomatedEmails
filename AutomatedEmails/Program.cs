using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Collections;
using System.Configuration;

namespace DomainNameRenewal
{
    class Program
    {

        static void Main(string[] args)
        {
            Admin db = new Admin(ConfigurationManager.ConnectionStrings["Connection"].ConnectionString);

            var getSoonToExpireDomains = from domains in db.Domain where domains.Expiry.Date > (DateTime.Today) && domains.Expiry.Date < (DateTime.Today.Date.AddDays(42)) group domains by domains.ClientID;

            foreach (var item in getSoonToExpireDomains)
            {

                String body = "";
                String subject = "";
                Double? domainCost = null;
                String domainName = "";
                String domainExpiry = "";

                if (item.Count() > 1)
                {
                    subject += "Domain renewals for ";
                    body += "<p style=\"font-family:&quot;Calibri&quot;,sans-serif; color:black\">I am writing to inform you that the following domain names you have registered with us are due for renewal.&nbsp; The costs of renewal are the same as in previous years and are displayed in the table below. All prices are for a further two years of service.</p>";
                    body += "<table border=\"0\" cellpadding=\"0\" style=\"border-collapse: collapse; border-style: solid; border-width: 1px;\"><tbody>";
                    body += "<td valign=\"top\" style=\"padding:0cm 5.4pt 0cm 5.4pt; font-family:&quot;Calibri&quot;,sans-serif; color:black; border-style: solid; border-width: 1px; font-size:1em; line-height:1em; \"><b>Domain Name</b></td><td valign=\"top\" style=\"padding:0cm 5.4pt 0cm 5.4pt; font-family:&quot;Calibri&quot;,sans-serif; color:black; border-style: solid; border-width: 1px; font-size:1em; line-height:1em; \"><b>Expiry</b></td><td valign=\"top\" style=\"padding:0cm 5.4pt 0cm 5.4pt; font-family:&quot;Calibri&quot;,sans-serif; color:black; border-style: solid; border-width: 1px; font-size:1em; line-height:1em; \"><b>Renewal Cost</b></td>";
                }

                foreach (var domain in item)
                {                    

                    domainName = domain.Domain;                    
                    domainExpiry = domain.Expiry.ToShortDateString();
                    DomainDetails domainDetails = new DomainDetails();
                    String domainExtension = domainDetails.checkDomainExtension(domainName);
                    domainCost = domainDetails.domainCost(domainExtension);
                    

                    // Console.WriteLine(String.Format("Domain name: {0}", domainName));
                    if (item.Count() > 1)
                    {

                        //Hack (for now) to send automatic email to Excellimore and ECS with their 20% discount
                        if (domain.ClientID == 28 || domain.ClientID == 420)
                        {
                            domainCost = domainCost * 0.80;
                        }                        

                         subject += String.Format("{0}, ", domainName);
                         body += String.Format("<tr><td valign=\"top\" width=\"200\" style=\"padding:0cm 5.4pt 0cm 5.4pt; font-family:&quot;Calibri&quot;,sans-serif; color:black; border-style: solid; border-width: 1px; font-size:1em; line-height:1em; \">{0}</td><td valign=\"top\" width=\"150\" style=\"padding:0cm 5.4pt 0cm 5.4pt; font-family:&quot;Calibri&quot;,sans-serif; color:black; border-style: solid; border-width: 1px; font-size:1em; line-height:1em; \">{1}</td><td valign=\"top\" width=\"150\" style=\"padding:0cm 5.4pt 0cm 5.4pt; font-family:&quot;Calibri&quot;,sans-serif; color:black; border-style: solid; border-width: 1px; font-size:1em; line-height:1em; \">&pound;{2}</td></tr>", domainName, domainExpiry, domainCost);
                    }  

                    if(domain.Expiry < (DateTime.Today.Date.AddDays(7)))
                    {
                        Email.AlertAdminDomainAboutToExpire(domainName, domainExpiry);
                    }
                    
                }

                    foreach (var clientContact in item.First().Client.ClientContacts.Where(x => x.IsDomainContact == 1))
                    {
                        String clientName = clientContact.Name;
                        String clientEmail = clientContact.Email;

                        if (domainCost != null)
                        {

                            if (!String.IsNullOrEmpty(clientEmail))
                            {
                                if (item.Count() == 1)
                                {
                                    //Hack (for now) to send automatic email to Excellimore and ECS with their 20% discount
                                    if (clientContact.ClientID == 28 || clientContact.ClientID == 420)
                                    {
                                        domainCost = domainCost * 0.80;
                                    }

                                    clientName = clientContact.Name;
                                    clientEmail = clientContact.Email;

                                    Email.SendRenewalEmailWithCost(clientName, clientEmail, domainName, domainExpiry, domainCost);
                                }

                                else
                                {
                                    /*Close the multiple domain renewal table*/
                                    String bodyAfterTable = "</tbody></table>";
                                    bodyAfterTable += "<p style=\"font-family:&quot;Calibri&quot;,sans-serif; color:black\">Can you please confirm if you would like us to arrange for renewal of these accounts?</p>";
                                    bodyAfterTable += "<p style=\"font-family:&quot;Calibri&quot;,sans-serif; color:black\">Many thanks,</p>";
                                    String emailContent = String.Format("<html><body><p style=\"font-family:&quot;Calibri&quot;,sans-serif; color:black\">Dear {0},</p>{1} {2}", clientName, body, bodyAfterTable);


                                    String trimmedSubject = subject.Remove(subject.Length - 2);
                                    Email.SendRenewalEmail(clientEmail, trimmedSubject, emailContent);
                                } 
                            }

                            else
                            {
                                Email.AlertAdminNoClientEmail(clientContact.Client.Name);
                            }                 
  
                        }

                        else
                        {
                            Email.AlertAdminNoDomainCost(domainName);
                        }
                      
                    }                   

                }
               
               //Console.ReadLine();                          

            }                    
        }
    }

