using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using Umbraco.Core.IO;
using umbraco.interfaces;

namespace Umblr.Umblr
{
    public class PostsMonitor : IApplicationStartupHandler
    {
        private FileSystemWatcher _postsWatcher;
        private string _folderToWatchFor = IOHelper.MapPath("~/posts");
 
        public PostsMonitor()
        {
            _postsWatcher = new FileSystemWatcher(_folderToWatchFor);
            _postsWatcher.EnableRaisingEvents = true;
            _postsWatcher.Filter = "*.md";
            _postsWatcher.Changed += _postsWatcher_Changed;
        }

        void _postsWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            var post = PostFactory.Convert(e.FullPath);
            PostPublisher publisher = new PostPublisher();
            publisher.ProcessPost(post);
        }
    }
}