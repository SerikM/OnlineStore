using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Abstracts
{
    public interface IMailProcessor
    {
       bool  SendMail(InquiryInfo inquiry);

    }
}
