using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Core
{
    public class Config
    {
        //TODO: Make dynamic
        static string basePath = @"C:\Users\Gustav\Projects\G-CPU\config\";
        const string fileappendix = ".xml";



        public XmlReader GetXmlReader(string fileName)
        {

            return XmlReader.Create(basePath + fileName + fileappendix);
        }










    }
}
