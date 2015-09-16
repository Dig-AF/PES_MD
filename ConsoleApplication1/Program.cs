using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace ConsoleApplication
{

    class Program
    {
        static void Main(string[] args)
        {
            string input;
            string output="";
            string errors = "";
            bool test;

            input = @"C:\GitHub\test\MD_TEST_DATA_Sprint14_2015-09-15.xml";
            test = EAWS.Core.SilverBullet.PES_MD.MD2PES(File.ReadAllBytes(input), ref output, ref errors);
            if (!test)
                System.IO.File.WriteAllText(@"C:\GitHub\test\error.csv", errors);
                
            System.IO.File.WriteAllText(@"C:\GitHub\test\1_out.xml", output);    

            //input = @"C:\test\1_out.xml";
            //XML_2 = NEAR.PES.PES2MD(File.ReadAllBytes(input));
            //System.IO.File.WriteAllText(@"C:\test\2_out.xml", XML_2);

            //input = @"C:\test\UPIA Model.emx";
            //XML_2 = NEAR.PES.RSA2PES(File.ReadAllBytes(input));
            //System.IO.File.WriteAllText(@"C:\test\3_out.xml", XML_2);

            //input = @"C:\test\3_out.xml";
            //XML_2 = NEAR.PES.PES2RSA(File.ReadAllBytes(input));
            //System.IO.File.WriteAllText(@"C:\test\4_out.xml", XML_2);
        }
    }
}
