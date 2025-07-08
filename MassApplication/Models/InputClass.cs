using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MassApplication.Models
{
    public class InputClass
    {
        public List<int> Years { get; set; }
        public IList<SelectListItem> AvailableSummaryPeriod { get; set; }
        public IList<SelectListItem> AvailableDetailPeriod { get; set; }
        public IList<SelectListItem> AvailableMonth { get; set; }
        public IList<SelectListItem> AvailableHour { get; set; }
        public IList<SelectListItem> AvailableVolumeUnit { get; set; }
        public IList<string> SelectedNetworks { get; set; }
        public IList<SelectListItem> AvailableNetworks { get; set; }
        public IList<SelectListItem> AvailableSections { get; set; }
        public IList<SelectListItem> AvailableCheckMeters { get; set; }
        public IList<SelectListItem> AvailablePipeGroups { get; set; }
        public IList<SelectListItem> AvailableStations { get; set; }

        public InputClass()
        {
            AvailableSummaryPeriod = new List<SelectListItem>();
            AvailableDetailPeriod = new List<SelectListItem>();
            AvailableMonth = new List<SelectListItem>();
            AvailableHour = new List<SelectListItem>();
            AvailableVolumeUnit = new List<SelectListItem>();
            SelectedNetworks = new List<string>();
            AvailableNetworks = new List<SelectListItem>();
            AvailableSections = new List<SelectListItem>();
            AvailableCheckMeters = new List<SelectListItem>();
            AvailablePipeGroups = new List<SelectListItem>();
            AvailableStations = new List<SelectListItem>();
        }

    }
}