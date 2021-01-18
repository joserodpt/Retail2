using Dapper;
using Retail2.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail2.Managers
{
    class ProductManager
    {

        public static String con = ConfigurationManager.ConnectionStrings["Products"].ConnectionString;
        public static List<Product> prodcach = new List<Product>();

        public static List<Product> loadProducts()
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                var output = cnn.Query<Product>("select * from Products", new DynamicParameters());
                prodcach = output.ToList();
                return prodcach;
            }
        }

        public static void saveProduct(Product p)
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                cnn.Execute("insert into Products (NAME, DESCRIPTION, PRICE, unlimitedSTOCK, STOCK, DATEADDED, CATEGORY, INFO, useOverlay, IDENTIFIER) values (@NAME, @DESCRIPTION, @PRICE, @unlimitedSTOCK, @STOCK, @DATEADDED, @CATEGORY, @INFO, @useOverlay, @IDENTIFIER)", p);
            }
        }

        public static void editProduct(Product p)
        {
            using (var connection = new SQLiteConnection(con))
            {
                var sqlStatement = @"
UPDATE Products 
SET  NAME = @NAME
,DESCRIPTION = @DESCRIPTION
,PRICE = @PRICE 
,unlimitedSTOCK = @unlimitedSTOCK
,STOCK = @STOCK
,DATEADDED = @DATEADDED
,CATEGORY = @CATEGORY
,INFO = @INFO
,useOverlay = @useOverlay
WHERE IDENTIFIER = @IDENTIFIER";
                connection.Execute(sqlStatement, p);
            }
        }

        public static void editStock(Product p)
        {
            using (var connection = new SQLiteConnection(con))
            {
                var sqlStatement = @"
UPDATE Products 
SET  STOCK = @STOCK
,unlimitedSTOCK = @unlimitedSTOCK
WHERE IDENTIFIER = @IDENTIFIER";
                connection.Execute(sqlStatement, p);
            }
        }

        public static void editStockNumb(Product p)
        {
            using (var connection = new SQLiteConnection(con))
            {
                var sqlStatement = @"
UPDATE Products 
SET  STOCK = @STOCK
WHERE IDENTIFIER = @IDENTIFIER";
                connection.Execute(sqlStatement, p);
            }
        }

        public static void deleteProduct(Product p)
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                cnn.Execute("delete from Products WHERE IDENTIFIER='" + p.IDENTIFIER + "'");
            }
        }

        public static Product getProduct(String ident)
        { 
            foreach (Product p in prodcach)
            {
                if (p.IDENTIFIER == ident)
                {
                    return p;
                }
            }
            return null;
        }
    }
}
