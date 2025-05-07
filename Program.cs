using MySql.Data.MySqlClient;
using System;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class Database
{
    private string connectionString;

    public Database(string connectionString)
    {
        this.connectionString = connectionString;
    }


    // CREATE User
    public void CreateUser(string username, string email, string password)
    {
        string query = "INSERT INTO users (username, email, password, created_at) VALUES (@username, @email, @password, @created_at)";
        using (MySqlConnection con = new MySqlConnection(connectionString))
        {
            con.Open();
            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@created_at", DateTime.Now);
                
                cmd.ExecuteNonQuery();
            }
        }    
    }

    // Read User
    public User ReadUser(int id)
    {
        string query = "SELECT * FROM users WHERE id = @id";

        using (MySqlConnection con = new MySqlConnection(connectionString))
        {
            con.Open();
            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@id", id);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new User
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Username = reader["username"].ToString(),
                            Email = reader["email"].ToString(),
                            CreatedAt = Convert.ToDateTime(reader["created_at"])
                        };
                    }
                }
            }
        }
        return null;
    }
    
    // Read all Users
    public List<User> ReadUsers()
    {
        string query = "SELECT * FROM users";
        List<User> users = new List<User>();

        using (MySqlConnection con = new MySqlConnection(connectionString))
        {
            con.Open();
            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Username = reader["username"].ToString(),
                            Email = reader["email"].ToString(),
                            CreatedAt = Convert.ToDateTime(reader["created_at"])
                        });
                    }
                }
            }
        }
        return users;
    }
    // Update User
    public void UpdateUser(int id,string username, string email, string password)
    {
        string query = "UPDATE users SET username = @username, email = @email, password = @password where id = @id";

        using (MySqlConnection con = new MySqlConnection(connectionString))
        {
            con.Open();

            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }

    // Delete User
    public void DeleteUser(int id)
    {
        string query = "DELETE FROM users WHERE id = @id";

        using (MySqlConnection con = new MySqlConnection(connectionString))
        {
            con.Open();
            using (MySqlCommand cmd = new MySqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
        }
    }
}

public class Program
{
    public static void Main()
    {
        string connectionString = "Server=localhost;Database=console;User ID=root;Password=123456;";

        Database db = new Database(connectionString);

        while (true)
        {
            Console.WriteLine("CRUD MENU");
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. Read User");
            Console.WriteLine("3. Read All User");
            Console.WriteLine("4. Update User");
            Console.WriteLine("5. Delete User");
            Console.WriteLine("6. Exit");
            Console.Write("Select an option: ");

            int choice = Convert.ToInt32(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    Console.WriteLine("1. Create User");
                    Console.WriteLine("Enter User Name:");
                    string username = Console.ReadLine();
                    Console.WriteLine("Enter Email:");
                    string email = Console.ReadLine();
                    Console.WriteLine("Enter Password:");
                    string password = Console.ReadLine();
                    db.CreateUser(username, email, password);
                    Console.WriteLine("User created successfully.");
                    break;
                case 2:
                    Console.WriteLine("2. Read Users");
                    Console.WriteLine("Enter Id User: ");
                    int id = Convert.ToInt32(Console.ReadLine());
                    var user = db.ReadUser(id);
                    if (user != null)
                    {
                        Console.WriteLine($"ID: {user.Id}, Username: {user.Username}, Email: {user.Email}, CreatedAt: {user.CreatedAt}");
                    } else
                    {
                        Console.WriteLine("User not found.");
                    }
                    break;
                case 3:
                    var users = db.ReadUsers();
                    if (users.Count == 0)
                    {
                        Console.WriteLine("No users found.");
                    }
                    
                    if (users != null)
                    {
                        foreach (var u in users)
                        {
                            Console.WriteLine($"\nID: {u.Id}\nUsername: {u.Username}\nEmail: {u.Email}\nCreated At: {u.CreatedAt}");
                        }
                    }
                    break;
                case 4:
                    Console.WriteLine("4. Update User");
                    Console.WriteLine("Enter Id User: ");
                    int Id = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("Enter User Name:");
                    string Username = Console.ReadLine();
                    Console.WriteLine("Enter Email:");
                    string Email = Console.ReadLine();
                    Console.WriteLine("Enter Password:");
                    string Password = Console.ReadLine();
                    db.UpdateUser(Id, Username, Email, Password);
                    break;
                case 5:
                    Console.WriteLine("5. Delete User");
                     Id = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine("User deleted successfully.");
                    db.DeleteUser(Id);
                    break;
                case 6:
                    Console.WriteLine("Exiting program.");
                    return;
                default:
                    Console.WriteLine("Invalid input.Please choose from 1 to 6");
                    break;
            }
        }
    }
}
