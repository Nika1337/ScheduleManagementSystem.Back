

using Application.Abstractions;
using Application.DataTransferObjects.Workers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Data;
public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var scheduleService = scope.ServiceProvider.GetRequiredService<IScheduleService>();
        var workerService = scope.ServiceProvider.GetRequiredService<IWorkerService>();
        var jobService = scope.ServiceProvider.GetRequiredService<IJobService>();


        await SeedRolesAsync(roleManager);
        await SeedAdminUserAsync(userManager);
        await SeedWorkersAsync(workerService);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        string[] roles = ["Worker", "Admin"];

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    private static async Task SeedAdminUserAsync(UserManager<IdentityUser> userManager)
    {
        const string adminEmail = "admin@example.com";
        const string adminPassword = "Admin@123";

        if (await userManager.FindByEmailAsync(adminEmail) is null)
        {
            var adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail };
            var result = await userManager.CreateAsync(adminUser, adminPassword);

            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }

    private static async Task SeedWorkersAsync(IWorkerService workerService)
    {
        if ((await workerService.GetWorkersAsync()).Any()) return;

        var workerCreateRequests = new WorkerCreateRequest[]
        {
            new() {
                FirstName = "Worker",
                LastName = "Example",
                Email = "worker@example.com"
            },
            new() {
                FirstName = "Worker2",
                LastName = "Example",
                Email = "worker2@example.com"
            }
        };

        foreach (var workerCreateRequest in workerCreateRequests)
        {
            await workerService.CreateWorkerAsync(workerCreateRequest);
        }
    }
}