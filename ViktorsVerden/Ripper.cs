using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ViktorsVerden
{
    public static class Ripper
    {
        public static void RipPage(string url) {
            using (var client = new WebClient()) {
                client.Encoding = Encoding.UTF8;
                var html = client.DownloadString(url);

                var document = new HtmlDocument();
                document.LoadHtml(html);

                var pictureUl = document.DocumentNode.Descendants("ul").Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value.Contains("jcarousel")).ToList<HtmlNode>();

                if (pictureUl != null) {
                    foreach (HtmlNode ul in pictureUl) {
                        var listItem = ul.Descendants("li");

                        foreach (HtmlNode li in listItem) {
                            var imgNode = li.Descendants("a").Where(i => i.Attributes.Contains("class") && i.Attributes["class"].Value.Contains("lightbox")).ToList<HtmlNode>()[0];

                            if (imgNode != null) {
                                var src = imgNode.Attributes["href"].Value;

                                var stream = ImageUtil.ImageUtil.GetImageStreamFromUrl(string.Format("{0}", src));

                                var dirPath = src.Replace("http://", "").Replace("/", "\\");
                                dirPath = dirPath.Substring(0, dirPath.LastIndexOf('\\'));
                                var filename = src.Substring(src.LastIndexOf('/') + 1);
                                if (filename.IndexOf('.') < 0)
                                    filename += ".jpg";

                                ImageUtil.ImageUtil.SaveImageStreamToFolder(stream, System.Web.HttpContext.Current.Server.MapPath(string.Format("{0}\\{1}", dirPath, filename)));
                            }
                        }
                    }
                }
            }
        }
    }
}
