using CookComputing.XmlRpc;

namespace Odoo.Integration.Service.Interfaces
{
    public interface IOdooCommon : IXmlRpcProxy
    {
        /// <summary>
        /// Logging in
        /// https://www.odoo.com/documentation/8.0/api_integration.html#logging-in
        /// EndPoint xmlrpc/2/common
        /// </summary>
        /// <param name="database"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [XmlRpcMethod("login")]
        int Login(string database, string username, string password);
    }
}