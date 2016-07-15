using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
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

        var context = new KarismaContext("Data Source=localhost;Initial Catalog=K4_DEMO;Integrated Security=SSPI");

        //context.Database.Initialize(true);

        var rx = (from r in context.WorkListReport
                  where 
                    !r.RequestDeleted  &&
                    r.RequestRegistered && 
                    !r.ReportDeleted &&
                    r.ReportProcessStatus < ReportInstanceProcessStatus.Completed

                  select new { r.RequestKey, r.ReportKey }).Take(100000);

        var rz = (from r in context.WorkListReport
                  where
                    !r.RequestDeleted &&
                    r.RequestRegistered &&
                    !r.ReportDeleted &&
                    r.ReportProcessStatus < ReportInstanceProcessStatus.Completed &&
                    !context.RequestService.Any(rs => r.ReportKey == rs.ReportInstanceKey && !rs.Deleted && rs.RequestServiceStep.Any(rss => !rss.Cancelled && !rss.Deleted && rss.Status != RequestServiceStepStatus.Verified))

                  select new { r.RequestKey, r.ReportKey }).Take(100000);

        var timer = Stopwatch.StartNew();
        var rsz = rz.ToArray();
        timer.Stop();
        timer.Start();
        var rsx = rx.ToArray();
        timer.Stop();

        foreach (var r in rx)
        {
          Console.WriteLine(string.Format("{0},{1}", r.RequestKey, r.ReportKey));
        }

        Console.WriteLine(rx);

        return;

        //var y = from p in context.Templates.PatientProperties
        //        where p.PreferredName.LastName == "Kestral"
        //        select p;

        //File.WriteAllText(@"c:\incoming\dump.sql", 
        //  "use " + context.Database.Connection.Database + "\r\n\r\n" + 
        //  y.ToString() + "\r\n\r\n" +
        //  "use master");
        
        //Console.WriteLine(y);
      } 
      catch (Exception ex)
      {
        Console.WriteLine(ex.Message);
      }

      //Console.ReadLine();
    }
  }
}
