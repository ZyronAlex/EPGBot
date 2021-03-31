using EPGManager.Interfaces;
using EPGManager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace EPGManager.Services
{
    public class EPGService : IEPGService
    {
        private readonly IEPGRepository _repository;
        public EPGService(IEPGRepository repository)
        {
            _repository = repository;
        }

        public bool ImporteEPG(string filePath)
        {
            XmlTextReader reader = new XmlTextReader(filePath);

            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        Console.Write("<" + reader.Name);

                        while (reader.MoveToNextAttribute()) // Read the attributes.
                            Console.Write(" " + reader.Name + "='" + reader.Value + "'");
                        Console.Write(">");
                        Console.WriteLine(">");
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        Console.WriteLine(reader.Value);
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        Console.Write("</" + reader.Name);
                        Console.WriteLine(">");
                        break;
                }
            }

            return true;
        }
    }

    public class tv
    {
        List<Channel> channel { get; set; }
        List<Programme> programme { get; set; }
    }
}
