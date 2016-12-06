using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace KVVspeichelner
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<fahrtInfo> summarise = new List<fahrtInfo>();
        private void btn_search_Click(object sender, EventArgs e)
        {
            var client = new RestClient("https://www.kvv.de/index.php?id=674");
            var request = new RestRequest(Method.POST);
            request.AddHeader("postman-token", "dfa38d4c-2bdf-8feb-6e32-6bd02f761642");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("cookie", "mdv_odvSug=%5B%22Karlsruhe,%20Durlacher%20Tor%20/%20KIT-Campus%20S%C3%BCd%7C7000003%7Cstop%7C936695%7C5723584%22,%22Karlsruhe,%20Herrenstra%C3%9Fe%7C7000099%7Cstop%7C935110%7C5723480%22,%22Karlsruhe,%20Karlstor%7C7000061%7Cstop%7C934481%7C5724307%22%5D; HASESSIONID=E; __utmt=1; __utma=108000856.455896153.1473700852.1474831294.1474915012.5; __utmb=108000856.10.10.1474915012; __utmc=108000856; __utmz=108000856.1473700852.1.1.utmcsr=(direct)|utmccn=(direct)|utmcmd=(none)");
            request.AddHeader("accept-language", "zh-CN,zh;q=0.8,en;q=0.6");
            request.AddHeader("accept-encoding", "gzip, deflate, br");
            request.AddHeader("referer", "https//www.kvv.de/");
            request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36");
            request.AddHeader("upgrade-insecure-requests", "1");
            request.AddHeader("origin", "https//www.kvv.de");
            //add the parameters
            request.AddParameter("application/x-www-form-urlencoded", "SpEncId=0&action=XSLT_TRIP_REQUEST2&anyMaxSizeHitList=20&anySigWhenPerfectNoOtherMatches=1&convertAddressesITKernel2LocationServer=1&convertCrossingsITKernel2LocationServer=1&convertPOIsITKernel2LocationServer=1&convertStopsPTKernel2LocationServer=1&itdDateDayMonthYear=02.10.2016&itdLPxx_formAction=https://www.kvv.de/index.php?id=674 &itdLPxx_snippet=true&itdTimeHour=12&itdTimeMinute=0&itdTripDateTimeDepArr=arr&language=de&lineRestriction=403&locationServerActive=1&nameDefaultText_destination=z.B. Bruchsal Rendezvous&nameDefaultText_origin=z.B. Karlsruhe Ettlinger Tor &nameInfo_destination=7000003&nameInfo_origin=streetID:1500000153::8215059:-1:Sternenfelser Straße:Oberderdingen:Sternenfelser Straße::Sternenfelser Straße: 75038:ANY:DIVA_STREET:980209:5714746:MRCV:B_W&name_destination=Karlsruhe, Durlacher Tor / KIT-Campus Süd&name_origin=Oberderdingen, Sternenfelser Straße&ptOptionsActive=1&requestID=0&sessionID=0&stateless=1&type_destination=any&type_origin=any&useRealtime=1", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string parse="";
            parse=WebParse(response.Content);
            List<string> bp = BasicParse(parse);
            DetailParse(bp);
            init();
            //tb_result.Text = fu.UmstiegType + "\r\n" + fu.Ab + "\r\n" + fu.AbStation + "\r\n" + fu.An + "\r\n" + fu.AnStation;
            //ResultForm rf = new ResultForm(summarise);
            //rf.Show();
        }
        private string WebParse(string oriText)
        {
            Regex reg = new Regex(@"<table[^>]*""fahrt_planen""[^>]*>([\s\S]*)</table>");
            Match match= reg.Match(oriText);
            string result = match.Groups[0].Value;
            return result;
        }
        private List<string> BasicParse(string oriTable)
        {
            //get raw data, including Abfahrt, Ankunft, Reisedauer...uaually 3 groups
            List<string> result = new List<string>();
            Regex reg = new Regex(@"<tr><td scope=""row"">[\s\S]*?Fahrt</span></a></p></td><td>(?<abfahrt>[\s\S]*?)</td><td>(?<ankunft>[\s\S]*?)</td><td>(?<reisedauer>[\s\S]*?)</td><td>(?<stiegsnum>[\s\S]*?)</td><td>[\s\S]*?<a href=""http://www.openstreetmap.org/copyright[\s\S]*?</tr>");
            Match match = reg.Match(oriTable);
            while(match.Success)
            {
                fahrtInfo fi = new fahrtInfo();
                fi.Abfahrt = match.Groups["abfahrt"].Value;
                fi.Ankunft = match.Groups["ankunft"].Value;
                fi.Reisedauer = match.Groups["reisedauer"].Value;
                fi.Umstiegnum = match.Groups["stiegsnum"].Value;
                summarise.Add(fi);
                result.Add(match.Groups[0].Value);
                match=match.NextMatch();
            }
            return result;
        }
        private void DetailParse(List<string> oriTable)
        {
            
            fahrtInfo allInfo = new fahrtInfo();
            List<string> singleresult = new List<string>();
            List<List<string>> resultbasket=new List<List<string>>();
            string resultstring = "";
            foreach(string s in oriTable)
            {
                resultstring += s;
            }
            //get partial route table raw data and divide into List<List<string>>-- all umstieg info, usually 3 groups of results.Todo: combine fahrtInfo in 3 groups;
            Regex reg = new Regex(@"id=""partialRouteTable(?<group>[0-9])[\s\S]*?=(?:</table>)?(?:[\s\S]*?).*?(?:(?:</a></td></tr>)|(?:</a></p></td></tr></table>)|(?:</td><td /></tr></table>))");
            Match match = reg.Match(resultstring);
            int tempcounter = 1;
            while (match.Success)
            {
                if (match.Groups["group"].Value == tempcounter.ToString())
                {
                    singleresult.Add(match.Groups[0].Value);
                    match = match.NextMatch();
                    if(!match.Success)
                        resultbasket.Add(singleresult);
                }
                else
                {
                    resultbasket.Add(singleresult);
                    singleresult=new List<string>();
                    tempcounter++;
                    singleresult.Add(match.Groups[0].Value);
                    match = match.NextMatch();
                }
            }
            //parse arrival time & station names, usually 3 pairs of results--save as entity
            reg = new Regex(@"(<td>((?<ab>ab[\s\S]*?)|(?<an>an.+?))</td>([\s\S]*?))?\)"">(?<station>[\s\S]*?)</a>");
            string type="";
            string ab="";
            string an="";
            string abstation="";
            string anstation="";
            int z = 0;
            foreach (List<string> sss in resultbasket)
            {
                for(int i=0; i<sss.Count;i++)
                {
                    type = stiegtypeParse(sss[i]);
                    Match stationM = reg.Match(sss[i]); //error
                    if (stationM.Success && stationM.Groups["ab"].Value.Length > 0)
                    {
                        ab = stationM.Groups["ab"].Value;
                        abstation = stationM.Groups["station"].Value;
                        stationM = stationM.NextMatch();
                    }
                    if (stationM.Success && stationM.Groups["an"].Value.Length > 0)
                    {
                        an = stationM.Groups["an"].Value;
                        anstation = stationM.Groups["station"].Value;
                        stationM = stationM.NextMatch();
                    }
                    if(stationM.Success && stationM.Groups["an"].Value.Length == 0&& stationM.Groups["ab"].Value.Length == 0)
                    {//fussweg
                        type = "Fussweg";
                        anstation = stationM.Groups["station"].Value;
                        stationM = stationM.NextMatch();
                        abstation = stationM.Groups["station"].Value;
                        stationM = stationM.NextMatch();
                    }
                    fahrtUmstieg fu = new fahrtUmstieg(type, ab, an, abstation, anstation);
                    allInfo.Umstieginfo.Add(fu);
                }
                //should change --> instead of add
                fahrtInfo fi=summarise[z];
                fi.Umstieginfo = allInfo.Umstieginfo;
                summarise[z] = fi;
                allInfo = new fahrtInfo();
                z++;
            }
        }

        /// <summary>
        /// only parses Stype like "Regionalbus 702 Richtung Flehingen Stadtbahnhof"
        /// </summary>
        /// <param name="oriText"></param>
        /// <returns></returns>
        private string stiegtypeParse(string oriText)
        {
            Regex reg = new Regex(@"<img alt=""(?<type>[\s\S]*?)""");
            Match match = reg.Match(oriText);
            return match.Groups["type"].Value;
        }

        private void init()
        {
            foreach (fahrtInfo initFI in summarise)
            {
                TreeNode tn = new TreeNode();
                tn.Text = "Abfahrt um " + initFI.Abfahrt;

                TreeNode ak = new TreeNode();
                ak.Text = "Ankunft " + initFI.Ankunft;
                tn.Nodes.Add(ak);

                TreeNode rd = new TreeNode();
                rd.Text = "Reisedauer " + initFI.Reisedauer;
                tn.Nodes.Add(rd);

                TreeNode um = new TreeNode();
                um.Text = "Umstiegsnummer " + initFI.Umstiegnum;
                tn.Nodes.Add(um);

                TreeNode au = new TreeNode();
                au.Text = "Ausführliche Umstieginfo ";
                tn.Nodes.Add(au);
                treeView1.Nodes.Add(tn);
                foreach (fahrtUmstieg fu in initFI.Umstieginfo)
                {
                    TreeNode futype = new TreeNode();
                    futype.Text = fu.UmstiegType;
                    if (fu.UmstiegType == "")
                        futype.Text = "Fussweg";
                    au.Nodes.Add(futype);

                    TreeNode fuab = new TreeNode();
                    fuab.Text = fu.Ab;
                    futype.Nodes.Add(fuab);

                    TreeNode fuabs = new TreeNode();
                    fuabs.Text = "Abfahrt Station " + fu.AbStation;
                    futype.Nodes.Add(fuabs);

                    TreeNode fuan = new TreeNode();
                    fuan.Text = fu.An;
                    futype.Nodes.Add(fuan);

                    TreeNode fuans = new TreeNode();
                    fuans.Text = "Ankunft Station " + fu.AnStation;
                    futype.Nodes.Add(fuans);
                }

            }
        }
    }
}
