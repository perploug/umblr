using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MarkdownSharp;
using Umbraco.Core.Models;
using umbraco.MacroEngines;
using Umbraco.Core.Services;
using System.Threading;

namespace Umblr.Umblr
{
    public class PostPublisher
    {
        private const string PostTypeAlias = "Post";
        private const string StreamTypeAlias = "Stream";


        public void ProcessPost(Post p)
        {
            if(!p.Draft)
                CreateAndPublish(p);
        }


        public bool Exists(Post p)
        {
            int id;
            return TryGetPostId(p, out id);
        }

        public void Delete(Post p)
        {
            int id;
            if (TryGetPostId(p, out id))
            {
                var cs = new ContentService();
                var post = cs.GetById(id);
                cs.UnPublish(post);
                cs.Delete(post);
            }
        }

        private void CreateAndPublish(Post p)
        {
            var stream = getStream();

            if (stream != null)
            {
                var cs = new ContentService();
                int postId;
                IContent post;

                if (TryGetPostId(p, out postId))
                    post = cs.GetById(postId);
                else
                {
                    var parent = createFolderScructure(p.Date, stream);
                    post = cs.CreateContent(p.Name, parent, PostTypeAlias);
                }

                MarkdownSharp.Markdown md = new Markdown();
                post.SetValue("bodyText", md.Transform(p.Content));

               post.SetValue("postDate", p.Date);
              
                if (p.Draft)
                    cs.Save(post);
                else
                    cs.SaveAndPublish(post);
            }
        }

        private int createFolderScructure(DateTime dt, DynamicNode stream)
        {
            var names = new string[] {dt.Year.ToString(), dt.Month.ToString(), dt.Day.ToString()};

            var cs = new ContentService();
            var current = stream;
            var lookUp = true;

            foreach (var name in names)
            {
                if (lookUp)
                {

                    var exists = current.Children.Where(x => x.Name == name).FirstOrDefault();
                    if (exists == null)
                    {
                        lookUp = false;
                        var node = cs.CreateContent(name, current.Id, "Folder");
                        cs.SaveAndPublish(node);

                        Thread.Sleep(2000);
                        current = current.Children.Where(x => x.Name == name).FirstOrDefault();
                    } 
                }
                else
                {
                    var node = cs.CreateContent(name, current.Id, "Folder");
                    cs.SaveAndPublish(node);
                    current = current.Children.Where(x => x.Name == name).FirstOrDefault();
                }
            }

            return current.Id;
        }


        private DynamicNode getStream()
        {
            DynamicNode n = new DynamicNode(-1);
            return n.DescendantsOrSelf(StreamTypeAlias).FirstOrDefault();
        }

        private bool TryGetPostId(Post p, out int id)
        {
            id = 0;
            var stream = getStream();

            if (stream == null)
                return false;

            var post = stream.Descendants(PostTypeAlias).Where(x => x.Name == p.Name).FirstOrDefault();
            if (post == null)
                return false;


            id = post.Id;
            return true;
        }

    }
}