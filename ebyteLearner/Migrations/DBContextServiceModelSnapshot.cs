﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ebyteLearner.Migrations
{
    [DbContext(typeof(DBContextService))]
    partial class DBContextServiceModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ebyteLearner.Models.Answer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("AnswerCorrect")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("AnswerResponse")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("QuestionID")
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("QuestionID");

                    b.ToTable("Answer");
                });

            modelBuilder.Entity("ebyteLearner.Models.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("ebyteLearner.Models.Course", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("CategoryID")
                        .HasColumnType("char(36)");

                    b.Property<string>("CourseDescription")
                        .HasColumnType("longtext");

                    b.Property<string>("CourseDirectory")
                        .HasColumnType("longtext");

                    b.Property<string>("CourseImageURL")
                        .HasColumnType("longtext");

                    b.Property<bool?>("CourseIsPublished")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<float?>("CoursePrice")
                        .HasColumnType("float");

                    b.Property<Guid?>("CourseTeacherID")
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryID");

                    b.HasIndex("CourseTeacherID");

                    b.ToTable("Course");
                });

            modelBuilder.Entity("ebyteLearner.Models.Module", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CourseId")
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ModuleDescription")
                        .HasColumnType("longtext");

                    b.Property<string>("ModuleName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("ModuleOrder")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.Property<bool?>("isFree")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool?>("isPublished")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("Module");
                });

            modelBuilder.Entity("ebyteLearner.Models.Pdf", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("ModuleID")
                        .HasColumnType("char(36)");

                    b.Property<string>("PDFContent")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<long>("PDFLength")
                        .HasColumnType("bigint");

                    b.Property<string>("PDFName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("PDFNumberPages")
                        .HasColumnType("int");

                    b.Property<string>("PDFPath")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("ModuleID");

                    b.ToTable("Pdf");
                });

            modelBuilder.Entity("ebyteLearner.Models.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("PDFId")
                        .HasColumnType("char(36)");

                    b.Property<string>("QuestionName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<float>("QuestionScore")
                        .HasColumnType("float");

                    b.Property<int>("QuestionSlide")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("PDFId");

                    b.ToTable("Question");
                });

            modelBuilder.Entity("ebyteLearner.Models.Session", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<DateTimeOffset>("EndSessionDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("ModuleId")
                        .HasColumnType("char(36)");

                    b.Property<byte[]>("QRCode")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("SessionDescription")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("SessionModuleID")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("SessionMonitoringID")
                        .HasColumnType("char(36)");

                    b.Property<string>("SessionName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("SessionPdfId")
                        .IsRequired()
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("StartSessionDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("ModuleId");

                    b.HasIndex("SessionMonitoringID");

                    b.HasIndex("SessionPdfId");

                    b.ToTable("Session");
                });

            modelBuilder.Entity("ebyteLearner.Models.SessionMonitoring", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("ShowingQrCode")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("ShowingQuestion")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("ShowingResult")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("Slide")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.ToTable("SessionMonitoring");
                });

            modelBuilder.Entity("ebyteLearner.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<bool>("Active")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("Birthday")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("CourseId")
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Docn")
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Gender")
                        .HasColumnType("longtext");

                    b.Property<string>("NIF")
                        .HasColumnType("longtext");

                    b.Property<string>("Nationality")
                        .HasColumnType("longtext");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<string>("ProfilePhoto")
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserRole")
                        .HasMaxLength(50)
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ZipCode")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("CourseId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("ebyteLearner.Models.UserSession", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("SessionId")
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.HasKey("UserId", "SessionId");

                    b.HasIndex("SessionId");

                    b.ToTable("UserSession");
                });

            modelBuilder.Entity("ebyteLearner.Models.Answer", b =>
                {
                    b.HasOne("ebyteLearner.Models.Question", "Question")
                        .WithMany("QuestionAnswers")
                        .HasForeignKey("QuestionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("ebyteLearner.Models.Course", b =>
                {
                    b.HasOne("ebyteLearner.Models.Category", "CourseCategory")
                        .WithMany()
                        .HasForeignKey("CategoryID");

                    b.HasOne("ebyteLearner.Models.User", "CourseTeacher")
                        .WithMany()
                        .HasForeignKey("CourseTeacherID");

                    b.Navigation("CourseCategory");

                    b.Navigation("CourseTeacher");
                });

            modelBuilder.Entity("ebyteLearner.Models.Module", b =>
                {
                    b.HasOne("ebyteLearner.Models.Course", "Course")
                        .WithMany("CourseModules")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("ebyteLearner.Models.Pdf", b =>
                {
                    b.HasOne("ebyteLearner.Models.Module", "Module")
                        .WithMany()
                        .HasForeignKey("ModuleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Module");
                });

            modelBuilder.Entity("ebyteLearner.Models.Question", b =>
                {
                    b.HasOne("ebyteLearner.Models.Pdf", "RelatedPDF")
                        .WithMany()
                        .HasForeignKey("PDFId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RelatedPDF");
                });

            modelBuilder.Entity("ebyteLearner.Models.Session", b =>
                {
                    b.HasOne("ebyteLearner.Models.Module", null)
                        .WithMany("Sessions")
                        .HasForeignKey("ModuleId");

                    b.HasOne("ebyteLearner.Models.SessionMonitoring", "SessionMonitoring")
                        .WithMany()
                        .HasForeignKey("SessionMonitoringID");

                    b.HasOne("ebyteLearner.Models.Pdf", "SessionPdf")
                        .WithMany()
                        .HasForeignKey("SessionPdfId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SessionMonitoring");

                    b.Navigation("SessionPdf");
                });

            modelBuilder.Entity("ebyteLearner.Models.User", b =>
                {
                    b.HasOne("ebyteLearner.Models.Course", null)
                        .WithMany("Users")
                        .HasForeignKey("CourseId");
                });

            modelBuilder.Entity("ebyteLearner.Models.UserSession", b =>
                {
                    b.HasOne("ebyteLearner.Models.Session", "Session")
                        .WithMany("UserSessions")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ebyteLearner.Models.User", "User")
                        .WithMany("UserSessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Session");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ebyteLearner.Models.Course", b =>
                {
                    b.Navigation("CourseModules");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("ebyteLearner.Models.Module", b =>
                {
                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("ebyteLearner.Models.Question", b =>
                {
                    b.Navigation("QuestionAnswers");
                });

            modelBuilder.Entity("ebyteLearner.Models.Session", b =>
                {
                    b.Navigation("UserSessions");
                });

            modelBuilder.Entity("ebyteLearner.Models.User", b =>
                {
                    b.Navigation("UserSessions");
                });
#pragma warning restore 612, 618
        }
    }
}
