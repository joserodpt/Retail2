using Dapper;
using Retail2.Classes;
using Retail2.Classes.MessageBoard;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Retail2.Classes.Enum;

namespace Retail2.Managers
{
    class PostManager
    {

        public static String con = ConfigurationManager.ConnectionStrings["Posts"].ConnectionString;

        public static List<Post> loadPosts()
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                var output = cnn.Query<Post>("select * from Posts", new DynamicParameters());
                return output.ToList();
            }
        }

        public static void savePost(Post cat)
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                cnn.Execute("insert into Posts (NAME, POSTERID, POSTDATE, TEXT, INTERACTION1, LIKES, DISLIKES, COMMENTS) values (@NAME, @POSTERID, @POSTDATE, @TEXT, @INTERACTION1, @LIKES, @DISLIKES, @COMMENTS)", cat);
            }
        }

        public static void deletePost(Post p)
        {
            using (IDbConnection cnn = new SQLiteConnection(con))
            {
                cnn.Execute("delete from Posts WHERE ID='" + p.ID + "'");
            }
        }

        public static void updateInteractions(Post cat, Boolean likes)
        {
            if (likes == false)
            {
                using (var connection = new SQLiteConnection(con))
                {
                    var sqlStatement = @"
UPDATE Posts 
SET  INTERACTION1 = @INTERACTION1
WHERE ID = @ID";
                    connection.Execute(sqlStatement, cat);
                }
            } else
            {
                using (var connection = new SQLiteConnection(con))
                {
                    var sqlStatement = @"
UPDATE Posts 
SET  INTERACTION1 = @INTERACTION1,
LIKES = @LIKES,
DISLIKES = @DISLIKES
WHERE ID = @ID";
                    connection.Execute(sqlStatement, cat);
                }
            }
        }

        public static void updateComments(Post cat)
        {
            using (var connection = new SQLiteConnection(con))
            {
                var sqlStatement = @"
UPDATE Posts 
SET  COMMENTS = @COMMENTS
WHERE ID = @ID";
                connection.Execute(sqlStatement, cat);
            }
        }

        public static void valueManager(ValueInfo type, ValueInfo wh, int cat)
        {
            if (type == ValueInfo.LIKE)
            {
                if (wh == ValueInfo.ADD)
                {
                    using (IDbConnection cnn = new SQLiteConnection(con))
                    {
                        cnn.Execute("UPDATE Posts SET LIKES = LIKES + 1 WHERE ID='" + cat + "'");
                    }
                }
                if (wh == ValueInfo.REMOVE)
                {
                    using (IDbConnection cnn = new SQLiteConnection(con))
                    {
                        cnn.Execute("UPDATE Posts SET LIKES = LIKES - 1 WHERE ID='" + cat + "'");
                    }
                }
            }
            if (type == ValueInfo.DISLIKE)
            {
                if (wh == ValueInfo.ADD)
                {
                    using (IDbConnection cnn = new SQLiteConnection(con))
                    {
                        cnn.Execute("UPDATE Posts SET DISLIKES = DISLIKES + 1 WHERE ID='" + cat + "'");
                    }
                }
                if (wh == ValueInfo.REMOVE)
                {
                    using (IDbConnection cnn = new SQLiteConnection(con))
                    {
                        cnn.Execute("UPDATE Posts SET DISLIKES = DISLIKES - 1 WHERE ID='" + cat + "'");
                    }
                }
            }
        }
    }
}

