using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;

namespace ormlitestringarrayissue
{
    public class PgsqlData
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { get; set; }

        [PgSqlTextArray]
        public List<string> ListStrings { get; set; }
    }
    
    class Program
    {
        static string PostgreSqlDb = "Server=localhost;Port=5432;User Id=test;Password=test;Database=test;Pooling=true;MinPoolSize=0;MaxPoolSize=200";
        
        static void Main(string[] args)
        {
            var dbFactory = new OrmLiteConnectionFactory(PostgreSqlDb, PostgreSqlDialect.Provider);

            using (var db = dbFactory.OpenDbConnection())
            {
                db.DropAndCreateTable<PgsqlData>();

                db.Insert<PgsqlData>(new PgsqlData
                {
                    ListStrings = new [] {"hello", "there"}.ToList()
                });

                var deserialized = db.Select<PgsqlData>(x => x.Id == 1).First();

                if (!deserialized.ListStrings[0].Equals("hello") || !deserialized.ListStrings[1].Equals("there"))
                {
                    throw new Exception("It came back wrong :/");
                }
            }
        }
    }
}
