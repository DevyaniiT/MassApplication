using MassApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MassApplication.Controllers
{
    public class AssetController : BaseController
    {
        /// <summary>
        /// Not in used
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// This method is called when we fetch the data of FC(Flow Computer)
        /// </summary>
        /// <returns></returns>
        public ActionResult FCIndex()
        {
            if (ClassFolder.Login_Class.access_token == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                try
                {
                    string username = ClassFolder.Login_Class.email_id;
                    string token = ClassFolder.Login_Class.access_token;
                    string ParameterData = string.Format("?page_no=1&page_size=5000&searchKey=&access_token={0}", token);
                    var response = CallGetAPIResource("asset/getdata", ParameterData);
                    if (response.IsSuccessStatusCode)
                    {
                        var JsonContent = response.Content.ReadAsStringAsync().Result;
                        var tokenContent = getJSONdata(JsonContent);
                        var asset_data = tokenContent["content"]["collection"];
                        ViewBag.AssetFCData = asset_data;
                        return View();
                    }
                    else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('" + response.StatusCode + "'); window.location.href = '/Login/Index'; </script>");
                    }
                }
                catch (Exception ex)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('" + ex.Message + "'); window.location.href = '/Login/Index'; </script>");
                }
            }

        }

        /// <summary>
        /// This method is used to add new fc
        /// </summary>
        /// <returns></returns>
        public ActionResult AddNewFC()
        {
            try
            {
                if (ClassFolder.Login_Class.access_token == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    try
                    {
                        string token = ClassFolder.Login_Class.access_token;
                        string networkParameterData = string.Format("?access_token={0}", token);
                        string meteringtypeParameterData = string.Format("?table=metering_type_new&access_token={0}", token);
                        string gcParameterData = string.Format("?mst_id=&access_token={0}", token);
                        string stationParameterData = string.Format("?region={0}&access_token={1}", "1", token);
                        var networkDropdownData = getCallFunction("pipelinemst/dropdown", networkParameterData);
                        var metertypeDropdownData = getCallFunction("assethelper/getdata", meteringtypeParameterData);
                        var gcDropdownData = getCallFunction("gascompress/getdata", gcParameterData);
                        var stationDropdownData = getCallFunction("pipestation/dropdown", stationParameterData);
                        ViewBag.DDNetwork = networkDropdownData;
                        ViewBag.DDMeteringType = metertypeDropdownData;
                        ViewBag.DDGC = gcDropdownData;
                        ViewBag.DDStationName = stationDropdownData;
                        return PartialView("_partialNewFCView");
                    }
                    catch (Exception ex)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('" + ex.Message + "'); window.location.href = '/Login/Index'; </script>");
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }

        /// <summary>
        /// This method is used to insert the details of new fc related data 
        /// </summary>
        /// <param name="fc"></param>
        /// holds the FCAssetClass class data
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddNewFCData(FCAssetClass fc)
        {
            try
            {
                return RedirectToAction("FCIndex", "Asset");
            }
            catch (Exception ex)
            {
                return Content("ERROR;" + ex.Message.ToString());
            }
        }

        /// <summary>
        /// This method is called when we type name of station name when its appropriate result shows
        /// This method called based on the search key input
        /// </summary>
        /// <param name="searchKey"></param>
        /// Holds the words used for search key
        /// <returns></returns>
        public JsonResult SearchFCKey(string searchKey)
        {
            string token = ClassFolder.Login_Class.access_token;
            string stationParameterData = string.Format("?searchKey={0}&access_token={1}", searchKey, token);
            var stationDropdownData = getCallFunction("config/longid", stationParameterData);
            return Json(stationDropdownData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This method is called when we change the network according to that selected network its station name shows in dropdown option tag
        /// </summary>
        /// <param name="id"></param>
        /// Holds the network(region) id
        /// <returns></returns>
        public JsonResult GetStationDropdown(int id)
        {
            string token = ClassFolder.Login_Class.access_token;
            string stationParameterData = string.Format("?region={0}&access_token={1}", id, token);
            var stationDDData = getCallFunction("pipestation/dropdown", stationParameterData);
            return Json(stationDDData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// This method is called when we want to edit the FC table using specific FC id
        /// then that FC detail related to that id is show on popup modal
        /// </summary>
        /// <param name="Id"></param>
        /// Holds the FC id
        /// <returns></returns>
        public ActionResult EditAssetFCData(int Id)
        {
            try
            {
                string username = ClassFolder.Login_Class.email_id;
                string token = ClassFolder.Login_Class.access_token;
                string ParameterData = string.Format("?asset_id={0}&access_token={1}", Id, token);
                var fc_data = getCallFunction("asset/getdata", ParameterData);
                FCAssetClass pc = new FCAssetClass();
                FCAssetClass fcData = pc.getFCAssetData(fc_data);

                string networkParameterData = string.Format("?access_token={0}", token);
                string meteringtypeParameterData = string.Format("?table=metering_type_new&access_token={0}", token);
                string gcParameterData = string.Format("?mst_id=&access_token={0}", token);
                string stationParameterData = string.Format("?region={0}&access_token={1}", "1", token);
                var networkDropdownData = getCallFunction("pipelinemst/dropdown", networkParameterData);
                var metertypeDropdownData = getCallFunction("assethelper/getdata", meteringtypeParameterData);
                var gcDropdownData = getCallFunction("gascompress/getdata", gcParameterData);
                var stationDropdownData = getCallFunction("pipestation/dropdown", stationParameterData);
                ViewBag.DDEditNetwork = networkDropdownData;
                ViewBag.DDEditMeteringType = metertypeDropdownData;
                ViewBag.DDEditGC = gcDropdownData;
                ViewBag.DDEditData = fc_data;
                ViewBag.DDEditStationName = stationDropdownData;

                return PartialView("_partialEditFCView", fcData);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// This method is used for editing the FC table using its fc id
        /// </summary>
        /// <param name="fcData"></param>
        /// Basically this parameter is an instance of FCAssetClass class that hold the fc related data
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditFCFormData(FCAssetClass fcData)
        {
            try
            {
                return RedirectToAction("FCIndex", "Asset");
            }
            catch (Exception ex)
            {
                return Content("ERROR;" + ex.ToString());
            }
        }

        /// <summary>
        /// This method is used for deleting the fc data using its fc id
        /// </summary>
        /// <param name="Id"></param>
        /// This parameter hold the fc id
        /// <returns></returns>
        public ActionResult DeleteFCData(int Id)
        {
            try
            {
                return RedirectToAction("FCIndex", "Asset");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// This method is called when we fetch the data of GC(Gas Chromatograph)
        /// </summary>
        /// <returns></returns>
        public ActionResult GCIndex()
        {
            if (ClassFolder.Login_Class.access_token == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                try
                {
                    string username = ClassFolder.Login_Class.email_id;
                    string token = ClassFolder.Login_Class.access_token;
                    string ParameterData = string.Format("?page_no=1&page_size=2000&searchKey=&access_token={0}", token);
                    var response = CallGetAPIResource("gascompress/getdata", ParameterData);
                    if (response.IsSuccessStatusCode)
                    {
                        var JsonContent = response.Content.ReadAsStringAsync().Result;
                        var tokenContent = getJSONdata(JsonContent);
                        var gc_data = tokenContent["content"]["collection"];
                        ViewBag.GCData = gc_data;
                        return View();
                    }
                    else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('" + response.StatusCode + "'); window.location.href = '/Login/Index'; </script>");
                    }
                }
                catch (Exception ex)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('" + ex.Message + "'); window.location.href = '/Login/Index'; </script>");
                }
            }

        }

        /// <summary>
        /// This method is used for GC Chart View where graphs are defined
        /// </summary>
        /// <param name="id"></param>
        /// Holds the gc_id
        /// <returns></returns>
        public ActionResult GCChartViewer(int id)
        {
            try
            {
                string token = ClassFolder.Login_Class.access_token;
                string start_date = DateTime.Now.ToString("yyyy-MM-dd");
                string GCDC1_C6ParameterData = string.Format("?gc_id={0}&msec={1}&type=GcDiagnosticC1_C6&access_token={2}", id, start_date, token);
                string GCDC126ParameterData = string.Format("?gc_id={0}&msec={1}&type=GcDiagnosticC126&access_token={2}", id, start_date, token);
                string GCDC345ParameterData = string.Format("?gc_id={0}&msec={1}&type=GcDiagnosticC345&access_token={2}", id, start_date, token);
                var GCDiagnosticC1_C6 = getCallResponse("gascompress/graph", GCDC1_C6ParameterData);
                var GCDiagnosticC126 = getCallResponse("gascompress/graph", GCDC126ParameterData);
                var GCDiagnosticC345 = getCallResponse("gascompress/graph", GCDC345ParameterData);
                ViewBag.GCDiagnosticC1_C6 = GCDiagnosticC1_C6["content"]["collection"];
                ViewBag.GCDiagnosticC126 = GCDiagnosticC126["content"]["collection"];
                ViewBag.GCDiagnosticC345 = GCDiagnosticC345["content"]["collection"];
                ViewBag.GCValueC1_C6 = GCDiagnosticC1_C6["content"]["GcR2Diagnostic"];
                ViewBag.GCValueC126 = GCDiagnosticC126["content"]["GcR2Diagnostic"];
                ViewBag.GCValueC345 = GCDiagnosticC345["content"]["GcR2Diagnostic"];

                return View();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        /// <summary>
        /// Not in used
        /// And return the string of parameter data for gc table
        /// </summary>
        /// <param name="id"></param>
        /// <param name="gcType"></param>
        /// <param name="from_date"></param>
        /// <param name="tokenData"></param>
        /// <returns></returns>
        public static string GetparameterValue(int id, string gcType, string from_date, string tokenData)
        {
            string parameterData = string.Format("?gc_id={0}&msec={1}&type={2}&access_token={3}", id, gcType, from_date, tokenData);
            return parameterData;
        }
    }
}