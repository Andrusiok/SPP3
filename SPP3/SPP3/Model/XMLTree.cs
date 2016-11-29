using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Linq;
using SPP3.ViewModel;

namespace SPP3.Model
{
    public class XMLTreeAsm
    {
        XDocument document;
        public List<Threads> threadList = new List<Threads> { };

        public XMLTreeAsm(Stream stream)
        {
            document = new XDocument();
            try
            {
                document = XDocument.Load(stream);
            }
            catch(Exception e)
            {

            }
        }

        public void AcquireThreads(XElement parent)
        {
            foreach (XElement child in parent.Elements())
            {
                int id=0;
                int time=0;

                foreach (XAttribute attribute in child.Attributes())
                {
                    switch (attribute.Name.LocalName)
                    {
                        case "id":
                            Int32.TryParse(attribute.Value, out id);
                            break;
                        case "time":
                            Int32.TryParse(attribute.Value, out time);
                            break;
                    }
                }

                Threads thread = new Threads(id, time);
                thread = AcquireMethods(child, thread) as Threads;
                threadList.Add(thread);
            }
        }

        public ObservableObject AcquireMethods(XElement parent, ObservableObject listparent)
        {
            foreach (XElement child in parent.Elements())
            {
                int paramsCount = 0;
                int time = 0;
                string name = null;
                string package = null;

                foreach (XAttribute attribute in child.Attributes())
                {
                    switch (attribute.Name.LocalName)
                    {
                        case "paramscount":
                            Int32.TryParse(attribute.Value, out paramsCount);
                            break;
                        case "time":
                            Int32.TryParse(attribute.Value, out time);
                            break;
                        case "name":
                            name = attribute.Value;
                            break;
                        case "package":
                            package = attribute.Value;
                            break;
                    }
                }

                Methods method = new Methods(time, paramsCount, package, name, listparent);
                if (child.HasElements) method = AcquireMethods(child, method) as Methods;
                listparent.Add(method);
                
            }
            return listparent;
        }

        public List<Threads> FillList()
        {
            XElement root = document.Root;
            AcquireThreads(root);
            return threadList;
        }

    }

    public class XMLTreeDAsm
    {
        private string path = null;
        XDocument document = new XDocument();

        public XMLTreeDAsm(string path)
        {
            this.path = path;
            document.Add(new XElement("root"));
        }

        public void LoadThreads(IEnumerable<Threads> list)
        {
            foreach (Threads thread in list)
            {
                XElement xthread = new XElement("thread");
                xthread.Add(new XAttribute("id", thread.ThreadID));
                xthread.Add(new XAttribute("time", thread.Time));

                xthread = LoadMethods(thread.MethodsList, xthread);
                document.Root.Add(xthread);
            }

            document.Save(path);
        }

        public XElement LoadMethods(IEnumerable<Methods> list, XElement xparent)
        {
            foreach (Methods method in list)
            {
                XElement xmethod = new XElement("method");
                xmethod.Add(new XAttribute("name", method.Name));
                xmethod.Add(new XAttribute("time", method.Time));
                xmethod.Add(new XAttribute("paramscount", method.ParamsCount));
                xmethod.Add(new XAttribute("package", method.Package));

                if (method.Count != 0) xmethod = LoadMethods(method.MethodsList, xmethod);
                xparent.Add(xmethod);
            }
            return xparent;
        }
    }

}
