using MassApplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MassApplication.Controllers
{
    public class PipelineController : BaseController
    {
        /// <summary>
        /// This method is called when we fetch the data of pipeline
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
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
                    string ParameterData = string.Format("?page_no=1&page_size=2000&searchKey=&username={0}&access_token={1}", username, token);
                    var response = CallGetAPIResource("pipelinemst/getdata", ParameterData);
                    if (response.IsSuccessStatusCode)
                    {
                        var JsonContent = response.Content.ReadAsStringAsync().Result;
                        var tokenContent = getJSONdata(JsonContent);
                        var pipe_data = tokenContent["content"]["collection"];
                        ViewBag.TableData = pipe_data;
                        return View();
                    }
                    else
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('" + response.StatusCode + "'); window.location.href = 'Index'; </script>");
                    }
                }
                catch (Exception ex)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('" + ex.Message + "'); window.location.href = 'Index'; </script>");
                }
            }
        }

        /// <summary>
        /// This method is called when we want to edit the pipeline table using specific pipeline id
        /// then that pipeline detail related to that id is show on popup modal
        /// </summary>
        /// <param name="Id"></param>
        /// This parameter hold the pipeline id
        /// <returns></returns>
        public ActionResult EditPipelineData(int Id)
        {
            try
            {
                string username = ClassFolder.Login_Class.email_id;
                string token = ClassFolder.Login_Class.access_token;
                string ParameterData = string.Format("?page_no=1&page_size=2000&searchKey=&username={0}&pipelinemst_id={1}&access_token={2}", username, Id, token);
                var pipeline_data = getCallFunction("pipelinemst/getdata", ParameterData);
                PipelineClass pc = new PipelineClass();
                PipelineClass pipelineData = pc.getPipelineData(pipeline_data);

                string stationParameter = string.Format("?region=&access_token={0}", token);
                var station_data = getCallFunction("pipestation/dropdown", stationParameter);
                List<SelectListItem> dropdown_station = dropdown_station_item(station_data);
                ViewBag.ModalStationData = dropdown_station;
                return PartialView("_partialEditPipelineView", pipelineData);

            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }

        }

        /// <summary>
        /// This method is used for editing the pipeline table using its pipeline id
        /// </summary>
        /// <param name="pipeData"></param>
        /// Basically this parameter is an instance of PipelineClass class that hold the pipeline related data
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditPipelineFormData(PipelineClass pipeData) 
        {
            try
            {
                //return RedirectToAction("Index", "Pipeline");
                return Content("ERROR;Devyani");
            }
            catch (Exception ex)
            {
                return Content("ERROR;" + ex.ToString());
            }
        }

        /// <summary>
        /// This method is used for deleting the pipeline data using its pipeline id
        /// </summary>
        /// <param name="Id"></param>
        /// This parameter hold the pipeline id
        /// <returns></returns>
        public ActionResult DeletePipelineData(int Id)
        {
            try 
            {
                int pipeId = Id;
                return RedirectToAction("Index", "Pipeline");
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }
    }
}