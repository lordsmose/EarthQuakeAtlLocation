using System;
using System.Collections.Generic;
using System.Text;

namespace WpfApp1
{
    class UserInformation
    {
        private double LongitudeValue;

        public double Longitude
        {
            get { return LongitudeValue; }
            set { LongitudeValue = value; }
        }

        private double LaditudeValue;

        public double Laditude
        {
            get { return LaditudeValue; }
            set { LaditudeValue= value; }
        }

        private double SearchRadiusValue;

        public double SearchRadius
        {
            get { return SearchRadiusValue; }
            set { SearchRadiusValue = value; }
        }

        private string StartTimeValue;

        public string StartTime
        {
            get { return StartTimeValue; }
            set { StartTimeValue = value; }
        }

        private string EndTimeValue;

        public string EndTime
        {
            get { return EndTimeValue; }
            set { EndTimeValue = value; }
        }
    }
}
