using System.Diagnostics;

namespace TaskFactory_Demo
{
    class Program
    {
        static async Task Main()
        {
            Console.WriteLine("=== Inicio del Programa ===\n");
          
            Stopwatch sw = Stopwatch.StartNew();
            Console.WriteLine($"[Principal] Ejecutándose en hilo: {Environment.CurrentManagedThreadId}\n");
            // Crear y ejecutar tarea con opciones avanzadas
            //Task task = Task.Factory.StartNew(() =>
            //{
            //    Console.WriteLine($"[Tarea] Iniciada en hilo: {Environment.CurrentManagedThreadId}");
            //    Console.WriteLine($"[Tarea] Esta es una tarea de larga duración\n");
            //    for (int i = 0; i <= 10; i++)
            //    {
            //        Console.WriteLine($"[Tarea] Iteración: {i}");
            //        Task.Delay(200).Wait(); // Simula trabajo pesado
            //    }
            //    Console.WriteLine("\n[Tarea] Finalizada correctamente");
            //}, TaskCreationOptions.LongRunning);

            Task[] tasks = [
                 Task.Factory.StartNew(() => Sum(500_000), TaskCreationOptions.LongRunning),
                 Task.Factory.StartNew(() => getData(), TaskCreationOptions.LongRunning),
                 Task.Factory.StartNew(() => ProcessData(), TaskCreationOptions.LongRunning),
                 Task.Factory.StartNew(() => {
                     Console.WriteLine($"[Task Anonymous] Ejecutándose en hilo: {Environment.CurrentManagedThreadId}");
                        for(int i = 0; i <= 100; i += 10){
                         if(i % 20 == 0) Console.WriteLine($"[Task Anonymous] Progreso: {i}%");
                     }
                     Console.WriteLine($"[Task Anonymous] Finalizada.");
                 })
            ];


            Console.WriteLine("[Principal] Tarea en ejecución...\n");

            // IMPORTANTE: Esperar a que la tarea termine
            await Task.WhenAll(tasks);

            Console.WriteLine("\n=== Todas las tareas han completado ===");

            Console.WriteLine("=== Fin del Programa ===\n");
            sw.Stop();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Tiempo trancurrido: {sw.ElapsedMilliseconds}ms");
            Console.ResetColor();
            /*
             ¿Se muestra la última línea del código original? (Sí/No y por qué)
              
                Entiendo que por ultima linea del codigo original se refiere a lo siguiente:
                         Console.WriteLine("=== Fin del Programa ===\n");
                
                Siempre se muestra esa linea solo si el hilo principal sigue vivo hasta ese punto
                porque el 'await task;' en 'async Task Main()' asegura que el hilo principal espere
                a que la tarea termine antes de continuar. Asi el mensaje "=== Fin del Programa ===" solo
                aparece despues de que la tarea ha finalizado.

                Pero si no se espera la tarea el hilo principal no espera y el programa termina antes.
                Y puede que tengamos algo asi:

                === Inicio del Programa ===

                [Principal] Ejecutándose en hilo: 1

                [Principal] Tarea en ejecución...

                === Tarea completada ===
                === Fin del Programa ===

                [Tarea] Iniciada en hilo: 11
                [Tarea] Esta es una tarea de larga duración

                [Tarea] Iteración: 0

             ¿El mensaje "Tarea completada" es correcto? Explica el problema

                El mensaje "Tarea completada" no es correcto si se coloca antes de que la tarea realmente
                haya finalizado. En el código original, si no se espera a que la tarea termine (es decir,
                si no se usa 'await task;'), el hilo principal puede llegar a imprimir "=== Tarea completada ==="
                antes de que la tarea haya terminado su ejecución. Esto puede llevar a confusión, ya que el
                mensaje sugiere que la tarea ha finalizado cuando en realidad aún está en progreso.
                Al usar 'await task;', se garantiza que el hilo principal espere hasta que la tarea haya
                completado su ejecución antes de continuar, asegurando así que el mensaje "=== Tarea completada ==="
                sea preciso y refleje el estado real de la tarea.
             

             ¿Qué sucede si la tarea tarda 5 segundos? Describe el comportamiento
                Si la tarea tarda 5 segundos en completarse, el comportamiento del programa dependerá de si
                se utiliza 'await task;' o no.
                1. Con 'await task;':
                   - El hilo principal se bloqueará y esperará a que la tarea termine su ejecución.
                   - Durante esos 5 segundos, el programa no avanzará más allá del punto donde se espera la tarea.
                   - Una vez que la tarea haya finalizado, el hilo principal continuará y se imprimirá "=== Tarea completada ==="
                     seguido de "=== Fin del Programa ===".
                2. Sin 'await task;':
                   - El hilo principal no esperará a que la tarea termine y continuará ejecutándose inmediatamente.
                   - Esto significa que "=== Tarea completada ===" y "=== Fin del Programa ===" se imprimirán casi instantáneamente,
                     sin importar cuánto tiempo tarde la tarea en completarse.
                   - La tarea seguirá ejecutándose en segundo plano, pero el programa principal puede terminar antes de que la tarea
                     haya finalizado, lo que puede llevar a una salida incompleta o confusa.
               
             ¿Qué diferencia hay entre usar async Task Main() sin await vs void Main()?
                La diferencia principal entre usar 'async Task Main()' sin 'await' y 'void Main()' radica en cómo se maneja la asincronía y el flujo de ejecución del programa.
                1. async Task Main() sin await:
                   - Permite que el método Main sea asincrónico, lo que significa que puede contener operaciones asincrónicas.
                   - Sin embargo, si no se utiliza 'await' dentro de este método, el hilo principal no esperará a que las tareas asincrónicas se completen antes de continuar.
                   - Esto puede llevar a que el programa termine antes de que las tareas hayan finalizado, lo que puede resultar en una salida incompleta o confusa.
                   - Además, al ser un método 'Task', permite un mejor manejo de excepciones y la posibilidad de devolver un estado de finalización.
                2. void Main():
                   - Es un método síncrono tradicional que no puede contener operaciones asincrónicas directamente.
                   - El hilo principal ejecutará todo el código secuencialmente y no podrá esperar a que las tareas asincrónicas se completen.
                   - Si se intenta iniciar una tarea asincrónica dentro de 'void Main()', esta tarea se ejecutará en segundo plano, pero el programa principal puede terminar antes de que la tarea haya finalizado.
                   - No permite un manejo adecuado de excepciones para tareas asincrónicas y no puede devolver un estado de finalización.
             */


        }

        static void Sum(int n)
        {
            Console.WriteLine($"[Task Sum] Ejecutándose en hilo: {Environment.CurrentManagedThreadId}");

            long sum = 0;
            for (int i = 1; i <= n; i++)
            {
                sum += i;
                if (i % (n / 10) == 0)
                {
                    Task.Delay(200).Wait();
                    Console.WriteLine($"[Task Sum] Progreso: {i * 100 / n}%");
                }
            }
            Console.WriteLine($"[Task Sum] Finalizada");
            Console.WriteLine($"[Task Sum] Resultado: {sum}");
        }

        static void getData()
        {
            Console.WriteLine($"[Task getData] Ejecutándose en hilo: {Environment.CurrentManagedThreadId}");
            for (int i = 0; i <= 5; i++)
            {
                Console.WriteLine($"[Task getData] Obteniendo datos... {i}");
                Thread.Sleep(500);
            }
            Console.WriteLine($"[Task getData] Finalizada");
        }

        static void ProcessData()
        {
            Console.WriteLine($"[Task ProcessData] Ejecutándose en hilo: {Environment.CurrentManagedThreadId}");
            for (int i = 0; i <= 5; i++)
            {
                Console.WriteLine($"[Task ProcessData] Procesando datos... {i}");
                Task.Delay(500).Wait();
            }
            Console.WriteLine($"[Task ProcessData] Finalizada");
        }
    }
}