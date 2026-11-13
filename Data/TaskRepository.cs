using GUIchartBot.Models;
using GUIchartBot.Services;
using MySql.Data.MySqlClient;
using MySqlConnector;

namespace GUIchartBot.Data
{
    public class TaskRepository
    {
        // Add a new task - used by ChatbotEngine.HandleTaskReminder
        public void Add(TaskItem task)
        {
            using (var conn = new MySqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand(
                    "INSERT INTO tasks (title, description, reminderDate, isCompleted) VALUES (@t, @d, @r, @c)", conn);
                cmd.Parameters.AddWithValue("@t", task.Title);
                cmd.Parameters.AddWithValue("@d", task.Description ?? "");
                cmd.Parameters.AddWithValue("@r", (object)task.ReminderDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@c", task.IsCompleted ? 1 : 0);
                cmd.ExecuteNonQuery();
            }
        }

        // Get ALL tasks - used by Form1.RefreshTaskList() in tabTasks
        public List<TaskItem> GetAll()
        {
            var list = new List<TaskItem>();
            using (var conn = new MySqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT id, title, description, reminderDate, isCompleted FROM tasks ORDER BY id", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new TaskItem
                        {
                            Id = reader.GetInt32("id"),
                            Title = reader.GetString("title"),
                            Description = reader.IsDBNull(reader.GetOrdinal("description")) ? "" : reader.GetString("description"),
                            ReminderDate = reader.IsDBNull(reader.GetOrdinal("reminderDate")) ? (DateTime?)null : reader.GetDateTime("reminderDate"),
                            IsCompleted = reader.GetInt32("isCompleted") == 1
                        });
                    }
                }
            }
            return list;
        }

        // Delete a task - used by Form1.DeleteSelectedTask()
        public void Delete(int id)
        {
            using (var conn = new MySqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("DELETE FROM tasks WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            ActivityLogger.Log("Task deleted, id=" + id);
        }

        // Mark as done - used by Form1.CompleteSelectedTask()
        public void Complete(int id)
        {
            using (var conn = new MySqlConnection(DbConfig.ConnectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("UPDATE tasks SET isCompleted = 1 WHERE id = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            ActivityLogger.Log("Task completed, id=" + id);
        }
    }
}

