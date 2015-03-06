using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ImageUtil;

namespace ImageRipperWeb {
    public partial class QuickTestPage : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            using (var client = new WebClient()) {
                //var url = "http://ekstrabladet.dk/side9/ekstra/elina/5464643";
                //http://ekstrabladet.dk/side9/side9artikler/article4302379.ece
                var startIndex = 5227181;
                var runthroughs = 1;
                var stopatfound = 10;
                var found = 0;

                for (int i = startIndex; i < startIndex + runthroughs; i++) {
                    try {
                        var url = string.Format("http://ekstrabladet.dk/ekstra/EKSTRA_Sex_og_samliv/article{0}.ece", i);
                        client.Encoding = Encoding.UTF8;
                        var page = client.DownloadString(url);

                        found++;
                        var html = new HtmlAgilityPack.HtmlDocument();
                        html.LoadHtml(page);

                        var meta = html.DocumentNode.Descendants("meta");

                        var title = meta.Where(d => d.Attributes.Contains("property") && d.Attributes["property"].Value.Contains("og:title")).ToList<HtmlAgilityPack.HtmlNode>()[0].Attributes["content"].Value;

                        form1.Controls.Add(new LiteralControl() { Text = string.Format("<h1>{0}</h1>", title) });
                        var imgSrc = meta.Where(d => d.Attributes.Contains("property") && d.Attributes["property"].Value.Contains("og:image")).ToList<HtmlAgilityPack.HtmlNode>();
                        //var container = html.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("page9image")).ToList<HtmlAgilityPack.HtmlNode>();
                        if (imgSrc != null) {
                            foreach (HtmlAgilityPack.HtmlNode node in imgSrc) {
                                var src = node.Attributes["content"].Value;

                                var dirPath = src.Replace("http://", "").Replace("/", "\\");
                                dirPath = dirPath.Substring(0, dirPath.LastIndexOf('\\'));
                                var filename = src.Substring(src.LastIndexOf('/') + 1);
                                if (filename.IndexOf('.') < 0)
                                    filename += ".jpg";

                                DirectoryInfo dir = new DirectoryInfo(Server.MapPath(dirPath));
                                if (!dir.Exists)
                                    dir.Create();

                                Byte[] lnByte = ImageUtil.ImageUtil.GetImageStreamFromUrl(src);


                                ImageUtil.ImageUtil.SaveImageStreamToFolder(lnByte, dir.FullName + "\\" + filename);

                                var b64s = Convert.ToBase64String(lnByte);
                                FileInfo f = new FileInfo(dir.FullName + "\\" + filename.Replace("jpg", "txt"));
                                var fs = f.OpenWrite();
                                StreamWriter sw = new StreamWriter(fs);
                                sw.Write(b64s);

                                sw.Close();
                                fs.Close();

                                using (var sr = new StreamReader(dir.FullName + "\\" + filename.Replace("jpg", "txt"))) {
                                    b64s = sr.ReadToEnd();
                                    ImageUtil.ImageUtil.SaveImageStreamToFolder(b64s, dir.FullName + "\\fromb64s.jpg");
                                }

                                form1.Controls.Add(new Image() { ImageUrl = src });
                            }
                        }
                    }
                    catch (Exception ex) {
                    }
                }

            }
        }
    }
}