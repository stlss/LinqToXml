using System.Xml.Linq;
using System.Xml;
using System.Linq;
using System;

namespace LinqToXml
{
    internal static class Task
    {
        private static string pathFilesTasks = GetPathFilesTasks();

        private static string GetPathFilesTasks()
        {
            string path = Environment.CurrentDirectory;
            int index = path.IndexOf("LinqToXml");
            return path[..index] + "LinqToXml\\" + "FilesTasks\\";
        }

        /// <summary>
        /// Решить задание 1.
        /// </summary>
        public static void Solve07()
        {
            string pathTextFile = pathFilesTasks + "07-1\\TextFile.txt";
            string pathXmlFile = pathFilesTasks + "07-1\\XMLFile.xml";

            var numbersLines = File.ReadAllLines(pathTextFile).
                    Select(line => line.Split(' ').
                    Select(number => int.Parse(number)));

            XDocument xDocument = new(
                new XDeclaration("1.0", "utf-8", null),
                new XElement(
                    "root",
                    numbersLines.Select(numbersLine => new XElement(
                        "line",
                        new XElement(
                            "sum-positive",
                            numbersLine.Where(number => number > 0).Sum()
                            ),
                        numbersLine.Where(number => number < 0).Reverse().
                        Select(negativeNumber => new XElement(
                            "number-negative",
                            negativeNumber
                            ))
                    ))
                )
            );
            xDocument.Save(pathXmlFile);

            Console.WriteLine("Задание 1 решено.");
        }

        /// <summary>
        /// Решить задание 2.
        /// </summary>
        public static void Solve17()
        {
            string pathTextFile = pathFilesTasks + "17-2\\TextFile.txt";
            string pathXmlFile = pathFilesTasks + "17-2\\XMLFile.xml";

            XDocument xDocument = XDocument.Load(pathXmlFile);

            var elements = xDocument.Descendants().
                    Where(element => element.Nodes().Any(node => node.NodeType == XmlNodeType.Text)).
                    Select(element => new { Name = element.Name.LocalName, Text = element.Value });

            var names = elements.Select(element => element.Name).Distinct();

            var lines = names.Select(name => name + "\n" +
                string.Join("\n", elements.Where(element => element.Name == name).
                    Select(element => "\t" + element.Text).Order()));

            File.WriteAllLines(pathTextFile, lines);

            Console.WriteLine("Задание 2 решено.");
        }

        /// <summary>
        /// Решить задание 3.
        /// </summary>
        public static void Solve27()
        {
            string pathXmlFile1 = pathFilesTasks + "27-3\\XMLFile1.xml";
            string pathXmlFile2 = pathFilesTasks + "27-3\\XMLFile2.xml";

            XDocument xDocument = XDocument.Load(pathXmlFile1);

            _ = xDocument.Root!.Elements().Elements().Select(element =>
                {
                    element.ReplaceNodes(element.LastNode);
                    return element;
                }).
                ToList();

            xDocument.Save(pathXmlFile2);

            Console.WriteLine("Задание 3 решено.");
        }

        /// <summary>
        /// Решить задание 4.
        /// </summary>
        public static void Solve37()
        {
            string pathXmlFile1 = pathFilesTasks + "37-4\\XMLFile1.xml";
            string pathXmlFile2 = pathFilesTasks + "37-4\\XMLFile2.xml";

            XDocument xDocument = XDocument.Load(pathXmlFile1);

            _ = xDocument.Root!.Elements().Elements().Select(element =>
                {
                    string value = element.Value;
                    if (element.HasElements)
                    {
                        value += element.Descendants().Select(element3 => element3.Value).
                            Aggregate((x1, x2) => x1 + x2);
                    }
                    element.RemoveNodes();
                    element.SetValue(value);
                    return element;
                }).
                ToList();

            xDocument.Save(pathXmlFile2);

            Console.WriteLine("Задание 4 решено.");
        }

        /// <summary>
        /// Решить задание 5.
        /// </summary>
        public static void Solve47()
        {
            string pathXmlFile1 = pathFilesTasks + "47-5\\XMLFile1.xml";
            string pathXmlFile2 = pathFilesTasks + "47-5\\XMLFile2.xml";

            XDocument xDocument = XDocument.Load(pathXmlFile1);

            _ = xDocument.Descendants().ToList().Select(element => 
                {
                    XElement xElement = new(
                        "has-comments", 
                        element.Nodes().Any(node => node.NodeType == XmlNodeType.Comment)
                    );

                    var nodes = element.Nodes().ToList();
                    if (nodes.Count > 1)
                        nodes.Insert(1, xElement);
                    else
                        nodes.Add(xElement);

                    element.ReplaceNodes(nodes);
                    return element; 
                }).
                ToList();


            xDocument.Save(pathXmlFile2);

            Console.WriteLine("Задание 5 решено.");
        }

        /// <summary>
        /// Решить задание 6.
        /// </summary>
        public static void Solve57()
        {
            string pathXmlFile1 = pathFilesTasks + "57-6\\XMLFile1.xml";
            string pathXmlFile2 = pathFilesTasks + "57-6\\XMLFile2.xml";

            string s1 = "namespace1";
            string s2 = "namespace2";

            XDocument xDocument = XDocument.Load(pathXmlFile1);

            xDocument.Root!.Add(new XAttribute(XNamespace.Xmlns + "x", s1));
            xDocument.Root!.Add(new XAttribute(XNamespace.Xmlns + "y", s2));

            _ = xDocument.Descendants().ToList().Select(element =>
            {
                return element;
            }).
            ToList();

            _ = xDocument.Root!.Elements().Select(element => 
                {
                    element.Name = (XNamespace)s1 + element.Name.LocalName;

                    _ = element.Descendants().Select(element =>
                        {
                            element.Name = (XNamespace)s2 + element.Name.LocalName;
                            return element;
                        }).
                        ToList();

                    return element;
                }).
                ToList();

            xDocument.Save(pathXmlFile2);

            Console.WriteLine("Задание 6 решено.");
        }

        /// <summary>
        /// Решить задание 7.
        /// </summary>
        public static void Solve67()
        {
            string pathXmlFile1 = pathFilesTasks + "67-7\\XMLFile1.xml";
            string pathXmlFile2 = pathFilesTasks + "67-7\\XMLFile2.xml";

            XDocument xDocument1 = XDocument.Load(pathXmlFile1);

            XDocument xDocument2 = new(
                xDocument1.Declaration,
                new XElement(
                    "root",
                    xDocument1.Root!.Elements().
                        Select(client => client.Element("year")!.Value).
                        Distinct().OrderBy(year => int.Parse(year)).
                        Select(year => new XElement(
                            "y" + year,
                            xDocument1.Root!.Elements().
                                Where(client => client.Element("year")!.Value == year).
                                Select(client => client.Element("month")!.Value).
                                Distinct().OrderBy(month => int.Parse(month)).
                                Select(month => new XElement(
                                    "m" + month,
                                    new XAttribute(
                                        "total-time",
                                        xDocument1.Root!.Elements().
                                            Where(client => client.Element("year")!.Value == year &&
                                                client.Element("month")!.Value == month).
                                            Sum(client => XmlConvert.ToTimeSpan(client.Attribute("time")!.Value).TotalMinutes)
                                    ),
                                    new XAttribute(
                                        "client-count",
                                        xDocument1.Root!.Elements().
                                            Count(client => client.Element("year")!.Value == year &&
                                                client.Element("month")!.Value == month)
                                    )
                                ))
                        ))
                )
            );

            xDocument2.Save(pathXmlFile2);

            Console.WriteLine("Задание 7 решено.");
        }

        /// <summary>
        /// Решить задание 8.
        /// </summary>
        public static void Solve77()
        {
            string pathXmlFile1 = pathFilesTasks + "77-8\\XMLFile1.xml";
            string pathXmlFile2 = pathFilesTasks + "77-8\\XMLFile2.xml";

            XDocument xDocument1 = XDocument.Load(pathXmlFile1);

            XDocument xDocument2 = new(
                xDocument1.Declaration,
                new XElement(
                    "root",
                    xDocument1.Root!.Elements().
                        OrderBy(debt => int.Parse(debt.Attribute("house")!.Value)).
                        ThenBy(debt => int.Parse(debt.Attribute("flat")!.Value)).
                        Select(debt => new XElement(
                            "house" + debt.Attribute("house")!.Value + 
                                "-" + debt.Attribute("flat")!.Value,
                            new XAttribute("name", debt.Attribute("name")!.Value),
                            new XAttribute("debt", debt.Value)
                        ))
                )
            );

            xDocument2.Save(pathXmlFile2);

            Console.WriteLine("Задание 8 решено.");
        }

        /// <summary>
        /// Решить задание 9.
        /// </summary>
        public static void Solve87()
        {
            Console.WriteLine("Задание 9 решено.");
        }

        /// <summary>
        /// Решить задание 10.
        /// </summary>
        public static void Solve88()
        {
            Console.WriteLine("Задание 10 решено.");
        }
    }
}
