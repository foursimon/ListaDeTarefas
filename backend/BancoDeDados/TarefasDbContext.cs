using backend.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace backend.BancoDeDados
{
	public class TarefasDbContext : DbContext
	{
		public TarefasDbContext(DbContextOptions<TarefasDbContext> options) 
			: base(options) { }

		public virtual DbSet<Usuario> Usuarios { get; set; }
		public virtual DbSet<Tarefas> Tarefas { get; set; }
		public virtual DbSet<CheckItem> CheckItems { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Usuario>(builder =>
			{
				builder.ToTable("Usuario", tableBuilder =>
				{
					tableBuilder.HasCheckConstraint(
						"CK_QtdTarefas_MaximoExcedido",
						sql: $"{nameof(Usuario.QuantidadeTarefa)} < 11 && {nameof(Usuario.QuantidadeTarefa)} >= 0");
				});
				builder.HasKey(p => p.Id);
				builder.Property(p => p.Nome).HasColumnName("nome")
					.HasMaxLength(100).IsRequired();
				builder.Property(p => p.Email).HasColumnName("email")
					.HasMaxLength(255).IsRequired();
				builder.Property(p => p.Senha).HasColumnName("senha")
					.HasMaxLength(255).IsRequired();
				builder.Property(p => p.QuantidadeTarefa).HasColumnName("quantidade_tarefas")
				;
				builder.Property(p => p.TempoToken).HasColumnName("tempo_token")
				.HasMaxLength(255);
				builder.Property(p => p.TokenRecarga).HasColumnName("token_recarga").HasMaxLength(255);
				builder.HasIndex(p => p.Email).IsUnique();
				builder.HasMany(p => p.Tarefa).WithOne(p => p.Usuario);
			});

			modelBuilder.Entity<Tarefas>(builder =>
			{
				builder.ToTable("Tarefas");
				builder.HasKey(p => p.Id);
				builder.Property(p => p.Titulo).HasColumnName("titulo")
					.HasMaxLength(50).IsRequired();
				builder.Property(p => p.Concluido).HasColumnName("concluido");
				builder.Property(p => p.DataDeEncerramento)
					.HasColumnName("data_de_encerramento");
				builder.Property(p => p.Descricao).HasColumnName("descricao")
					.HasMaxLength(500);
				builder.Property(p => p.Tipo).HasColumnName("tipo")
					.HasMaxLength(50);
				builder.Property(p => p.IdUsuario).HasColumnName("id_usuario");
				builder.HasOne(p => p.Usuario)
					.WithMany(p => p.Tarefa).HasForeignKey(p => p.IdUsuario);
				builder.HasMany(p => p.Itens)
					.WithOne(p => p.Tarefa);
			});

			modelBuilder.Entity<CheckItem>(builder =>
			{
				builder.ToTable("CheckItem");
				builder.HasKey(p => p.Id);
				builder.Property(p => p.Item).HasColumnName("item")
					.HasMaxLength(255).IsRequired();
				builder.Property(p => p.Concluido).HasColumnName("concluido");
				builder.Property(p => p.IdTarefa).HasColumnName("id_tarefa");
				builder.HasOne(p => p.Tarefa)
					.WithMany(p => p.Itens).HasForeignKey(p => p.IdTarefa);
			});
		}
	}
}
