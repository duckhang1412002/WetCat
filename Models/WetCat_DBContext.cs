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
                optionsBuilder.UseSqlServer("Server=.;uid=sa;pwd=123456;database=WetCat_DB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.HasKey(e => new { e.CommentId, e.PostId })
                    .HasName("PK__comment__94780EF176095010");

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

                entity.Property(e => e.CommentContent)
                    .IsRequired()
                    .HasColumnType("ntext")
                    .HasColumnName("comment_content");

                entity.Property(e => e.CommentTime)
                    .HasColumnType("datetime")
                    .HasColumnName("comment_time");

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
                entity.HasKey(e => new { e.FollowerUsername, e.FollowedUsername })
                    .HasName("PK__follow__1278340630F16297");

                entity.ToTable("follow");

                entity.Property(e => e.FollowerUsername)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("follower_username");

                entity.Property(e => e.FollowedUsername)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("followed_username");

                entity.HasOne(d => d.FollowedUsernameNavigation)
                    .WithMany(p => p.FollowFollowedUsernameNavigations)
                    .HasForeignKey(d => d.FollowedUsername)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__follow__followed__46E78A0C");

                entity.HasOne(d => d.FollowerUsernameNavigation)
                    .WithMany(p => p.FollowFollowerUsernameNavigations)
                    .HasForeignKey(d => d.FollowerUsername)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__follow__follower__45F365D3");
            });

            modelBuilder.Entity<Friend>(entity =>
            {
                entity.HasKey(e => new { e.FirstUsername, e.SecondUsername })
                    .HasName("PK__friend__13CF439254298DA9");

                entity.ToTable("friend");

                entity.Property(e => e.FirstUsername)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("first_username");

                entity.Property(e => e.SecondUsername)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("second_username");

                entity.Property(e => e.FriendStatus).HasColumnName("friend_status");

                entity.Property(e => e.StatusTime)
                    .HasColumnType("date")
                    .HasColumnName("status_time");

                entity.HasOne(d => d.FirstUsernameNavigation)
                    .WithMany(p => p.FriendFirstUsernameNavigations)
                    .HasForeignKey(d => d.FirstUsername)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__friend__first_us__49C3F6B7");

                entity.HasOne(d => d.SecondUsernameNavigation)
                    .WithMany(p => p.FriendSecondUsernameNavigations)
                    .HasForeignKey(d => d.SecondUsername)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__friend__second_u__4AB81AF0");
            });

            modelBuilder.Entity<Hobby>(entity =>
            {
                entity.ToTable("hobby");

                entity.Property(e => e.HobbyId).HasColumnName("hobby_id");

                entity.Property(e => e.HobbyName)
                    .HasMaxLength(50)
                    .HasColumnName("hobby_name");
            });

            modelBuilder.Entity<HobbyList>(entity =>
            {
                entity.HasKey(e => new { e.HobbyId, e.Username })
                    .HasName("PK__hobby_li__84F68163C33FD3B1");

                entity.ToTable("hobby_list");

                entity.Property(e => e.HobbyId).HasColumnName("hobby_id");

                entity.Property(e => e.Username)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.HasOne(d => d.Hobby)
                    .WithMany(p => p.HobbyLists)
                    .HasForeignKey(d => d.HobbyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__hobby_lis__hobby__3B75D760");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.HobbyLists)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__hobby_lis__usern__3C69FB99");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.NotificationType)
                    .HasName("PK__notifica__9C93F27997784365");

                entity.ToTable("notification");

                entity.Property(e => e.NotificationType)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("notification_type");

                entity.Property(e => e.NotificationName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("notification_name");
            });

            modelBuilder.Entity<NotificationList>(entity =>
            {
                entity.HasKey(e => e.NotificationId)
                    .HasName("PK__notifica__E059842FCA9DD335");

                entity.ToTable("notification_list");

                entity.Property(e => e.NotificationId).HasColumnName("notification_id");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.NotificationType)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("notification_type");

                entity.Property(e => e.NotifyTime)
                    .HasColumnType("datetime")
                    .HasColumnName("notify_time");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.HasOne(d => d.NotificationTypeNavigation)
                    .WithMany(p => p.NotificationLists)
                    .HasForeignKey(d => d.NotificationType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__notificat__notif__33D4B598");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.NotificationLists)
                    .HasForeignKey(d => new { d.CommentId, d.PostId })
                    .HasConstraintName("FK__notification_lis__32E0915F");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("post");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.PostAuthor)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("post_author");

                entity.Property(e => e.PostContent)
                    .IsRequired()
                    .HasColumnType("ntext")
                    .HasColumnName("post_content");

                entity.Property(e => e.PostImgSrc)
                    .HasColumnType("text")
                    .HasColumnName("post_img_src");

                entity.Property(e => e.PostTime)
                    .HasColumnType("datetime")
                    .HasColumnName("post_time");

                entity.Property(e => e.PrivacyMode)
                    .IsRequired()
                    .HasMaxLength(20)
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
                    .HasName("PK__react__F604EFCD6A3A5FB7");

                entity.ToTable("react");

                entity.Property(e => e.ReactType)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("react_type");

                entity.Property(e => e.ReactName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("react_name");
            });

            modelBuilder.Entity<ReactList>(entity =>
            {
                entity.HasKey(e => new { e.ReactType, e.PostId, e.Username })
                    .HasName("PK__react_li__F71A4C7EC7A500F5");

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
                    .HasConstraintName("FK__react_lis__post___37A5467C");

                entity.HasOne(d => d.ReactTypeNavigation)
                    .WithMany(p => p.ReactLists)
                    .HasForeignKey(d => d.ReactType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__react_lis__react__36B12243");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.ReactLists)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__react_lis__usern__38996AB5");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Username)
                    .HasName("PK__user__F3DBC5735460264C");

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
                    .HasColumnType("datetime")
                    .HasColumnName("birthday");

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.Nickname)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("nickname");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Phone)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("phone");

                entity.Property(e => e.Quote)
                    .HasColumnType("ntext")
                    .HasColumnName("quote");

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("role");

                entity.Property(e => e.UserMail)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("user_mail");
            });

            modelBuilder.Entity<Warning>(entity =>
            {
                entity.HasKey(e => e.WarningType)
                    .HasName("PK__warning__0F27B8CDFA8E6BD4");

                entity.ToTable("warning");

                entity.Property(e => e.WarningType)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("warning_type");

                entity.Property(e => e.WarningName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("warning_name");
            });

            modelBuilder.Entity<WarningList>(entity =>
            {
                entity.HasKey(e => e.WarningId)
                    .HasName("PK__warning___DFF7B6E5961C766B");

                entity.ToTable("warning_list");

                entity.Property(e => e.WarningId).HasColumnName("warning_id");

                entity.Property(e => e.CommentId).HasColumnName("comment_id");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.Reason)
                    .HasColumnType("ntext")
                    .HasColumnName("reason");

                entity.Property(e => e.TimeEnd)
                    .HasColumnType("date")
                    .HasColumnName("time_end");

                entity.Property(e => e.TimeStart)
                    .HasColumnType("date")
                    .HasColumnName("time_start");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("username");

                entity.Property(e => e.WarningType)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("warning_type");

                entity.HasOne(d => d.UsernameNavigation)
                    .WithMany(p => p.WarningLists)
                    .HasForeignKey(d => d.Username)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__warning_l__usern__412EB0B6");

                entity.HasOne(d => d.WarningTypeNavigation)
                    .WithMany(p => p.WarningLists)
                    .HasForeignKey(d => d.WarningType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__warning_l__warni__4316F928");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.WarningLists)
                    .HasForeignKey(d => new { d.CommentId, d.PostId })
                    .HasConstraintName("FK__warning_list__4222D4EF");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
