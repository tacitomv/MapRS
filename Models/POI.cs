using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Threading;
using AutoMapper;
using Mapa.Extensions;
using Mapa.Models;
using Newtonsoft.Json;

namespace Mapa.Models
{
    public class POI
    {
        public POI(string csvline)
        {
            try
            {
                //name;category;address;latitude;longitude;description;tags;url;logo
                if (string.IsNullOrEmpty(csvline) || !csvline.Contains(";"))
                    throw new ArgumentNullException("Isto não parece ser uma CSV string!");

                var row = csvline.Split(';');
                this.Activated = true;
                this.Name = row[0];
                this.Category = row[1];
                this.Address = row[2];
                this.Latitude = Convert.ToDouble(row[3]);
                this.Longitude = Convert.ToDouble(row[4]);
                this.Description = row[5];
                this.Tags = row[6];
                this.Website = row[7];
                this.Logo = row[8];
            }
            catch (Exception e)
            {
                throw new Exception(e + "\nLinha que deu pau: " + csvline);
            }
        }

        public POI()
        {
            this.Activated = false;
			this.DefinedTags = new List<POITag>();
        }

        public POI(IList<object> row)
        {
            try
            {
                // System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
                // customCulture.NumberFormat.NumberDecimalSeparator = ".";
                // System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
                
                var today = DateTime.Now;
                this.Activated = true;
                this.CreationDate = today;
                this.ApprovalDate = today;
                this.Name = row.GetAt(0);
                this.Category = row.GetAt(1);
                this.Address = row.GetAt(2);
                this.Latitude = Convert.ToDouble(row.GetAt(3).Replace('.', ','));
                this.Longitude = Convert.ToDouble(row.GetAt(4).Replace('.', ','));
                this.Description = row.GetAt(5);
                this.Tags = row.GetAt(6);
                this.Website = row.GetAt(7);
                this.Logo = row.GetAt(8);
            }
            catch (Exception e)
            {
                throw new Exception(e + "\nDeu pau: " + row);
            }
        }

        public int Id { get; set; }
        [Display(Name = "Nome")]
        public string Name { get; set; }
        public string Logo { get; set; }
        [Display(Name = "Descrição")]
        public string Description { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public bool Activated { get; set; }
        [Display(Name = "Categoria")]
        public string Category { get; set; }
        [Display(Name = "Telefone")]
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Email { get; set; }
        public string Tags { get; set; }
        [Display(Name = "Endereço")]
        public string Address { get; set; }
        [Display(Name = "Nome de Contato")]
        public string ContactName { get; set; }
        [Display(Name = "Criação")]
        public DateTime CreationDate { get; set; }
        [Display(Name = "Aprovação")]
        public DateTime ApprovalDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Ano de Fundação")]
        public DateTime FoundationDate { get; set; }
        public string CNPJ { get; set; }
		public ICollection<POITag> DefinedTags { get; set; }

		public static POI GetPOI(POIViewModel vm) => Mapper.Map<POI>(vm);
	}

	public class POIViewModel : POI
	{
		public List<int> Segments { get; set; }
		public List<int> Tecnologies { get; set; }
	}
}