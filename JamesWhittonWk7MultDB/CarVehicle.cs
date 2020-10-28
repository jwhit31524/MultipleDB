using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace JamesWhittonWk7MultDB
{
    [Table("CVT002_CAR_VEHICLE")]
    public class CarVehicle
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int VehicleNo { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string VehicleTypeCode { get; set; }

        [Required]
        [MaxLength(50)]
        [Column(TypeName = "VARCHAR")]
        public string VehicleID { get; set; }

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string VehicleColor { get; set; }

        [Required]
        [MaxLength(100)]
        [Column(TypeName = "VARCHAR")]
        public string CarType { get; set; }

    }
}

//comment
