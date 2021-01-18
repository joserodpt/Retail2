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
    class OrderManager
    {
        public static String con = ConfigurationManager.ConnectionStrings["Orders"].ConnectionString;
        public static List<Order> orderCache = new List<Order>();
        public static List<int> tableNumbers = new List<int>();

        public static List<Order> loadOrders()
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                var output = cnn.Query<Order>("select * from Orders", new DynamicParameters());
                orderCache = output.ToList();

                foreach(Order o in orderCache)
                {
                    if (o.ORDERTYPE == 1)
                    {
                        tableNumbers.Add(o.TABLE);
                    }
                }

                return orderCache;
            }
        }

        public static int getOrderNumber()
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                var count = cnn.ExecuteScalar<int>("SELECT COUNT(*) FROM Orders");
                return count;
            }
        }

        public static void saveOrder(Order o)
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                cnn.Execute("insert into Orders (CREATORUSERID, ORDERTYPE, CONTENTLIST, INDEXLIST, EVENTS, VALUE, 'TABLE', DATECREATED, DATECLOSED, DONE, INFO, OCCURRENCE, 'OCCURRENCEINFO', PAYMENTDETAILS, PEOPLEPROFILEID, IDENTIFIER) values (@CREATORUSERID, @ORDERTYPE, @CONTENTLIST, @INDEXLIST, @EVENTS, @VALUE, @TABLE, @DATECREATED, @DATECLOSED, @DONE, @INFO, @OCCURRENCE, @OCCURRENCEINFO, @PAYMENTDETAILS, @PEOPLEPROFILEID, @IDENTIFIER)", o);
            }
        }

        public static Order getOrder(int n)
        {
            foreach (Order o in orderCache)
            {
                if (o.TABLE == n)
                {
                    return o;
                }
            }
            return null;
        }

        public static Boolean orderExistsNormal(Order o)
        {
            if (o != null)
            {
                foreach (Order so in orderCache)
                {
                    if (so.IDENTIFIER == o.IDENTIFIER)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static void editOrder(Order o)
        {
            using (var connection = new SQLiteConnection(con))
            {
                var sqlStatement = @"
UPDATE Orders 
SET  CREATORUSERID = @CREATORUSERID
,ORDERTYPE = @ORDERTYPE 
,CONTENTLIST = @CONTENTLIST
,INDEXLIST = @INDEXLIST
,EVENTS = @EVENTS
,VALUE = @VALUE
,'TABLE' = @TABLE
,DATECREATED = @DATECREATED
,DATECLOSED = @DATECLOSED
,DONE = @DONE
,INFO = @INFO
,OCCURRENCE = @OCCURRENCE
,OCCURRENCEINFO = @OCCURRENCEINFO
,PAYMENTDETAILS = @PAYMENTDETAILS
,PEOPLEPROFILEID = @PEOPLEPROFILEID
WHERE IDENTIFIER = @IDENTIFIER";
                connection.Execute(sqlStatement, o);
            }
        }

        public static void deleteOrder(Order o)
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                cnn.Execute("delete from Orders WHERE IDENTIFIER='" + o.IDENTIFIER + "'");
            }
        }

        public static Order getOrder(String ident)
        {
            foreach (Order p in orderCache)
            {
                if (p.IDENTIFIER == ident)
                {
                    return p;
                }
            }
            return null;
        }

        public static Order getOrderByTable(int t)
        {
            foreach (Order p in orderCache)
            {
                if (p.TABLE == t)
                {
                    return p;
                }
            }
            return null;
        }

        public static Double getTotal(DataTable p)
        {
            List<String> tot = new List<string>();
            tot.Clear();

            foreach (DataRow dr in p.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    tot.Add(dr[2].ToString().Replace(".", ","));
                }
            }

            Double xd = 0;
            foreach (String s in tot)
            {
                xd += Double.Parse(s);
            }

            return xd;
        }
    }
}

