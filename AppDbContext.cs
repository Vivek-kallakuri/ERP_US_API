using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using ChatSystem.Controllers;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Define a table for storing chat messages
    public DbSet<ChatMessage> ChatMessages { get; set; }
}
