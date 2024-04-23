using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Events.Calendar.Domain
{

    public class Country : EntityBase
    {
        [MaxLength(50)]
        public string? IsoCode { get; set; }

    }
    public class City : EntityBase
    {
        [ForeignKey("Country")]
        public Guid CountryId { get; set; }

        [MaxLength(50)]
        public string? IsoCode { get; set; }

    }

    public class Location : EntityBase
    {
        [ForeignKey("City")]
        public Guid CityId { get; set; }

        [MaxLength(50)]
        public string? IsoCode { get; set; }

        [Required]
        [MaxLength(500)]
        public required string? Description { get; set; }

        [Required]
        public required double? MapsLongitude { get; set; }
        [Required]
        public required double? MapsLatitude { get; set; }
        //public object CountryId { get; internal set; }
    }

    public class Event : EntityBase
    {
        [Required]
        [MaxLength(500)]
        public required string Description { get; set; }

        [MaxLength(50000)]
        public string? DescriptionFull { get; set; }

        public DateTime? DatePlanned { get; set; }

        public DateTime? DatePlannedEnd { get; set; }

        [MaxLength(50)]
        public string? ContactMobile { get; set; }

        [MaxLength(100)]
        public string? ContactEmail { get; set; }

        [MaxLength(50)]
        public string? ContactWebsite { get; set; }

        public int? Popularity { get; set; }
        public int? ExpectedAudiance { get; set; }
        public bool? IsFree { get; set; }
        public decimal? EntryFee { get; set; }

        [ForeignKey("SystemUser")]
        public Guid SystemUserId { get; set; }

        [ForeignKey("Location")]
        public Guid LocationId { get; set; }
    }

    public class EventRating : EntityBase
    {
        [ForeignKey("Event")]
        public Guid EventId { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Description { get; set; }

        [Required]
        public int Rating { get; set; }

        [ForeignKey("SystemUser")]
        public Guid SystemUserId { get; set; }
    }

    public class EventTag : EntityBase
    {
        [ForeignKey("Event")]
        public Guid EventId { get; set; }

        [Required]
        [MaxLength(100)]
        public required string Tag { get; set; }

        [ForeignKey("SystemUser")]
        public Guid SystemUserId { get; set; }
    }

    public class EventPics : EntityBase
    {
        [ForeignKey("Event")]
        public Guid EventId { get; set; }

        [Required]
        [MaxLength(255)]
        public required string Description { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Url { get; set; }

        [Required]
        public EventPicsType EventPicsType { get; set; }

        [ForeignKey("SystemUser")]
        public Guid SystemUserId { get; set; }

    }


    public class Timeslot : EntityBase
    {
        [Required]
        [MaxLength(255)]
        public required string Description { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [MaxLength(255)]
        public string? Timezone { get; set; }

        public bool? IsFree { get; set; }
    }

    public class EventTimeslot : EntityBase
    {
        [ForeignKey("Event")]
        public Guid EventId { get; set; }

        [ForeignKey("SystemUser")]
        public Guid SystemUserId { get; set; }

    }


    public enum EventPicsType
    {
        Picture = 1,
        Video = 2,
        Links = 3
    }

}
