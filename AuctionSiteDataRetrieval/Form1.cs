using AuctionSiteDataRetrieval.Entities;
using AuctionSiteDataRetrieval.Models;
using AuctionSiteDataRetrieval.Services;
using AuctionSiteDataRetrieval.Services.Authentication;
using AuctionSiteDataRetrieval.Services.WebServices;
using Newtonsoft.Json;
using PuppeteerSharp;
using PuppeteerSharp.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AuctionSiteDataRetrieval
{
  
    public partial class Form1 : Form
    {
        
        public Authentication _authentication;
        public List<AuctionSite> _auctionSites;
        public List<AuctionSearchWord> _auctionSearchWords;
        public AuctionWebServices _auctionWebServices;
        public const string apiUrl = "http://securityapi-env.eba-3534xsm2.us-east-2.elasticbeanstalk.com/api/";
        public Form1()
        {
            InitializeComponent();

         
            // Task detail = TestCall("acne");
        }
        public static string captchaCheck = @"() => {const formCaptchaCheck = document.getElementsByTagName('form'); 
if(formCaptchaCheck.length> 0) {
for(var i = 0;i<formCaptchaCheck.length;i++){
if(formCaptchaCheck[i].id.indexOf('captcha') > -1)  {
return true;
}
}
}   
return false;
}";
        public static string jsCode = @"() => {
const selectors = Array.from(document.querySelectorAll('.item-col'));
var itemDetails = [];
if(selectors.length==0){
return itemDetails;
}
try{
for(var i = 0;i<selectors.length;i++){


var interiorSelector = selectors[i].querySelectorAll('.feat-item_info');
var itemLi = selectors[i].getElementsByTagName('li')

var itemDetail = {}
itemDetail.productName = interiorSelector[0].getElementsByTagName('a')[0].innerText
itemDetail.itemUrl = interiorSelector[0].getElementsByTagName('a')[0].href;

for(var i2 = 0;i2 < itemLi.length;i2++){
if(itemLi[i2].innerText.indexOf('Time remaining:') > -1){

var timeDetails = itemLi[i2].innerText.split('\n');
itemDetail.timeLeft = timeDetails[1];

}


if(itemLi[i2].innerText.indexOf('Bids: ') > -1){
itemDetail.totalBids = itemLi[i2].innerText.split('Bids: ')[1];
}


}

var itemPrices = selectors[i].getElementsByTagName('p');
for(var iItem = 0;iItem < itemPrices.length;  iItem++){
if(itemPrices[iItem].className =='feat-item_price'){
itemDetail.itemPrice = itemPrices[iItem].innerText;
}

}




itemDetails.push(itemDetail); 

}
}
catch(ex){}
return itemDetails;
}";
       public static async Task GetAuctionItems(List<AuctionSite> auctionSites,List<AuctionSearchWord> auctionSearchWords, AuctionWebServices auctionWebServices)
        {

            foreach (var auctionSite in auctionSites)
            {

                if (auctionSite.Id == 1)
                {
                    continue;
                }
                foreach (var auctionSearchWord in auctionSearchWords)
                {
                    try
                    {

                        var options = new LaunchOptions { Headless = true };
                        Console.WriteLine("Downloading chromium");

                        await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
                        Console.WriteLine("Navigating to developers.google.com");

                        using (var browser = await Puppeteer.LaunchAsync(options))
                        using (var page = await browser.NewPageAsync())
                        {
                            List<SiteAuctionItemDto> totalresultsTest = new List<SiteAuctionItemDto>();
                            bool processNextPage = true;
                            int pageRecords = 1;
                            while (processNextPage)
                            {
                                //alc
                                string url = auctionSite.SearchURL.Replace(auctionSite.SearchWordReplace, auctionSearchWord.SearchWord).Replace(auctionSite.PageReplace, pageRecords.ToString());
                                // await page.GoToAsync(url);
                                if (pageRecords == 1 || auctionSite.Id == 1)
                                {
                                    await page.GoToAsync(url);
                                    //await page.GoToAsync("https://www.ebay.com");
                                }
                                else
                                {
                                    try
                                    {
                                        await page.ClickAsync("[class='pagination__next icon-link']", new ClickOptions() { Button = MouseButton.Left });
                                    }
                                    catch(Exception ex)
                                    {
                                        processNextPage = false;
                                    }
                                    
                                    //  await page.EvaluateFunctionAsync("() => {var links = document.getElementsByClassName('pagination__next icon-link');if(links.length > 0) {links[0].click();}}");

                                }
                                //"https://shopgoodwill.com/categories/listing?st=" + searchCall + "&sg=&c=&s=&lp=0&hp=999999&sbn=&spo=false&snpo=false&socs=false&sd=false&sca=false&caed=2%2F7%2F2022&cadb=7&scs=false&sis=false&col=1&p=" + pageRecords.ToString() + "&ps=40&desc=false&ss=0&UseBuyerPrefs=true&sus=false&cln=1&catIds=&pn=&wc=false&mci=false&hmt=false&layout=grid");
                                // Type into search box.
                                Thread.Sleep(10000);


                                if (pageRecords == 1 && auctionSite.Id == 2)
                                {
                                    
                                    await page.EvaluateFunctionAsync("() => {var searchBar = document.getElementById('gh-ac'); searchBar.value = '" + auctionSearchWord.SearchWord + "';}");
                                    Thread.Sleep(1000);

                                    await page.ClickAsync("[id='gh-btn']", new ClickOptions() { Button = MouseButton.Left });

                                    //await page.EvaluateFunctionAsync("() => {var btnSubmit = document.getElementById('gh-btn');btnSubmit.click();}");
                                    Thread.Sleep(10000);
                                }
                                else
                                {

                                }


                                try
                                {
                                    
                                    bool captchaRequired = await page.EvaluateFunctionAsync<bool>(captchaCheck);
                                    if (captchaRequired)
                                    {
                                        Thread.Sleep(30000);

                                    }

                                    SiteAuctionItemDto[] resultsTest = await page.EvaluateFunctionAsync<SiteAuctionItemDto[]>(auctionSite.JsCode);

                                    if (resultsTest == null || resultsTest.Length < 40  || totalresultsTest.Count > 500)
                                    {
                                        processNextPage = false;
                                    }
                                    pageRecords += 1;
                                    var info2 = 12;

                                    var matchingRecords = totalresultsTest.Join(resultsTest, x => x.ItemUrl, y => y.ItemUrl, (newRecs, curRecs) => new { newRecs, curRecs }).Select(x=>x.curRecs);
                                    var matchingRecordCount = matchingRecords.Count();
                                    var currentMissingRecords = resultsTest.Except(matchingRecords).ToList();

                                    var otherMatchingRecords = totalresultsTest.Join(currentMissingRecords, x => x.ImageUrl, y => y.ImageUrl, (newRecs, curRecs) => new { newRecs, curRecs }).Select(x => x.curRecs);
                                    var otherMatchingRecordCount = otherMatchingRecords.Count();
                                    var missingRecords = currentMissingRecords.Except(otherMatchingRecords).ToList();
                                    if (missingRecords.Count == 0)
                                    {
                                        processNextPage = false;
                                    }
                                    else {

                                        totalresultsTest.AddRange(missingRecords);
                                    }

                                }
                                catch (Exception ex)
                                {
                                    processNextPage = false;
                                    string error = ex.Message;
                                }

                            }
                            List<AuctionItemDto> auctionItems = new List<AuctionItemDto>();
                            if (totalresultsTest.Count > 0)
                            {

                                foreach (var totalresult in totalresultsTest)
                                {
                                    try
                                    {
                                        AuctionItemDto auctionItem = new AuctionItemDto();
                                        auctionItem.AuctionSiteId = auctionSite.Id;
                                        auctionItem.AuctionSearchWordId = auctionSearchWord.Id;//to do change later to but the auction search id

                                        auctionItem.ImageUrl = totalresult.ImageUrl;
                                        decimal output;
                                        if (totalresult.ItemPrice == null || totalresult.ItemPrice == "")
                                        {
                                            totalresult.ItemPrice = "0";
                                        }
                                        decimal.TryParse(totalresult.ItemPrice.Replace("$", ""), out output);
                                        auctionItem.ItemPrice = output;

                                        auctionItem.Url = totalresult.ItemUrl;
                                        int totalBidsOutPut;
                                        if (totalresult.TotalBids == null || totalresult.TotalBids == "")
                                        {
                                            totalresult.TotalBids = "0";
                                        }
                                        Int32.TryParse(totalresult.TotalBids.Replace("Bids: ", ""), out totalBidsOutPut);

                                        auctionItem.TotalBids = totalBidsOutPut;

                                        auctionItem.ProductName = totalresult.ProductName;
                                        //totalresult.TimeLeft

                                        DateTime currendate = GetAuctionDate(totalresult.TimeLeft);

                                        auctionItem.AuctionEndDate = currendate;
                                        //3d 5h
                                        //2h 5m

                                        auctionItems.Add(auctionItem);
                                    }
                                    catch (Exception ex)
                                    {
                                        string excep = ex.Message;
                                    }
                                }


                                int partitionLoop = 0;
                                int partitionInterval = 100;

                                while (auctionItems.Count >= partitionLoop)
                                {
                                    auctionWebServices.UpsertAuctionItems(auctionItems.Skip(partitionLoop).Take(partitionInterval).ToList());
                                    partitionLoop += partitionInterval;
                                }


                            }
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadLine();



                        }
                        AuctionSearchSiteRunDto searchSiteRun = new AuctionSearchSiteRunDto();
                        searchSiteRun.AuctionSearchWordId = auctionSearchWord.Id;
                        searchSiteRun.AuctionSiteId = auctionSite.Id;
                        auctionWebServices.UpsertAuctionSearchSiteRun(searchSiteRun);

                    } catch(Exception ex)
                    {
                        string testError = auctionSearchWord.SearchWord + " " + ex.Message;
                    }
                    }
            }
        }


        private static DateTime GetAuctionDate(string timeLeft)
        {
            
            DateTime currendate = DateTime.UtcNow;


            if (timeLeft == null || timeLeft == "")
            {
                return currendate.AddYears(5);
            }


            DateTime dateResult;
            bool isDate =  DateTime.TryParse(timeLeft, out dateResult);
            if (isDate)
            {
                return dateResult;
            }


            if (timeLeft.Contains("d"))//add days
            {
                string[] days = timeLeft.Split('d');
                currendate = currendate.AddDays(Int32.Parse(days[0]));
            }

            if (timeLeft.Contains("h"))//add hours
            {
                string[] hours = timeLeft.Split('h');
                string hour = hours[0].Length == 1 ? hours[0] : hours[0].Substring(hours[0].Length - 2, 2);

                currendate = currendate.AddHours(Int32.Parse(hour));
            }

            if (timeLeft.Contains("m"))//add hours
            {
                string[] minutes = timeLeft.Split('m');
                string minute = minutes[0].Length == 1 ? minutes[0] : minutes[0].Substring(minutes[0].Length - 2, 2);

                currendate = currendate.AddMinutes(Int32.Parse(minute));
            }
            return currendate;
        }
        private void setupBrowser_Click(object sender, EventArgs e)
        {
            if(lblToken.Text == "") { return; }


            _auctionSites = _auctionWebServices.GetAuctionSites();
            _auctionSearchWords = _auctionWebServices.GetAuctionSearchWords();

            
            Task detail = GetAuctionItems(_auctionSites, _auctionSearchWords, _auctionWebServices);
            Task info = Task.WhenAll(detail);

            //   Task detail = TestCall("t3");

        }

        private void btnRetrieveToken_Click(object sender, EventArgs e)
        {
            _authentication = new Authentication(lblToken, txtSecurityCode);
            _authentication.AuthenticationToken();
            _auctionWebServices = new AuctionWebServices(_authentication);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtSecurityCode.Text = CryptoEngine.Encrypt(txtSecurityCode.Text, "");
        }





        public static async Task TestCall(string searchCall)
        {
            var options = new LaunchOptions { Headless = true };
            Console.WriteLine("Downloading chromium");

            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultRevision);
            Console.WriteLine("Navigating to developers.google.com");

            using (var browser = await Puppeteer.LaunchAsync(options))
            using (var page = await browser.NewPageAsync())
            {
                List<AuctionItemDto> totalresultsTest = new List<AuctionItemDto>();
                bool processNextPage = true;
                int pageRecords = 1;
                while (processNextPage)
                {
                    await page.GoToAsync("https://shopgoodwill.com/categories/listing?st=" + searchCall + "&sg=&c=&s=&lp=0&hp=999999&sbn=&spo=false&snpo=false&socs=false&sd=false&sca=false&caed=2%2F7%2F2022&cadb=7&scs=false&sis=false&col=1&p=" + pageRecords.ToString() + "&ps=40&desc=false&ss=0&UseBuyerPrefs=true&sus=false&cln=1&catIds=&pn=&wc=false&mci=false&hmt=false&layout=grid");
                    // Type into search box.

                    Thread.Sleep(10000);
                    //3d 5h
                    //2h 5m
                    try
                    {
                        AuctionItemDto[] resultsTest = await page.EvaluateFunctionAsync<AuctionItemDto[]>(jsCode);
                        if (resultsTest.Length < 40)
                        {
                            processNextPage = false;
                        }
                        pageRecords += 1;
                        var info2 = 12;
                        totalresultsTest.AddRange(resultsTest);

                    }
                    catch (Exception ex)
                    {
                        processNextPage = false;
                        string error = ex.Message;
                    }

                }



                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();



            }
        }



    }
}


/*
                var options = new LaunchOptions
                {
                    Headless = false
                };

                Console.WriteLine("Downloading chromium");
                using var browserFetcher = new BrowserFetcher();
                await browserFetcher.DownloadAsync();

                Console.WriteLine("Navigating google");
                using (var browser = await Puppeteer.LaunchAsync(options))
                using (var page = await browser.NewPageAsync())
                {
                    await page.GoToAsync("http://www.google.com");

                    Console.WriteLine("Generating PDF");
                    await page.PdfAsync(Path.Combine(Directory.GetCurrentDirectory(), "google.pdf"));

                    Console.WriteLine("Export completed");

                    // if (!args.Any(arg => arg == "auto-exit"))
                    //{
                    //    Console.ReadLine();
                    // }
                }
                */


//selectors[i].getElementsByTagName('li')[5].innerText.split('\n')
//return selectors.map( t=> {return { title: t.innerHTML}});
//selectors.map( t=> {return { title: t.getElementsByTagName('a')[0].innerText}});


/*
var six = await page.EvaluateFunctionAsync<int>("async () => await Promise.resolve(6)");
var sixtoo = await page.EvaluateFunctionAsync<int>("async () => await Promise.resolve(document.querySelectorAll('div').length)");

var jsonObjects = await page.EvaluateFunctionAsync("async () => await Promise.resolve(document.querySelectorAll('.feat-item_info'))");
*/

//var results = await page.EvaluateFunctionAsync(jsCode);
//selectors.map( t=> {return { title: t.innerHTML, url: t.href}});
/*
var itemDetails = await page.EvaluateFunctionAsync<string[]>("async () => await Promise.resolve(" +
    "var itemCount = itemInfo.length" +
    "for(var iMatt = 0; iMatt < itemCount;iMatt++) " +
    "" +
    ")");
*/
//document.querySelectorAll('.item-col').length;


/*
 * () => {var itemDetails = [];try{const selectors = Array.from(document.querySelectorAll('.s-item'));for(var i = 1; i< selectors.length;i++){if(selectors[i].innerText.indexOf('Best offer accepted') > -1){continue;}var itemDetail = {};var soldDetails =selectors[i].getElementsByClassName("s-item__title--tag");if(soldDetails.length > 0){var dateSold=soldDetails[0].innerText.split('\n');itemDetail.timeLeft = dateSold[0].replace('Sold ','');}var itemTitle=selectors[i].getElementsByClassName("s-item__title");if(itemTitle.length > 0){itemDetail.productName = itemTitle[0].innerText;}var itemPriceDetails = selectors[i].getElementsByClassName("s-item__price");if(itemPriceDetails.length > 0){itemDetail.itemPrice = itemPriceDetails[0].innerText;}var itemLink = selectors[i].getElementsByClassName("s-item__link");if(itemLink.length > 0){itemDetail.itemUrl = itemLink[0].href;}var totalBids = selectors[i].getElementsByClassName("s-item__bids");if(totalBids.length > 0){itemDetail.totalBids = totalBids[0].innerText.replace(' bids','');}itemDetails.push(itemDetail);}}catch(ex){}return itemDetails;}
*/