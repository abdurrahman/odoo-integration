using Odoo.Integration.Service.Models;

namespace Odoo.Integration.Test.BaseTests
{
    public class OdooTestBase
    {
        protected readonly OdooConnection Connection = new OdooConnection
        {
            Url = "",
            Database = "",
            Username = "",
            Password = ""
        };
    }
}