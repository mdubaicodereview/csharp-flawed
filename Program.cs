using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace csharp_flawed
{
    class Program
    {
        static List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        static string pwd = "admin123"; // Hardcoded password
        
        static void Main(string[] args)
        {
            // Initialize with some data
            var t1 = new Dictionary<string, object>();
            t1.Add("id", 1);
            t1.Add("title", "Buy milk");
            t1.Add("done", false);
            t1.Add("date", DateTime.Now.AddDays(-1));
            data.Add(t1);
            
            var t2 = new Dictionary<string, object>();
            t2.Add("id", 2);
            t2.Add("title", "Call mom");
            t2.Add("done", false);
            t2.Add("date", DateTime.Now.AddDays(-2));
            data.Add(t2);
            
            var t3 = new Dictionary<string, object>();
            t3.Add("id", 3);
            t3.Add("title", "Finish report");
            t3.Add("done", true);
            t3.Add("date", DateTime.Now.AddDays(-3));
            data.Add(t3);
            
            bool running = true;
            while (running) {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════╗");
                Console.WriteLine("║           TODO MANAGER            ║");
                Console.WriteLine("╚════════════════════════════════════╝");
                Console.WriteLine();
                
                // Inefficient way to display todos - rebuilds the entire list every time
                for (int i = 0; i < 100; i++) { // Unnecessary loop
                    if (i < data.Count) {
                        var item = data[i];
                        string status = (bool)item["done"] ? "[X]" : "[ ]";
                        Console.WriteLine($"{item["id"]}. {status} {item["title"]} ({item["date"]})");
                    }
                }
                
                Console.WriteLine("\n1. Add todo");
                Console.WriteLine("2. Mark as done");
                Console.WriteLine("3. Delete todo");
                Console.WriteLine("4. Exit");
                Console.Write("\nChoice: ");
                
                // No input validation
                int choice = Convert.ToInt32(Console.ReadLine());
                
                switch(choice) {
                    case 1:
                        AddTodo();
                        break;
                    case 2:
                        MarkAsDone();
                        break;
                    case 3:
                        DeleteTodo();
                        break;
                    case 4:
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        Thread.Sleep(1000); // Blocking sleep
                        break;
                }
            }
        }
        
        // Poorly named method
        static void AddTodo() {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║           ADD NEW TODO            ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            
            Console.Write("Enter todo title: ");
            string title = Console.ReadLine();
            
            // Redundant code - could be a method
            var newTodo = new Dictionary<string, object>();
            newTodo.Add("id", data.Count > 0 ? (int)data.Max(t => t["id"]) + 1 : 1); // Inefficient ID generation
            newTodo.Add("title", title);
            newTodo.Add("done", false);
            newTodo.Add("date", DateTime.Now);
            
            // No validation
            data.Add(newTodo);
            
            Console.WriteLine("Todo added successfully!");
            Thread.Sleep(1000); // Blocking sleep
        }
        
        // Different naming convention
        static void MarkAsDone()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║          MARK AS COMPLETE         ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            
            Console.Write("Enter todo ID: ");
            try {
                int id = int.Parse(Console.ReadLine());
                
                // Inefficient search
                bool found = false;
                foreach (var item in data) {
                    if ((int)item["id"] == id) {
                        item["done"] = true;
                        found = true;
                        Console.WriteLine("Todo marked as done!");
                        break;
                    }
                }
                
                if (!found) {
                    Console.WriteLine("Todo not found!");
                }
            } catch {
                // Empty catch block - swallows all exceptions
            }
            
            Thread.Sleep(1000); // Blocking sleep
        }
        
        // Different formatting style
        static void DeleteTodo() 
        { 
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║           DELETE TODO             ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            
            // Security issue - password check with hardcoded password
            Console.Write("Enter admin password: ");
            string input = Console.ReadLine();
            
            if (input != pwd) {
                Console.WriteLine("Wrong password!");
                Thread.Sleep(1000);
                return;
            }
            
            Console.Write("Enter todo ID to delete: ");
            int id = Convert.ToInt32(Console.ReadLine()); // No error handling
            
            // Memory inefficient - creates a new list each time
            data = data.Where(t => (int)t["id"] != id).ToList();
            
            Console.WriteLine("Todo deleted if it existed!"); // Misleading message
            Thread.Sleep(1000);
        }
    }
}
