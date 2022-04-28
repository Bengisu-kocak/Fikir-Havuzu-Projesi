using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace oneri_sikayet.Models
{
    public class Discussion
    {
        public int ID { get; set; }
        [Display(Name = "Başlık")]
        public string Title { get; set; }
        [Display(Name = "Konu")]
        public string Subject { get; set; }
        [Display(Name = "Faydası")]
        public string Benefit { get; set; }
        [Display(Name = "Açıklama")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Display(Name ="Karar")]
        public bool Decision { get; set; }
        public bool DecisionMade { get; set; }
        [Display(Name ="Karar Açıklaması")]
        [DataType(DataType.MultilineText)]
        public string DecisionDescription { get; set; }
        [Display(Name ="Puan")]
        public int Points { get; set; }
        public Personel Owner { get; set; }
        [Display(Name ="Sahibi")]
        public string OwnerName { get {
                if (Owner == null)
                    return "";
                return Owner.Name + " " + Owner.Surname; } }
        public string Message { get; set; }
    }
}
