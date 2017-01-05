using CookComputing.XmlRpc;

namespace Odoo.Integration.Service.Interfaces
{
    public interface IOdooObject : IXmlRpcProxy
    {
        [XmlRpcMethod("execute")]
        int Create(string dbname, int uid, string pwd, string model, string method, XmlRpcStruct fieldValues);

        [XmlRpcMethod("execute")]
        XmlRpcStruct ButtonProformaVoucher(string dbname, int uid, string pwd, string model, string method, int[] ids);

        [XmlRpcMethod("execute")]
        int[] Search(string dbname, int uid, string pwd, string model, string method, object[] filter, int? offset = null, int? limit = null);

        [XmlRpcMethod("execute")]
        int Count(string dbname, int uid, string pwd, string model, string method, object[] filter);

        [XmlRpcMethod("execute")]
        XmlRpcStruct[] Read(string dbname, int uid, string pwd, string model, string method, int[] ids, string[] fields);

        [XmlRpcMethod("execute")]
        XmlRpcStruct[] SearchAndRead(string dbname, int uid, string pwd, string model, string method, object[] filter, string[] fields, int? offset = null, int? limit = null);

        [XmlRpcMethod("execute")]
        bool Unlink(string dbname, int uid, string pwd, string model, string method, int[] ids);

        [XmlRpcMethod("execute")]
        bool Write(string dbname, int uid, string pwd, string model, string method, int[] ids, XmlRpcStruct fieldValues);
    }
}