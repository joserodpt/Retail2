using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Retail2.Classes.MessageBoard
{
    public class Post
    {
        public int ID { get; set; }
        public String POSTERID { get; set; }
        public String NAME { get; set; }
        public String POSTDATE { get; set; }
        public String TEXT { get; set; }
        public String INTERACTION1 { get; set; }

        public int LIKES { get; set; }
        public int DISLIKES { get; set; }
        public String COMMENTS { get; set; }
    }
}
