namespace XMLProcessing
{
    using System;
    using System.Collections;
    using System.Xml;
    using System.Linq;
    using System.Xml.Linq;
    using System.Collections.Generic;
    using System.Xml.Xsl;

    class Catalogue
    {
        static void ExtractsArtistsWithDOM(XmlDocument doc)
        {
            doc.Load("../../Catalogue.xml");
            XmlNode rootNode = doc.DocumentElement;

            foreach (XmlNode node in rootNode.ChildNodes)
            {
                var autor = node["artist"].InnerText;
                Console.WriteLine("autor {0}", autor);

            }
            Console.WriteLine();
        }

        static void PrintAlbumNumWithDOM(XmlDocument doc)
        {
            doc.Load("../../Catalogue.xml");
            XmlNode rootNode = doc.DocumentElement;
            Hashtable hashtable = new Hashtable();
            int i = 0;

            foreach (XmlNode node in rootNode.ChildNodes)
            {
                var autor = node["artist"].InnerText;
                i++;
                hashtable.Add(i, autor);

            }

            foreach (DictionaryEntry item in hashtable)
                Console.WriteLine("{0} Albums in catalog, Author {1}", item.Key, item.Value);
        }

        static void ExtractsArtistsWithXPath(XmlDocument doc)
        {
            doc.Load("../../Catalogue.xml");
            string xPathQuery = ("/catalogue/album");

            XmlNodeList autorList =
                doc.SelectNodes(xPathQuery);
            foreach (XmlNode node in autorList)
            {
                string autorName = node.SelectSingleNode("artist").InnerText;
                Console.WriteLine(autorName);

            }
            Console.WriteLine();
        }

        static void DeleteAlbume(XmlDocument doc)
        {
            doc.Load("../../Catalogue.xml");
            XmlNodeList nodes = doc.SelectNodes("/catalogue/album[price>20]");
            foreach (XmlNode node in nodes)
            {
                Console.WriteLine("Album removed:{0}",node["name"].InnerText);
                node.ParentNode.RemoveChild(node);
            }
            Console.WriteLine();
        }

        static void ExtractSongsNames()
        {
            Console.WriteLine("Songs titles:");
            using (XmlReader reader = XmlReader.Create("../../Catalogue.xml"))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        //return only when you have START tag

                        switch (reader.Name.ToString())
                        {
                            case "title":
                                Console.WriteLine(reader.ReadString());
                                break;
                        }
                    }
                }
                Console.WriteLine();
            }
        }

        static void ExtractSongsNamesWithXDocumentLINQ()
        {
            
            var xdoc = XDocument.Load("../../Catalogue.xml");

            var songs = from album in xdoc.Descendants("song")
                       select new
                       {
                           Title = album.Element("title").Value
                       };

            Console.WriteLine("Songs name extract with xdocument");
            foreach (var song in songs)
            {
                Console.WriteLine(song.Title);
            }
            Console.WriteLine();
        }

        static void ExtractAlbumNameAndAuthor()
        {
            List<string> namesAndAuthors = new List<string>();

            using (XmlReader reader = XmlReader.Create("../../Catalogue.xml"))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        //return only when you have START tag

                        if (reader.Name == "name")
                        {
                            reader.Read();
                            var res = reader.Value;
                            namesAndAuthors.Add(res);
                        }

                        if (reader.Name == "artist")
                        {
                            reader.Read();
                            var res = reader.Value;
                            namesAndAuthors.Add(res);
                        }
                    }
                }

                using (XmlWriter writer = XmlWriter.Create("albums.xml"))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("albums");

                    while (namesAndAuthors.Count>0)
                    {
                        writer.WriteStartElement("album");

                        writer.WriteElementString("name", namesAndAuthors[0]);
                        writer.WriteElementString("artist", namesAndAuthors[1]);

                        namesAndAuthors.RemoveRange(0, 2);

                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Close();
                }

            }
        }

        static void ExtractPrices(XmlDocument doc)
        {
            doc.Load("../../Catalogue.xml");
            XmlNodeList nodes = doc.SelectNodes("/catalogue/album[year>2012]");
            Console.WriteLine("Albums published 5 years ago or earlier");
            foreach (XmlNode node in nodes)
            {
                Console.WriteLine("Album name:{0}-price:{1}",node["name"].InnerText, node["price"].InnerText);
                node.ParentNode.RemoveChild(node);
            }
            Console.WriteLine();
        }

        static void ExtractPricesXDocumentLINQ()
        {

            var xdoc = XDocument.Load("../../Catalogue.xml");

            var prices = from album in xdoc.Descendants("album")
                         where Int32.Parse(album.Element("year").Value) > 2012
                         select new
                          {
                            year = album.Element("year").Value,
                            price = album.Element("price").Value
                          };

            Console.WriteLine("Prices extract with xdocument and LINQ");
            foreach (var newest in prices)
            {
                    Console.WriteLine(newest.year);
                    Console.WriteLine(newest.price);
            }
            Console.WriteLine();
        }

        static void Main()
        {
            XmlDocument doc = new XmlDocument();

            ExtractsArtistsWithDOM(doc);

            PrintAlbumNumWithDOM(doc);

            ExtractsArtistsWithXPath(doc);

            DeleteAlbume(doc);

            ExtractSongsNames();

            ExtractSongsNamesWithXDocumentLINQ();

            ExtractAlbumNameAndAuthor();

            ExtractPrices(doc);

            ExtractPricesXDocumentLINQ();

            //var myXslTrans = new XslCompiledTransform();
            //myXslTrans.Load("'Catalogue.xsl");
            //myXslTrans.Transform("Catalogue.xml", "result.html");
        }
    }
}
