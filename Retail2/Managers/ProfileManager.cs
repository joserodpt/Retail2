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
    class ProfileManager
    {

        public static String con = ConfigurationManager.ConnectionStrings["Profiles"].ConnectionString;
        public static List<Profile> profileCache = new List<Profile>();

        public static List<Profile> loadProfiles()
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                var output = cnn.Query<Profile>("select * from Profiles", new DynamicParameters());
                profileCache = output.ToList();
                return profileCache;
            }
        }

        public static int getTotalProfiles()
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                var count = cnn.ExecuteScalar<int>("SELECT COUNT(*) FROM Profiles");
                return count;
            }
        }

        public static void saveProfile(Profile p)
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                cnn.Execute("insert into Profiles (FIRSTNAME, LASTNAME, ADRESS1, ADRESS2, CITY, STATE, 'FISCAL', REFERENCE, INFO, PHONE, EMAIL, DATECREATED, IDENTIFIER) values (@FIRSTNAME, @LASTNAME, @ADRESS1, @ADRESS2, @CITY, @STATE, @FISCAL, @REFERENCE, @INFO, @PHONE, @EMAIL, @DATECREATED, @IDENTIFIER)", p);
            }
        }

        public static Boolean profileExists(Profile o)
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                var check = cnn.ExecuteScalar<Boolean>("SELECT 1 WHERE EXISTS (SELECT 1 FROM Profiles WHERE IDENTIFIER = @IDENTIFIER)", o);
                return check;
            }
        }

        public static void editProfile(Profile o)
        {
            using (var connection = new SQLiteConnection(con))
            {
                var sqlStatement = @"
UPDATE Profiles 
SET  FIRSTNAME = @FIRSTNAME
,LASTNAME = @LASTNAME 
,ADRESS1 = @ADRESS1
,ADRESS2 = @ADRESS2
,CITY = @CITY
,STATE = @STATE
,'FISCAL' = @FISCAL
,REFERENCE = @REFERENCE
,INFO = @INFO
,PHONE = @PHONE
,EMAIL = @EMAIL
,DATECREATED = @DATECREATED
WHERE IDENTIFIER = @IDENTIFIER";
                connection.Execute(sqlStatement, o);
            }
        }

        public static void deleteProfile(Profile o)
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                cnn.Execute("delete from Profiles WHERE IDENTIFIER='" + o.IDENTIFIER + "'");
            }
        }

        public static Profile getProfile(String ident)
        {
            foreach (Profile p in profileCache)
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
