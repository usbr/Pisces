using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Reclamation.TimeSeries;
using System.IO;
using Reclamation.TimeSeries.Hydromet;
using Reclamation.Core;
using Reclamation.TimeSeries.Owrd;
using System.Configuration;
using Reclamation.TimeSeries.Parser;

namespace Pisces.NunitTests.SeriesMath
{
    [TestFixture]
   public class TestWeirEquations
    {

       [Test]
       public void TestRectangularContractedWeir()
       {

           // bully creek feder canal
           Series ch = new Series("bul_ch");
           DateTime t1 = DateTime.Parse("2015-2-8 8:15");
           DateTime t2 = DateTime.Parse("2015-2-12 7:15");

           ch.AfterRead += new EventHandler(delegate (object o, EventArgs a)
               {
                   ch.Add(t1, 2);
                   ch.Add(t2, 1.69);
               });
           

           Logger.EnableLogger();
           SeriesExpressionParser.Debug = true;
           var c = new CalculationSeries("bul_qc");
           c.Parser.VariableResolver.Add("bul_ch", new ParserResult(ch));
           c.TimeInterval = TimeInterval.Irregular;
           c.Expression = "RectangularContractedWeir(bul_ch,10)";
           c.Calculate();


           Assert.AreEqual(2, c.Count," expected two flow calculations");
           Assert.AreEqual(90.42, c[t1].Value, 0.01);
           Assert.AreEqual(70.69, c[t2].Value, 0.01);

       }
       [Test]
       public void TestGenericWeirAtStAnthonyUnionFeederCanal()
       {

           // bully creek feder canal
           Series ch = new Series("afci_ch");
           DateTime t1 = DateTime.Parse("2014-6-11 1:15");
           DateTime t2 = DateTime.Parse("2014-6-11 14:00");

           ch.AfterRead += new EventHandler(delegate(object o, EventArgs a)
           {
               ch.Add(t1, 0.67);
               ch.Add(t2, 0.08);
           });
           //6/11/2014  1:15	0.67	 	60.4
           //6/11/2014 14:00	0.08	 	31.1

           Logger.EnableLogger();
           SeriesExpressionParser.Debug = true;
           var c = new CalculationSeries("canal_flow");
           c.Parser.VariableResolver.Add("afci_ch", new ParserResult(ch));
           c.TimeInterval = TimeInterval.Irregular;
           c.Properties.Set("shift", "-0.22");
           c.Expression = "GenericWeir(afci_ch+%property%.shift+1.2,28.5,1.5)";
           c.Calculate(); 


           Assert.AreEqual(2, c.Count, " expected two flow calculations");
           Assert.AreEqual(60.4, c[t1].Value, 0.1);
           Assert.AreEqual(31.1, c[t2].Value, 0.1);

       }


    }
}
