using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using EcoHotels.Core.Domain.Models.Localization;
using EcoHotels.Core.Infrastructure.Services;
using EcoHotels.Extensions;
using Microsoft.Practices.Unity;

namespace EcoHotels.Web.UI.Controllers
{
    /// <summary>
    ///  http://www.sitemaps.org/protocol.html
    /// following search engines support Sitemap
    /// Google, Bing, Yahoo, Ask
    /// 
    /// 
    /// Google specefics
    /// http://support.google.com/webmasters/bin/answer.py?hl=en&answer=2620865    
    /// 
    /// </summary>
    public class SitemapController : Controller
    {
        #region - Services -

        [Dependency]
        public ILanguageService LanguageService { get; set; }

        [Dependency]
        public IPageInformationService PageInformationService { get; set; }

        #endregion


        private string CurrentDomain { get; set; }
        private IEnumerable<Language> Languages { get; set; }

        [HttpGet]
        public void Index()
        {
            CurrentDomain = string.Concat(Request.Url.Scheme, "://", Request.Url.Host, "/");

            Languages = LanguageService.FindAllActive();

            var pageInformations = PageInformationService.FindAll();

            Response.ContentType = "text/xml";

            using(var writer = XmlWriter.Create(Response.OutputStream))
            {
                // ROOT                
                writer.WriteStartElement("urlset", "http://www.sitemaps.org/schemas/sitemap/0.9");
                {
                    foreach (var information in pageInformations)
                    {
                        AppendUrlElement(information, writer);
                    }

                } // end root
                writer.WriteEndElement();
            }
        }

        [NonAction]
        private void AppendUrlElement(PageInformation pageInformation, XmlWriter writer)
        {
            foreach (var language in Languages)
            {
                writer.WriteStartElement("url");
                {
                    writer.WriteElementString("loc", CreateAbsoluteUrl(pageInformation, language));

                    if (Languages.Count() > 1)
                    {
                        AppendAlternateUrlElement(pageInformation, language, writer);
                    }

                    writer.WriteElementString("lastmod", pageInformation.LastModified.ToW3CTime());
                    writer.WriteElementString("changefreq", "monthly");
                    writer.WriteElementString("priority", "0.8");
                }
                writer.WriteEndElement(); // end url                
            }
        }

        [NonAction]
        private void AppendAlternateUrlElement(PageInformation pageInformation, Language currentLanguage, XmlWriter writer)
        {
            foreach (var language in Languages)
            {
                if (language != currentLanguage)
                {
                    writer.WriteStartElement("xhtml", "link");
                    writer.WriteAttributeString("rel", "alternate");
                    writer.WriteAttributeString("hreflang", language.Shortname);
                    writer.WriteAttributeString("href", CreateAbsoluteUrl(pageInformation, language));
                    writer.WriteEndElement();   
                }
            }
        }

        private string CreateAbsoluteUrl(PageInformation pageInformation, Language language)
        {
            return string.Concat(CurrentDomain, language.Shortname, "/hotel/", pageInformation.Filename);
        }
    }
}
