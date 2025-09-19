using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GGApp;

public static class TestDataInitializer
{
    public static async Task InitializeTestDataAsync()
    {
        try
        {
            using var context = new GGContext();
            
            // Проверяем, есть ли уже тестовый пользователь
            var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Id == 1);
            
            if (existingUser == null)
            {
                // Создаем тестового пользователя
                var testUser = new User
                {
                    Id = 1,
                    Name = "Иван",
                    Surname = "Петров",
                    Email = "ivan.petrov@example.com",
                    Phone = "+7 (999) 123-45-67",
                    Login = "testuser",
                    Password = "password123",
                    RoleId = 1 // Предполагаем, что 1 - это роль студента
                };
                
                context.Users.Add(testUser);
                await context.SaveChangesAsync();
                
                Console.WriteLine("Тестовый пользователь создан успешно");
            }
            else
            {
                Console.WriteLine("Тестовый пользователь уже существует");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка инициализации тестовых данных: {ex.Message}");
        }
    }
}
