using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace bma_license_repository.Dto;

public partial class DevBmaContext : DbContext
{
    public DevBmaContext()
    {
    }

    public DevBmaContext(DbContextOptions<DevBmaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SysConfiguration> SysConfigurations { get; set; }

    public virtual DbSet<SysJobRepairStatus> SysJobRepairStatuses { get; set; }

    public virtual DbSet<SysKeyType> SysKeyTypes { get; set; }

    public virtual DbSet<SysMessageConfiguration> SysMessageConfigurations { get; set; }

    public virtual DbSet<SysOrgranize> SysOrgranizes { get; set; }

    public virtual DbSet<SysRepairCategory> SysRepairCategories { get; set; }

    public virtual DbSet<SysUser> SysUsers { get; set; }

    public virtual DbSet<SysUserGroup> SysUserGroups { get; set; }

    public virtual DbSet<SysUserPermission> SysUserPermissions { get; set; }

    public virtual DbSet<SysUserType> SysUserTypes { get; set; }

    public virtual DbSet<TransEquipment> TransEquipments { get; set; }

    public virtual DbSet<TransJobRepair> TransJobRepairs { get; set; }

    public virtual DbSet<TransKey> TransKeys { get; set; }

    public virtual DbSet<TransKeyHistory> TransKeyHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<SysConfiguration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sys_configuration_pkey");

            entity.ToTable("sys_configuration");

            entity.HasIndex(e => e.Key, "idx_sys_configuration_key");

            entity.HasIndex(e => e.Key, "sys_configuration_key_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.Description)
                .HasMaxLength(512)
                .HasColumnName("description");
            entity.Property(e => e.Group)
                .HasMaxLength(128)
                .HasColumnName("group");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Key)
                .HasMaxLength(256)
                .HasColumnName("key");
            entity.Property(e => e.Seq)
                .ValueGeneratedOnAdd()
                .HasColumnName("seq");
            entity.Property(e => e.UpdatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.Value).HasColumnName("value");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SysConfigurationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_created_by");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SysConfigurationUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_updated_by");
        });

        modelBuilder.Entity<SysJobRepairStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sys_job_repair_status_pkey");

            entity.ToTable("sys_job_repair_status");

            entity.HasIndex(e => e.Name, "sys_job_repair_status_name_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .HasColumnName("name");
            entity.Property(e => e.Seq)
                .ValueGeneratedOnAdd()
                .HasColumnName("seq");
            entity.Property(e => e.UpdatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SysJobRepairStatusCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_created_by");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SysJobRepairStatusUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_updated_by");
        });

        modelBuilder.Entity<SysKeyType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sys_key_type_pkey");

            entity.ToTable("sys_key_type");

            entity.HasIndex(e => e.Name, "idx_name");

            entity.HasIndex(e => e.IsActive, "sys_key_type_is_active_idx");

            entity.HasIndex(e => e.Name, "sys_key_type_name_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .HasColumnName("name");
            entity.Property(e => e.Seq)
                .ValueGeneratedOnAdd()
                .HasColumnName("seq");
            entity.Property(e => e.UpdatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SysKeyTypeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_created_by");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SysKeyTypeUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_updated_by");
        });

        modelBuilder.Entity<SysMessageConfiguration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("table_name_pkey");

            entity.ToTable("sys_message_configuration");

            entity.HasIndex(e => e.Code, "table_name_code_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(64)
                .HasColumnName("code");
            entity.Property(e => e.CreatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.MessageEn)
                .HasMaxLength(1024)
                .HasColumnName("message_en");
            entity.Property(e => e.MessageTh)
                .HasMaxLength(1024)
                .HasColumnName("message_th");
            entity.Property(e => e.Seq)
                .ValueGeneratedOnAdd()
                .HasColumnName("seq");
            entity.Property(e => e.UpdatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SysMessageConfigurationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_created_by");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SysMessageConfigurationUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_updated_by");
        });

        modelBuilder.Entity<SysOrgranize>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sys_orgranize_pkey");

            entity.ToTable("sys_orgranize");

            entity.HasIndex(e => e.DepartmentCode, "idx_department_code");

            entity.HasIndex(e => e.DivisionCode, "idx_division_code");

            entity.HasIndex(e => e.JobCode, "idx_job_code");

            entity.HasIndex(e => e.SectionCode, "idx_section_code");

            entity.HasIndex(e => e.IsActive, "sys_orgranize_is_active_idx");

            entity.HasIndex(e => e.TypeName, "sys_orgranize_type_name_idx");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .HasColumnName("code");
            entity.Property(e => e.CreatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.DepartmentCode)
                .HasMaxLength(2)
                .HasColumnName("department_code");
            entity.Property(e => e.DivisionCode)
                .HasMaxLength(2)
                .HasColumnName("division_code");
            entity.Property(e => e.InstallatioLocation)
                .HasMaxLength(256)
                .HasColumnName("Installatio_location");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.JobCode)
                .HasMaxLength(2)
                .HasColumnName("job_code");
            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .HasComment("ชื่อหน่วยงาน")
                .HasColumnName("name");
            entity.Property(e => e.SectionCode)
                .HasMaxLength(2)
                .HasColumnName("section_code");
            entity.Property(e => e.Seq)
                .ValueGeneratedOnAdd()
                .HasColumnName("seq");
            entity.Property(e => e.TypeName)
                .HasMaxLength(64)
                .HasComment("DEPARTMENT , DIVISION , SECTION , JOB")
                .HasColumnName("type_name");
            entity.Property(e => e.UpdatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SysOrgranizeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_created_by");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SysOrgranizeUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_updated_by");
        });

        modelBuilder.Entity<SysRepairCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sys_repair_category_pkey");

            entity.ToTable("sys_repair_category");

            entity.HasIndex(e => e.Name, "sys_repair_category_name_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .HasColumnName("name");
            entity.Property(e => e.Seq)
                .ValueGeneratedOnAdd()
                .HasColumnName("seq");
            entity.Property(e => e.UpdatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SysRepairCategoryCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_created_by");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SysRepairCategoryUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_updated_by");
        });

        modelBuilder.Entity<SysUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sys_user_pkey");

            entity.ToTable("sys_user");

            entity.HasIndex(e => e.SysUserGroupId, "idx_sys_user_group_id");

            entity.HasIndex(e => e.SysUserPermissionId, "idx_sys_user_permission_id");

            entity.HasIndex(e => e.SysUserTypeId, "idx_sys_user_type_id");

            entity.HasIndex(e => e.Username, "idx_username");

            entity.HasIndex(e => e.IsActive, "sys_user_is_active_idx");

            entity.HasIndex(e => e.Username, "sys_user_username_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.AccessToken).HasColumnName("access_token");
            entity.Property(e => e.AccessTokenExpire).HasColumnName("access_token_expire");
            entity.Property(e => e.CreatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.DepartmentCode)
                .HasMaxLength(5)
                .HasColumnName("department_code");
            entity.Property(e => e.DivisionCode)
                .HasMaxLength(5)
                .HasColumnName("division_code");
            entity.Property(e => e.FullName)
                .HasMaxLength(256)
                .HasColumnName("full_name");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.JobCode)
                .HasMaxLength(5)
                .HasColumnName("job_code");
            entity.Property(e => e.Password).HasColumnName("password");
            entity.Property(e => e.SectionCode)
                .HasMaxLength(5)
                .HasColumnName("section_code");
            entity.Property(e => e.Seq)
                .ValueGeneratedOnAdd()
                .HasColumnName("seq");
            entity.Property(e => e.SysUserGroupId).HasColumnName("sys_user_group_id");
            entity.Property(e => e.SysUserPermissionId).HasColumnName("sys_user_permission_id");
            entity.Property(e => e.SysUserTypeId).HasColumnName("sys_user_type_id");
            entity.Property(e => e.UpdatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");
            entity.Property(e => e.Username)
                .HasMaxLength(128)
                .HasColumnName("username");

            entity.HasOne(d => d.SysUserGroup).WithMany(p => p.SysUsers)
                .HasForeignKey(d => d.SysUserGroupId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_sys_user_group");

            entity.HasOne(d => d.SysUserPermission).WithMany(p => p.SysUsers)
                .HasForeignKey(d => d.SysUserPermissionId)
                .HasConstraintName("fk_sys_user_permission");

            entity.HasOne(d => d.SysUserType).WithMany(p => p.SysUsers)
                .HasForeignKey(d => d.SysUserTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_sys_user_type");
        });

        modelBuilder.Entity<SysUserGroup>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sys_user_group_pkey");

            entity.ToTable("sys_user_group");

            entity.HasIndex(e => e.Name, "sys_user_group_name_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .HasColumnName("name");
            entity.Property(e => e.Seq)
                .ValueGeneratedOnAdd()
                .HasColumnName("seq");
            entity.Property(e => e.UpdatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SysUserGroupCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_created_by");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SysUserGroupUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_updated_by");
        });

        modelBuilder.Entity<SysUserPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sys_user_permission_pkey");

            entity.ToTable("sys_user_permission");

            entity.HasIndex(e => e.Name, "sys_user_permission_name_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .HasColumnName("name");
            entity.Property(e => e.Seq)
                .ValueGeneratedOnAdd()
                .HasColumnName("seq");
            entity.Property(e => e.UpdatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SysUserPermissionCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_created_by");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SysUserPermissionUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_updated_by");
        });

        modelBuilder.Entity<SysUserType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("sys_user_type_pkey");

            entity.ToTable("sys_user_type");

            entity.HasIndex(e => e.Name, "sys_user_type_name_key").IsUnique();

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .HasColumnName("name");
            entity.Property(e => e.Seq)
                .ValueGeneratedOnAdd()
                .HasColumnName("seq");
            entity.Property(e => e.UpdatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.SysUserTypeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_created_by");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.SysUserTypeUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_updated_by");
        });

        modelBuilder.Entity<TransEquipment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("trans_equipment_pkey");

            entity.ToTable("trans_equipment");

            entity.HasIndex(e => e.CreatedBy, "trans_equipment_created_by_idx");

            entity.HasIndex(e => e.EquipmentCode, "trans_equipment_equipment_code_idx");

            entity.HasIndex(e => e.IsActive, "trans_equipment_is_active_idx");

            entity.HasIndex(e => e.TransKeyId, "trans_equipment_trans_key_id_idx");

            entity.HasIndex(e => e.UpdatedBy, "trans_equipment_updated_by_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Brand)
                .HasMaxLength(256)
                .HasColumnName("brand");
            entity.Property(e => e.CreatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.EquipmentCode)
                .HasMaxLength(128)
                .HasColumnName("equipment_code");
            entity.Property(e => e.Generation)
                .HasMaxLength(256)
                .HasColumnName("generation");
            entity.Property(e => e.InstallDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("install_date");
            entity.Property(e => e.InstallLocation)
                .HasMaxLength(256)
                .HasColumnName("install_location");
            entity.Property(e => e.IpAddress)
                .HasMaxLength(256)
                .HasColumnName("ip_address");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.MacAddress)
                .HasMaxLength(256)
                .HasColumnName("mac_address");
            entity.Property(e => e.MachineName)
                .HasMaxLength(256)
                .HasColumnName("machine_name");
            entity.Property(e => e.MachineNumber)
                .HasMaxLength(256)
                .HasColumnName("machine_number");
            entity.Property(e => e.MachineType)
                .HasMaxLength(256)
                .HasColumnName("machine_type");
            entity.Property(e => e.Remark)
                .HasMaxLength(512)
                .HasColumnName("remark");
            entity.Property(e => e.TransKeyId).HasColumnName("trans_key_id");
            entity.Property(e => e.UpdatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TransEquipmentCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_create_by");

            entity.HasOne(d => d.TransKey).WithMany(p => p.TransEquipments)
                .HasForeignKey(d => d.TransKeyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_trans_key_id");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.TransEquipmentUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_updated_by");
        });

        modelBuilder.Entity<TransJobRepair>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("trans_job_repair_pkey");

            entity.ToTable("trans_job_repair");

            entity.HasIndex(e => e.SysJobRepairStatusId, "idx_sys_job_repair_status_id");

            entity.HasIndex(e => e.SysRepairCategoryId, "idx_sys_repair_category_id");

            entity.HasIndex(e => e.TransKeyId, "idx_trans_key_id");

            entity.HasIndex(e => e.IsActive, "trans_job_repair_is_active_idx");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.DateOfFixed)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_of_fixed");
            entity.Property(e => e.DateOfRequest)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_of_request");
            entity.Property(e => e.DateOfStart)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date_of_start");
            entity.Property(e => e.Description)
                .HasMaxLength(512)
                .HasColumnName("description");
            entity.Property(e => e.FixedDescription).HasColumnName("fixed_description");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.Name)
                .HasMaxLength(128)
                .HasColumnName("name");
            entity.Property(e => e.Remark)
                .HasMaxLength(512)
                .HasColumnName("remark");
            entity.Property(e => e.Seq)
                .ValueGeneratedOnAdd()
                .HasColumnName("seq");
            entity.Property(e => e.SysJobRepairStatusId).HasColumnName("sys_job_repair_status_id");
            entity.Property(e => e.SysRepairCategoryId).HasColumnName("sys_repair_category_id");
            entity.Property(e => e.Telephone)
                .HasMaxLength(10)
                .HasColumnName("telephone");
            entity.Property(e => e.TransEquipmentId).HasColumnName("trans_equipment_id");
            entity.Property(e => e.TransKeyId).HasColumnName("trans_key_id");
            entity.Property(e => e.UpdatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TransJobRepairCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("fk_created_by");

            entity.HasOne(d => d.SysJobRepairStatus).WithMany(p => p.TransJobRepairs)
                .HasForeignKey(d => d.SysJobRepairStatusId)
                .HasConstraintName("fk_sys_job_repair_status");

            entity.HasOne(d => d.SysRepairCategory).WithMany(p => p.TransJobRepairs)
                .HasForeignKey(d => d.SysRepairCategoryId)
                .HasConstraintName("fk_sys_repair_category");

            entity.HasOne(d => d.TransEquipment).WithMany(p => p.TransJobRepairs)
                .HasForeignKey(d => d.TransEquipmentId)
                .HasConstraintName("fk_trans_equipment");

            entity.HasOne(d => d.TransKey).WithMany(p => p.TransJobRepairs)
                .HasForeignKey(d => d.TransKeyId)
                .HasConstraintName("fk_trans_key");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.TransJobRepairUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("fk_updated_by");
        });

        modelBuilder.Entity<TransKey>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("trans_key_pkey");

            entity.ToTable("trans_key");

            entity.HasIndex(e => e.IsActive, "idx_is_active");

            entity.HasIndex(e => e.SysOrgranizeId, "idx_sys_orgranize_id");

            entity.HasIndex(e => e.SysKeyTypeId, "trans_key_sys_key_type_id_idx");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("uuid_generate_v4()")
                .HasColumnName("id");
            entity.Property(e => e.CreatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("created_by");
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_date");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("is_active");
            entity.Property(e => e.License)
                .HasMaxLength(128)
                .HasComment("ชื่อ License")
                .HasColumnName("license");
            entity.Property(e => e.Remark)
                .HasMaxLength(512)
                .HasColumnName("remark");
            entity.Property(e => e.SysKeyTypeId)
                .HasDefaultValueSql("'40c1f1ea-da6d-495d-927b-4c18cdd99b28'::uuid")
                .HasComment("ประเภท License")
                .HasColumnName("sys_key_type_id");
            entity.Property(e => e.SysOrgranizeId)
                .HasComment("รหัสหน่วยงาน")
                .HasColumnName("sys_orgranize_id");
            entity.Property(e => e.UpdatedBy)
                .HasDefaultValueSql("'4d143f87-6228-4afe-ad15-aedcf5741eaf'::uuid")
                .HasColumnName("updated_by");
            entity.Property(e => e.UpdatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("updated_date");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TransKeyCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("fk_created_by");

            entity.HasOne(d => d.SysKeyType).WithMany(p => p.TransKeys)
                .HasForeignKey(d => d.SysKeyTypeId)
                .HasConstraintName("fk_sys_key_type");

            entity.HasOne(d => d.SysOrgranize).WithMany(p => p.TransKeys)
                .HasForeignKey(d => d.SysOrgranizeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_sys_orgranize");

            entity.HasOne(d => d.UpdatedByNavigation).WithMany(p => p.TransKeyUpdatedByNavigations)
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("fk_updated_by");
        });

        modelBuilder.Entity<TransKeyHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("trans_key_history_pkey");

            entity.ToTable("trans_key_history");

            entity.HasIndex(e => e.SysOrgranizeId, "idx_trans_key_history_sys_orgranize_id");

            entity.HasIndex(e => e.Id, "trans_key_history_id_key").IsUnique();

            entity.HasIndex(e => e.TransKeyId, "trans_key_history_trans_key_id_idx");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.ActionBy).HasColumnName("action_by");
            entity.Property(e => e.EndDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("end_date");
            entity.Property(e => e.Remark)
                .HasMaxLength(512)
                .HasColumnName("remark");
            entity.Property(e => e.StartDate)
                .HasColumnType("timestamp without time zone")
                .HasColumnName("start_date");
            entity.Property(e => e.SysOrgranizeId).HasColumnName("sys_orgranize_id");
            entity.Property(e => e.TransKeyId).HasColumnName("trans_key_id");

            entity.HasOne(d => d.ActionByNavigation).WithMany(p => p.TransKeyHistories)
                .HasForeignKey(d => d.ActionBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("action_by_fk");

            entity.HasOne(d => d.SysOrgranize).WithMany(p => p.TransKeyHistories)
                .HasForeignKey(d => d.SysOrgranizeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("trans_key_history_sys_orgranize_id_fkey");

            entity.HasOne(d => d.TransKey).WithMany(p => p.TransKeyHistories)
                .HasForeignKey(d => d.TransKeyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("trans_key_history_trans_key_fk");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
