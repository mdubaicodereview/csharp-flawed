using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Text.Json; // Added for potential future serialization

namespace csharp_flawed
{
    class Program
    {
        static List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();
        static string pwd = "admin123"; // Hardcoded password
        
        // Predefined categories and tags - intentionally using different naming conventions
        static List<string> Categories = new List<string>() { "Work", "Personal", "Shopping", "Health", "Finance" };
        static List<string> tags = new List<string>() { "urgent", "important", "can-wait", "delegated", "in-progress" };
        
        static void Main(string[] args)
        {
            // Initialize with some data
            var t1 = new Dictionary<string, object>();
            t1.Add("id", 1);
            t1.Add("title", "Buy milk");
            t1.Add("done", false);
            t1.Add("date", DateTime.Now.AddDays(-1));
            t1.Add("category", "Shopping"); // Added category
            t1.Add("tags", new List<string>() { "urgent" }); // Added tags
            data.Add(t1);
            
            var t2 = new Dictionary<string, object>();
            t2.Add("id", 2);
            t2.Add("title", "Call mom");
            t2.Add("done", false);
            t2.Add("date", DateTime.Now.AddDays(-2));
            t2.Add("category", "Personal"); // Added category
            t2.Add("tags", new List<string>() { "important" }); // Added tags
            data.Add(t2);
            
            var t3 = new Dictionary<string, object>();
            t3.Add("id", 3);
            t3.Add("title", "Finish report");
            t3.Add("done", true);
            t3.Add("date", DateTime.Now.AddDays(-3));
            t3.Add("category", "Work"); // Added category
            t3.Add("tags", new List<string>() { "urgent", "important" }); // Added tags
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
                        string category = item.ContainsKey("category") ? (string)item["category"] : "None";
                        var tags = item.ContainsKey("tags") ? (List<string>)item["tags"] : new List<string>();
                        string tagDisplay = tags.Count > 0 ? $"[{string.Join(", ", tags)}]" : "";
                        Console.WriteLine($"{item["id"]}. {status} {item["title"]} ({item["date"]}) - {category} {tagDisplay}");
                    }
                }
                
                Console.WriteLine("\n1. Add todo");
                Console.WriteLine("2. Mark as done");
                Console.WriteLine("3. Delete todo");
                Console.WriteLine("4. Filter by category"); // Added option
                Console.WriteLine("5. Exit");
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
                        FilterByCategory(); // Added case
                        break;
                    case 5:
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
            
            // Get category - intentionally using a different variable name style
            Console.WriteLine("\nAvailable Categories:");
            for (int i = 0; i < Categories.Count; i++) {
                Console.WriteLine($"{i+1}. {Categories[i]}");
            }
            Console.Write("Select category number (or 0 for none): ");
            int cat_choice = Convert.ToInt32(Console.ReadLine()); // Intentionally using snake_case
            
            // Intentional flaw: No bounds checking
            if (cat_choice > 0) {
                newTodo.Add("category", Categories[cat_choice-1]);
            }
            
            // Get tags - intentional flaw: using different naming convention
            Console.WriteLine("\nAvailable Tags:");
            for (int i = 0; i < tags.Count; i++) {
                Console.WriteLine($"{i+1}. {tags[i]}");
            }
            Console.Write("Enter tag numbers separated by commas (or 0 for none): ");
            string TagInput = Console.ReadLine(); // Intentionally using PascalCase
            
            if (!string.IsNullOrEmpty(TagInput) && TagInput != "0") {
                try {
                    // Intentional flaw: No error handling for invalid input format
                    var selectedTags = new List<string>();
                    foreach (var tagIndex in TagInput.Split(',')) {
                        int idx = int.Parse(tagIndex.Trim());
                        // Intentional flaw: No bounds checking
                        if (idx > 0) {
                            selectedTags.Add(tags[idx-1]);
                        }
                    }
                    newTodo.Add("tags", selectedTags);
                } catch {
                    // Empty catch - swallows all exceptions
                    // Intentional flaw: No feedback to user about error
                    newTodo.Add("tags", new List<string>());
                }
            } else {
                newTodo.Add("tags", new List<string>());
            }
            
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
        
        // New method to filter by category - intentionally using a different style
        static void FilterByCategory() 
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║        FILTER BY CATEGORY        ║");
            Console.WriteLine("╚════════════════════════════════════╝");
            
            // Intentional flaw: Inefficient way to get unique categories
            var uniqueCategories = new List<string>();
            foreach (var item in data) {
                if (item.ContainsKey("category")) {
                    string cat = (string)item["category"];
                    if (!uniqueCategories.Contains(cat)) {
                        uniqueCategories.Add(cat);
                    }
                }
            }
            
            Console.WriteLine("\nAvailable Categories:");
            for (int i = 0; i < uniqueCategories.Count; i++) {
                Console.WriteLine($"{i+1}. {uniqueCategories[i]}");
            }
            Console.WriteLine($"{uniqueCategories.Count+1}. Show all");
            
            Console.Write("\nSelect category number: ");
            // Intentional flaw: No input validation
            int selection = Convert.ToInt32(Console.ReadLine());
            
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║           FILTERED TODOS         ║");
            Console.WriteLine("╚════════════════════════════════════╝\n");
            
            // Intentional flaw: Inefficient filtering
            if (selection <= uniqueCategories.Count) {
                string selectedCategory = uniqueCategories[selection-1];
                Console.WriteLine($"Category: {selectedCategory}\n");
                
                // Intentional flaw: Duplicate code from main display
                foreach (var item in data) {
                    if (item.ContainsKey("category") && (string)item["category"] == selectedCategory) {
                        string status = (bool)item["done"] ? "[X]" : "[ ]";
                        var tags = item.ContainsKey("tags") ? (List<string>)item["tags"] : new List<string>();
                        string tagDisplay = tags.Count > 0 ? $"[{string.Join(", ", tags)}]" : "";
                        Console.WriteLine($"{item["id"]}. {status} {item["title"]} ({item["date"]}) {tagDisplay}");
                    }
                }
            } else {
                Console.WriteLine("All Todos:\n");
                // Intentional flaw: More duplicate code
                foreach (var item in data) {
                    string status = (bool)item["done"] ? "[X]" : "[ ]";
                    string category = item.ContainsKey("category") ? (string)item["category"] : "None";
                    var tags = item.ContainsKey("tags") ? (List<string>)item["tags"] : new List<string>();
                    string tagDisplay = tags.Count > 0 ? $"[{string.Join(", ", tags)}]" : "";
                    Console.WriteLine($"{item["id"]}. {status} {item["title"]} ({item["date"]}) - {category} {tagDisplay}");
                }
            }
            
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}
