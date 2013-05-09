using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ultima
{
    public sealed class StringTable
    {
        private int m_Header1;
        private short m_Header2;

        public Dictionary<int, StringEntry> Entries
        {
            get { return m_Entries; }
            set { m_Entries = value; }
        }
        private Dictionary<int, StringEntry> m_Entries;

        public StringTable() : this(Files.GetFilePath("cliloc.enu"))
        {
        }

        public StringTable(string path)
        {
            m_Entries = new Dictionary<int, StringEntry>();

            using (BinaryReader bin = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                m_Header1 = bin.ReadInt32();
                m_Header2 = bin.ReadInt16();
                
                byte[] m_Buffer = new byte[1024];

                while (bin.BaseStream.Length != bin.BaseStream.Position)
                {
                    int number = bin.ReadInt32();
                    byte flag = bin.ReadByte();
                    int length = bin.ReadInt16();

                    if (length > m_Buffer.Length)
                        m_Buffer = new byte[(length + 1023) & ~1023];

                    bin.Read(m_Buffer, 0, length);
                    string text = Encoding.UTF8.GetString(m_Buffer, 0, length);

                    m_Entries.Add(number, new StringEntry(number, text, flag));
                }
            }
        }

        public StringEntry GetEntry(int number)
        {
            StringEntry entry;
            bool result = m_Entries.TryGetValue(number, out entry);
            if (!result)
                entry = null;
            return entry;
        }
    
        public string GetText(int number)
        {
            StringEntry entry = GetEntry(number);

            string text = null;
            if(entry != null)
                text = entry.Text;

            return text;
        }
    }
}
