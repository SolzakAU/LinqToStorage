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
        Database.SetInitializer<EfContext>(new EfInitializer());

        var context = new EfContext("Data Source=.\\dev14;Initial Catalog=integration_311_2;Integrated Security=SSPI");

        context.Database.Initialize(true);

        var y = from p in context.Templates.PatientProperties
                where p.PreferredName.LastName == "Kestral"
                select p;

        File.WriteAllText(@"c:\incoming\dump.sql", "use integration_311_2\r\n\r\n" + y.ToString() + "\r\n\r\nuse master");

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
