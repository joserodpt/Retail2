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
    class CategoryManager
    {

        public static String con = ConfigurationManager.ConnectionStrings["Products"].ConnectionString;

        public static List<Category> loadCategories()
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                var output = cnn.Query<Category>("select * from Categories", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void saveCat(Category cat)
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                cnn.Execute("insert into Categories (NAME, IDENTIFIER) values (@NAME, @IDENTIFIER)", cat);
            }
        }

        public static void editCat(Category cat)
        {
            using (var connection = new SQLiteConnection(con))
            {
                var sqlStatement = @"
UPDATE Categories 
SET  NAME = @NAME
WHERE IDENTIFIER = @IDENTIFIER";
                connection.Execute(sqlStatement, cat);
            }
        }

        public static void deleteCat(Category cat)
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                cnn.Execute("delete from Categories WHERE IDENTIFIER='" + cat.IDENTIFIER + "'");
            }
        }
    }
}

