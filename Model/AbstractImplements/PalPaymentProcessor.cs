using Model.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using Model.Entities;


namespace Model
{
    public class PalPaymentProcessor : IPaymentProcessor
    {
        public void ProcessPaymentConfirmation(CartShipPayInfo csp)
        {

        }
    }
}