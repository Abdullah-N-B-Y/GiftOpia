using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GiftStoreMVC.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DepEmpView> DepEmpViews { get; set; }

    public virtual DbSet<GiftstoreBank> GiftstoreBanks { get; set; }

    public virtual DbSet<GiftstoreBankcard> GiftstoreBankcards { get; set; }

    public virtual DbSet<GiftstoreCategory> GiftstoreCategories { get; set; }

    public virtual DbSet<GiftstoreGift> GiftstoreGifts { get; set; }

    public virtual DbSet<GiftstoreMessage> GiftstoreMessages { get; set; }

    public virtual DbSet<GiftstoreNotification> GiftstoreNotifications { get; set; }

    public virtual DbSet<GiftstoreOrder> GiftstoreOrders { get; set; }

    public virtual DbSet<GiftstorePage> GiftstorePages { get; set; }

    public virtual DbSet<GiftstorePaymentrecored> GiftstorePaymentrecoreds { get; set; }

    public virtual DbSet<GiftstoreRole> GiftstoreRoles { get; set; }

    public virtual DbSet<GiftstoreSenderrequest> GiftstoreSenderrequests { get; set; }

    public virtual DbSet<GiftstoreTestimonial> GiftstoreTestimonials { get; set; }

    public virtual DbSet<GiftstoreUser> GiftstoreUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("USER ID=JOR18_USER73;PASSWORD=123123;DATA SOURCE=94.56.229.181:3488/traindb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("JOR18_USER73")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<DepEmpView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("DEP_EMP_VIEW");

            entity.Property(e => e.Age)
                .HasColumnType("NUMBER")
                .HasColumnName("AGE");
            entity.Property(e => e.Depaddress)
                .HasMaxLength(50)
                .HasColumnName("DEPADDRESS");
            entity.Property(e => e.Depid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("DEPID");
            entity.Property(e => e.EDepid)
                .HasColumnType("NUMBER")
                .HasColumnName("E_DEPID");
            entity.Property(e => e.Empaddress)
                .HasMaxLength(50)
                .HasColumnName("EMPADDRESS");
            entity.Property(e => e.Empid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("EMPID");
            entity.Property(e => e.Firstname)
                .HasMaxLength(20)
                .HasColumnName("FIRSTNAME");
            entity.Property(e => e.Lastname)
                .HasMaxLength(20)
                .HasColumnName("LASTNAME");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(13)
                .IsUnicode(false)
                .HasColumnName("PHONENUMBER");
            entity.Property(e => e.Salary)
                .HasColumnType("NUMBER")
                .HasColumnName("SALARY");
        });

        modelBuilder.Entity<GiftstoreBank>(entity =>
        {
            entity.HasKey(e => e.Bankid).HasName("SYS_C00379822");

            entity.ToTable("GIFTSTORE_BANK");

            entity.Property(e => e.Bankid)
                .HasColumnType("NUMBER")
                .HasColumnName("BANKID");
            entity.Property(e => e.Bankname)
                .HasMaxLength(255)
                .HasColumnName("BANKNAME");
            entity.Property(e => e.Cardnumber)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("CARDNUMBER");

            entity.HasOne(d => d.CardnumberNavigation).WithMany(p => p.GiftstoreBanks)
                .HasForeignKey(d => d.Cardnumber)
                .HasConstraintName("SYS_C00379823");
        });

        modelBuilder.Entity<GiftstoreBankcard>(entity =>
        {
            entity.HasKey(e => e.Cardnumber).HasName("SYS_C00379820");

            entity.ToTable("GIFTSTORE_BANKCARDS");

            entity.HasIndex(e => e.Cvv, "SYS_C00379821").IsUnique();

            entity.Property(e => e.Cardnumber)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("CARDNUMBER");
            entity.Property(e => e.Cardholdername)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("CARDHOLDERNAME");
            entity.Property(e => e.Cvv)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasColumnName("CVV");
            entity.Property(e => e.Expirationdate)
                .HasColumnType("DATE")
                .HasColumnName("EXPIRATIONDATE");
            entity.Property(e => e.Totalamount)
                .HasColumnType("FLOAT")
                .HasColumnName("TOTALAMOUNT");
        });

        modelBuilder.Entity<GiftstoreCategory>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("SYS_C00379796");

            entity.ToTable("GIFTSTORE_CATEGORY");

            entity.Property(e => e.Categoryid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORYID");
            entity.Property(e => e.Categorydescription)
                .HasMaxLength(255)
                .HasColumnName("CATEGORYDESCRIPTION");
            entity.Property(e => e.Categoryname)
                .HasMaxLength(255)
                .HasColumnName("CATEGORYNAME");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(255)
                .HasColumnName("IMAGEPATH");
        });

        modelBuilder.Entity<GiftstoreGift>(entity =>
        {
            entity.HasKey(e => e.Giftid).HasName("SYS_C00379817");

            entity.ToTable("GIFTSTORE_GIFT");

            entity.Property(e => e.Giftid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("GIFTID");
            entity.Property(e => e.Categoryid)
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORYID");
            entity.Property(e => e.Giftavailability)
                .HasPrecision(1)
                .HasColumnName("GIFTAVAILABILITY");
            entity.Property(e => e.Giftdescription)
                .HasMaxLength(255)
                .HasColumnName("GIFTDESCRIPTION");
            entity.Property(e => e.Giftname)
                .HasMaxLength(255)
                .HasColumnName("GIFTNAME");
            entity.Property(e => e.Giftprice)
                .HasColumnType("FLOAT")
                .HasColumnName("GIFTPRICE");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(255)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Category).WithMany(p => p.GiftstoreGifts)
                .HasForeignKey(d => d.Categoryid)
                .HasConstraintName("SYS_C00379818");

            entity.HasOne(d => d.User).WithMany(p => p.GiftstoreGifts)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("FK_USER_GIFT");
        });

        modelBuilder.Entity<GiftstoreMessage>(entity =>
        {
            entity.HasKey(e => e.Messagelid).HasName("SYS_C00379813");

            entity.ToTable("GIFTSTORE_MESSAGE");

            entity.Property(e => e.Messagelid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("MESSAGELID");
            entity.Property(e => e.Messagecontent)
                .HasMaxLength(255)
                .HasColumnName("MESSAGECONTENT");
            entity.Property(e => e.Messagedate)
                .HasColumnType("DATE")
                .HasColumnName("MESSAGEDATE");
            entity.Property(e => e.Reseiverid)
                .HasColumnType("NUMBER")
                .HasColumnName("RESEIVERID");
            entity.Property(e => e.Senderid)
                .HasColumnType("NUMBER")
                .HasColumnName("SENDERID");

            entity.HasOne(d => d.Reseiver).WithMany(p => p.GiftstoreMessageReseivers)
                .HasForeignKey(d => d.Reseiverid)
                .HasConstraintName("SYS_C00379815");

            entity.HasOne(d => d.Sender).WithMany(p => p.GiftstoreMessageSenders)
                .HasForeignKey(d => d.Senderid)
                .HasConstraintName("SYS_C00379814");
        });

        modelBuilder.Entity<GiftstoreNotification>(entity =>
        {
            entity.HasKey(e => e.Notificationlid).HasName("SYS_C00379810");

            entity.ToTable("GIFTSTORE_NOTIFICATION");

            entity.Property(e => e.Notificationlid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("NOTIFICATIONLID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Isread)
                .HasPrecision(1)
                .HasColumnName("ISREAD");
            entity.Property(e => e.Notificationcontent)
                .HasMaxLength(255)
                .HasColumnName("NOTIFICATIONCONTENT");
            entity.Property(e => e.Notificationdate)
                .HasColumnType("DATE")
                .HasColumnName("NOTIFICATIONDATE");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.User).WithMany(p => p.GiftstoreNotifications)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("SYS_C00379811");
        });

        modelBuilder.Entity<GiftstoreOrder>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("SYS_C00379794");

            entity.ToTable("GIFTSTORE_ORDER");

            entity.Property(e => e.Orderid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ORDERID");
            entity.Property(e => e.Arrivaldate)
                .HasColumnType("DATE")
                .HasColumnName("ARRIVALDATE");
            entity.Property(e => e.Finalprice)
                .HasColumnType("FLOAT")
                .HasColumnName("FINALPRICE");
            entity.Property(e => e.Orderdate)
                .HasColumnType("DATE")
                .HasColumnName("ORDERDATE");
            entity.Property(e => e.Orderstatus)
                .HasMaxLength(255)
                .HasColumnName("ORDERSTATUS");
            entity.Property(e => e.Recipientaddress)
                .HasMaxLength(255)
                .HasColumnName("RECIPIENTADDRESS");
            entity.Property(e => e.Requestid)
                .HasColumnType("NUMBER")
                .HasColumnName("REQUESTID");

            entity.HasOne(d => d.Request).WithMany(p => p.GiftstoreOrders)
                .HasForeignKey(d => d.Requestid)
                .HasConstraintName("FK_REQUEST_ORDER");
        });

        modelBuilder.Entity<GiftstorePage>(entity =>
        {
            entity.HasKey(e => e.Pageid).HasName("SYS_C00379804");

            entity.ToTable("GIFTSTORE_PAGE");

            entity.Property(e => e.Pageid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("PAGEID");
            entity.Property(e => e.Adminid)
                .HasColumnType("NUMBER")
                .HasColumnName("ADMINID");
            entity.Property(e => e.Pagecontent)
                .HasMaxLength(255)
                .HasColumnName("PAGECONTENT");
            entity.Property(e => e.Pagetitle)
                .HasMaxLength(255)
                .HasColumnName("PAGETITLE");

            entity.HasOne(d => d.Admin).WithMany(p => p.GiftstorePages)
                .HasForeignKey(d => d.Adminid)
                .HasConstraintName("SYS_C00379805");
        });

        modelBuilder.Entity<GiftstorePaymentrecored>(entity =>
        {
            entity.HasKey(e => e.Paymentid).HasName("SYS_C00379825");

            entity.ToTable("GIFTSTORE_PAYMENTRECORED");

            entity.Property(e => e.Paymentid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("PAYMENTID");
            entity.Property(e => e.Cardnumber)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("CARDNUMBER");
            entity.Property(e => e.Paymentamount)
                .HasColumnType("FLOAT")
                .HasColumnName("PAYMENTAMOUNT");
            entity.Property(e => e.Paymentdate)
                .HasColumnType("DATE")
                .HasColumnName("PAYMENTDATE");
            entity.Property(e => e.Paymentstatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PAYMENTSTATUS");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.User).WithMany(p => p.GiftstorePaymentrecoreds)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("SYS_C00379826");
        });

        modelBuilder.Entity<GiftstoreRole>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("SYS_C00379791");

            entity.ToTable("GIFTSTORE_ROLE");

            entity.HasIndex(e => e.Rolename, "SYS_C00379792").IsUnique();

            entity.Property(e => e.Roleid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Rolename)
                .HasMaxLength(255)
                .HasColumnName("ROLENAME");
        });

        modelBuilder.Entity<GiftstoreSenderrequest>(entity =>
        {
            entity.HasKey(e => e.Requestid).HasName("SYS_C00379828");

            entity.ToTable("GIFTSTORE_SENDERREQUEST");

            entity.Property(e => e.Requestid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("REQUESTID");
            entity.Property(e => e.Giftname)
                .HasMaxLength(255)
                .HasColumnName("GIFTNAME");
            entity.Property(e => e.Giftprice)
                .HasColumnType("FLOAT")
                .HasColumnName("GIFTPRICE");
            entity.Property(e => e.Makerid)
                .HasColumnType("NUMBER")
                .HasColumnName("MAKERID");
            entity.Property(e => e.Recipientaddress)
                .HasMaxLength(255)
                .HasColumnName("RECIPIENTADDRESS");
            entity.Property(e => e.Recipientname)
                .HasMaxLength(255)
                .HasColumnName("RECIPIENTNAME");
            entity.Property(e => e.Requestdate)
                .HasColumnType("DATE")
                .HasColumnName("REQUESTDATE");
            entity.Property(e => e.Requeststatus)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("REQUESTSTATUS");
            entity.Property(e => e.Senderid)
                .HasColumnType("NUMBER")
                .HasColumnName("SENDERID");
            entity.Property(e => e.Sendername)
                .HasMaxLength(255)
                .HasColumnName("SENDERNAME");

            entity.HasOne(d => d.Maker).WithMany(p => p.GiftstoreSenderrequestMakers)
                .HasForeignKey(d => d.Makerid)
                .HasConstraintName("FK_MAKER_ID");

            entity.HasOne(d => d.Sender).WithMany(p => p.GiftstoreSenderrequestSenders)
                .HasForeignKey(d => d.Senderid)
                .HasConstraintName("SYS_C00379829");
        });

        modelBuilder.Entity<GiftstoreTestimonial>(entity =>
        {
            entity.HasKey(e => e.Testimonialid).HasName("SYS_C00379807");

            entity.ToTable("GIFTSTORE_TESTIMONIAL");

            entity.Property(e => e.Testimonialid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("TESTIMONIALID");
            entity.Property(e => e.Testimonialcontent)
                .HasMaxLength(255)
                .HasColumnName("TESTIMONIALCONTENT");
            entity.Property(e => e.Testimonialdate)
                .HasColumnType("DATE")
                .HasColumnName("TESTIMONIALDATE");
            entity.Property(e => e.Testimonialstatus)
                .HasMaxLength(255)
                .HasColumnName("TESTIMONIALSTATUS");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.User).WithMany(p => p.GiftstoreTestimonials)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("SYS_C00379808");
        });

        modelBuilder.Entity<GiftstoreUser>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("SYS_C00379798");

            entity.ToTable("GIFTSTORE_USER");

            entity.HasIndex(e => e.Username, "SYS_C00379799").IsUnique();

            entity.HasIndex(e => e.Email, "SYS_C00379800").IsUnique();

            entity.Property(e => e.Userid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");
            entity.Property(e => e.Approvalstatus)
                .HasMaxLength(255)
                .HasColumnName("APPROVALSTATUS");
            entity.Property(e => e.Categoryid)
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORYID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(255)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .HasColumnName("NAME");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(20)
                .HasColumnName("PHONENUMBER");
            entity.Property(e => e.Profits)
                .HasColumnType("NUMBER")
                .HasColumnName("PROFITS");
            entity.Property(e => e.Roleid)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .HasColumnName("USERNAME");

            entity.HasOne(d => d.Category).WithMany(p => p.GiftstoreUsers)
                .HasForeignKey(d => d.Categoryid)
                .HasConstraintName("SYS_C00379801");

            entity.HasOne(d => d.Role).WithMany(p => p.GiftstoreUsers)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("SYS_C00379802");
        });
        modelBuilder.HasSequence("ADMIN_SEQ");
        modelBuilder.HasSequence("DEPIDSEQ");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
