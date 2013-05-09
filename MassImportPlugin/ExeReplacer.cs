using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MassImport
{
    public class ExeReplacer
    {
        private static readonly string exe_path = @"C:\UltimaOnline\client\client-7.18.16.3.exe";
        private static readonly string out_path = @"C:\UltimaOnline\client\client-7.18.16.3_new.exe";
        private static readonly string nul_file = @"null";
        private static readonly string dir_path = @"C:\UltimaOnline\tools\Fiddler+\Output\_new (Новая версия)\bak";

        public static void RemoveFiles()
        {
            var buf = File.ReadAllBytes(exe_path);
            var nul = Encoding.ASCII.GetBytes(nul_file);
            Array.Resize(ref nul, nul.Length+1);
            nul[nul.Length-1] = 0x00;

            foreach (var file in Directory.GetFiles(dir_path)) {
                var name = Path.GetFileName(file);
                var arr1 = Encoding.ASCII.GetBytes(name.ToLower());
                var arr2 = Encoding.ASCII.GetBytes(name.ToUpper());
                Array.Resize(ref arr1, arr1.Length+1);
                Array.Resize(ref arr2, arr2.Length + 1);
                arr1[arr1.Length-1] = 0x00;
                arr2[arr2.Length-1] = 0x00;

                for (int k = 0, i = 0; i < buf.Length; ++i)
                    if (buf[i] != arr1[k] && buf[i] != arr2[k]) k = 0;
                    else if (++k == arr1.Length) {
                        Array.Copy(nul, 0, buf, i - k + 1, nul.Length);
                        for (int j = i - k + nul.Length; j < i; buf[++j] = 0xCD) ;
                        k = 0;
                    }
            }

            File.WriteAllBytes(out_path, buf);
        }
    }
}
