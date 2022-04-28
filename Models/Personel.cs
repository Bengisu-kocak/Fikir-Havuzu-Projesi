using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace oneri_sikayet.Models
{
    public class Personel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Sicil { get; set; }
        public string Tcno { get; set; }
        public int Id { get; set; }
        [Display(Name="Yönetici")]
        public bool isAdmin
        {
            get { return Permission == 1; }
            set { Permission = value ? 1 : 0;  }
        }
        public int Permission { get; set; }
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name="Pasif")]
        public bool isPassive { get; set; }

        public string ErrorMessage { get; set; }


    }
}
