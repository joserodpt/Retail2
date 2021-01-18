using Dapper;
using Retail.Managers;
using Retail2.Classes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;

namespace Retail2.Managers
{
    class UserManager
    {
        public static String con = ConfigurationManager.ConnectionStrings["Users"].ConnectionString;
        public static List<User> usrs = new List<User>();

        public static List<User> loadUsers()
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                var output = cnn.Query<User>("select * from Users", new DynamicParameters());
                usrs = output.ToList();
                return usrs;
            }
        }

        public static void saveUser(User usr)
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                cnn.Execute("insert into Users (FIRSTNAME, LASTNAME, PASSWORD, ADMIN, ONLINE, LOGININFO, PERMISSIONS, INFO, IDENTIFIER) values (@FIRSTNAME, @LASTNAME, @PASSWORD, @ADMIN, @ONLINE, @LOGININFO, @PERMISSIONS, @INFO, @IDENTIFIER)", usr);
            }
        }

        public static void editUser(User usr)
        {
            using (var connection = new SQLiteConnection(con))
            {
                var sqlStatement = @"
UPDATE Users 
SET  FIRSTNAME = @FIRSTNAME
,LASTNAME = @LASTNAME
,PASSWORD = @PASSWORD 
,ADMIN = @ADMIN
,ONLINE = @ONLINE
,LOGININFO = @LOGININFO
,PERMISSIONS = @PERMISSIONS
,INFO = @INFO
WHERE IDENTIFIER = @IDENTIFIER";
                connection.Execute(sqlStatement, usr);
            }
        }

        public static void updateLoginDate(User usr)
        {
            using (var connection = new SQLiteConnection(con))
            {
                var sqlStatement = @"
UPDATE Users 
SET LOGININFO = @LOGININFO
WHERE IDENTIFIER = @IDENTIFIER";
                connection.Execute(sqlStatement, usr);
            }
        }

        public static void updateLoginBoth(User usr)
        {
            using (var connection = new SQLiteConnection(con))
            {
                var sqlStatement = @"
UPDATE Users 
 SET LOGININFO = @LOGININFO
,ONLINE = @ONLINE
WHERE IDENTIFIER = @IDENTIFIER";
                connection.Execute(sqlStatement, usr);
            }
        }

        public static void updateLoginState(User usr)
        {
            using (var connection = new SQLiteConnection(con))
            {
                var sqlStatement = @"
UPDATE Users 
SET ONLINE = @ONLINE
WHERE IDENTIFIER = @IDENTIFIER";
                connection.Execute(sqlStatement, usr);
            }
        }

        public static void deleteUser(User u)
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                cnn.Execute("delete from Users WHERE IDENTIFIER='" + u.IDENTIFIER + "'");
            }
        }

        public static Boolean compare(User u, String input)
        {
            if (Transformer.Decrypt(u.PASSWORD, u.IDENTIFIER) == input)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public static User getUser(String ident)
        {
            if (usrs.Count == 0)
            {
                loadUsers();
            }
            foreach (User u in usrs)
            {
                if (u.IDENTIFIER == ident)
                {
                    return u;
                }
            }
            return null;
        }
        public static String getUserFirstName(String ident)
        {
            foreach (User u in usrs)
            {
                if (u.IDENTIFIER == ident)
                {
                    return u.FIRSTNAME;
                }
            }
            return "Eliminado";
        }

        public static String getUserNameFull(String ident)
        {
            foreach (User u in usrs)
            {
                if (u.IDENTIFIER == ident)
                {
                    return u.FIRSTNAME + " " + u.LASTNAME;
                }
            }
            return "Eliminado";
        }
    }
}
