using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace BaseArchitect.Utility
{
    public class ZipHelper
    {
        public static string Zip(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    msi.CopyTo(gs);
                }

                return Convert.ToBase64String(mso.ToArray());
            }
        }

        public static string Unzip(string input)
        {
            byte[] bytes = Convert.FromBase64String(input);
            using (var msi = new MemoryStream(bytes))
            {
                using (var mso = new MemoryStream())
                {
                    using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                    {
                        gs.CopyTo(mso);
                    }

                    return Encoding.UTF8.GetString(mso.ToArray());
                }
            }
        }
    }
}
