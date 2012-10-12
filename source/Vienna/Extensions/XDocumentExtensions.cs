using System;
using System.Xml.Linq;
using OpenTK;

namespace Vienna.Extensions
{
    public static class XDocumentExtensions
    {
        public static XElement GetRootOrThrow(this XDocument doc)
        {
            if (doc == null) throw new ArgumentNullException("doc");

            var root = doc.Root;
            if (root == null) throw new Exception("Xml document does not contain a root element");

            return root;
        }

        public static XAttribute GetAttributeOrThrow(this XElement el, string name)
        {
            if (el == null) throw new ArgumentNullException("el");

            var attr = el.Attribute("name");
            if (attr == null) throw new Exception(string.Format("Element {0} does not contain attribute {1}", el.Name, name));

            return attr;
        }

        public static XElement GetElement(this XElement el, string name)
        {
            if (el == null) throw new ArgumentNullException("el");

            var value = el.Element(name);
            if (value == null) throw new Exception(string.Format("Element {0} does not contain sub element {1}", el.Name, name));

            return value;
        }

        public static Vector2 AttributeAsVector2(this XElement el, string xname, string yname)
        {
            return new Vector2( el.AttributeAsFloat(xname), el.AttributeAsFloat(yname) );
        }

        public static float AttributeAsFloat(this XElement el, string name)
        {
            if (el == null) throw new ArgumentNullException("el");
            var attr = el.Attribute(name);
            
            if(attr == null) throw new Exception(string.Format("Element {0} does not contain attribute {1}", el.Name, name));
            
            return float.Parse(attr.Value);
        }
    }
}
