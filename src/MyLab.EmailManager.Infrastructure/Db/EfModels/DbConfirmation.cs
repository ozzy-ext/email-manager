using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyLab.EmailManager.Infrastructure.Db.EfModels;

[Table("confirmation")]
[Index("Seed", Name = "IX_confirmation_seed")]
public partial class DbConfirmation
{
    [Key]
    [Column("email_id")]
    [MySqlCharSet("ascii")]
    [MySqlCollation("ascii_general_ci")]
    public Guid EmailId { get; set; }

    [Column("seed")]
    [MySqlCharSet("ascii")]
    [MySqlCollation("ascii_general_ci")]
    public Guid Seed { get; set; }

    [Column("step")]
    public string Step { get; set; } = null!;

    [Column("step_dt")]
    [MaxLength(6)]
    public DateTime? StepDt { get; set; }

    [ForeignKey("EmailId")]
    public virtual DbEmail DbEmail { get; set; } = null!;
}
