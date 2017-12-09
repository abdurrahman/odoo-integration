# odoo-integration

A C# [XML-RPC](https://en.wikipedia.org/wiki/XML-RPC) library for integration with Odoo 8.0

Web Service API Documentation - https://www.odoo.com/documentation/8.0/api_integration.html

## Usage

Create your credentials for odoo connection

```csharp
public class OdooBase
{
	protected readonly OdooConnection Connection = new OdooConnection
	{
	    Url = "https://mycompany.odoo.com",
	    Database = "database",
	    Username = "admin",
	    Password = "password"
	};
}
```

A sample for getting partner id from res.partner model

```csharp
public class OdooReadTest : OdooBase
{
	public void PartnerList()
	{
		var odooService = new OdooService(Connection);

		// res.partner search filter for user vat code
		var partnerFilter = new object[]
		{
			new object[] {"vat", "=", "TR1234567890"},
		};

		// get partner id from res.partner
		var partnerIds = odooService.SearchAndRead("res.partner", partnerFilter, new string[] { "id" }, 0, 1);
	}
}
```

## Licence

This software is made available under the LGPL v3 license.
