using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace ApplyXsltTransformToXml
{
    class Program
    {
        static void Main(string[] args)
        {
            var transform = @"<xsl:stylesheet version=""1.0"" xmlns:xsl=""http://www.w3.org/1999/XSL/Transform""> 
                            <xsl:output method=""xml"" omit-xml-declaration=""yes"" indent=""yes"" />      
                            <xsl:strip-space elements=""*"" />
                            <xsl:template match=""//root/fruit"">
                                <xsl:attribute name=""name"">
                                    <xsl:value-of select=""Orange"" />
                                </xsl:attribute >
                            </xsl:template>
                            </xsl:stylesheet>";

            var source = @"<?xml version=""1.0"" encoding=""UTF-8""?><root><fruit name=""Apple""></fruit></root>";

            using (XmlReader transformReader = XmlReader.Create(new StringReader(transform)))
            {
                // Create an XPathDocument using the reader containing the XML
                MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(source));
                XPathDocument sourceDoc = new XPathDocument(new StreamReader(ms));

                using (XmlReader sourceReader = XmlReader.Create(new StringReader(source)))
                {
                    // Create an XmlWrite to store the resulting transformed XML
                    StringBuilder resultString = new StringBuilder();
                    var xws = new XmlWriterSettings();
                    xws.ConformanceLevel = ConformanceLevel.Fragment;
                    XmlWriter writer = XmlWriter.Create(resultString, xws);

                    // Load the XSLT stylesheet into the transformer
                    var xsltTransform = new XslCompiledTransform();
                    xsltTransform.Load(transformReader);

                    // Perform the transformation
                    xsltTransform.Transform(sourceReader, writer);

                    Console.WriteLine(writer);
                }
            }

            Console.ReadLine();
        }
    }
}