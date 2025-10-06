using System.Diagnostics;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Inicio del Programa ===\n");

        Stopwatch sw = Stopwatch.StartNew();
        Task<int> hornear = Task.Run(() => HornearGalletas(24));

        Task prepararEmpaque = Task.Factory.StartNew(() => PrepararEmpaque(2), TaskCreationOptions.AttachedToParent);


        Task enfriar = hornear.ContinueWith((t) => EnfriarGalletas(2));
        await enfriar;

        Task empacar = Task.WhenAll(hornear, prepararEmpaque).ContinueWith((t) => EmpacarGalletas(1), TaskContinuationOptions.OnlyOnRanToCompletion);
        await empacar;

        Task enviar = empacar.ContinueWith((t) => EnviarATienda(1));
        await enviar;

        Console.WriteLine("\n=== Todas las tareas han completado ===");

        Console.WriteLine("=== Fin del Programa ===\n");
        sw.Stop();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Tiempo transcurrido: {sw.ElapsedMilliseconds}ms");
        Console.ResetColor();
    }

    static int HornearGalletas(int n)
    {
        Console.WriteLine($"[Task HonearGalletas] Iniciada en el hilo {Environment.CurrentManagedThreadId}");
        Console.WriteLine($"[Task HonearGalletas] Horneado {n} galletas");
        
       
        for (int i = 3; i > 0; i--) {
            Console.WriteLine($"[Task HonearGalletas] Listas en... {i}");
            Thread.Sleep(1000);
        }

        Console.WriteLine("[Task HonearGalletas] Finalizada");
        return n;
    }

    static void PrepararEmpaque(int s)
    {
        Console.WriteLine($"[Task PrepararEmpaque] Iniciada en el hilo {Environment.CurrentManagedThreadId}");
        for (int i = s; i > 0; i--) {
            Console.WriteLine($"[Task PrepararEmpaque] Empacadas en... {i}");
            Thread.Sleep(1000);
        }
        Console.WriteLine($"[Task PrepararEmpaque] Finalizada");
    }

    static void EnfriarGalletas(int s)
    {
        Console.WriteLine($"[Task EnfriarGalletas] Iniciada en el hilo {Environment.CurrentManagedThreadId}");
        for (int i = s; i > 0; i--) {
            Console.WriteLine($"[Task EnfriarGalletas] Enfriadas en... {i}");
            Thread.Sleep(1000);
        }
        Console.WriteLine($"[Task EnfriarGalletas] Finalizada");
    }

    static void EmpacarGalletas(int s)
    {
        Console.WriteLine($"[Task EmpacarGalletas] Iniciada en el hilo {Environment.CurrentManagedThreadId}");
        for (int i = s; i > 0; i--) {
            Console.WriteLine($"[Task EmpacarGalletas] Empacadas en... {i}");
            Thread.Sleep(1000);
        }
        Console.WriteLine($"[Task EmpacarGalletas] Finalizada");
    }

    static void EnviarATienda(int s)
    {
        Console.WriteLine($"[Task EnviarATienda] Iniciada en el hilo {Environment.CurrentManagedThreadId}");
        for (int i = s; i > 0; i--) {
            Console.WriteLine($"[Task EnviarATienda] Enviadas en... {i}");
            Thread.Sleep(1000);
        }
        Console.WriteLine($"[Task EnviarATienda] Finalizada. Ultima tarea de todo el proceso");
    }
}