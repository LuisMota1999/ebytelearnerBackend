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

                    b.Property<float>("AnswerScore")
                        .HasColumnType("float");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("QuestionID")
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("QuestionID");

                    b.ToTable("Answer");
                });

            modelBuilder.Entity("ebyteLearner.Models.Course", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("CourseDescription")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<float>("CoursePrice")
                        .HasColumnType("float");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

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
                        .HasColumnType("datetime(6)");

                    b.Property<string>("ModuleDescription")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ModuleName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .HasColumnType("datetime(6)");

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

                    b.Property<DateTimeOffset>("UpdatedDate")
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
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("PDFId")
                        .HasColumnType("char(36)");

                    b.Property<string>("QuestionName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<float>("Score")
                        .HasColumnType("float");

                    b.Property<Guid>("SessionID")
                        .HasColumnType("char(36)");

                    b.Property<int>("Slide")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("PDFId");

                    b.HasIndex("SessionID");

                    b.ToTable("Question");
                });

            modelBuilder.Entity("ebyteLearner.Models.Session", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("CreatedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTimeOffset>("EndSessionDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("ModuleID")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("PdfDocumentID")
                        .HasColumnType("char(36)");

                    b.Property<byte[]>("QRCode")
                        .IsRequired()
                        .HasColumnType("longblob");

                    b.Property<string>("SessionDescription")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid>("SessionMonitoringID")
                        .HasColumnType("char(36)");

                    b.Property<string>("SessionName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTimeOffset>("StartSessionDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTimeOffset>("UpdatedDate")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("ModuleID");

                    b.HasIndex("PdfDocumentID");

                    b.HasIndex("SessionMonitoringID");

                    b.ToTable("Session");
                });

            modelBuilder.Entity("ebyteLearner.Models.SessionMonitoring", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTimeOffset>("CreatedDate")
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
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Docn")
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Gender")
                        .HasColumnType("longtext");

                    b.Property<string>("NIF")
                        .IsRequired()
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

                    b.HasKey("UserId", "SessionId");

                    b.HasIndex("SessionId");

                    b.ToTable("UserSession");
                });

            modelBuilder.Entity("ebyteLearner.Models.Answer", b =>
                {
                    b.HasOne("ebyteLearner.Models.Question", "Question")
                        .WithMany("Answers")
                        .HasForeignKey("QuestionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("ebyteLearner.Models.Module", b =>
                {
                    b.HasOne("ebyteLearner.Models.Course", "Course")
                        .WithMany("Modules")
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

                    b.HasOne("ebyteLearner.Models.Session", "RelatedSession")
                        .WithMany()
                        .HasForeignKey("SessionID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("RelatedPDF");

                    b.Navigation("RelatedSession");
                });

            modelBuilder.Entity("ebyteLearner.Models.Session", b =>
                {
                    b.HasOne("ebyteLearner.Models.Module", null)
                        .WithMany("Sessions")
                        .HasForeignKey("ModuleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ebyteLearner.Models.Pdf", "PdfDocument")
                        .WithMany()
                        .HasForeignKey("PdfDocumentID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ebyteLearner.Models.SessionMonitoring", "SessionMonitoring")
                        .WithMany()
                        .HasForeignKey("SessionMonitoringID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PdfDocument");

                    b.Navigation("SessionMonitoring");
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
                    b.Navigation("Modules");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("ebyteLearner.Models.Module", b =>
                {
                    b.Navigation("Sessions");
                });

            modelBuilder.Entity("ebyteLearner.Models.Question", b =>
                {
                    b.Navigation("Answers");
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
