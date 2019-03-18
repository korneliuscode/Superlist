/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Xml;

namespace BinaryComponents.Utility.Assemblies
{
    public sealed class ManifestResources
    {
        public ManifestResources(string baseNamespace)
            : this(baseNamespace, Assembly.GetCallingAssembly())
        {
        }

        public string[] ResourceNames
        {
            get
            {
                return _assembly.GetManifestResourceNames();
            }
        }

        public ManifestResources(string baseNamespace, Assembly assembly)
        {
            if (baseNamespace == null)
            {
                throw new ArgumentNullException("baseNamespace");
            }
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            _baseNamespace = baseNamespace;
            _assembly = assembly;
        }

        public XmlDocument GetXmlDocument(string path)
        {
            XmlDocument xmlDoc = new XmlDocument();

            using (Stream stream = GetStream(path))
            {
                if (stream == null)
                {
                    throw new ArgumentException(string.Format("Resource '{0}' not found.", path), "path");
                }
                xmlDoc.Load(stream);
            }

            return xmlDoc;
        }

        public string GetString(string path)
        {
            using (Stream stream = GetStream(path))
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        public Stream GetStream(string path)
        {
            return _assembly.GetManifestResourceStream(_baseNamespace + "." + path);
        }

        public Icon GetIcon(string path)
        {
            using (Stream stream = GetStream(path))
            {
                return new Icon(stream);
            }
        }

        public Image GetImage(string path)
        {
            using (Stream stream = GetStream(path))
            {
                return Image.FromStream(stream);
            }
        }

        private string _baseNamespace;
        private Assembly _assembly;
    }
}