using System;
using System.Xml.Serialization;

namespace PolarPersonalTrainerLib
{
    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://www.topografix.com/GPX/1/1")]
    [XmlRoot(ElementName = "gpx", Namespace = "http://www.topografix.com/GPX/1/1", IsNullable = false)]
    public class GpxExercise
    {
        [XmlElement("metadata")]
        public GpxMetadata MetaData
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElement("trk")]
        public GpxTrack Track
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttribute("creator")]
        public string Creator
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttribute("version")]
        public string Version
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://www.topografix.com/GPX/1/1")]
    public class GpxMetadata
    {
        /// <remarks/>
        [XmlElement("name")]
        public string Name
        {
            get; set;
        }

        /// <remarks/>
        [XmlElement("author")]
        public GpxMetadataAuthor Author
        {
            get; set;
        }

        /// <remarks/>
        [XmlElement("time")]
        public DateTime Time
        {
            get; set;
        }

        /// <remarks/>
        [XmlElement("bounds")]
        public GpxMetadataBounds Bounds
        {
            get; set;
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.topografix.com/GPX/1/1")]
    public class GpxMetadataAuthor
    {
        [XmlElement("name")]
        public string Name
        {
            get; set;
        }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://www.topografix.com/GPX/1/1")]
    public class GpxMetadataBounds
    {
        /// <remarks/>
        [XmlAttribute("maxlon")]
        public decimal MaxLongitude { get; set; }

        /// <remarks/>
        [XmlAttribute("maxlat")]
        public decimal MaxLatitude
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttribute("minlon")]
        public decimal MinLongitude
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttribute("minlat")]
        public decimal MinLatitude { get; set; }
    }

    /// <summary>
    /// GPX track
    /// </summary>
    [XmlType(AnonymousType = true, Namespace = "http://www.topografix.com/GPX/1/1")]
    public class GpxTrack
    {
        /// <summary>
        /// Track points
        /// </summary>
        [XmlArray("trkseg")]
        [XmlArrayItemAttribute("trkpt", IsNullable = false)]
        public GpxTrackpoint[] Trackpoints
        {
            get;
            set;
        }
    }

    /// <remarks/>
    [XmlType(AnonymousType = true, Namespace = "http://www.topografix.com/GPX/1/1")]
    public class GpxTrackpoint
    {
        /// <remarks/>
        [XmlElement("ele")]
        public decimal Elevation
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElement("time")]
        public DateTime Time
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlElement("sat")]
        public byte Satellites
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttribute("lon")]
        public decimal Longitude
        {
            get;
            set;
        }

        /// <remarks/>
        [XmlAttribute("lat")]
        public decimal Latitude
        {
            get;
            set;
        }
    }
}
