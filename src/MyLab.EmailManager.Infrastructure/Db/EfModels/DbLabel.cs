using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MyLab.EmailManager.Infrastructure.Db.EfModels;

[Table("label")]
[Index("EmailId", Name = "IX_label_email_id")]
public partial class DbLabel
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = null!;

    [Column("value")]
    public string Value { get; set; } = null!;

    [Column("email_id")]
    [MySqlCharSet("ascii")]
    [MySqlCollation("ascii_general_ci")]
    public Guid EmailId { get; set; }

    [ForeignKey("EmailId")]
    [InverseProperty("Labels")]
    public virtual DbEmail Email { get; set; } = null!;
}
