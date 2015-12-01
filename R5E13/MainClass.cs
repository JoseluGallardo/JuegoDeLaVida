// //  Copyright (c) 2015 José Luis del Pino Gallardo
// //  Nombre: Juego De la Vida
// //  Versión: 1.0
// //  Fecha: 26/11/2015
// //  Comentario: Juego de la Vida terminado y comentado. Versión FINAL
// //  This program is free software: you can redistribute it and/or modify
// //  it under the terms of the GNU Lesser General Public License as published by
// //  the Free Software Foundation, either version 3 of the License, or
// //  (at your option) any later version.
// //
// //  This program is distributed in the hope that it will be useful,
// //  but WITHOUT ANY WARRANTY; without even the implied warranty of
// //  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// //  GNU Lesser General Public License for more details.
// //
// //  You should have received a copy of the GNU Lesser General Public License
// //  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.IO;

namespace JuegoDeLaVida
{
	class MainClass
	{
		const int ANCHO = 80; //Ancho de la Matriz
		const int ALTO = 24; //Alto de la Matriz
		static int total = 0; //Nº de Iteraciones Totales
		static byte[,] Master = new byte[ANCHO, ALTO]; //Array Principal, sobre el que se opera
		static byte[,] Slave = new byte[ANCHO, ALTO]; //Array Secundario, que se muestra

		static void Main(string[] args)
		{
			Interfaz (); //Llamamos a la Interfaz
			Rellenar1 (); //Rellenamos aleatoriamente por primera vez

			while (!Console.KeyAvailable)
			{
				Iterar ();
				Console.Title = "José Luis del Pino Gallardo - Juego de la Vida - Iteraciones: " + ++total;
				System.Threading.Thread.Sleep(100);
			}

			Console.ReadKey();
		}

		/// <summary>
		/// Cambiamos la interfaz de la consola
		/// </summary>
		public static void Interfaz()
		{
			Console.CursorVisible = false;
			Console.Title = "José Luis del Pino Gallardo - Juego de la Vida - Pulse Escape para salir. Iteraciones: " + total;
			Console.SetWindowSize (81, 24);
			Console.BackgroundColor = ConsoleColor.White;
			Console.ForegroundColor = ConsoleColor.Black;
			Console.Clear ();
		}

		/// <summary>
		/// Se rellena el array 'Master' por primera vez con números aleatorios.
		/// </summary>
		public static void Rellenar1 ()
		{
			Random rnd = new Random ();
			byte[] caracteres = new byte[2]{ 0, 1 };

			for (int i = 0; i < ANCHO; i++) {
				for (int j = 0; j < ALTO; j++) {
					Master [i, j] = caracteres [rnd.Next (2)];
				}
			}
		}

		/// <summary>
		/// Para cada celda contamos los vecinos, calculamos su vida y lo asignamos en la celda de "Slave"
		/// </summary>
		static void Iterar()
		{
			int nVecinos;

			for (int i = 0; i < ANCHO; i++)
			{
				for (int j = 0; j < ALTO; j++)
				{
					nVecinos = ContaVecinos(i, j);

					if (nVecinos < 2) //Un vecino o menos muere por soledad
						Slave[i, j] = 0;

					else if (nVecinos == 2) //Dos vecinos se mantiene vivo
						Slave[i, j] = Master[i, j];

					else if (nVecinos == 3) //Tres vecinos se mantiene vivo o nace uno nuevo
						Slave[i, j] = 1;

					else //Cuatro o más muere por exceso de población
						Slave[i, j] = 0;
				}
			}

			Array.Copy(Slave, Master, ANCHO * ALTO); //Copiamos el Array 'Slave' en 'Master' para operar luego sobre esa generación
			EscribirPantalla();
		}

		/// <summary>
		/// Contamos los Vecinos de la celda
		/// </summary>
		/// <returns>Número de vecinos VIVOS.</returns>
		/// <param name="posX">Position X de la celda.</param>
		/// <param name="posY">Position y de la celda.</param>
		static int ContaVecinos(int posX, int posY)
		{
			int nVecinos = 0;

			for (int i = posX - 1; i < posX + 2; i++) //Fila -1, Fila 0 y Fila 1
			{
				for (int j = posY - 1; j < posY + 2; j++) //Columna -1, Columna 0 y Columna 1
				{
					if ((i == posX) && (j == posY)) //No contar ella misma
						continue;

					if (Master[((i + ANCHO) % ANCHO), ((j + ALTO) % ALTO)] == 1) //Teoría del Módulo para contar las vecinas.
						nVecinos++;
				}
			}
			return nVecinos;
		}

		/// <summary>
		/// Escribimos por pantalla el Array 'Nuevo'
		/// 0 -> Muerta
		/// 1 -> Viva
		/// </summary>
		static void EscribirPantalla()
		{
			byte flag;
            Console.SetCursorPosition(0, 0);

			for (int i = 0; i < ALTO; ++i)
			{
				for (int j = 0; j < ANCHO; ++j)
				{
					flag = Master[j, i];
					if (flag == 0) //Si es un Cero escribimos un espacio
					{
						Console.Write(' ');
					}
					else if (flag == 1) //Si es un Uno un asterisco
					{

						Console.Write('*');
					}
					else //Si por lo que sea se le ha ido escribimos un espacio
					{
						Console.Write(' ');
					}
				}

				if (i < ALTO - 1) Console.Write("\n"); //Si no es la fila 23, cuando acabe de escribir que escriba un salto de linea
			}
		}
	}
}