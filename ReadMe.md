#Umblr

_Yes, the name sounds like "Tumblr", but lets just pretend I was building this late at night and needed a name, oh wait, that was actually what happened..._

###So what does umblr do?
Umblr is an extremely simple blog / stream implementation in umbraco, which just lists a bunch of posts from the root of the site, which is not that interesting. 

What is more interesting is that it runs off markdown files. So if you create a .md file in the /posts folder, umblr will sync this to the blog
and parse the content to html.

This enables you to write blog-posts in any simple editor like sublime or markpad and simply push them to your site as files. Ftp, webdeploy or git can 
all do that for you. 

###Work in progress
This is very much a late-nigh-at-a-hotel project atm, so it might not actually work atm, and it lacks a lot of features I would like to add

###Posts format

- Any .md file in /posts is processed
- if the file ends in .draft.md then it wont be synced
- if the file starts with a date, it will use that as creation data, like "2013-01-22-post.md" 
- if the file starts with a headline (like h1 or h2) it will use that in the stream, otherwise it will parse the name from the file


###Markdown
Standard markdown supported through markdownsharp

