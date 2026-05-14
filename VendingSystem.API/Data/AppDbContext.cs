using Microsoft.EntityFrameworkCore;
using VendingSystem.API.Models;

namespace VendingSystem.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<VendingMachine> VendingMachines => Set<VendingMachine>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Sale> Sales => Set<Sale>();
        public DbSet<MaintenanceRecord> MaintenanceRecords => Set<MaintenanceRecord>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- VendingMachines Constraints ---

            // 1.e, 1.f: Уникальность серийного и инвентарного номеров
            modelBuilder.Entity<VendingMachine>()
                .HasIndex(m => m.SerialNumber).IsUnique();
            modelBuilder.Entity<VendingMachine>()
                .HasIndex(m => m.InventoryNumber).IsUnique();

            // 1.l: Ресурс ТА > 0
            modelBuilder.Entity<VendingMachine>()
                .HasCheckConstraint("CK_ResourceHours_Positive", "\"resource_hours\" > 0");

            // 1.n: Время обслуживания от 1 до 20 часов
            modelBuilder.Entity<VendingMachine>()
                .HasCheckConstraint("CK_MaintenanceTime_Range",
                    "\"maintenance_time_hours\" >= 1 AND \"maintenance_time_hours\" <= 20");

            // 1.o: Статус из списка
            modelBuilder.Entity<VendingMachine>()
                .HasCheckConstraint("CK_Machine_Status",
                    "\"machine_status\" IN ('Работает', 'Вышел из строя', 'В ремонте/на обслуживании')");

            // 1.c: Тип оплаты из списка (опционально, но рекомендуется)
            modelBuilder.Entity<VendingMachine>()
                .HasCheckConstraint("CK_Payment_Type",
                    "\"payment_type\" IN ('с оплатой картой', 'с оплатой наличными', 'два вида оплаты')");

            // 1.i: Дата ввода в эксплуатацию >= даты изготовления
            modelBuilder.Entity<VendingMachine>()
                .HasCheckConstraint("CK_Commissioning_Date",
                    "\"date_of_commissioning\" >= \"manufacture_date\"");

            // --- Users Constraints ---

            // 1.aj: Роль из списка
            modelBuilder.Entity<User>()
                .HasCheckConstraint("CK_User_Role",
                    "\"role\" IN ('Администратор', 'Оператор')");

            // --- Sales Constraints ---

            // 1.af: Метод оплаты из списка
            modelBuilder.Entity<Sale>()
                .HasCheckConstraint("CK_Sale_Payment_Type",
                    "\"payment_type\" IN ('Карта', 'Наличные', 'QR-код')");

            // 1.ac: Количество > 0
            modelBuilder.Entity<Sale>()
                .HasCheckConstraint("CK_Sale_Quantity", "\"quantity\" > 0");

            // --- Связи (Foreign Keys) ---

            modelBuilder.Entity<MaintenanceRecord>()
                .HasOne(m => m.Machine)
                .WithMany()
                .HasForeignKey(m => m.MachineId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MaintenanceRecord>()
                .HasOne(m => m.DoneByUserNavigation)
                .WithMany()
                .HasForeignKey(m => m.DoneByUserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Machine)
                .WithMany()
                .HasForeignKey(s => s.MachineId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Sale>()
                .HasOne(s => s.Product)
                .WithMany()
                .HasForeignKey(s => s.ProductId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}