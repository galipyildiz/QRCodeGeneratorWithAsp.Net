using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QRCode.Models
{
    public class QRModel
    {
        [Required]
        [Display(Name = "QR Code Content")]
        public string QrCodeContent { get; set; }
        public string QrCodePath { get; set; }
    }
}
