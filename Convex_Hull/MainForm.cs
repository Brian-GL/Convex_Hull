/*
 * Created by SharpDevelop.
 * User: qwert
 * Date: 15/06/2019
 * Time: 17:03
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Threading;
using System.Globalization;
using System.Configuration;

namespace Convex_Hull
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		
		List<Point> puntos = new List<Point>();
		bool ya_genero = false;
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void BtnLimpiarClick(object sender, EventArgs e)
		{
			ya_genero = false;
			puntos.Clear();
			this.Refresh();
		}
		void Dibujar(object sender, MouseEventArgs e)
		{
			Graphics g = this.CreateGraphics();
			var lapiz = new Pen(Color.Black,2);
			
			g.DrawEllipse(lapiz,new RectangleF(e.X,e.Y,10,10));
			
			lapiz.Dispose();
			g.Dispose();
			
			puntos.Add(new Point(e.X,e.Y));
			
		}

		void dibujar_linea(int a,int b, int c, int d){
			Graphics graficar = this.CreateGraphics();
			var pincel = new Pen(Color.Black,3);
			graficar.DrawLine(pincel,a,b,c,d);
			pincel.Dispose();
			graficar.Dispose();
		}
		
		
		//Funcion que nos sirve para encontrar orientación de triplete ordenado (p, q, r).
		 // La función devuelve los siguientes valores
     	// 0 -> p, q y r son colineales (se hallan en la misma recta).
     	// 1 -> En el sentido de las agujas del reloj
    	 // 2 -> A la izquierda
		int orientacion(Point p, Point q, Point r) 
    	{ 
   		 	int val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y); 
      
        	if (val == 0) return 0;  //Colineales
        	
       	 	return (val > 0)? 1: 2;  //reloj o en sentido contrario a las agujas del reloj
    	} 
	
		
	    void convexHull(int n) 
	    { 
	        
	        // Lista que nos servira para guardar los puntos convexos 
	        var hull = new List<Point>(); 
	      
	        // Encontramos el punto más a la izquierda (Valor 'X' Mas a la izquierda)
	        int l = 0; 
	        for (int i = 1; i < n; i++) {
	        	if (puntos[i].X > puntos[l].X){
	                l = i; 
	        	}
	        }
	        // Comenzamos desde el punto más a la izquierda y seguimos moviendonos.
			// en sentido antihorario hasta llegar al punto de inicio otra vez. 
			//Este bucle corre O (h) veces donde h es número de puntos en resultado o salida.
	        int p = l, q; 
	        do
	        { 

				// Añadimos el punto actual al resultado 
	            hull.Add(puntos[p]); 
	      
	            // Buscamos un punto 'q' tal que la orientación (p, x, q) es a la izquierda para todos los puntos 'x'. 
			    //La idea es mantener pista de los últimos visitados más contra reloj punto sabio en q. 
			    //Si algún punto 'i' es más en sentido contrario a las agujas del reloj que q, entonces se actualice q.
	            
			    q = (p + 1) % n;
	              
	            for (int i = 0; i < n; i++) { 
	            	// Si i es más a la izquierda que el valor actual q, entonces actualizamos q
	            	if (orientacion(puntos[p], puntos[i], puntos[q])  == 2)  q = i; 
	            } 
	      
	            // Ahora q es lo más contrario a las manecillas del reloj con respecto a la p. 
				// Establecemos p como q para la siguiente iteración, para que q se agregue a la lista hull.
	            p = q; 
	      
	        } while (p != l); //Mientras no vengamos de nuevo al primer punto.
	        
	        
	        
	        // Dibujar Resultado:
	        
	        Graphics g = this.CreateGraphics();
        	var lapiz = new SolidBrush(Color.Red);
	        
	        for(int i = 0; i < hull.Count;i++){

				g.FillEllipse(lapiz,new RectangleF(hull[i].X,hull[i].Y,10,10));
				if(i == hull.Count-1) dibujar_linea(hull[i].X,hull[i].Y,hull[0].X,hull[0].Y);
					else dibujar_linea(hull[i].X,hull[i].Y,hull[i+1].X,hull[i+1].Y);

	        }
	        
    		lapiz.Dispose();
			g.Dispose();
			hull.Clear();
			ya_genero = true;
			
	    }
	    
		void BtnGenerarClick(object sender, EventArgs e)
		{
			if(!ya_genero){
				if(puntos.Count > 2) convexHull(puntos.Count);
				
				else MessageBox.Show("ERROR, SOLO HAY DOS PUNTOS INGRESADOS, DEBE DE SER POR LO MENOS 3!","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Warning);
			}
			else{
				MessageBox.Show("ERROR, LIMPIE PRIMERO EL RESULTADO OBTENIDO","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Warning);
			}
			
		} 
		
	}
}
