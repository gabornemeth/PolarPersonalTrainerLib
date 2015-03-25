using System;
using System.Collections.Generic;
using System.Text;

namespace PolarPersonalTrainerLib
{

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.topografix.com/GPX/1/1")]
    [System.Xml.Serialization.XmlRootAttribute(ElementName= "gpx", Namespace = "http://www.topografix.com/GPX/1/1", IsNullable = false)]
    public partial class GPXExercise
    {

        private gpxMetadata metadataField;

        private gpxTrk trkField;

        private string creatorField;

        private decimal versionField;

        /// <remarks/>
        public gpxMetadata metadata
        {
            get
            {
                return this.metadataField;
            }
            set
            {
                this.metadataField = value;
            }
        }

        /// <remarks/>
        public gpxTrk trk
        {
            get
            {
                return this.trkField;
            }
            set
            {
                this.trkField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string creator
        {
            get
            {
                return this.creatorField;
            }
            set
            {
                this.creatorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.topografix.com/GPX/1/1")]
    public partial class gpxMetadata
    {

        private string nameField;

        private gpxMetadataAuthor authorField;

        private System.DateTime timeField;

        private gpxMetadataBounds boundsField;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public gpxMetadataAuthor author
        {
            get
            {
                return this.authorField;
            }
            set
            {
                this.authorField = value;
            }
        }

        /// <remarks/>
        public System.DateTime time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        public gpxMetadataBounds bounds
        {
            get
            {
                return this.boundsField;
            }
            set
            {
                this.boundsField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.topografix.com/GPX/1/1")]
    public partial class gpxMetadataAuthor
    {

        private string nameField;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.topografix.com/GPX/1/1")]
    public partial class gpxMetadataBounds
    {

        private decimal maxlonField;

        private decimal maxlatField;

        private decimal minlonField;

        private decimal minlatField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal maxlon
        {
            get
            {
                return this.maxlonField;
            }
            set
            {
                this.maxlonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal maxlat
        {
            get
            {
                return this.maxlatField;
            }
            set
            {
                this.maxlatField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal minlon
        {
            get
            {
                return this.minlonField;
            }
            set
            {
                this.minlonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal minlat
        {
            get
            {
                return this.minlatField;
            }
            set
            {
                this.minlatField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.topografix.com/GPX/1/1")]
    public partial class gpxTrk
    {

        private gpxTrkTrkpt[] trksegField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("trkpt", IsNullable = false)]
        public gpxTrkTrkpt[] trkseg
        {
            get
            {
                return this.trksegField;
            }
            set
            {
                this.trksegField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.topografix.com/GPX/1/1")]
    public partial class gpxTrkTrkpt
    {

        private decimal eleField;

        private System.DateTime timeField;

        private byte satField;

        private decimal lonField;

        private decimal latField;

        /// <remarks/>
        public decimal ele
        {
            get
            {
                return this.eleField;
            }
            set
            {
                this.eleField = value;
            }
        }

        /// <remarks/>
        public System.DateTime time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }

        /// <remarks/>
        public byte sat
        {
            get
            {
                return this.satField;
            }
            set
            {
                this.satField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal lon
        {
            get
            {
                return this.lonField;
            }
            set
            {
                this.lonField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal lat
        {
            get
            {
                return this.latField;
            }
            set
            {
                this.latField = value;
            }
        }
    }

}

