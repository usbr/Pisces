using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace PiscesWebServices
{
    class WebPageBuilder
    {
        private static string webPage1stPart = "" +
#region
@"<!DOCTYPE html>" +
@"<!-- template-v2015  -->" +
@"<!--[if lte IE 7]><html class=""no-js old-ie lte-ie7"" lang=""en"" dir=""ltr""><![endif]-->" +
@"<!--[if lte IE 8]><html class=""no-js old-ie lte-ie8 lte-ie7"" lang=""en"" dir=""ltr""><![endif]-->" +
@"<!--[if gt IE 8]><!--><html class=""no-js"" lang=""en"" dir=""ltr""><!--<![endif]-->" +
@"<head>" +
@"  <!-- Head Content  -->" +
@"<meta charset=""utf-8"">" +
@"" +
@"<!--[if gt IE 8 ]--><meta http-equiv=""X-UA-Compatible"" content=""IE=Edge"" /><!--<![endif]-->" +
@"<title>Reclamation Water Data | Bureau of Reclamation</title>" +
@"<meta name=""description"" content=""Water Data, Bureau of Reclamation - Managing water and power in the West"" />" +
@"<meta name=""keywords"" content=""Water Data, Department of the Interior, Bureau of Reclamation, Water, Dams, Dam, Projects &amp; Facilties, Hydromet, Agrimet, hydropower"" />" +
@"<meta name=""author"" content=""Bureau of Reclamation"" />" +
@"<meta name=""publisher"" content=""Bureau of Reclamation"" />" +
@"<meta name=""created"" content=""20150101"" />" +
@"<meta name=""expires"" content=""Never"" />" +
@"" +
@"" +
@"<meta name=""viewport"" content=""width=device-width, initial-scale=1"">" +
@"" +
@"" +
@"<link rel=""shortcut icon"" href=""http://www.usbr.gov/img/favicon.ico"">" +
@"" +
@"<script type=""text/javascript""> " +
@"var _gaq = _gaq || [];" +
@"_gaq.push(['_setAccount', 'UA-17251781-1']);" +
@"_gaq.push(['_anonymizeIp']);" +
@"_gaq.push(['_trackPageview']);" +
@"(function() {" +
@"var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;" +
@"ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';" +
@"var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);" +
@"})();" +
@"</script>" +
@"" +
@"<script type=""text/javascript"" src=""http://www.usbr.gov/js/federated-analytics.js""></script>" +
@"" +
@"<!-- Styles -->" +
@"<link rel=""stylesheet"" href=""http://www.usbr.gov/css/main.min.css"" />" +
@"" +
@"<link href=""./css/main.pn.css"" rel=""stylesheet"" type=""text/css"" />" +
@"" +
@"<!-- Modernizr -->" +
@"<script src=""http://www.usbr.gov/js/lib/modernizr.js""></script>" +
@"  " +
@"</head>" +
@"<body>" +
@"  <div class=""MainContainer"">" +
@"    <!-- Skip Links (only visible to screen readers) -->" +
@"<div id=""SkipLinks"" class=""SkipLinks"">" +
@"  <ul>" +
@"    <li><a href=""#Nav"">Skip to Primary Navigation</a></li>" +
@"    <li><a href=""#Main"">Skip to Main Content</a></li>" +
@"    <li><a href=""#Footer"">Skip to Footer</a></li>" +
@"  </ul>" +
@"</div>" +
@"<!-- #BeginLibraryItem ""/Library/topnav.lbi"" -->" +
@"<!-- Header -->" +
@"<div class=""Header"" id=""top"" role=""banner"">" +
@"  <div class=""Header-wrap container"">" +
@"    <!-- Logo -->" +
@"    <a class=""Header-logo"" href=""http://www.usbr.gov"" title=""Reclamation Home""><img src=""http://www.usbr.gov/img/logo-white.png"" alt=""U.S. Bureau of Reclamation: Managing Water in the West""></a>" +
@"    <div class=""Header-social""> <span class=""Header-social-label"">Share</span>" +
@"      <ul>" +
@"        <li> <a class=""critical-icon"" title=""Share on Facebook"" target=""_blank"" href=""http://www.facebook.com/sharer.php?u={url}""> <span class=""icon-facebook"" aria-hidden=""true""></span> <span class=""fallback"">Facebook</span> </a> </li>" +
@"        <li> <a class=""critical-icon"" title=""Share on Twitter"" target=""_blank"" href=""https://twitter.com/share?url={url}&text={title}&via={via}&hashtags={hashtags}""> <span class=""icon-twitter"" aria-hidden=""true""></span> <span class=""fallback"">Twitter</span> </a> </li>" +
@"        <li> <a class=""critical-icon"" title=""Share on Tumblr"" target=""_blank"" href=""http://www.tumblr.com/share/link?url={url}&name={title}&description={desc}""> <span class=""icon-tumblr"" aria-hidden=""true""></span> <span class=""fallback"">Tumblr</span> </a> </li>" +
@"        <li> <a class=""critical-icon"" title=""Share on Pinterest"" target=""_blank"" href=""https://pinterest.com/pin/create/bookmarklet/?media={img}&url={url}&is_video={is_video}&description={title}""> <span class=""icon-pinterest"" aria-hidden=""true""></span> <span class=""fallback"">Pinterest</span> </a> </li>" +
@"      </ul>" +
@"    </div>" +
@"    <!-- end Header-social -->" +
@"  </div>" +
@"  <!-- end container -->" +
@"</div>" +
@"<!-- end Header -->" +
@"<!-- Nav Menu -->" +
@"<div class=""Nav"" id=""Nav"">" +
@"  <div class=""Nav-wrap"">" +
@"    <!-- Navigation -->" +
@"    <ol class=""Nav-list resetList"" role=""navigation"" aria-label=""Primary Navigation"">" +
@"      <li class=""Nav-item""> <a class=""Nav-item-link"" href=""#"">Water &amp; Power</a>" +
@"        <ol class=""Nav-sublist resetList"" aria-label=""submenu"">" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/projects/dams.jsp"" class=""Nav-sublist-link"">Dams</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/projects/powerplants.jsp"" class=""Nav-sublist-link"">Powerplants</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/projects/projects.jsp"" class=""Nav-sublist-link"">Projects</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/main/agrihydro.html"" class=""Nav-sublist-link"">Agrimet/Hydromet</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/main/water/"" class=""Nav-sublist-link"">Water Operations</a></li>" +
@"        </ol>" +
@"      </li>" +
@"      <li class=""Nav-item""> <a class=""Nav-item-link"" href=""#"">Resources &amp; Research</a>" +
@"        <ol class=""Nav-sublist resetList"" aria-label=""submenu"">" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/main/programs/"" class=""Nav-sublist-link"">Programs</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/tsc/"" class=""Nav-sublist-link"">Technical Services Center</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/research/"" class=""Nav-sublist-link"">Research &amp; Development</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/recman/"" class=""Nav-sublist-link"">Reclamation Manual</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/environmental/"" class=""Nav-sublist-link"">Environmental Resources/Reports</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/library/"" class=""Nav-sublist-link"">Library</a></li>" +
@"        </ol>" +
@"      </li>" +
@"      <li class=""Nav-item""> <a class=""Nav-item-link"" href=""#"">About Us</a>" +
@"        <ol class=""Nav-sublist resetList"" aria-label=""submenu"">" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/main/about/mission.html"" class=""Nav-sublist-link"">Mission/Vision</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/main/about/fact.html"" class=""Nav-sublist-link"">Fact Sheet</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/newsroom/presskit/bios/biosdetail.cfm?recordid=1"" class=""Nav-sublist-link"">Commissioner</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/main/offices.html"" class=""Nav-sublist-link"">Addresses/Contacts</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/main/images/br_org_chart.pdf"" class=""Nav-sublist-link"">Organizational Chart</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/newsroom/presskit/bios/bios.cfm"" class=""Nav-sublist-link"">Leadership Bios</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/pmts/hr/"" class=""Nav-sublist-link"">Employment</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/main/comments.cfm/"" class=""Nav-sublist-link"">Contact</a></li>" +
@"        </ol>" +
@"      </li>" +
@"      <li class=""Nav-item""> <a class=""Nav-item-link"" href=""#"">Recreation &amp; Public Use</a>" +
@"        <ol class=""Nav-sublist resetList"" aria-label=""submenu"">" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/recreation/"" class=""Nav-sublist-link"">Find Recreation</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.recreation.gov/"" class=""Nav-sublist-link"">Recreation.gov</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/lands/"" class=""Nav-sublist-link"">Public Use</a></li>" +
@"        </ol>" +
@"      </li>" +
@"      <li class=""Nav-item""> <a class=""Nav-item-link"" href=""#"">News &amp; Multimedia</a>" +
@"        <ol class=""Nav-sublist resetList"" aria-label=""submenu"">" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/newsroom/newsrelease/"" class=""Nav-sublist-link"">News Releases</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/newsroom/stories/"" class=""Nav-sublist-link"">News Stories</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/newsroom/speech/"" class=""Nav-sublist-link"">Speeches</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/newsroom/testimony/"" class=""Nav-sublist-link"">Congressional Testimony</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/newsroom/presskit/factsheet/"" class=""Nav-sublist-link"">Fact Sheets</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/main/multimedia/index.html#photos"" class=""Nav-sublist-link"">Photos</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/main/multimedia/index.html#video"" class=""Nav-sublist-link"">Multimedia</a></li>" +
@"          <li class=""Nav-sublist-item""><a href=""http://www.usbr.gov/main/multimedia/index.html#social"" class=""Nav-sublist-link"">Social Media</a></li>" +
@"        </ol>" +
@"      </li>" +
@"    </ol>" +
@"    <!-- Search -->" +
@"    <div class=""Header-search"" id=""recSearch"">" +
@"      <form method=""get"" action="" http://search.usa.gov/search"" role=""search"">" +
@"        <input type=""hidden"" name=""affiliate"" value=""Reclamation"" />" +
@"        <label class=""visuallyhidden"" for=""query"">Search</label>" +
@"        <input type=""text"" name=""query"" id=""query"" placeholder=""Search"" />" +
@"        <button type=""submit"" id=""search"">Search</button>" +
@"      </form>" +
@"    </div>" +
@"  </div>" +
@"</div>" +
@"<!-- #EndLibraryItem --><!-- Update inline style to change banner image -->" +
@"<div class=""Main"" id=""Main"" role=""main"" style=""background-image: url(http://www.usbr.gov/img/bg-banner.jpg)"">" +
@"      <!-- Additional wrapper for bottom background image -->" +
@"      <div class=""Main-wrap"" style=""background-image: url('http://www.usbr.gov/img/bg-footer.jpg')"">" +
@"        <!-- Banner -->" +
@"        <div class=""Banner Banner--full"">" +
@"          <div class=""Banner-content"">" +
@"            <div class=""container"">" +
@"              <h1>Reclamation Water Data</h1>" +
@"              <p></p>" +
@"            </div>" +
@"          </div>" +
@"        </div><!-- end Banner -->" +
@"" +
@"        <div class=""Main-content container"">" +
@"          <!-- Breadcrumbs -->" +
@"<div class=""Breadcrumbs"">" +
@"  <div class=""container"">" +
@"    <ul class=""resetList"">" +
@"      <li><a href=""http://www.usbr.gov/"" target=""_blank"">Reclamation</a></li>" +
@"      <li>Water Data</li>" +
@"    </ul>" +
@"  </div>" +
@"</div>" +
@"" +
@"          <!-- Left Navigation --><!-- #BeginLibraryItem ""/Library/waterusbrnav.lbi"" -->" +
@"          <div class=""Main-nav"">" +
@"            <!-- Left Navigation -->" +
@"            <div class=""LeftNav"" role=""navigation""> <span class=""LeftNav-title"">Water Data</span>" +
@"              <ul class=""LeftNav-list"">" +
@"                <li><a href=""http://www.usbr.gov/main/water/"">Water Operations</a></li>" +
@"                <li><a href=""http://www.usbr.gov/research/"">Research Office</a></li>" +
@"                <li>Web Services</li>" +
@"              </ul>" +
@"            </div>" +
@"          </div>" +
@"          <!-- #EndLibraryItem --><!-- end Main-nav -->" +
@"" +
@"          <div class=""Main-well"">" +
@"          " +
@"<h2 class=""h1""><font color=""red"">D R A F T</font> -- Reclamation Water Data Web Service -- <font color=""red"">D R A F T</font></h2>";
#endregion

        private static string webPage2ndPart = "" +
#region
@"" +
@"<br>" +
@"</div><!-- end Main-wrap -->" +
@"</div><!-- end Main -->" +
@"<div>" +
@"  <!-- #BeginLibraryItem ""/Library/footer.lbi"" -->" +
@"  <!-- Footer -->" +
@"  <div class=""Footer"" id=""Footer"" role=""contentinfo"">" +
@"    <div class=""container"">" +
@"      <h2 class=""visuallyhidden"">More Information about the U.S. Bureau of Reclamation</h2>" +
@"      <!-- Social Links -->" +
@"      <div class=""Footer-social"">" +
@"        <h2>Stay in Touch</h2>" +
@"        <ul>" +
@"          <li> <a href=""http://www.facebook.com/bureau.of.reclamation"" target=""new"" class=""critical-icon""> <span class=""icon-facebook"" aria-hidden=""true""></span> <span class=""fallback"">Facebook</span> </a> </li>" +
@"          <li> <a href=""http://twitter.com/usbr"" class=""critical-icon"" target=""new""> <span class=""icon-twitter"" aria-hidden=""true""></span> <span class=""fallback"">Twitter</span> </a> </li>" +
@"          <li> <a href=""http://www.youtube.com/user/reclamation"" class=""critical-icon"" target=""new""> <span class=""icon-youtube"" aria-hidden=""true""></span> <span class=""fallback"">YouTube</span> </a> </li>" +
@"          <li> <a href=""http://www.flickr.com/photos/usbr/"" class=""critical-icon"" target=""new""> <span class=""icon-flickr"" aria-hidden=""true""></span> <span class=""fallback"">Flickr</span> </a> </li>" +
@"          <li> <a href=""http://usbr.tumblr.com/"" class=""critical-icon"" target=""new""> <span class=""icon-tumblr"" aria-hidden=""true""></span> <span class=""fallback"">Tumblr</span> </a> </li>" +
@"          <li> <a href=""http://www.pinterest.com/usbrgov/"" class=""critical-icon"" target=""new""> <span class=""icon-pinterest"" aria-hidden=""true""></span> <span class=""fallback"">Pinterest</span> </a> </li>" +
@"          <li> <a href=""http://instagram.com/bureau_of_reclamation"" class=""critical-icon"" target=""new""> <span class=""icon-instagram"" aria-hidden=""true""></span> <span class=""fallback"">Instagram</span> </a> </li>" +
@"          <li> <a href=""http://www.usbr.gov/newsroom/rssxml/rssfeeds.html"" class=""critical-icon"" target=""new""> <span class=""icon-rss"" aria-hidden=""true""></span> <span class=""fallback"">RSS</span> </a> </li>" +
@"        </ul>" +
@"      </div>" +
@"      <!-- Site Links -->" +
@"      <div class=""Footer-links"">" +
@"        <ul>" +
@"          <li><a href=""http://www.usbr.gov/main/comments.cfm"">Contact Us</a></li>" +
@"          <li><a href=""http://www.usbr.gov/main/index/"">Site Index</a></li>" +
@"        </ul>" +
@"        <ul>" +
@"          <li><a href=""http://www.usbr.gov/main/access.html"">Accessibility</a></li>" +
@"          <li><a href=""http://www.usbr.gov/main/disclaimer.html"">Disclaimer</a></li>" +
@"          <li><a href=""http://www.doi.gov/"" target=""new""><abbr title=""Department of the Interior"">DOI</abbr></a></li>" +
@"          <li><a href=""http://www.usbr.gov/foia/"">FOIA</a></li>" +
@"          <li><a href=""http://www.doi.gov/pmb/eeo/no-fear-act.cfm"">No Fear Act</a></li>" +
@"          <li><a href=""http://www.usbr.gov/main/notices.html"">Notices</a></li>" +
@"          <li><a href=""http://www.usbr.gov/main/privacy.html"">Privacy Policy</a></li>" +
@"          <li><a href=""http://www.usbr.gov/main/qoi/"">Quality of Information</a></li>" +
@"          <li><a href=""http://www.recreation.gov/"" target=""new"">Recreation.gov</a></li>" +
@"          <li><a href=""http://www.usa.gov/"" target=""new"">USA.gov</a></li>" +
@"        </ul>" +
@"      </div>" +
@"      <!-- Logo -->" +
@"      <a class=""Footer-logo"" href=""http://www.usbr.gov""><img src=""http://www.usbr.gov/img/seal-white.png"" alt=""U.S. Department of the Interior - Bureau of Reclamation""></a> </div>" +
@"  </div>" +
@"  <!-- #EndLibraryItem --></div>" +
@"" +
@"<!-- jQuery -->" +
@"<script src=""http://www.usbr.gov/js/lib/jquery.js""></script>" +
@"" +
@"<script>" +
@"// Only load FastClick if touch events are supported" +
@"if ( Modernizr.touch ) {" +
@"  $(function() {" +
@"    $.getScript(""http://www.usbr.gov/js/lib/fastclick.min.js"", function() {" +
@"      FastClick.attach(document.body);" +
@"    });" +
@"  });" +
@"}" +
@"</script>" +
@"" +
@" <!-- Responsive Video Plugin -->" +
@"<script src=""http://www.usbr.gov/js/lib/jquery.fitvids.min.js""></script>" +
@"<script>" +
@"  $("".fluid-video"").fitVids();" +
@"</script>" +
@" " +
@"" +
@"<!-- Outlines -->" +
@"<script src=""http://www.usbr.gov/js/components/outlines.js""></script>" +
@"" +
@"<!-- Mobile Navigation -->" +
@"<script src=""http://www.usbr.gov/js/components/navigation.js""></script>" +
@"" +
@"<!-- Responsive Tabs -->" +
@"<script src=""http://www.usbr.gov/js/lib/responsive-tabs.js""></script>" +
@"<script src=""http://www.usbr.gov/js/components/tabs.js""></script>" +
@"" +
@"<!-- Explore Accordion -->" +
@"<script src=""http://www.usbr.gov/js/components/explore.js""></script>" +
@"" +
@"<!-- Carousel -->" +
@"<script src=""http://www.usbr.gov/js/lib/carousel.js""></script>" +
@"<script src=""http://www.usbr.gov/js/components/carousel.js""></script>" +
@"" +
@"<!-- Left Navigation -->" +
@"<script src=""http://www.usbr.gov/js/components/left-nav.js""></script>" +
@"" +
@"</body>" +
@"</html>";
#endregion
        
        public static string BuildHomePage()
        {
            string webPage = @"" +
                webPage1stPart +
                "<br><a href=/sites>List of Sites</a>" +
                "<br><a href=/series>List of Series</a> " +
                "<br><a href=/types>List of Types</a> " +
                "<br><a href=/query>Query Interface</a> " +
                webPage2ndPart;
            return webPage;
        }

        public static string BuildTypesPage()
        {
            string webPage = @"" +
                webPage1stPart +
                "<br><a href=/types/agrimet>Agrimet Stations</a>" +
                "<br><a href=/types/canal>Canals</a> " +
                "<br><a href=/types/reservoir>Reservoirs and Dams</a> " +
                "<br><a href=/types/snotel>Snotel Sites</a> " +
                "<br><a href=/types/stream>Stream Gages</a> " +
                "<br><a href=/types/weather>Weather Stations</a>" +
                webPage2ndPart;
            return webPage;
        }

        public static string BuildQueryPage(DataTable siteList)
        {
            string webPage = @"IN PROGRESS...";


            return webPage;
        }

        public static string UpdateQueryPage(DataTable parameterList)
        {
            string webPage = @"";


            return webPage;
        }
    }
}