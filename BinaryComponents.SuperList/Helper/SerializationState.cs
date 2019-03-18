/////////////////////////////////////////////////////////////////////////////
//
// (c) 2007 BinaryComponents Ltd.  All Rights Reserved.
//
// http://www.binarycomponents.com/
//
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using System.Xml;

namespace BinaryComponents.SuperList.Helper
{
    public class SerializationState
    {
        public enum GroupState
        {
            GroupCollapsed,
            GroupExpanded
        } ;

        public class ColumnState
        {
            public string Name;
            public int Width;
            public SortOrder SortOrder = SortOrder.None;
            public SortOrder GroupSortOrder = SortOrder.None;
            public int VisibleIndex = -1;
            public int GroupedIndex = -1;
        }

        public class GroupInstance
        {
            public string GroupName;
            public object[] GroupPath;
        }

        public ColumnState[] ColumnStates = new ColumnState[0];
        public GroupState GlobalGroupState;
        public GroupInstance[] GroupStates = new GroupInstance[0];

        public static void Serialize(XmlTextWriter tw, SerializationState ss)
        {
            tw.WriteStartElement("SerializationState2");
            tw.WriteAttributeString("GlobalGroupState", ss.GlobalGroupState.ToString());

            tw.WriteStartElement("ColumnStates");

            foreach (ColumnState cs in ss.ColumnStates)
            {
                tw.WriteStartElement("ColumnState");
                tw.WriteAttributeString("Name", cs.Name);
                tw.WriteAttributeString("Width", cs.Width.ToString(CultureInfo.InvariantCulture));
                tw.WriteAttributeString("SortOrder", cs.SortOrder.ToString());
                tw.WriteAttributeString("GroupSortOrder", cs.GroupSortOrder.ToString());
                tw.WriteAttributeString("VisibleIndex", cs.VisibleIndex.ToString(CultureInfo.InvariantCulture));
                tw.WriteAttributeString("GroupedIndex", cs.GroupedIndex.ToString(CultureInfo.InvariantCulture));
                tw.WriteEndElement();
            }

            tw.WriteEndElement();

            tw.WriteStartElement("GroupStates");

            foreach (GroupInstance gi in ss.GroupStates)
            {
                tw.WriteStartElement("GroupState");
                tw.WriteAttributeString("GroupName", gi.GroupName);

                tw.WriteStartElement("GroupPath");

                foreach (string s in gi.GroupPath)
                {
                    tw.WriteStartElement("String");
                    tw.WriteAttributeString("Value", s);
                    tw.WriteEndElement();
                }

                tw.WriteEndElement();

                tw.WriteEndElement();
            }

            tw.WriteEndElement();

            tw.WriteEndElement();
        }

        public static SerializationState Deserialize(XmlTextReader tr)
        {
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(tr);

            XmlNode ssNode = xmlDoc.SelectSingleNode("/SerializationState2");

            if (ssNode == null)
            {
                return null;
            }

            SerializationState ss = new SerializationState();

            if (ssNode.Attributes["GlobalGroupState"] != null)
            {
                ss.GlobalGroupState = (GroupState) Enum.Parse(typeof (GroupState), ssNode.Attributes["GlobalGroupState"].Value);
            }

            List<ColumnState> columnStates = new List<ColumnState>();

            foreach (XmlNode csNode in ssNode.SelectNodes("ColumnStates/ColumnState"))
            {
                ColumnState cs = new ColumnState();

                if (csNode.Attributes["Name"] != null)
                {
                    cs.Name = csNode.Attributes["Name"].Value;
                }
                if (csNode.Attributes["Width"] != null)
                {
                    cs.Width = int.Parse(csNode.Attributes["Width"].Value, CultureInfo.InvariantCulture);
                }
                if (csNode.Attributes["SortOrder"] != null)
                {
                    cs.SortOrder = (SortOrder) Enum.Parse(typeof (SortOrder), csNode.Attributes["SortOrder"].Value);
                }
                if (csNode.Attributes["GroupSortOrder"] != null)
                {
                    cs.GroupSortOrder = (SortOrder) Enum.Parse(typeof (SortOrder), csNode.Attributes["GroupSortOrder"].Value);
                }
                if (csNode.Attributes["VisibleIndex"] != null)
                {
                    cs.VisibleIndex = int.Parse(csNode.Attributes["VisibleIndex"].Value, CultureInfo.InvariantCulture);
                }
                if (csNode.Attributes["GroupedIndex"] != null)
                {
                    cs.GroupedIndex = int.Parse(csNode.Attributes["GroupedIndex"].Value, CultureInfo.InvariantCulture);
                }

                columnStates.Add(cs);
            }

            ss.ColumnStates = columnStates.ToArray();

            List<GroupInstance> groupInstances = new List<GroupInstance>();

            foreach (XmlNode giNode in ssNode.SelectNodes("GroupStates/GroupState"))
            {
                GroupInstance gi = new GroupInstance();

                if (giNode.Attributes["GroupName"] != null)
                {
                    gi.GroupName = giNode.Attributes["GroupName"].Value;
                }

                List<string> path = new List<string>();

                foreach (XmlNode pathNode in giNode.SelectNodes("GroupPath/String"))
                {
                    path.Add(pathNode.Attributes["Value"].Value);
                }

                gi.GroupPath = path.ToArray();

                groupInstances.Add(gi);
            }

            ss.GroupStates = groupInstances.ToArray();

            return ss;
        }
    }
}