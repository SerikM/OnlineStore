using Model.Abstracts;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net.Mail;
using System.Net;


namespace Model.AbstractImplements
{
    public class MailProcessor : IMailProcessor
    {

        public bool SendMail(InquiryInfo inquiry)
        {
           MailAddress fromAddr = new MailAddress("admin@stesha.com.au", "stesha.com.au");
           MailAddress toAddr = new MailAddress(inquiry.RecepientAddress, inquiry.RecepientName);
           MailMessage message = new MailMessage(fromAddr, toAddr);

           message.Body = inquiry.FullMessage;
           message.Subject = inquiry.Subject;

            //create an smtp client(necessary to send an email.)
            using (SmtpClient smtpCl = new SmtpClient())
            {
                try
                {
                    smtpCl.Host = "smtp.gmail.com";
                    smtpCl.Port = 587;
                    smtpCl.EnableSsl = true;
                    
                    NetworkCredential cred = new NetworkCredential("tyrkamotchka@gmail.com", "10868774", "");
                    smtpCl.Credentials = cred;
                    //send the message
                    smtpCl.Send(message);
                    //Response.Redirect("Contact.aspx");
                    
                    return true;
                }
                catch (SmtpException)
                {
                    return false;
                }


                
            }

        }
    }
}



    