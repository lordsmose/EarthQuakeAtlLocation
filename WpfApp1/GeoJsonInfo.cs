using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json.Serialization;

namespace WpfApp1
{
    public class GeoJsonInfo
    {
        public string type { get; set; }

        public Metadata metadata { get; set; }

        public Features[] features { get; set; }

        public double?[] bbox { get; set; }
    }
    public class Metadata
    { 
        public string generated { get; set; }

        public string url { get; set; }


    }
    public class Features
    {
        public string type { get; set; }

        public Properties properties { get; set; }

        public Geometry geometry { get; set; }

        public string id { get; set; }
    }
    public class Properties
    {
        public decimal? mag { get; set; }

        public string place { get; set; }
        
        public long? time { get; set; }

        public long? updated { get; set; }

        public int? tx { get; set; }

        public string url { get; set; }

        public string detail { get; set; }

        public string felt { get; set; }

        public decimal? cdi { get; set; }

        public decimal? mmi { get; set; }

        public int? sig { get; set; }

        public string net { get; set; }

        public string code { get; set; }

        public string ids { get; set; }

        public string sources { get; set; }

        public string types { get; set; }

        public int? nst { get; set; }

        public decimal? dmin { get; set; }

        public decimal? rms { get; set; }

        public decimal? gap { get; set; }

        public string magType { get; set; }

        public string type { get; set; }
    }

    public class Geometry
    {
        public string type { get; set; }

        public double?[] coordinates { get; set; }
    }

}
