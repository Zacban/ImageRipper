using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Patteguf
{
    public static class Ripper {
        public static void RipPage(string url) {
            using (var client = new WebClient()) {
                client.Encoding = Encoding.UTF8;
                var html = client.DownloadString(url);

                var document = new HtmlDocument();
                document.LoadHtml(html);

                var pictureDiv = document.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("id") && d.Attributes["id"].Value == "picture").ToList<HtmlNode>();

                if (pictureDiv != null) {
                    foreach (HtmlNode node in pictureDiv) {
                        var imgNode = node.SelectSingleNode("img");

                        if (imgNode != null) {
                            var src = imgNode.Attributes["src"].Value;

                            var stream = ImageUtil.ImageUtil.GetImageStreamFromUrl(string.Format("http://patteguf.dk{0}", src));
                            ImageUtil.ImageUtil.SaveImageStreamToFolder(stream, System.Web.HttpContext.Current.Server.MapPath(string.Format("patteguf.dk{0}", src)));
                        }
                    }
                }
            }
        }
    }
}
