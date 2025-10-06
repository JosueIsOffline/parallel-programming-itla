using System;
using System.Diagnostics;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace Task_Run_Demo
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("=== Inicio del programa (Hilo Principal) ===\n");
            Stopwatch sw = Stopwatch.StartNew();

            Task<BigInteger> t1 = Task.Run(() => SumaImpares(2_000_000));
            Task<BigInteger> t2 = Task.Run(() => SumaCubos(1_500_000));
            Task<BigInteger> t3 = Task.Run(() => Potencia(2, 1_000_000));

            Console.WriteLine("[Hilo Principal] Las 3 tareas están en ejecución...\n");
            await Task.Delay(500);
            Console.WriteLine("[Hilo Principal] Continuando con otras operaciones...\n");


            BigInteger[] result = await Task.WhenAll(t1, t2, t3);
          
            Console.WriteLine("\n=== Resultados ===");
            Console.WriteLine($"[Hilo Principal] Suma de impares: {result[0]}");
            Console.WriteLine($"[Hilo Principal] Suma de cubos: {result[1]}");
            Console.WriteLine($"[Hilo Principal] Potencia 2^1000000: {result[2].ToString().Substring(0, 50)}... ({result[2].ToString().Length} dígitos)");

            Console.WriteLine("=== Fin del programa ===\n");
            sw.Stop();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Tiempo total transcurrido: {sw.ElapsedMilliseconds} ms");
            Console.ResetColor();
        }

        static BigInteger SumaImpares(int n)
        {
            Console.WriteLine($"[Task SumaImpares] Iniciada en hilo: {Thread.CurrentThread.ManagedThreadId}");
            BigInteger res = 0;
            for (int i = 1; i <= n; i += 2)
            {
                res += i;
                if (i % 500000 == 1)
                {
                    Console.WriteLine($"[Task SumaImpares] Progreso: {i}/{n}");
                    Task.Delay(50).Wait();
                }
            }
            Console.WriteLine($"[Task SumaImpares] Finalizada - Resultado: {res}\n");
            return res;
        }

        static BigInteger SumaCubos(int n)
        {
            Console.WriteLine($"[Task SumaCubos] Iniciada en hilo: {Thread.CurrentThread.ManagedThreadId}");
            BigInteger res = 0;
            for (int i = 1; i <= n; i++)
            {
                res += (BigInteger)i * i * i;
                if (i % 500000 == 0)
                {
                    Console.WriteLine($"[Task SumaCubos] Progreso: {i}/{n}");
                    Task.Delay(50).Wait();
                }
            }
            Console.WriteLine($"[Task SumaCubos] Finalizada - Resultado: {res}\n");
            return res;
        }

        static BigInteger Potencia(int baseNum, int exponente)
        {
            Console.WriteLine($"[Task Potencia] Iniciada en hilo: {Thread.CurrentThread.ManagedThreadId}");
            BigInteger res = 1;
            for (int i = 0; i < exponente; i++)
            {
                res *= baseNum;
                if (i % 200000 == 0)
                {
                    Console.WriteLine($"[Task Potencia] Progreso: {i}/{exponente}");
                    Task.Delay(50).Wait();
                }
            }
            Console.WriteLine($"[Task Potencia] Finalizada\n");
            return res;
        }
    }
}
