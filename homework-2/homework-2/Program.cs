using
System;
using System.Diagnostics;
using System.Numerics;
using
System.Threading.Tasks;

namespace Task_Constructor_Demo
{
    class Program
    {
        static async Task Main()
        {

            Console.WriteLine("=== Creación de Tareas con Constructor===\n");
            Stopwatch sw = Stopwatch.StartNew();

            // Crear la tarea (AÚN NO SE EJECUTA)
            //Task<long> t1 = new Task<long>(() => CalcularSumaDeCuadrados(23));
            //Task<long> t2 = new Task<long>(() => SumaDeNumeroPrimos(23));

            Task<BigInteger>[] tasks = new Task<BigInteger>[]
            {
                new (() => CalcularSumaDeCuadrados(100_000)),
                new (() =>  SumaDeNumeroPrimos(100_000)),
                new (() => SumaDeCubos(1000_000)),
                new (() => SerieFibonacci(100_000))
            };

            string[] methodsName = { "CalcularSumaDeCuadrados", "SumaDeNumeroPrimos", "SumaDeCubos", "SerieFibonnacci" };


            Console.WriteLine("[Principal] Tarea creada pero NO iniciada aún\n");

            // Aquí podemos hacer otras operaciones antes de iniciar

            Console.WriteLine("[Principal] Realizando configuraciones...");

            Console.WriteLine("[Principal] Preparando recursos...\n");

            // Ahora SÍ iniciamos la tarea

            Console.WriteLine("[Principal] Iniciando la tarea...");

            foreach(var task in tasks)
            {
                task.Start();
            }

            Task.WaitAll(tasks);

            Console.WriteLine("\n=== Resultados ===");
            BigInteger combinationResult = 0;
           for(int i = 0; i < tasks.Length; i++)
            {
                string res = tasks[i].Result.ToString();
                string showResult = res.Length > 50 ? $"{methodsName[i]}: {res[..50]}... {res.Length} digitos" :
                    $"{methodsName[i]}: {res}";
                Console.WriteLine(showResult);
                combinationResult += tasks[i].Result;
            }

            Console.WriteLine($"\nResultado combinado de todas las tareas: {combinationResult.ToString()[..50]}... {combinationResult.ToString().Length} digitos");

            Console.WriteLine("=== Fin del programa ===\n");
            sw.Stop();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Tiempo transcurrido: {sw.ElapsedMilliseconds} ms");
            Console.ResetColor();
        }

        static BigInteger CalcularSumaDeCuadrados(int n)
        {
            Console.WriteLine($"[Task CalcularSumaCuadrados] Iniciando Iniciada - Task ID: {Task.CurrentId}, Thread ID: {Environment.CurrentManagedThreadId}");
            BigInteger suma = 0;
            for (int i = 1; i <= n; i++)
            {
                suma += (BigInteger)i * i;
            }
            Console.WriteLine($"[Task CalcularSumaCuadrados] Completada - Task ID: {Task.CurrentId}, Thread ID: {Environment.CurrentManagedThreadId}");
            return suma;
        }

        static BigInteger SumaDeNumeroPrimos(int n)
        {
            Console.WriteLine($"[Task SumaDeNumeroPrimos] Iniciada - Task ID: {Task.CurrentId}, Thread ID: {Environment.CurrentManagedThreadId}");
            BigInteger suma = 0;
            for (int i = 2; i <= n; i++)
            {
                if (EsPrimo(i))
                {
                    suma += i;
                }
            }
            Console.WriteLine($"[Task SumaDeNumeroPrimos] Completada - Task ID: {Task.CurrentId}, Thread ID: {Environment.CurrentManagedThreadId}");
            return suma;
        }

        static BigInteger SumaDeCubos(int n)
        {
            Console.WriteLine($"[Task SumaDeCubos] Iniciada - Task ID: {Task.CurrentId}, Thread ID: {Environment.CurrentManagedThreadId}");
            BigInteger suma = 0;
            for (int i = 1; i <= n; i++)
            {
                suma += i * i * i;
            }
            Console.WriteLine($"[Task SumaDeCubos] Completada - Task ID: {Task.CurrentId}, Thread ID: {Environment.CurrentManagedThreadId}");
            return suma;
        }

        static BigInteger SerieFibonacci(int n)
        {
            Console.WriteLine($"[Task SerieFibonacci] Iniciada - Task ID: {Task.CurrentId}, Thread ID: {Environment.CurrentManagedThreadId}");
            if (n <= 0) return 0;
            if (n == 1) return 1;
            BigInteger a = 0, b = 1, fib = 0;
            for (int i = 2; i <= n; i++)
            {
                fib = a + b;
                a = b;
                b = fib;
            }
            Console.WriteLine($"[Task SerieFibonacci] Completada - Task ID: {Task.CurrentId}, Thread ID: {Environment.CurrentManagedThreadId}");
            return fib;

        }

        static bool EsPrimo(int n)
        {
            if (n < 2) return false;
            for (int i = 2; i <= Math.Sqrt(n); i++)
            {
                if (n % i == 0) return false;
            }
            return true;
        }
    }
}