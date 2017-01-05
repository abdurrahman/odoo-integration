using CookComputing.XmlRpc;
using Odoo.Integration.Service.Interfaces;
using Odoo.Integration.Service.Models;
using System;

namespace Odoo.Integration.Service
{
    public class OdooService
    {
        private const string LoginPath = "/xmlrpc/2/common";
        private const string DataPath = "/xmlrpc/2/object";

        private readonly OdooConnection _connection;
        private readonly OdooContext _context;

        public OdooService(OdooConnection connection)
        {
            _connection = connection;
            _context = new OdooContext();

            _context.OdooAuthentication = XmlRpcProxyGen.Create<IOdooCommon>();
            _context.OdooAuthentication.Url = string.Format(@"{0}{1}", _connection.Url, LoginPath);

            _context.OdooData = XmlRpcProxyGen.Create<IOdooObject>();
            _context.OdooData.Url = string.Format(@"{0}{1}", _connection.Url, DataPath);

            _context.Database = connection.Database;
            _context.Username = connection.Username;
            _context.Password = connection.Password;
            Login();
        }

        public void Login()
        {
            try
            {
                _context.UserId = _context.OdooAuthentication.Login(_connection.Database, _connection.Username, _connection.Password);
            }
            catch (XmlRpcFaultException ex)
            {
                throw new Exception("Login failed, XmlRpc Error", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Login failed, Error", ex);
            }
        }

        public int Create(string model, XmlRpcStruct fieldValues)
        {
            return _context.OdooData.Create(_connection.Database, _context.UserId, _connection.Password, model, "create", fieldValues);
        }

        public XmlRpcStruct ButtonProformaVoucher(string model, int[] ids)
        {
            return _context.OdooData.ButtonProformaVoucher(_connection.Database, _context.UserId, _connection.Password, model, "button_proforma_voucher", ids);
        }

        public int[] Search(string model, object[] filter, int? offset = null, int? limit = null)
        {
            return _context.OdooData.Search(_connection.Database, _context.UserId, _connection.Password, model, "search", filter, offset, limit);
        }

        public int Count(string model, object[] filter)
        {
            return _context.OdooData.Count(_connection.Database, _context.UserId, _connection.Password, model, "count", filter);
        }

        public XmlRpcStruct[] Read(string model, int[] ids, string[] fields)
        {
            return _context.OdooData.Read(_connection.Database, _context.UserId, _connection.Password, model, "read", ids, fields);
        }

        public XmlRpcStruct[] SearchAndRead(string model, object[] filter, string[] fields, int? offset = null, int? limit = null)
        {
            return _context.OdooData.SearchAndRead(_connection.Database, _context.UserId, _connection.Password, model, "search_read", filter, fields, offset, limit);
        }

        public bool Remove(string model, int[] ids, string[] fields)
        {
            return _context.OdooData.Unlink(_connection.Database, _context.UserId, _connection.Password, model, "unlink", ids);
        }

        public bool Update(string model, int[] ids, XmlRpcStruct fields)
        {
            return _context.OdooData.Write(_connection.Database, _context.UserId, _connection.Password, model, "write", ids, fields);
        }
    }
}