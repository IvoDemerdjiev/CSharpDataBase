namespace FileToXml
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    class FileToXml
    {
        static void Main(string[] args)
        {
            string path = @"E:\C#\Tree\XMLProcessing\FileToXml\Person.txt";

            if (!File.Exists(path))
            {
                File.AppendAllLines(path, new[] { "Ivan" });
                File.AppendAllLines(path, new[] { "Izgrev 23" });
                File.AppendAllLines(path, new[] { "0897345432" });
            }

            String[] data = File.ReadAllLines(@"E:\C#\Tree\XMLProcessing\FileToXml\Person.txt");
            XDocument xml = new XDocument(
                new XElement("Person",
                new XElement("name",data[0]),
                new XElement("addresss",data[1]),
                new XElement("Phone-Numeber",data[2])));

            xml.Save("XmlFile.Xml");
        }
    }
}
