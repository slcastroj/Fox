using System;
using System.ComponentModel.DataAnnotations;
using Fox.DataAccess.Utils;

namespace Fox.WebService.Models.Transfer.Producto
{

	public class PutById
	{
		[Required(ErrorMessage = "Se requiere campo nombre")]
		[RegularExpression(Matches.Nombre, ErrorMessage = "Formato de nombre inválido")]
		public String Nombre { get; set; }

		[Required(ErrorMessage = "Se requiere campo descripción")]
		[RegularExpression(Matches.Nombre, ErrorMessage = "Formato de descripción inválido")]
		public String Descripcion { get; set; }

		[Required(ErrorMessage = "Se requiere campo necesita receta")]
		public Boolean NecesitaReceta { get; set; }

		[Required(ErrorMessage = "Se requiere campo máximo semanal")]
		[Range(0, Byte.MaxValue, ErrorMessage = "Máximo semanal fuera de rango")]
		public Int32 MaximoSemanal { get; set; }

		[Required(ErrorMessage = "Se requiere campo peso gr")]
		[Range(0, Int16.MaxValue, ErrorMessage = "Peso gr fuera de rango")]
		public Int32 PesoGr { get; set; }

		[Required(ErrorMessage = "Se requiere campo precio unidad")]
		[Range(0, Int16.MaxValue, ErrorMessage = "Precio unidad fuera de rango")]
		public Int32 PrecioUnidad { get; set; }

		[Required(ErrorMessage = "Se requiere campo laboratorio")]
		[Range(0, Int16.MaxValue, ErrorMessage = "Id laboratorio fuera de rango")]
		public Int32 Laboratorio { get; set; }

		[Required(ErrorMessage = "Se requiere campo tipo")]
		[Range(0, Int16.MaxValue, ErrorMessage = "Id tipo fuera de rango")]
		public Int32 Tipo { get; set; }

		[Required(ErrorMessage = "Se requiere campo stock")]
		[Range(0, Int16.MaxValue, ErrorMessage = "Stock fuera de rango")]
		public Int32 Stock { get; set; }
	}
}
