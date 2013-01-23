using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Umblr.Umblr
{
    public class PostFactory
    {
        public static Post Convert(string path)
        {
            return Convert(new FileInfo(path));
        }

        public static Post Convert(FileInfo fi)
        {
            var p = new Post();
            p.FileName = fi.Name;
            p.Draft = p.FileName.ToLower().EndsWith(".draft.md");

            DateTime _d;
            bool hasDate = true;
            if (p.FileName.Length > 10 && DateTime.TryParse(p.FileName.Substring(0, 10), out _d))
                p.Date = _d;
            else
            {
                p.Date = DateTime.Now;
                hasDate = false;

            }
            p.Name = GetName(p, hasDate);
            var s = fi.OpenText();
            p.Content = s.ReadToEnd();
            s.Close();
            
            return p;
        }

        private static string GetName(Post p, bool hasDate)
        {
            var name = p.FileName;

            if (p.Draft)
                name = name.Remove(name.Length - 9);
            else
                name = name.Remove(name.Length - 3);

            if (hasDate)
                name = name.Substring(11);

            name = name.Replace('-', ' ');
            return name;
        }
    }
}