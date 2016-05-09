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
@"<title> Bureau of Reclamation| Web services</title>" +
@"<meta name=""Description"" content=""Bureau of Reclamation, Lower Colorado Region."" />" +
@"<meta name=""Keywords"" content=""Department of the Interior, Bureau of Reclamation, Lower Colorado Region, Lower Colorado River, Colorado River, Hoover Dam, Phoenix, Yuma, Temecula, Area Offices"" />" +
@"<meta name=""author"" content=""Bureau of Reclamation, Lower Colorado Region Web Team"" />" +
@"<meta name=""publisher"" content=""Bureau of Reclamation, Lower Colorado Region"" />" +
@"<meta name=""created"" content=""20150101"" />" +
@"<meta name=""expires"" content=""Never"" />" +
@"" +
@"" +
@"<meta name=""viewport"" content=""width=device-width, initial-scale=1"">" +
@"" +
@"" +
@"" +
@"<!-- Styles -->" +
@"<link rel=""stylesheet"" href=""http://www.usbr.gov/css/main.min.css"" />" +
@"" +
@"<!-- Modernizr -->" +
@"<script src=""http://www.usbr.gov/js/lib/modernizr.js""></script>" +
@"" +
@"" +
@" " +
@"</head>" +
@"" +
@"<body>" +
@"  <div class=""MainContainer"">" +
@"    " +
@"" +
@"<div class=""Header"" id=""top"" role=""banner"">" +
@"<div class=""Header-wrap container"">" +
@"<!-- Logo -->" +
@"<a class=""Header-logo"" title=""Reclamation Home""><img src=""http://www.usbr.gov/img/logo-white.png"" alt=""U.S. Bureau of Reclamation: Managing Water in the West""></a> " +
@"</div><!-- end container -->" +
@"</div><!-- end Header -->" +
@"" +
@"" +
@"<div class=""Main"" id=""Main"" role=""main"" style=""background-image: url('images/bg-banner.jpg')"">" +
@"<!-- Additional wrapper for bottom background image " +
@"<div class=""Main-wrap"" style=""background-image: url('http://www.usbr.gov/img/bg-footer.jpg')"">" +
@"        -->" +
@"		" +
@"<div class=""Main-content container"">" +
@"" +
@"" +
@"<div class=""Explore"">" +
@"     <div class=""Explore-titleBlock"">" +
@"	 <font size=""2.0"">" +
@"	 " +
@"     <strong><p class=""Explore-label"">PISCES DATABASE WEB SERVICES</p></strong>" +
@"     </font>" +
@"     </div>" +
@"     <ul class=""Explore-list resetList"">" +
@"     <li class=""Explore-section"">";
#endregion

        private static string webPage2ndPart = "" +
#region
 @"     </div>" +
@"	 </li>" +
@"" +
@"	 </ul>" +
@"  </div>" +
@"          " +
@"		" +
@"		" +
@"</div><!-- end Main-content -->    " +
@"</div><!-- end Main-wrap -->" +
@"</div><!-- end Main -->" +
@"" +
@"<div><!-- #BeginLibraryItem ""/library/footer.lbi"" -->" +
@"<!-- Footer -->" +
@"<div class=""Footer"" id=""Footer"" role=""contentinfo"">" +
@"<div class=""container"">" +
@"" +
@"    " +
@"<br>" +
@"<br>" +
@"<br>" +
@"<br>" +
@"<!-- Logo -->" +
@"<a class=""Footer-logo""><img src=""http://www.usbr.gov/img/seal-white.png"" alt=""U.S. Department of the Interior-Bureau of Reclamation""></a>" +
@"" +
@"</div>" +
@"</div>" +
@"" +
@" <!-- #EndLibraryItem --></div>" +
@"" +
@"<!-- jQuery -->" +
@"<script src=""http://www.usbr.gov/js/lib/jquery.js""></script>" +
@"" +
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
                "<strong>"+
                "<br><li><a href=https://github.com/usbr/Pisces >Source Code (GitHub Link)</a></li>" +
                "<br><li><a href=/query>Query Interface</a></li>" +
                "<br><li>Pisces Database Predefined Views</li> " +
                "</strong>" +
                "<a href=/sites>List of Sites</a>" +
                "<br><a href=/series>List of Series</a>" +
                "<br><a href=/types>List of Types</a>" +
                
                webPage2ndPart;
            return webPage;
        }

        public static string BuildTypesPage()
        {
            string webPage = @"" +
                webPage1stPart +
                "<strong>" +
                "<br><li><a href=/types/agrimet>Agrimet Stations</a></li>" +
                "<br><li><a href=/types/canal>Canals</a></li>" +
                "<br><li><a href=/types/reservoir>Reservoirs and Dams</a></li>" +
                "<br><li><a href=/types/snotel>Snotel Sites</a></li>" +
                "<br><li><a href=/types/stream>Stream Gages</a></li>" +
                "<br><li><a href=/types/weather>Weather Stations</a></li>" +
                "</strong>" +
                webPage2ndPart;
            return webPage;
        }

        private static string queryPage2ndPart = "" +
        #region
 @"     </div>" +
@"	 </li>" +
@"" +
@"	 </ul>" +
@"  </div>" +
@"          " +
@"		" +
@"		" +
@"</div><!-- end Main-content -->    " +
@"</div><!-- end Main-wrap -->" +
@"</div><!-- end Main -->" +
@"" +
@"<div><!-- #BeginLibraryItem ""/library/footer.lbi"" -->" +
@"<!-- Footer -->" +
@"<div class=""Footer"" id=""Footer"" role=""contentinfo"">" +
@"<div class=""container"">" +
@"" +
@"    " +
@"<br>" +
@"<br>" +
@"<br>" +
@"<br>" +
@"<!-- Logo -->" +
@"<a class=""Footer-logo""><img src=""http://www.usbr.gov/img/seal-white.png"" alt=""U.S. Department of the Interior-Bureau of Reclamation""></a>" +
@"" +
@"</div>" +
@"</div>" +
@"" +
@" <!-- #EndLibraryItem --></div>" +
@"" +
@"<!-- jQuery -->" +
@"<script src=""http://www.usbr.gov/js/lib/jquery.js""></script>" +
@"<script src=""http://www.usbr.gov/js/lib/jquery.js""></script>" + 
@"<link href=""http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css"" rel=""stylesheet"" type=""text/css""/>" +
@"<script type=""text/javascript"" src=""http://ajax.googleapis.com/ajax/libs/jquery/1.6.2/jquery.min.js""></script>" +
@"<script src=""http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js""></script>" +
// Define date picker JS functions
@"<script>$(document).ready(function(){$(""#startDate"").datepicker({" +
@"dateFormat: 'mm-dd-yy', onSelect: function(dateText, inst) {" +
@"    $('#sDate').val(dateText);" +
@"}});});" +
@"</script>" +
@"<script>$(document).ready(function(){$(""#endDate"").datepicker({" +
@"dateFormat: 'mm-dd-yy', onSelect: function(dateText, inst) {" +
@"    $('#eDate').val(dateText);" +
@"}});});" +
@"</script>" +
@"" +
// Build HTML Form request
//http://ibr3lcrsrv02.bor.doi.net:8080/series/pnhydromet_daily_abei_sr?&start=2014-06-10&end=2016-03-28&format=xml
@"<script>" +
@"$(function() {" +
@"$('#piscesForm').submit(function(){" +
@"  var tableN = $('#tableName').val();" +
@"  $(this).attr('action', ""series/"" + tableN);" +
@"});" +
@"});" +
@"</script>" +
// Define list filter
@"<script>" +
@"    jQuery.fn.filterByText = function(textbox, selectSingleMatch) {" +
@"        return this.each(function() {" +
@"            var select = this;" +
@"            var options = [];" +
@"            $(select).find('option').each(function() {" +
@"                options.push({value: $(this).val(), text: $(this).text()});" +
@"            });" +
@"            $(select).data('options', options);" +
@"            $(textbox).bind('change keyup', function() {" +
@"                var options = $(select).empty().data('options');" +
@"                var search = $.trim($(this).val());" +
@"                var regex = new RegExp(search,""gi"");" +
@"              " +
@"                $.each(options, function(i) {" +
@"                    var option = options[i];" +
@"                    if(option.text.match(regex) !== null) {" +
@"                        $(select).append(" +
@"                           $('<option>').text(option.text).val(option.value)" +
@"                        );" +
@"                    }" +
@"                });" +
@"                if (selectSingleMatch === true && $(select).children().length === 1) {" +
@"                    $(select).children().get(0).selected = true;" +
@"                }" +
@"            });            " +
@"        });" +
@"    };" +
@"" +
@"    $(function() {" +
@"        $('#tableName').filterByText($('#textbox'), true);" +
@"    });  " +
@"</script>" +


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
                
        public static string BuildQueryPage(DataTable siteList)
        {
            string webPage = "" +
            webPage1stPart +
#region
@"<form action='' method=""GET"" id=""piscesForm"" target=""_blank"">" +
@"<div style=""border:5px solid #244A9F;"">"+

@"<strong>Select Pisces Data from the drop-down list: (sorted alphabetically by site name)</strong>" +
@"" +
@"<blockquote>" +
@"<input id=""textbox"" placeholder=""Type to filter"">"+
@"<br><br>" +
@"<select id=""tableName"" name=""tableName"" class=""List"" style=""width:500px"">" + 
BuildQueryPageHtmlList() + 
@"</select>" + 
@"</blockquote>" +

@"<strong>Select your desired date range (MM-DD-YYYY):</strong>" +
@"" +
@"<blockquote>" +
@"<strong>From: &nbsp&nbsp</strong>" +
@"<input id=""startDate"" />" +
@"<input type=""hidden"" name=""start"" id=""sDate"">" +
@"<strong>To: &nbsp&nbsp</strong>" +
@"<input id=""endDate"" />" +
@"<input type=""hidden"" name=""end"" id=""eDate"">" +
@"<br>" +
@"You may type in your own date but you have to click on the date in the calendar for the query to work" +
@"</blockquote>" +

@"<strong>Select output format:</strong>" +
@"<blockquote>"+
@"<input type=""radio"" name=""format"" value=""csv"">CSV &nbsp;&nbsp;"+
@"<input type=""radio"" name=""format"" value=""xml"">XML &nbsp;&nbsp;"+
@"<input type=""radio"" name=""format"" value=""html"" checked=""checked"">HTML &nbsp;&nbsp;" +
@"<input type=""radio"" name=""format"" value=""chart"">Chart &nbsp;&nbsp;" +
@"</blockquote>"+

@"<center><h2>" +
@"<input type=""submit"" value=""Submit Query"" style=""font-size:18px; font-weight:bold; color:#244A9F"">" +
@"</h2></center>" +
@"</form>" +
#endregion
            queryPage2ndPart;

            return webPage;
        }

        private static string BuildQueryPageHtmlList()
        {
            var dTab = Database.GetTableProperties();
            string queryList = @"<option value="""" "+
                    @" label="""">";
            for (int i = 0; i < dTab.Rows.Count; i++)
            {
                var dRow = dTab.Rows[i];
                queryList += @"<option value=""" + dRow[0].ToString().Trim() + @""">" +
                   dRow[1] + " " + dRow[2] + @"</option>";
            }
            return queryList;
        }
       
    }
}