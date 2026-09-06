
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using tp1;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace tpfinal
{

	public class Estrategia
	{
		
        //Todos los que tengan un return con "implementar" pueden ser los que quieras empezar a hacer
        //Veo si te llego a hacer un UML para cuando veas el github o el README... Si te lo imaginas de otra forma
        //Podes modificarlo, no hay drama alguna
		public string GetUrlSeoPorId(ArbolGeneral<ItemCat> arbol, int id)
        {
            return "Implementar";
        }
        

        public List<string> GetURLsSEO(ArbolGeneral<ItemCat> arbol)
		{
			return ["Implementar"];
		}
        

              

        public List<List<string>> ConsultaNiveles(ArbolGeneral<ItemCat> arbol)
		{
            return [["Implementar"]];
        }

        //Devuelve todos los productos del catalogo, no especifica absolutamente nada, manda todo de una//
        public List<ItemCat> Todos(ArbolGeneral<ItemCat> arbol)
        {
            List<ItemCat> resultado = new List<ItemCat>();
            RecolectarProductos(arbol, resultado);
            return resultado;
        }
        //Metodo para recolectar todos los productos, de paso los aloja momentaneamente asi el return funciona//
        private void RecolectarProductos(ArbolGeneral<ItemCat> nodo, List<ItemCat> resultado)
        {
            if (nodo.getDatoRaiz().Tipo == TipoElemento.Producto)
            {
                resultado.Add(nodo.getDatoRaiz());
            }

            foreach (var hijo in nodo.getHijos())
            {
                RecolectarProductos(hijo, resultado);
            }

        }

        public void Agregar(ArbolGeneral<ItemCat> arbol, ItemCat dato, string rutaAlPadre)
		{
            return [["implementar"]];
        }

        //buscar(): Basicamente, a diferencia de todos() este busca filtrado por categorias y productos.//
        public List<ItemCat> Buscar(ArbolGeneral<ItemCat> arbol, string elementoABuscar)
		{
			List<ItemCat> resultado = new List<ItemCat>();
            RecolectarCoincidencias(arbol, elementoABuscar, resultado);
            return resultado;
		}

        //Este es su metodo privado para que funcione correctamente//
        private void RecolectarCoincidencias(ArbolGeneral<ItemCat> nodo, string texto, List<ItemCat> resultado)
        {
            if (nodo.getDatoRaiz().Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase))
            {
                resultado.Add(nodo.getDatoRaiz());
            }

            foreach (var hijo in nodo.getHijos())
            {
                RecolectarCoincidencias(hijo, texto, resultado);
            }
        }
            
    }
}