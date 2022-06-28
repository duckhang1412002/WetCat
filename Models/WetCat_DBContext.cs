using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WetCat.Models
{
    public partial class WetCat_DBContext : DbContext
    {
        public WetCat_DBContext()
        {
        }

        public WetCat_DBContext(DbContextOptions<WetCat_DBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Follow> Follows { get; set; }
        public virtual DbSet<Friend> Friends { get; set; }
        public virtual DbSet<Hobby> Hobbies { get; set; }
        public virtual DbSet<HobbyList> HobbyLists { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationList> NotificationLists { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<React> Reacts { get; set; }
        public virtual DbSet<ReactList> ReactLists { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Warning> Warnings { get; set; }
        public virtual DbSet<WarningList> WarningLists { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=(local);uid=sa;pwd=123456;database=WetCat_DB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AI");

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => new { e.CommentId, e.PostId })
                    .HasName("PK__comment__94780EF155F47AC8");

                entity.ToTable("comment");

                entity.Property(e => e.CommentId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("comment_id");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.CommentAuthor)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("comment_author");

                entity.Property(e => e.CommentTime)
                    .HasColumnType("date")
                    .HasColumnName("comment_time");

                entity.Property(e => e.Content)
                    .HasColumnType("text")
                    .HasColumnName("content");

                entity.Property(e => e.ParentId).HasColumnName("parent_id");

                entity.HasOne(d => d.CommentAuthorNavigation)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.CommentAuthor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__comment__comment__29572725");

                entity.HasOne(d => d.P)
                    .WithMany(p => p.InverseP)
                    .HasForeignKey(d => new { d.ParentId, d.PostId })
                    .HasConstraintName("FK__comment__2A4B4B5E");
            });

            modelBuilder.Entity<Follow>(entity =>
            {
                entity.HasKey(e => new { e.Username1, e.Username2 })
                    .HasName("PK__follow__A314E41540B548F5");

                entity.ToTable("follow");

                entity.Property(e => e.Username1)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("username1");

                entity.Property(e => e.Username2)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("username2");

                entity.HasOne(d => d.Username1Navigation)
                    .WithMany(p => p.FollowUsername1Navigations)
                    .HasForeignKey(d => d.Username1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__follow__username__4316F928");

                entity.HasOne(d => d.Username2Navigation)
                    .WithMany(p => p.FollowUsername2Navigations)
                    .HasForeignKey(d => d.Username2)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__follow__username__440B1D61");
            });

            modelBuilder.Entity<Friend>(entity =>
            {
                entity.HasKey(e => new { e.Username1, e.Username2 })
                    .HasName("PK__friend__A314E4151A5ADC4A");

                entity.ToTable("friend");

                entity.Property(e => e.Username1)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("username1");

                entity.Property(e => e.Username2)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("username2");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Username1Navigation)
                    .WithMany(p => p.FriendUsername1Navigations)
                    .HasForeignKey(d => d.Username1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__friend__username__46E78A0C");

                entity.HasOne(d => d.Username2Navigation)
                    .WithMany(p => p.FriendUsername2Navigations)
                    .HasForeignKey(d => d.Username2)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__friend__username__47DBAE45");
            });

            modelBuilder.Entity<Hobby>(entity =>
            {
                entity.ToTable("hobby");

                entity.Property(e => e.HobbyId).HasColumnName("hobby_id");

                entity.Property(e => e.HobbyName)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("hobby_name");
            });

            modelBuilder.Entity<HobbyList>(entity =>
            {
                entity.HasKey(e => new { e.HobbyId, e.Username })
                    .HasName("PK__hobby_li__84F68163B5B514F6");

                entity.ToTable("hobby_list");

                entity.Property(e => e.HobbyId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("hobby_id");

                entity.Property(e => e.Username)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.HasOne(d => d.Hobby)
                    .WithMany(p => p.HobbyLists)
                    .HasForeignKey(d => d.HobbyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__hobby_lis__hobby__37A5467C");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.HobbyLists)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__hobby_lis__usern__38996AB5");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.NotificationType)
                    .HasName("PK__notifica__9C93F2793E0DAE45");

                entity.ToTable("notification");

                entity.Property(e => e.NotificationType)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("notification_type");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<NotificationList>(entity =>
            {
                entity.HasKey(e => e.NotificationId)
                    .HasName("PK__notifica__E059842FB6236F2D");

                entity.ToTable("notification_list");

                entity.Property(e => e.NotificationId).HasColumnName("notification_id");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.NotificationType)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("notification_type");

                entity.Property(e => e.NotifyTime)
                    .HasColumnType("date")
                    .HasColumnName("notify_time");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.HasOne(d => d.NotificationTypeNavigation)
                    .WithMany(p => p.NotificationLists)
                    .HasForeignKey(d => d.NotificationType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__notificat__notif__403A8C7D");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.NotificationLists)
                    .HasForeignKey(d => new { d.CommentId, d.PostId })
                    .HasConstraintName("FK__notification_lis__3F466844");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("post");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.Content)
                    .HasColumnType("text")
                    .HasColumnName("content");

                entity.Property(e => e.MediaSrc)
                    .HasColumnType("text")
                    .HasColumnName("media_src");

                entity.Property(e => e.PostAuthor)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("post_author");

                entity.Property(e => e.PostTime)
                    .HasColumnType("date")
                    .HasColumnName("post_time");

                entity.Property(e => e.PrivacyMode)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("privacy_mode");

                entity.HasOne(d => d.PostAuthorNavigation)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.PostAuthor)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__post__post_autho__267ABA7A");
            });

            modelBuilder.Entity<React>(entity =>
            {
                entity.HasKey(e => e.ReactType)
                    .HasName("PK__react__F604EFCD74D387F9");

                entity.ToTable("react");

                entity.Property(e => e.ReactType)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("react_type");

                entity.Property(e => e.ReactName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("react_name");
            });

            modelBuilder.Entity<ReactList>(entity =>
            {
                entity.HasKey(e => new { e.ReactType, e.PostId, e.Username })
                    .HasName("PK__react_li__F71A4C7E90C8BFF3");

                entity.ToTable("react_list");

                entity.Property(e => e.ReactType)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("react_type");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.Username)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.ReactLists)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__react_lis__post___33D4B598");

                entity.HasOne(d => d.ReactTypeNavigation)
                    .WithMany(p => p.ReactLists)
                    .HasForeignKey(d => d.ReactType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__react_lis__react__32E0915F");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.ReactLists)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__react_lis__usern__34C8D9D1");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK__user__F3DBC573249ABF51");

                entity.ToTable("user");

                entity.Property(e => e.Username)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.Property(e => e.AvatarSrc)
                    .HasColumnType("text")
                    .HasColumnName("avatar_src");

                entity.Property(e => e.BackgroundSrc)
                    .HasColumnType("text")
                    .HasColumnName("background_src");

                entity.Property(e => e.Birthday)
                    .HasColumnType("date")
                    .HasColumnName("birthday");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.Nickname)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("nickname");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("role");

                entity.Property(e => e.UserMail)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("user_mail");
            });

            modelBuilder.Entity<Warning>(entity =>
            {
                entity.HasKey(e => e.WarningType)
                    .HasName("PK__warning__0F27B8CDA24E3ADD");

                entity.ToTable("warning");

                entity.Property(e => e.WarningType)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("warning_type");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<WarningList>(entity =>
            {
                entity.HasKey(e => e.PunishmentId)
                    .HasName("PK__warning___E62BCF7EADAA2D0C");

                entity.ToTable("warning_list");

                entity.Property(e => e.PunishmentId).HasColumnName("punishment_id");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.NotificationType)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("notification_type");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.Reason)
                    .HasColumnType("text")
                    .HasColumnName("reason");

                entity.Property(e => e.TimeEnd)
                    .HasColumnType("date")
                    .HasColumnName("time_end");

                entity.Property(e => e.TimeStart)
                    .HasColumnType("date")
                    .HasColumnName("time_start");

                entity.HasOne(d => d.NotificationTypeNavigation)
                    .WithMany(p => p.WarningLists)
                    .HasForeignKey(d => d.NotificationType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__warning_l__notif__3C69FB99");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.WarningLists)
                    .HasForeignKey(d => new { d.CommentId, d.PostId })
                    .HasConstraintName("FK__warning_list__3B75D760");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
