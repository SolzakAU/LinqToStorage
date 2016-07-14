using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace LinqToStorage
{
  class Program
  {
    static void Main(string[] args)
    {
      /*var x = from p in StorageContext.Patient
              where p.Name.LastName == "Kestral"
              select p;
      Console.WriteLine(x);*/

      try
      {
        //Database.SetInitializer(new KarismaSampleInitialiser());

        var context = new KarismaContext("Data Source=gavinm\\std12;Initial Catalog=Capital_Dev;Integrated Security=SSPI");

        //context.Database.Initialize(true);

        var rs = (from r in context.WorkListReport
                 orderby r.Key                 
                 select new { r.RequestKey, r.ReportKey }).Take(10);

        foreach (var r in rs)
        {
          Console.WriteLine(r.ReportKey.ToString());
        }

        Console.WriteLine(rs);

        var y = from p in context.Templates.PatientProperties
                where p.PreferredName.LastName == "Kestral"
                select p;

        File.WriteAllText(@"c:\incoming\dump.sql", 
          "use " + context.Database.Connection.Database + "\r\n\r\n" + 
          y.ToString() + "\r\n\r\n" +
          "use master");
        
        Console.WriteLine(y);
      } 
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

      //Console.ReadLine();
    }
  }
}
