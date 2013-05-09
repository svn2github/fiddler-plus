using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Ultima;

namespace MassImport
{
    public partial class MoveShips : Form
    {
        public MoveShips()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(tbFolder.Text, "_new");

            int minid = 0x5780; int minid2 = 0x49FC; int minid3 = 0x4AB9;
            int maxid = 0x992C; int maxid2 = 0x4A8D; int maxid3 = 0x4AF4;
            //int newid = 0xA000;
            int newid = 0x8000;
            int count = 0;

            int[] idmat = new int[0xFFFF];
            for (int id = 0; id < idmat.Length; ++id)
                idmat[id] = -1;

            Ultima.Files.SetMulPath(tbFolder.Text);
            for (int id = 0; id < idmat.Length; ++id)
                if ((id >= minid && id <= maxid) || (id >= minid2 && id <= maxid2) || (id >= minid3 && id <= maxid3))
                    if (Art.IsValidStatic(id)) {
                        idmat[id] = newid + count;
                        ++count;
                    }

            string fpath1 = Path.Combine(Path.Combine(Ultima.Files.RootDir, "_old"), "VirtualTiles.xml");
            string fpath2 = Path.Combine(path, "VirtualTiles.xml");
            using (StreamReader sr = new StreamReader(new FileStream(fpath1, FileMode.Open,   FileAccess.Read,      FileShare.Read))) {
            using (StreamWriter sw = new StreamWriter(new FileStream(fpath2, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))) {
                String line;
                // Read and display lines from the file until the end of
                // the file is reached.
                bool galeon = false;
                while ((line = sr.ReadLine()) != null) {
                    var st = line.Trim().Split(new char[] {'\"', '<', ' ', '/', '>'}, StringSplitOptions.RemoveEmptyEntries);
                    var si = st.Length-1;
                    /*
                    if (si > 2 && st[0] == "Unused") {
                        galeon = (st[si] == "галеон");
                    } 

                    if (galeon && st.Length > 0 && st[si].Length == 6 && st[si][0] == '0' && st[si][1] == 'x') {
                        var se = Int32.Parse(st[si].Substring(2), NumberStyles.HexNumber);

                        idmat[se] = newid + count;
                        ++count;
                        line = line.Replace(st[si], String.Format("0x{0:X4}", idmat[se]));
                    }
                    */
                    if (st.Length > 0 && st[si].Length == 6 && st[si][0] == '0' && st[si][1] == 'x') {
                        var se = Int32.Parse(st[si].Substring(2), NumberStyles.HexNumber);
                        if (idmat[se] >= 0)
                            line = line.Replace(st[si], String.Format("0x{0:X4}", idmat[se]));
                    }
                    sw.WriteLine(line);
                }
                sr.Close(); sw.Close();
            } }

            fpath1 = Path.Combine(Path.Combine(Ultima.Files.RootDir, "_old"), "TilesGroup.xml");
            fpath2 = Path.Combine(path, "TilesGroup.xml");
            using (StreamReader sr = new StreamReader(new FileStream(fpath1, FileMode.Open,   FileAccess.Read,      FileShare.Read))) {
            using (StreamWriter sw = new StreamWriter(new FileStream(fpath2, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))) {
                String line;
                // Read and display lines from the file until the end of
                // the file is reached.
                bool galeon = false;
                while ((line = sr.ReadLine()) != null) {
                    var st = line.Trim().Split(new char[] {'\"', '<', ' ', '/', '>'}, StringSplitOptions.RemoveEmptyEntries);
                    var si = st.Length-1;
                    if (st.Length > 0 && st[si].Length == 6 && st[si][0] == '0' && st[si][1] == 'x') {
                        var se = Int32.Parse(st[si].Substring(2), NumberStyles.HexNumber);
                        if (idmat[se] >= 0)
                            line = line.Replace(st[si], String.Format("0x{0:X4}", idmat[se]));
                    }
                    sw.WriteLine(line);
                }
                sr.Close(); sw.Close();
            } }

            int ccc = 0;
            for (int id = idmat.Length - 1; id >= 0; --id)
                if (idmat[id] >= 0) {
                    Art.ReplaceStatic(idmat[id], Art.GetStatic(id));
                    Art.RemoveStatic(id);

                    var tdtemp = TileData.ItemTable[idmat[id]];
                    TileData.ItemTable[idmat[id]] = TileData.ItemTable[id];
                    TileData.ItemTable[id] = tdtemp;

                    var rctemp = RadarCol.GetItemColor(idmat[id]);
                    RadarCol.SetItemColor(idmat[id], RadarCol.GetItemColor(id));
                    RadarCol.SetItemColor(id, rctemp);

                    // Cliloc
                } else if (idmat[id] == -55)
                    ++ccc;
  
            MoveMultis(Ultima.Files.RootDir, path, idmat);
            MoveStatic(Ultima.Files.RootDir, path, 0, idmat);
            MoveStatic(Ultima.Files.RootDir, path, 1, idmat);
            /*
            for (int multi = 0; multi < 0x2000; ++multi) {
                var comp = Multis.GetComponents(multi);
                if (comp == MultiComponentList.Empty) continue;
                for (int tid = 0; tid < comp.SortedTiles.Length; ++tid) {
                    var tile = comp.SortedTiles[tid];
                    if (idmat[tile.m_ItemID] >= 0)
                        tile.m_ItemID = (ushort)idmat[tile.m_ItemID];
                }
                //Multis.Add(multi, comp);                
            } 
             
            for (int x = 0; x < Map.Dangeon.Width; ++x)
                for (int y = 0; y < Map.Dangeon.Height; ++y) {
                    var tiles = Map.Dangeon.Tiles.GetStaticTiles(x, y, false);
                    for (int tid = 0; tid < tiles.Length; ++tid)
                        if (idmat[tiles[tid].ID] > 0)
                            tiles[tid].ID = (ushort)idmat[tiles[tid].ID]; 
                }
                
            for (int x = 0; x < Map.Sosaria.Width; ++x)
                for (int y = 0; y < Map.Sosaria.Height; ++y) {
                    var tiles = Map.Sosaria.Tiles.GetStaticTiles(x, y, false);
                    for (int tid = 0; tid < tiles.Length; ++tid)
                        if (idmat[tiles[tid].ID] > 0)
                            tiles[tid].ID = (ushort)idmat[tiles[tid].ID]; 
                }
            */
          
            // Saving data....
            Art.Save(path);
            TileData.SaveTileData(Path.Combine(path, "tiledata.mul"));
            RadarCol.Save(Path.Combine(path, "radarcol.mul"));
//            Multis.Save(path);
            MessageBox.Show("Done");
        }

        public void MoveMultis(string path, string newpath, int[] idmat)
        {
            string fpath1 = Path.Combine(path, "multi.idx");
            string fpath2 = Path.Combine(path, "multi.mul");
            string fpath3 = Path.Combine(newpath, "multi.idx");
            string fpath4 = Path.Combine(newpath, "multi.mul");

            for (int a = 0; a < idmat.Length; ++a)
                if (idmat[a] > 0) {
                    int b = idmat[a];
                    continue;
                }

            using (BinaryReader bin1 = new BinaryReader(new FileStream(fpath1, FileMode.Open,   FileAccess.Read,  FileShare.Read)),
                                bin2 = new BinaryReader(new FileStream(fpath2, FileMode.Open,   FileAccess.Read,  FileShare.Read))) {
            using (BinaryWriter bin3 = new BinaryWriter(new FileStream(fpath3, FileMode.Create, FileAccess.ReadWrite, FileShare.Read)),
                                bin4 = new BinaryWriter(new FileStream(fpath4, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))) {
                int MultisCount = (int)bin1.BaseStream.Length / 12;
                bin4.Write(bin2.ReadBytes((int)bin2.BaseStream.Length));
                for (uint k = 0; k < MultisCount; ++k) {
                    uint offset  = bin1.ReadUInt32();       bin3.Write(offset);
                    uint length  = bin1.ReadUInt32();       bin3.Write(length);
                    uint unknown = bin1.ReadUInt32();       bin3.Write(unknown);

                    if (offset == 0xFFFFFFFF) continue;
                    if (offset + length > bin2.BaseStream.Length)
                        throw new IOException("Попытка чтения за пределами файла.");

                    uint cellCount = length / 16;
                    bin2.BaseStream.Position = offset;
                    bin4.BaseStream.Position = offset;
                    //bin4.Seek((int)offset, SeekOrigin.Begin);
                    for (uint i = 0; i < cellCount; ++i) {
                        ushort tileID  = bin2.ReadUInt16();     bin4.Write((ushort)(idmat[tileID] > 0 ? idmat[tileID] : tileID));
                        short  tileX   = bin2.ReadInt16();      bin4.Write(tileX);
                        short  tileY   = bin2.ReadInt16();      bin4.Write(tileY);
                        short  tileZ   = bin2.ReadInt16();      bin4.Write(tileZ);
                        int    tileFlg = bin2.ReadInt32();      bin4.Write(tileFlg);
                        int    tileHue = bin2.ReadInt32();      bin4.Write(tileHue);
                    }
                }
                bin1.Close(); bin2.Close(); bin3.Close(); bin4.Close();
            } }
        }

        public void MoveStatic(string path, string newpath, int map, int[] idmat)
        {
            string fpath1 = Path.Combine(path, String.Format("staidx{0}.mul", map));
            string fpath2 = Path.Combine(path, String.Format("statics{0}.mul", map));

            string fpath3 = Path.Combine(newpath, String.Format("staidx{0}.mul", map));
            string fpath4 = Path.Combine(newpath, String.Format("statics{0}.mul", map));

            using (BinaryReader bin1 = new BinaryReader(new FileStream(fpath1, FileMode.Open,   FileAccess.Read,  FileShare.Read)), 
                                bin2 = new BinaryReader(new FileStream(fpath2, FileMode.Open,   FileAccess.Read,  FileShare.Read))) { 
            using (BinaryWriter bin3 = new BinaryWriter(new FileStream(fpath3, FileMode.Create, FileAccess.Write, FileShare.Read)),
                                bin4 = new BinaryWriter(new FileStream(fpath4, FileMode.Create, FileAccess.Write, FileShare.Read))) {                  
                int MapBlockCount = (int)bin1.BaseStream.Length / 12;
                bin4.Write(bin2.ReadBytes((int)bin2.BaseStream.Length));
                for (uint k = 0; k < MapBlockCount; ++k) {
                    uint offset  = bin1.ReadUInt32();           bin3.Write(offset);
                    uint length  = bin1.ReadUInt32();           bin3.Write(length);
                    uint unknown = bin1.ReadUInt32();           bin3.Write(unknown);
                    
                    if (offset == 0xFFFFFFFF) continue;
                    if (offset + length > bin2.BaseStream.Length)
                        throw new IOException("Попытка чтения за пределами файла.");

                    uint cellCount = length / 7;
                    bin2.BaseStream.Position = offset;
                    bin4.BaseStream.Position = offset;
                    for (uint i = 0; i < cellCount; ++i) {
                        ushort  tileID  = bin2.ReadUInt16();        bin4.Write((ushort)(idmat[tileID] > 0 ? idmat[tileID] : tileID));
                        byte    tileX   = bin2.ReadByte();          bin4.Write(tileX);
                        byte    tileY   = bin2.ReadByte();          bin4.Write(tileY);
                        byte    tileAlt = bin2.ReadByte();          bin4.Write(tileAlt);
                        ushort  tileHue = bin2.ReadUInt16();        bin4.Write(tileHue);
                    }
                }
                bin1.Close();   bin2.Close();   bin3.Close();   bin4.Close();
            } } 
        }
    }
}
